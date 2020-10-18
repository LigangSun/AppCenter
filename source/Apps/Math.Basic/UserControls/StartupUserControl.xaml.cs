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
using Math.Basic.Data;
using SoonLearning.Math.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Math.Basic.UserControls
{
    /// <summary>
    /// Interaction logic for StartupUserControl.xaml
    /// </summary>
    public partial class StartupUserControl
    {
        private ExerciseUserControl exerciseUserControl;
        private ExamUserControl examUserControl;
        private MathSubTypeListUserControl mathSubTypeListUserControl;
        private TeachUserControl teachUserControl;
        private ExerciseSettingUserControl exerciseSettingUserControl;
        private ExamSettingUserControl examSettingUserControl;
        private AllQuestionUserControl allQuestionUserControl;
        private EndExamUserControl endExamUserControl;
        private ExerciseHistoryUserControl exerciseHistoryUserControl;
        private ExamHistoryUserControl examHistoryUserControl;
        private NotImplementUserControl notImplementuserControl;

        public StartupUserControl()
        {
            InitializeComponent();

            this.Title = ControlMgr.Instance.Entry.Title;
        }

        private void GadgetUserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void GadgetUserControl_Initialized(object sender, EventArgs e)
        {
            this.ShowSubTypeListPage();
        }

        internal void ShowExercisePage()
        {
            if (this.exerciseUserControl == null)
                this.exerciseUserControl = new ExerciseUserControl();

            this.ShowPage(this.exerciseUserControl, false);
            this.exerciseUserControl.GenerateExercise();
        }

        internal void BackToExercisePage()
        {
            this.ShowPage(this.exerciseUserControl, false);
        }

        internal void ShowExerciseSettingPage(bool restart)
        {
            if (this.exerciseSettingUserControl == null)
                this.exerciseSettingUserControl = new ExerciseSettingUserControl();

            this.ShowPage(this.exerciseSettingUserControl, true);
            if (restart)
                this.rootPanel.RemoveEntry();
        }

        internal void ShowExerciseHistoryPage()
        {
            if (this.exerciseHistoryUserControl == null)
                this.exerciseHistoryUserControl = new ExerciseHistoryUserControl();

            this.ShowPage(this.exerciseHistoryUserControl, true);
        }

        internal void ShowExamPage(int examDuration)
        {
            if (this.examUserControl == null)
                this.examUserControl = new ExamUserControl();

            this.examUserControl.ExamDuration = examDuration;
            this.examUserControl.GenerateExam();
            this.ShowPage(this.examUserControl, false);
        }

        internal void ShowExamSettingPage()
        {
            if (this.examSettingUserControl == null)
                this.examSettingUserControl = new ExamSettingUserControl();

            this.ShowPage(this.examSettingUserControl, true);
        }

        internal void ShowEndExamPage(Exam exam)
        {
            if (this.endExamUserControl == null)
                this.endExamUserControl = new EndExamUserControl();

            this.endExamUserControl.ShowExam(exam);
            this.ShowPage(this.endExamUserControl, false);
        }

        internal void ShowExamHistoryPage()
        {
            if (this.examHistoryUserControl == null)
                this.examHistoryUserControl = new ExamHistoryUserControl();

            this.ShowPage(this.examHistoryUserControl, true);
        }

        internal void ShowSubTypeListPage()
        {
            if (this.mathSubTypeListUserControl == null)
                this.mathSubTypeListUserControl = new MathSubTypeListUserControl();

            this.Title = ControlMgr.Instance.Entry.Title;
            this.ShowPage(this.mathSubTypeListUserControl, true);
        }

        internal void ShowTeachPage()
        {        
            this.Title = string.Format("{0} -- {1}", ControlMgr.Instance.Entry.Title, DataMgr.Instance.ActiveMathSubTypeItem.Title);
            if (!DataMgr.Instance.IsCreatorExist)
            {
                if (this.notImplementuserControl == null)
                    this.notImplementuserControl = new NotImplementUserControl();

                this.ShowPage(this.notImplementuserControl, true);
            }
            else
            {
                if (this.teachUserControl == null)
                    this.teachUserControl = new TeachUserControl();

                this.ShowPage(this.teachUserControl, true);
            }
        }

        internal void EndExercise()
        {
            this.Title = string.Format("{0} -- {1}", ControlMgr.Instance.Entry.Title, DataMgr.Instance.ActiveMathSubTypeItem.Title);
            this.rootPanel.RemoveEntry();
            this.ShowPage(this.teachUserControl, false);
        }

        internal void EndExam()
        {
            this.Title = string.Format("{0} -- {1}", ControlMgr.Instance.Entry.Title, DataMgr.Instance.ActiveMathSubTypeItem.Title);
            this.rootPanel.RemoveEntry();
            this.ShowPage(this.teachUserControl, false);
        }

        internal void ShowAllQuestionPage(Exercise exercise, bool canBackExercise)
        {
            if (this.allQuestionUserControl == null)
                this.allQuestionUserControl = new AllQuestionUserControl();

            this.allQuestionUserControl.ShowAllQuestion(exercise, canBackExercise);
            this.ShowPage(this.allQuestionUserControl, !canBackExercise);
        }

        private void ShowPage(UserControl ctrl, bool canGoback)
        {
            this.rootPanel.Content = ctrl;
            if (!canGoback)
            {
                this.rootPanel.RemoveEntry();
            }
            else
            {
            }
        }

        internal bool GoBack()
        {
            this.rootPanel.GoBack();

            if (this.rootPanel.Content == this.teachUserControl)
                this.Title = ControlMgr.Instance.Entry.Title;

            return !this.rootPanel.CanGoback;
        }
    }
}
