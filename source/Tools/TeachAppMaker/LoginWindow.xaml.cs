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
using SoonLearning.TeachAppMaker.Data;
using System.Diagnostics;

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string userId;
        private string password;

        public string UserId
        {
            get { return this.userId; }
        }

        public string Password
        {
            get { return this.password; }
        }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            userId = this.idTextBox.Text;
            password = this.passwordTextBox.Password;
            if (string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("请输入用户Id或者邮箱。", "登陆", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入密码。", "登陆", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Cursor cursor = this.Cursor;
            try
            {
                this.Cursor = Cursors.Wait;
                AppServerSoapClient service = new AppServerSoapClient();
                service.Endpoint.Address = new System.ServiceModel.EndpointAddress(@"http://www.soonlearning.com/WebServers/AppServer.asmx");
                string result = service.GetLoginName(userId, Helper.GetMD5Hash(password), Helper.GetMD5Hash("4%!@s*&d"));
                if (!string.IsNullOrEmpty(result))
                {
                    string[] parts = result.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 3)
                    {
                        if (Convert.ToInt32(parts[2]) != 3)
                        {
                            MessageBox.Show("该账户不是教师账户，请先申请注册为教师账户，再上传应用。", "登陆", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        else
                        {
                            this.DialogResult = true;
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "上传应用", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.Cursor = cursor;
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Hyperlink link = sender as Hyperlink;
            Process.Start(link.NavigateUri.ToString());
        }
    }
}
