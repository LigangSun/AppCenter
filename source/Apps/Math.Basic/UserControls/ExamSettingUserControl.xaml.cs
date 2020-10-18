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
using SoonLearning.AppCenter.Controls;

namespace Math.Basic.UserControls
{
    /// <summary>
    /// Interaction logic for ExamSettingUserControl.xaml
    /// </summary>
    public partial class ExamSettingUserControl : UserControl
    {
        public ExamSettingUserControl()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            string examDurationText = this.examDurationTextBox.Text;
            int examDuration = 0;
            if (string.IsNullOrEmpty(examDurationText))
            {
                MessageWindow msgWnd = new MessageWindow();
                msgWnd.ShowMessage("请填写测试所需时间。", MessageBoxButton.OK, null);
                this.examDurationTextBox.SelectAll();
                this.examDurationTextBox.Focus();
                return;
            }
            else
            {
                examDuration = Convert.ToInt32(examDurationText);
                if (examDuration == 0)
                {
                    MessageWindow msgWnd = new MessageWindow();
                    msgWnd.ShowMessage("测试所需时间不能为0。", MessageBoxButton.OK, null);
                    this.examDurationTextBox.SelectAll();
                    this.examDurationTextBox.Focus();
                    return;
                }
            }

            ControlMgr.Instance.StartupUserControl.ShowExamPage(examDuration);
        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.StartupUserControl.ShowExamHistoryPage();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.startButton.Focus();
        }
    }
}
