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
using System.Windows.Shapes;
using SoonLearning.AppCenter.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace SoonLearning.AppCenter.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private bool loginSuc = false;
        private string message = string.Empty;

        internal bool ShowDirectLogin
        {
            set
            {
                if (value)
                    this.directLoginBtn.Visibility = System.Windows.Visibility.Visible;
                else
                    this.directLoginBtn.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public LoginWindow()
        {
            InitializeComponent();

            this.DataContext = DataMgr.Instance.LoginInfo;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.idTextBox.Text))
                {
                    MessageBox.Show("请输入用户编号！如果您还没有速学账号，请登录速学网站注册。", "速学应用平台", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrEmpty(this.passwordTextBox.Password))
                {
                    MessageBox.Show("请输入密码！", "速学应用平台", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (DataMgr.Instance.LoginInfo.Password != this.passwordTextBox.Password)
                    DataMgr.Instance.LoginInfo.Password = LoginInfo.GetMD5Hash(this.passwordTextBox.Password);
                DataMgr.Instance.LoginInfo.Save();

                this.loginAnimationCtrl.Visibility = System.Windows.Visibility.Visible;
                this.controlGrid.IsEnabled = false;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.RunWorkerAsync();
            }
            catch
            {
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            DataMgr.Instance.LoginInfo.Id = "0";
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                this.loginSuc = false;
                string ret = DataMgr.Instance.AppService.GetLoginName(DataMgr.Instance.LoginInfo.LoginId,
                        DataMgr.Instance.LoginInfo.Password,
                        LoginInfo.GetMD5Hash("4%!@s*&d"));

                string[] rets = ret.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                if (rets != null && rets.Length > 1)
                {
                    if (rets[0] == "0")
                    {
                        StringBuilder strBuilder = new StringBuilder();
                        strBuilder.AppendLine("登录失败。");
                        strBuilder.AppendLine(rets[1]);
                        this.message = strBuilder.ToString();
                    }
                    else
                    {
                        DataMgr.Instance.LoginInfo.Id = rets[0];
                        this.loginSuc = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.message = "登录失败，请稍后重试！";
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.loginAnimationCtrl.Visibility = System.Windows.Visibility.Hidden;
            this.controlGrid.IsEnabled = true;
            if (this.loginSuc)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show(this.message, "速学应用平台", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void regLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
            e.Handled = true;
        }

        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.idTextBox.Text != DataMgr.Instance.LoginInfo.LoginId)
                this.passwordTextBox.Password = string.Empty;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataMgr.Instance.LoginInfo.Remember)
                this.passwordTextBox.Password = DataMgr.Instance.LoginInfo.Password;
        }

        private void directLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            DataMgr.Instance.LoginInfo.Id = "0";
            this.Close();
        }

        private void forgetPasswordLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {

        }
    }
}
