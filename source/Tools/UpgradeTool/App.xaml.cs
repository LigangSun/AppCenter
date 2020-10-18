using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace SoonLearning.UpgradeTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static string appCenterName = string.Empty;
        internal static string version = string.Empty;
        internal static string versionState = string.Empty;
        internal static string packFile = string.Empty;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length != 4)
            {
                MessageBox.Show("更新参数错误，自动更新不能启动。", "自动更新", MessageBoxButton.OK, MessageBoxImage.Error);
                App.Current.Shutdown();
                return;
            }

            App.appCenterName = e.Args[0];
            App.version = e.Args[1];
            App.versionState = e.Args[2];
            App.packFile = e.Args[3];
        }
    }
}
