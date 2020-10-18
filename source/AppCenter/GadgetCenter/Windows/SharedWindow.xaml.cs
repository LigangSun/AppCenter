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
using System.Deployment.Application;
using System.Diagnostics;

namespace SoonLearning.AppCenter.Windows
{
    /// <summary>
    /// Interaction logic for SharedWindow.xaml
    /// </summary>
    public partial class SharedWindow : Window
    {
        private string appId = string.Empty;

        public SharedWindow(string appId)
        {
            InitializeComponent();
            this.appId = appId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if (DataMgr.Instance.LoginInfo.Id == "0")
            //{
            //    LoginWindow loginWnd = new LoginWindow();
            //    loginWnd.ShowDialog();
            //}

            Process.Start(string.Format(@"http://www.soonlearning.com/sharedpage2.aspx?AppUniqueId={0}&SharedUID={1}", this.appId, DataMgr.Instance.LoginInfo.LoginId));
            this.Close();
        }

        private void x_Name__cancelBtn__Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
