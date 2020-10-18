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
using System.Diagnostics;

namespace SoonLearning.AppCenter.Windows
{
    /// <summary>
    /// Interaction logic for CommentWindow.xaml
    /// </summary>
    public partial class CommentWindow : Window
    {
        private string appId;

        public CommentWindow(string appId)
        {
            InitializeComponent();

            this.appId = appId;
            this.sendButton.IsEnabled = false;
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataMgr.Instance.AppService.AddTopic(this.appId,
                    Convert.ToInt32(DataMgr.Instance.LoginInfo.Id), 
                    this.commentTextBox.Text, 
                    LoginInfo.GetMD5Hash("#$32*d_&"));

                this.Close();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                MessageBox.Show("发表评论失败，请稍后再试！", "速学应用平台", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void x_Name__cancelBtn__Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void commentTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.textCountTextBlock.Text = this.commentTextBox.Text.Length.ToString();
            this.sendButton.IsEnabled = this.commentTextBox.Text.Length > 0;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoginWindow loginCtrl = new LoginWindow();
                loginCtrl.ShowDirectLogin = false;
                bool? ret = loginCtrl.ShowDialog();
                if (ret != null)
                {
                    if (!ret.Value)
                    {
                        this.Close();
                        return;
                    }
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            finally
            {
                App.Current.MainWindow.Activate();
            }
        }
    }
}
