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
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.Math.Data;
using SoonLearning.AppCenter;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    /// <summary>
    /// Interaction logic for AddMinusUserControl.xaml
    /// </summary>
    public partial class AddMinusStartupControl
    {
        #region Members

        #endregion

        #region Constructor and Init

        public AddMinusStartupControl()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.rootGrid.ClearEntry();
            this.ShowSettingControl();
        }

        #endregion

        #region Switch Page

        internal void ShowSettingControl()
        {
            RapidGadgetControl.Instance.ControlAbility = ControlAbility.CanGoback;
            this.rootGrid.Content = ControlMgr.Instance.MathSettingControl;
            this.rootGrid.RemoveEntry();
        }

        internal void ShowQuestionControl()
        {
            RapidGadgetControl.Instance.ControlAbility = ControlAbility.None;
            this.rootGrid.Content = ControlMgr.Instance.MathQuestionControl;
        }

        internal void RetryQuiz()
        {
            this.rootGrid.Content = ControlMgr.Instance.MathQuestionControl;
            ControlMgr.Instance.MathQuestionControl.RetryQuiz();
        }

        internal void ContinueQuiz()
        {
            this.rootGrid.Content = ControlMgr.Instance.MathQuestionControl;
            ControlMgr.Instance.MathQuestionControl.ContinueQuiz();
        }

        internal void ShowResultControl()
        {
            RapidGadgetControl.Instance.ControlAbility = ControlAbility.CanGoback;
            this.rootGrid.Content = ControlMgr.Instance.MathResultControl;
            this.rootGrid.RemoveEntry();
        }

        internal void ShowHistoryControl()
        {
            RapidGadgetControl.Instance.ControlAbility = ControlAbility.CanGoback;
            this.rootGrid.Content = ControlMgr.Instance.HistoryUserControl;
        }

        internal void ShowPrintControl(QuestionData data)
        {
            ControlMgr.Instance.PrintWindow.Load(data);
            this.rootGrid.Content = ControlMgr.Instance.PrintWindow;
        }

        internal void ShowDetailControl(QuestionData data)
        {
            this.rootGrid.Content = ControlMgr.Instance.DetailUserControl;
            ControlMgr.Instance.DetailUserControl.ShowResult(data);
        }

        #endregion

        internal bool GoBack()
        {
            this.rootGrid.GoBack();

            return !this.rootGrid.CanGoback;
        }
    }
}
