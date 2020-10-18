using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SoonLearning.Assessment.Data;
using System.Threading;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter.Utility;
using System.Reflection;
using SoonLearning.Assessment.Player.CommonControl;
using System.Windows.Threading;

namespace SoonLearning.Assessment.Player.UserControls
{
    /// <summary>
    /// Interaction logic for ExerciseUserControl.xaml
    /// </summary>
    public partial class ExerciseUserControl : UserControl
    {
        private Exercise exercise;
        private int index = 0;
        private DateTime startTime;

        public ExerciseUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.nextButton.Focus();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            DataCreator dataMgr = DataMgr.Instance.DataCreator;
            dataMgr.CreateDataProgressEvent -= dataMgr_CreateDataProgressEvent;
            dataMgr.CreateDataCompletedEvent -= dataMgr_CreateDataCompletedEvent;
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
        }

        internal void GenerateExercise()
        {
            this.index = 0;
            this.sectionInfoLabel.Visibility = System.Windows.Visibility.Hidden;
            this.questionPanel.Visibility = System.Windows.Visibility.Hidden;
            this.questionControlPanel.Visibility = System.Windows.Visibility.Hidden;
            this.infoLabel.Visibility = System.Windows.Visibility.Visible;

            DataCreator dataMgr = DataMgr.Instance.DataCreator;
            dataMgr.CreateDataProgressEvent += new CreateDataProgress(dataMgr_CreateDataProgressEvent);
            dataMgr.CreateDataCompletedEvent += new CreateDataCompleted(dataMgr_CreateDataCompletedEvent);
            dataMgr.GenerateExercise();
            this.UpdateInfo();
        }

        private void dataMgr_CreateDataProgressEvent(BaseObject obj)
        {
            this.index++;
            this.UpdateInfo();
        }

        private void dataMgr_CreateDataCompletedEvent(BaseObject obj)
        {
            this.exercise = obj as Exercise;

            this.saveExerciseData();

            this.startTime = DateTime.Now;
        }

