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
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.UserControls
{
    /// <summary>
    /// Interaction logic for LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        public LoginUserControl()
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
                    MessageBox.Show("请输入用户编号！如果您还没有快学账号，请登录快学网站注册。", "快学", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (string.IsNullOrEmpty(this.passwordTextBox.Password))
                {
                    MessageBox.Show("请输入密码！", "快学", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DataMgr.Instance.LoginInfo.Password = this.passwordTextBox.Password;
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
                        MessageBox.Show(strBuilder.ToString(), "快学", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录失败，请稍后重试！", "快学", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            GC_UIHelper.CloseMessageWindow();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            GC_UIHelper.CloseMessageWindow();
        }
    }
}
