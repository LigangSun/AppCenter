using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Windows.Threading;
using SoonLearning.AppCenter.Controls;
using System.Diagnostics;
using System.Threading;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.Assessment.Player.UserControls
{
    /// <summary>
    /// Interaction logic for ExamUserControl.xaml
    /// </summary>
    public partial class ExamUserControl : UserControl
    {
        private Exam exam;
        private int index;
        private int examDuration;

        public int ExamDuration
        {
            set { this.examDuration = value; }
        }

        public ExamUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            this.timeCtrl.TimeUsedUpEvent += (sender1, e1) => { this.TimeUsedUp(); };
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            DataCreator dataMgr = DataMgr.Instance.DataCreator;
            dataMgr.CreateDataProgressEvent -= dataMgr_CreateDataProgressEvent;
            dataMgr.CreateDataCompletedEvent -= dataMgr_CreateDataCompletedEvent;

            this.nextButton.Focus();
        }

        private void TimeUsedUp()
        {
            MessageWindow msgWnd = new MessageWindow();
            msgWnd.ShowMessage("测试时间到，测试结束！", MessageBoxButton.OK, ExamMessageWindowCallback);
        }

        private void ExamMessageWindowCallback(bool ok)
        {
            this.EndExam();
        }

        internal void GenerateExam()
        {
            this.Dispatcher.BeginInvoke(new EventHandler<EventArgs>((sender1, e1) => { SafeGenerateExam(sender1, e1); }), DispatcherPriority.Background, new object[] { null, null });
        }

        private void SafeGenerateExam(object sender, EventArgs e)
        {
            this.index = 0;
            this.sectionInfoLabel.Visibility = System.Windows.Visibility.Hidden;
            this.questionPanel.Visibility = System.Windows.Visibility.Hidden;
            this.questionControlPanel.Visibility = System.Windows.Visibility.Hidden;
            this.infoLabel.Visibility = System.Windows.Visibility.Visible;

            DataCreator dataMgr = DataMgr.Instance.DataCreator;
            dataMgr.CreateDataProgressEvent += new CreateDataProgress(dataMgr_CreateDataProgressEvent);
            dataMgr.CreateDataCompletedEvent += new CreateDataCompleted(dataMgr_CreateDataCompletedEvent);
            dataMgr.GenerateExam();
            this.UpdateInfo();
        }

        private void dataMgr_CreateDataProgressEvent(BaseObject obj)
        {
            this.index++;
            Debug.WriteLine(this.index);
            this.UpdateInfo();
        }

        private void dataMgr_CreateDataCompletedEvent(BaseObject obj)
        {
            this.exam = obj as Exam;

            this.ShowNextQuestion();
            this.infoLabel.Visibility = System.Windows.Visibility.Hidden;
            this.sectionInfoLabel.Visibility = System.Windows.Visibility.Visible;
            this.questionPanel.Visibility = System.Windows.Visibility.Visible;
            this.questionControlPanel.Visibility = System.Windows.Visibility.Visible;

            this.saveExamData();

            this.timeCtrl.Start(this.examDuration * 60);
        }

        private void saveExamData()
        {
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                string dataFolder = DataMgr.Instance.DataFolder;
                dataFolder = System.IO.Path.Combine(dataFolder, "Exam");

                if (!System.IO.Directory.Exists(dataFolder))
                {
                    System.IO.Directory.CreateDirectory(dataFolder);
                }
                SerializerHelper<Exam>.XmlSerialize(System.IO.Path.Combine(dataFolder, this.exam.Id + ".mxd"), this.exam);
            }),
            DispatcherPriority.Background,
            null);
        }

        private void preButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowPreQuestion();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowNextQuestion();
        }

        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow msgWnd = new MessageWindow();
            msgWnd.ShowMessage("你确定要结束本次测验吗？", MessageBoxButton.OKCancel, MessageWindowCallback);
        }

        private void MessageWindowCallback(bool ok)
        {
            if (ok)
            {
                this.timeCtrl.Stop();
                this.EndExam();
            }
        }

        private void UpdateInfo()
        {
            this.infoLabel.Text = string.Format("正在生成第{0}题... ...", this.index);
        }

        private void ShowNextQuestion()
        {
            this.ShowQuestion(this.exam.NextQuestion);
        }

        private void ShowPreQuestion()
        {
            this.ShowQuestion(this.exam.PreQuestion);
        }

        private void ShowQuestion(Question question)
        {
            if (question == null)
                return;

            this.sectionInfoLabel.Text = this.exam.CurrentSection.Title + this.exam.CurrentSection.Description;

            this.UpdateButtonState();

            this.questionPanel.Children.Clear();
            if (question is TableQuestion)
            {
                TableQuestionUserControl ctrl = new TableQuestionUserControl(question as TableQuestion, 
                    this.exam.GetQuestionResponse(question.Id) as SelectableQuestionResponse);
                ctrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ctrl.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                this.questionPanel.Children.Add(ctrl);
            }
            else if (question is SelectableQuestion)
            {
                MCQuestionUserControl ctrl = new MCQuestionUserControl(question as SelectableQuestion,
                    this.exam.GetQuestionResponse(question.Id) as SelectableQuestionResponse, 
                    this.exam.QuestionIndex);
                ctrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ctrl.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                this.questionPanel.Children.Add(ctrl);
            }
            else if (question is FIBQuestion)
            {
                FIBQuestionUserControl ctrl = new FIBQuestionUserControl(question as FIBQuestion,
                    this.exam.GetQuestionResponse(question.Id) as FIBQuestionResponse,
                    this.exam.QuestionIndex);
                ctrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ctrl.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                this.questionPanel.Children.Add(ctrl);
            }
            else if (question is ESQuestion)
            {
                ESQuestionUserControl ctrl = new ESQuestionUserControl(question as ESQuestion,
                    this.exam.GetQuestionResponse(question.Id) as ESQuestionResponse,
                    this.exam.QuestionIndex);
                ctrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ctrl.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                this.questionPanel.Children.Add(ctrl);
            }
        }

        private void UpdateButtonState()
        {
            if (this.exam.CurrentSectionIndex == 0 &&
                this.exam.QuestionIndex == 0)
            {
                this.preButton.Visibility = System.Windows.Visibility.Hidden;
                this.nextButton.Visibility = System.Windows.Visibility.Visible;
            }
            else if (this.exam.CurrentSectionIndex == this.exam.SectionCollection.Count - 1 &&
                this.exam.QuestionIndex == this.exam.CurrentSection.QuestionCollection.Count - 1)
            {
                this.nextButton.Visibility = System.Windows.Visibility.Hidden;
                this.preButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.nextButton.Visibility = System.Windows.Visibility.Visible;
                this.preButton.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void AddControlToPanel(Panel panel, UIElement element)
        {
            if (!panel.Children.Contains(element))
                panel.Children.Add(element);
        }

        private void RemoveControlFromPanel(Panel panel, UIElement element)
        {
            if (panel.Children.Contains(element))
                panel.Children.Remove(element);
        }

        private void EndExam()
        {
            this.saveExamData();
            ControlMgr.Instance.StartupUserControl.ShowEndExamPage(this.exam);
        }

        private void questionListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.nextButton.Focus();
        }
    }
}