        private void saveExerciseData()
        {
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                string dataFolder = DataMgr.Instance.DataFolder;
                dataFolder = System.IO.Path.Combine(dataFolder, "Exercise");

                if (!System.IO.Directory.Exists(dataFolder))
                {
                    System.IO.Directory.CreateDirectory(dataFolder);
                }
                SerializerHelper<Exercise>.XmlSerialize(System.IO.Path.Combine(dataFolder, this.exercise.Id + ".mxd"), this.exercise);

                this.ShowNextQuestion();
                this.infoLabel.Visibility = System.Windows.Visibility.Hidden;
                this.sectionInfoLabel.Visibility = System.Windows.Visibility.Visible;
                this.questionPanel.Visibility = System.Windows.Visibility.Visible;
                this.questionControlPanel.Visibility = System.Windows.Visibility.Visible;
            }),
            DispatcherPriority.Background,
            null);
        }

        private void UpdateInfo()
        {
            this.infoLabel.Content = string.Format("正在生成第{0}题... ...", this.index);
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.questionPanel.Children[0] is MCQuestionUserControl)
            {
                MCQuestionUserControl mcQuestionCtrl = this.questionPanel.Children[0] as MCQuestionUserControl;
                if (!mcQuestionCtrl.IsCorrect)
                {
                    MessageWindow msgWin = new MessageWindow();
                    msgWin.ShowMessage("请检查答案或者察看解题方法！", MessageBoxButton.OK, null);
                    return;
                }
            }
            else if (this.questionPanel.Children[0] is TableQuestionUserControl)
            {
                TableQuestionUserControl tableQuestionCtrl = this.questionPanel.Children[0] as TableQuestionUserControl;
                if (!tableQuestionCtrl.IsCorrect)
                {
                    MessageWindow msgWin = new MessageWindow();
                    msgWin.ShowMessage("请检查答案或者察看解题方法！", MessageBoxButton.OK, null);
                    return;
                }
            }
            else if (this.questionPanel.Children[0] is FIBQuestionUserControl)
            {
                FIBQuestionUserControl fibQuestionCtrl = this.questionPanel.Children[0] as FIBQuestionUserControl;
                if (!fibQuestionCtrl.IsCorrect)
                {
                    MessageWindow msgWin = new MessageWindow();
                    msgWin.ShowMessage("请检查答案或者察看解题方法！", MessageBoxButton.OK, null);
                    return;
                }
            }

            this.ShowNextQuestion();
        }

        private void preButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowPreQuestion();
        }

        private void ShowNextQuestion()
        {
            this.ShowQuestion(this.exercise.NextQuestion);
        }

        private void ShowPreQuestion()
        {
            this.ShowQuestion(this.exercise.PreQuestion);
        }

        private void ShowQuestion(Question question)
        {
            if (question == null)
                return;

            this.solutionWrapPanel.Children.Clear();

            this.sectionInfoLabel.Text = this.exercise.CurrentSection.Title + this.exercise.CurrentSection.Description;

            this.UpdateButtonState();

            this.questionPanel.Children.Clear();
            if (question is TableQuestion)
            {
                TableQuestionUserControl ctrl = new TableQuestionUserControl(question as TableQuestion,
                    this.exercise.GetQuestionResponse(question.Id) as SelectableQuestionResponse);
                ctrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ctrl.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                this.questionPanel.Children.Add(ctrl);
            }
            else if (question is SelectableQuestion)
            {
                MCQuestionUserControl ctrl = new MCQuestionUserControl(question as SelectableQuestion,
                    this.exercise.GetQuestionResponse(question.Id) as SelectableQuestionResponse, 
                    this.exercise.QuestionIndex);
                ctrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ctrl.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                this.questionPanel.Children.Add(ctrl);
            }
            else if (question is FIBQuestion)
            {
                FIBQuestionUserControl ctrl = new FIBQuestionUserControl(question as FIBQuestion,
                    this.exercise.GetQuestionResponse(question.Id) as FIBQuestionResponse, 
                    this.exercise.QuestionIndex);
                ctrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ctrl.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                this.questionPanel.Children.Add(ctrl);
            }
            else if (question is ESQuestion)
            {
                ESQuestionUserControl ctrl = new ESQuestionUserControl(question as ESQuestion,
                    this.exercise.GetQuestionResponse(question.Id) as ESQuestionResponse,
                    this.exercise.QuestionIndex);
                ctrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ctrl.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                this.questionPanel.Children.Add(ctrl);
            }
        }

        private void restartButton_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow msgWnd = new MessageWindow();
            msgWnd.ShowMessage("你确定要重新开始练习吗？", MessageBoxButton.OKCancel, RestartMessageWindowCallback);
        }

        private void RestartMessageWindowCallback(bool ok)
        {
            if (!ok)
                return;

            this.exercise.UsedTime = DateTime.Now - startTime;
            this.saveExerciseData();
            ControlMgr.Instance.StartupUserControl.ShowExerciseSettingPage(true);
        }

        private void solutionButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.solutionWrapPanel.Children.Count == 0)
            {
                if (this.exercise.CurrentSection.CurrentQuestion.Solution != null)
                {
                    if (!string.IsNullOrEmpty(this.exercise.CurrentSection.CurrentQuestion.Solution.Content))
                    {
                        CommonControlCreator.CreateContentControl(this.exercise.CurrentSection.CurrentQuestion.Solution,
                            this.solutionWrapPanel,
                            new SolidColorBrush(Colors.White),
                            null);
                    }
                    else if (!string.IsNullOrEmpty(this.exercise.CurrentSection.CurrentQuestion.Tip))
                    {
                        TextBlock noSolutionTextBlock = new TextBlock();
                        noSolutionTextBlock.Text = this.exercise.CurrentSection.CurrentQuestion.Tip;
                        noSolutionTextBlock.FontSize = 20;
                        noSolutionTextBlock.FontWeight = FontWeights.Medium;
                        noSolutionTextBlock.Foreground = Brushes.White;
                        this.solutionWrapPanel.Children.Add(noSolutionTextBlock);
                    }
                }
                
                if (this.solutionWrapPanel.Children.Count == 0)
                {
                    TextBlock noSolutionTextBlock = new TextBlock();
                    noSolutionTextBlock.Text = "抱歉，该题没有解题方法!";
                    noSolutionTextBlock.FontSize = 20;
                    noSolutionTextBlock.FontWeight = FontWeights.Medium;
                    noSolutionTextBlock.Foreground = Brushes.White;
                    this.solutionWrapPanel.Children.Add(noSolutionTextBlock);
                }
            }

            this.solutionPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
            this.solutionPopup.IsOpen = true;
        }

        private void UpdateButtonState()
        {
            if (this.exercise.CurrentSectionIndex == 0 &&
                this.exercise.QuestionIndex == 0)
            {
                this.preButton.Visibility = System.Windows.Visibility.Hidden;
                this.nextButton.Visibility = System.Windows.Visibility.Visible;
            }
            else if (this.exercise.CurrentSectionIndex == this.exercise.SectionCollection.Count - 1 &&
                this.exercise.QuestionIndex == this.exercise.CurrentSection.QuestionCollection.Count - 1)
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

        private void allQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.StartupUserControl.ShowAllQuestionPage(this.exercise, true);
        }

        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow msgWnd = new MessageWindow();
            msgWnd.ShowMessage("你确定要结束本次练习吗？", MessageBoxButton.OKCancel, MessageWindowCallback);
        }

        private void MessageWindowCallback(bool ok)
        {
            if (!ok)
                return;

            this.exercise.UsedTime = DateTime.Now - startTime;
            this.saveExerciseData();
            ControlMgr.Instance.StartupUserControl.EndExercise();
        }
    }
}
