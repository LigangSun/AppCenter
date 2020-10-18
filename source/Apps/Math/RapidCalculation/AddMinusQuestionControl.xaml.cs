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
using SoonLearning.Math.Data;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Threading;
using System.ComponentModel;
using System.Reflection;
using Microsoft.Win32;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    /// <summary>
    /// Interaction logic for AddMinusQuestionControl.xaml
    /// </summary>
    public partial class AddMinusQuestionControl
    {
        private QuestionData questionData;
        private int totalPage = 1;
        private int currentPage = 0;
        private float opacityTime = 0.1f;
        private string failMp3;
        private string okMp3;

        private bool isRetry;
        private bool isContinue;

        private int totalTime = 0;

        private Question_a_b_c_Control activeQuestionCtrl;

        private Question_a_b_c_Control ActiveQuestionControl
        {
            get { return this.activeQuestionCtrl; }
            set
            {
                if (this.activeQuestionCtrl != null)
                    this.activeQuestionCtrl.IsSelected = false;
                this.activeQuestionCtrl = value;
            }
        }

        private TimeControlEngine timeControl;

        public AddMinusQuestionControl()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetEntryAssembly();
            string folder = System.IO.Path.GetDirectoryName(assembly.Location);
            this.failMp3 = System.IO.Path.Combine(folder, @"Voice\Math\fail.mp3");
            this.okMp3 = System.IO.Path.Combine(folder, @"Voice\Math\ok.mp3");

            this.voiceElement.IsMuted = true;
            this.PlayMusic(this.failMp3);
            this.PlayMusic(this.okMp3);
            this.voiceElement.IsMuted = false;
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
        }

        private void CreateQuestionControl()
        {
            this.questionGrid.Children.Clear();
            this.questionGrid.RowDefinitions.Clear();
            this.questionGrid.ColumnDefinitions.Clear();
            MathSetting.Instance.QuestionGenerator.Generate(this.questionGrid);
        }

        internal void NewQuiz()
        {
            this.ShowGeneratingInfo(true);

            if (this.timeControl != null)
            {
                this.timerPanel.Visibility = System.Windows.Visibility.Hidden;
                this.leftTimeLabel.Content = "";
                this.timeControl.Stop();
            }

            this.totalTime = MathSetting.Instance.ExamTime * 60;

            this.Dispatcher.BeginInvoke(new EventHandler<EventArgs>(SafeNewQuiz),
                DispatcherPriority.ApplicationIdle,
                new object[] {this, EventArgs.Empty});
        }

        internal void RetryQuiz()
        {
            this.questionData = MathSetting.Instance.CurrentQuestionData;
            foreach (Question q in this.questionData.Items)
            {
                if (q is Question_a_b_c)
                {
                    Question_a_b_c abc = q as Question_a_b_c;
                    abc.Result = null;
                }
            }
            this.totalTime = MathSetting.Instance.ExamTime * 60;
            this.isRetry = true;
        }

        internal void ContinueQuiz()
        {
            this.questionData = MathSetting.Instance.CurrentQuestionData;
            this.totalTime = this.questionData.LeftTime;
            this.isContinue = true;
        }

        private void SafeNewQuiz(object sender, EventArgs e)
        {
            this.questionGrid.IsEnabled = true;

            if (this.questionData != null)
            {
                this.questionData.QuestionGeneratingEvent -= questionData_QuestionGeneratingEvent;
                this.questionData.QuestionGeneratedEvent -= questionData_QuestionGeneratedEvent;
            }

            this.questionData = MathSetting.Instance.GetQuestionData();
            this.questionData.QuestionGeneratedEvent +=new QuestionGeneratingDelegate(questionData_QuestionGeneratedEvent);
            this.questionData.QuestionGeneratingEvent += new QuestionGeneratingDelegate(questionData_QuestionGeneratingEvent);
            this.questionData.Generate(MathSetting.Instance.QuestionCount);
        }

        private void timeControl_TimeElapsed(object sender, EventArgs e)
        {
            if (Thread.CurrentThread == this.Dispatcher.Thread)
            {
                this.SafeUpdateLeftTimeInfo(sender, e);
            }
            else
            {
                this.Dispatcher.BeginInvoke(new EventHandler(this.SafeUpdateLeftTimeInfo), 
                    DispatcherPriority.Normal,
                    new object[] { sender, e });
            }
        }

        private void SafeUpdateLeftTimeInfo(object sender, EventArgs e)
        {
            this.questionData.LeftTime = this.totalTime - this.timeControl.Elapsed;

            TimeSpan ts = TimeSpan.FromSeconds(this.questionData.LeftTime);
            this.leftTimeLabel.Content = string.Format("{0}:{1}", ts.Minutes.ToString("00"), ts.Seconds.ToString("00"));
            if (ts <= TimeSpan.Zero)
            {
                this.timeControl.Stop();
                this.questionGrid.IsEnabled = false;
                MessageWindow msgWnd = new MessageWindow();
                msgWnd.ShowMessage(SoonLearning.Math.Fast.Properties.Resources.QuizTimeOut, MessageBoxButton.OK, null);
            }
        }

        private void questionData_QuestionGeneratingEvent(int index)
        {
            if (Thread.CurrentThread == this.Dispatcher.Thread)
            {
                this.SafeUpdateGeneratingInfo(index);
            }
            else
            {
                this.Dispatcher.BeginInvoke(new QuestionGeneratingDelegate(this.SafeUpdateGeneratingInfo), new object[] { index });
            }
        }

        private void SafeUpdateGeneratingInfo(int index)
        {
            this.generatingLabel.Text = string.Format(SoonLearning.Math.Fast.Properties.Resources.generatingQuestion, index, MathSetting.Instance.QuestionCount);
        }

        private void questionData_QuestionGeneratedEvent(int index)
        {
            if (Thread.CurrentThread == this.Dispatcher.Thread)
            {
                this.SafeGeneratedQuestionCompleted(index);
            }
            else
            {
                this.Dispatcher.BeginInvoke(new QuestionGeneratingDelegate(this.SafeGeneratedQuestionCompleted), new object[] { index });
            }
        }

        private void SafeGeneratedQuestionCompleted(int index)
        {
            this.CreateQuestionControl();

            this.questionData.AutoSave();

            this.ShowGeneratingInfo(false);

            this.timeControl = new TimeControlEngine();
            this.timeControl.TimeElapsed += new EventHandler(timeControl_TimeElapsed);

            this.timerPanel.Visibility = System.Windows.Visibility.Visible;

            this.timeControl.Start();

            int i = 0;
            foreach (Question_a_b_c_Control ctrl in this.questionGrid.Children)
            {
                if (ctrl == null)
                    continue;

                if (ctrl.DataContext is Question_a_b_c)
                {
                    Question_a_b_c oldQ = ctrl.DataContext as Question_a_b_c;
                    oldQ.PropertyChanged -= q_PropertyChanged;
                }

                Question_a_b_c q = this.questionData.Items[i++] as Question_a_b_c;
                ctrl.DataContext = q;
                q.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(q_PropertyChanged);
                if (i == this.questionData.Items.Count)
                    break;

                ctrl.SelectedChanged += OnQuestonCtrlSelectedChanged;
                ctrl.QuestionValidated += OnQuestionValidated;
            }

            this.ActiveQuestionControl = (Question_a_b_c_Control)this.questionGrid.Children[0];
            if (this.ActiveQuestionControl != null)
                this.ActiveQuestionControl.Select();

            this.currentPage = 0;
            this.totalPage = this.questionData.Items.Count / MathSetting.Instance.QuestionCountPerPage;
            if (this.questionData.Items.Count % MathSetting.Instance.QuestionCountPerPage != 0)
                this.totalPage++;

         //   this.countPerPageLabel.Content = string.Format(SoonLearning.Math.Fast.Properties.Resources.questionCountPerPage, MathSetting.Instance.QuestionCountPerPage);
            //this.Total = this.totalPage;
            this.UpdatePageInfo();
        }

        private void ShowGeneratingInfo(bool show)
        {
            if (show)
            {
                this.prePage.Visibility = System.Windows.Visibility.Hidden;
                this.nextPage.Visibility = System.Windows.Visibility.Hidden;
                this.controlGrid.Visibility = System.Windows.Visibility.Hidden;
                this.questionGrid.Visibility = System.Windows.Visibility.Hidden;
            //    this.pageIndexLabel.Visibility = System.Windows.Visibility.Hidden;
            //    this.countPerPageLabel.Visibility = System.Windows.Visibility.Hidden;
                this.generatingLabel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.prePage.Visibility = System.Windows.Visibility.Visible;
                this.nextPage.Visibility = System.Windows.Visibility.Visible;
                this.controlGrid.Visibility = System.Windows.Visibility.Visible;
                this.questionGrid.Visibility = System.Windows.Visibility.Visible;
            //    this.pageIndexLabel.Visibility = System.Windows.Visibility.Visible;
            //    this.countPerPageLabel.Visibility = System.Windows.Visibility.Visible;
                this.generatingLabel.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void prePage_Click(object sender, RoutedEventArgs e)
        {
            foreach (Question_a_b_c_Control q in this.questionGrid.Children)
            {
                if (q == null)
                    return;

                if (q == this.ActiveQuestionControl)
                    q.IsSelected = false;
            }

            if (this.prePage.IsVisible && this.prePage.IsEnabled)
                this.OpacityShowQuestionPage(0.0f, preOpacityAnimate_Completed);
        }

        private void preOpacityAnimate_Completed(object sender, EventArgs e)
        {
            this.questionGrid.Opacity = 0.0f;

            this.currentPage--;

            int count = 0;
            int index = this.currentPage * MathSetting.Instance.QuestionCountPerPage;
            foreach (Question_a_b_c_Control ctrl in this.questionGrid.Children)
            {
                if (ctrl == null)
                    continue;

                if (ctrl.DataContext is Question_a_b_c)
                {
                    Question_a_b_c oldQ = ctrl.DataContext as Question_a_b_c;
                    oldQ.PropertyChanged -= q_PropertyChanged;
                }

                ctrl.Visibility = System.Windows.Visibility.Visible;
                Question_a_b_c q = this.questionData.Items[index++] as Question_a_b_c;
                ctrl.DataContext = q;
                q.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(q_PropertyChanged);
                count++;
                if (count == MathSetting.Instance.QuestionCountPerPage ||
                    index == this.questionData.Items.Count)
                    break;
            }

            this.OpacityShowQuestionPage(1.0f, EndSwtichPage);

            this.UpdatePageInfo();
        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            foreach (Question_a_b_c_Control q in this.questionGrid.Children)
            {
                if (q == null)
                    continue;

                if (q == this.ActiveQuestionControl)
                    q.IsSelected = false;
            }

            if (this.nextPage.IsVisible && this.nextPage.IsEnabled)
                this.OpacityShowQuestionPage(0.0f, nextOpacityAnimate_Completed);
        }

        private void nextOpacityAnimate_Completed(object sender, EventArgs e)
        {
            this.questionGrid.Opacity = 0.0f;

            this.currentPage++;

            int count = 0;
            int index = this.currentPage * MathSetting.Instance.QuestionCountPerPage;
            foreach (Question_a_b_c_Control ctrl in this.questionGrid.Children)
            {
                if (ctrl == null)
                    continue;

                if (index == this.questionData.Items.Count)
                {
                    ctrl.Visibility = System.Windows.Visibility.Hidden;
                    continue;
                }
                else
                {
                    ctrl.Visibility = System.Windows.Visibility.Visible;
                }

                if (ctrl.DataContext is Question_a_b_c)
                {
                    Question_a_b_c oldQ = ctrl.DataContext as Question_a_b_c;
                    oldQ.PropertyChanged -= q_PropertyChanged;
                }

                Question_a_b_c q = this.questionData.Items[index++] as Question_a_b_c;
                ctrl.DataContext = q;
                q.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(q_PropertyChanged);
                count++;
                if (count == MathSetting.Instance.QuestionCountPerPage)
                    break;
            }

            this.OpacityShowQuestionPage(1.0f, EndSwtichPage);

            this.UpdatePageInfo();
        }

        private void EndSwtichPage(object sender, EventArgs e)
        {
            this.prePage.IsEnabled = true;
            this.nextPage.IsEnabled = true;
        }

        private void OpacityShowQuestionPage(float to, EventHandler handler)
        {
            DoubleAnimation opacityAnim = new DoubleAnimation(to, TimeSpan.FromSeconds(opacityTime), FillBehavior.HoldEnd);
            if (handler != null)
                opacityAnim.Completed += handler;

            this.prePage.IsEnabled = false;
            this.nextPage.IsEnabled = false;

            this.questionGrid.BeginAnimation(Grid.OpacityProperty, opacityAnim);
        }

        private void UpdatePageInfo()
        {
            this.ActiveQuestionControl = (Question_a_b_c_Control)this.questionGrid.Children[0];
            if (this.ActiveQuestionControl != null)
                this.ActiveQuestionControl.Select();

            if (this.totalPage == 1)
            {
                this.prePage.Visibility = System.Windows.Visibility.Hidden;
                this.nextPage.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                if (this.currentPage == 0)
                {
                    this.prePage.Visibility = System.Windows.Visibility.Hidden;
                    this.nextPage.Visibility = System.Windows.Visibility.Visible;
                }
                else if (this.currentPage == this.totalPage - 1)
                {
                    this.prePage.Visibility = System.Windows.Visibility.Visible;
                    this.nextPage.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    this.prePage.Visibility = System.Windows.Visibility.Visible;
                    this.nextPage.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            switch (e.Key)
            {
                case Key.Enter:
                    this.EnterPressed();
                    break;
                case Key.Up:
                    {
                        int index = 0;
                        foreach (Question_a_b_c_Control ctrl in this.questionGrid.Children)
                        {
                            if (ctrl == null)
                                continue;

                            if (ctrl == this.ActiveQuestionControl)
                            {
                                ctrl.IsSelected = false;
                                break;
                            }
                            index++;
                        }

                        if (this.currentPage + 1 == this.totalPage)
                        {
                            if (index == 0)
                                index = this.questionData.Items.Count - this.currentPage * MathSetting.Instance.QuestionCountPerPage - 1;
                            else
                                index--;
                        }
                        else
                        {
                            if (index == 0)
                                index = this.questionGrid.Children.Count - 1;
                            else
                                index--;
                        }

                        this.ActiveQuestionControl = (Question_a_b_c_Control)this.questionGrid.Children[index];
                        if (this.ActiveQuestionControl != null)
                            this.ActiveQuestionControl.Select();
                    }
                    break;
                case Key.Down:
                    {
                        int index = 0;
                        foreach (Question_a_b_c_Control ctrl in this.questionGrid.Children)
                        {
                            if (ctrl == null)
                                continue;

                            if (ctrl == this.ActiveQuestionControl)
                            {
                                ctrl.IsSelected = false;
                                break;
                            }
                            index++;
                        }

                        if (this.currentPage + 1 == this.totalPage)
                        {
                            if (MathSetting.Instance.QuestionCountPerPage * this.currentPage + index == this.questionData.Items.Count - 1)
                                index = 0;
                            else
                                index++;
                        }
                        else
                        {
                            if (index == this.questionGrid.Children.Count - 1)
                                index = 0;
                            else
                                index++;
                        }

                        this.ActiveQuestionControl = (Question_a_b_c_Control)this.questionGrid.Children[index];
                        if (this.ActiveQuestionControl != null)
                            this.ActiveQuestionControl.Select();
                    }
                    break;
                case Key.Left:
                    this.prePage_Click(this, null);
                    break;
                case Key.Right:
                    this.nextPage_Click(this, null);
                    break;
            }
        }

        private void EnterPressed()
        {
            int index = 0;
            Question_a_b_c_Control currentCtrl = this.activeQuestionCtrl;
            foreach (Question_a_b_c_Control ctrl in this.questionGrid.Children)
            {
                if (ctrl == null)
                    continue;

                if (ctrl == currentCtrl)
                {
                    break;
                }
                index++;
            }

            if (currentCtrl != null)
                currentCtrl.IsSelected = false;

            if (index == this.questionGrid.Children.Count - 1)
                this.nextPage_Click(this, null);
            else
            {
                index++;
                if (this.currentPage + 1 == this.totalPage)
                {
                    if (MathSetting.Instance.QuestionCountPerPage * this.currentPage + index == this.questionData.Items.Count)
                        return;
                }

                if (index < this.questionGrid.Children.Count)
                {
                    this.ActiveQuestionControl = (Question_a_b_c_Control)this.questionGrid.Children[index];
                    if (this.ActiveQuestionControl != null)
                        this.ActiveQuestionControl.Select();
                }
            }
        }

        private void OnQuestonCtrlSelectedChanged(object sender, EventArgs e)
        {
            Question_a_b_c_Control preCtrl = this.ActiveQuestionControl;
            if (preCtrl != null)
            {
                preCtrl.ValidResult();
                this.questionData.AutoSave();
            }

            Question_a_b_c_Control ctrl = sender as Question_a_b_c_Control;
            if (ctrl == null)
                return;

            if (ctrl.IsSelected)
            {
                this.ActiveQuestionControl = ctrl;
                if (!this.openVirtualKeyboardCheckBox.IsChecked.Value)
                    return;

                int index = this.questionGrid.Children.IndexOf(ctrl);

                this.numberKeyboardPopup.IsOpen = false;
                this.numberKeyboardPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.Right;
                this.numberKeyboardPopup.PlacementTarget = ctrl;
                this.numberKeyboardPopup.IsOpen = true;
            }
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow msgWnd = new MessageWindow();
            msgWnd.ShowMessage(SoonLearning.Math.Fast.Properties.Resources.NewQuizWarning, MessageBoxButton.OKCancel, MessageWindowCallback);
        }

        private void MessageWindowCallback(bool ok)
        {
            if (!ok)
                return;

            this.timeControl.Stop();
            this.timerPanel.Visibility = System.Windows.Visibility.Hidden;
            ControlMgr.Instance.MathStartupControl.ShowSettingControl();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            this.timeControl.Pause();
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Filter = SoonLearning.Math.Fast.Properties.Resources.QuestionDataFilter;
            bool? ret = saveFileDlg.ShowDialog(App.Current.MainWindow);
            if (ret != null && ret.Value)
            {
                try
                {
                    this.questionData.Save(saveFileDlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageWindow msgWnd = new MessageWindow();
                    msgWnd.ShowMessage(string.Format(SoonLearning.Math.Fast.Properties.Resources.SaveFail, ex.Message), MessageBoxButton.OK, null);
                }
            }
            this.timeControl.Start();
        }

        private void viewResultBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow msgWnd = new MessageWindow();
            if (!this.questionData.AllQuestionAnswered)
            {
                msgWnd.ShowMessage(SoonLearning.Math.Fast.Properties.Resources.QuestionNotAnswered, MessageBoxButton.OKCancel, ViewResultMessageWindowCallback);
            }
            else
            {
                msgWnd.ShowMessage(SoonLearning.Math.Fast.Properties.Resources.FinishTestWarning, MessageBoxButton.OKCancel, ViewResultMessageWindowCallback);
            }
        }

        private void ViewResultMessageWindowCallback(bool ok)
        {
            if (!ok)
                return;

            ControlMgr.Instance.MathStartupControl.ShowResultControl();
            ControlMgr.Instance.MathResultControl.ShowResult(this.questionData);
        }

        private void voiceCheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void fullScreenCheckBox_Click(object sender, RoutedEventArgs e)
        {
        //    ((MainWindow)App.Current.MainWindow).SwitchToFullScreen(this.fullScreenCheckBox.IsChecked.Value);
        }

        private void numberKeyboard_NumberVirtualKeyboardInputEvent(Key key)
        {
            if (key == Key.Enter)
            {
                this.EnterPressed();
                return;
            }

            if (this.numberKeyboardPopup.PlacementTarget is Question_a_b_c_Control)
            {
                Question_a_b_c_Control ctrl = this.numberKeyboardPopup.PlacementTarget as Question_a_b_c_Control;
                ctrl.numberKeyboard_NumberVirtualKeyboardInputEvent(key);
            }
        }

        private void openVirtualKeyboardCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (this.openVirtualKeyboardCheckBox.IsChecked.Value)
            {
                foreach (Question_a_b_c_Control ctrl in this.questionGrid.Children)
                {
                    if (ctrl == this.ActiveQuestionControl)
                    {
                        ctrl.Select();
                        break;
                    }
                }
            }
            else
            {
                this.numberKeyboardPopup.IsOpen = false;
            }
        }

        private void q_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsCorrect")
            {
                if (!MathSetting.Instance.VoiceTip)
                    return;

                Question_a_b_c question = sender as Question_a_b_c;
                if (question != null)
                {
                    if (question.IsCorrect != null)
                    {
                        if (!question.IsCorrect.Value)
                        {
                            this.PlayMusic(this.failMp3);
                        }
                        else
                        {
                            this.PlayMusic(this.okMp3);
                        }
                    }
                    else
                    {
                    }
                }
            }
        }

        private void PlayMusic(string file)
        {
            this.voiceElement.Source = new Uri(file);
            this.voiceElement.Play();
        }

        private void OnQuestionValidated(object sender, EventArgs e)
        {
        }

        private void printBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.MathStartupControl.ShowPrintControl(this.questionData);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.timeControl != null)
            {
                this.timeControl.TimeElapsed -= timeControl_TimeElapsed;
                this.timeControl.Stop();
            }
        }

        private void GadgetUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.isRetry || this.isContinue)
            {
                this.Dispatcher.BeginInvoke(new QuestionGeneratingDelegate(this.SafeGeneratedQuestionCompleted), new object[] { 0 });
                this.isRetry = false;
                this.isContinue = false;
            }
        }
    }
}
