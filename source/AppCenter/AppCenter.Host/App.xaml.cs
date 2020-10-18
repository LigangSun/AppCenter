using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.AppCenter.License;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.AppCenter.Host
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string appDllFile;
        public static string entryType;

        public static UIStyleSetting styleSetting = UIStyleSetting.Instance;

        public static IGadgetEntry currentEntry;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!LicenseMgr.Instance.Init())
            {
                MessageBox.Show("找不到速学应用平台证书文件，请访问http://www.soonlearning.com获取！", "证书错误", MessageBoxButton.OK, MessageBoxImage.Stop);
                App.Current.Shutdown();
                return;
            }

            if (e.Args.Length < 5)
            {
                App.Current.Shutdown();
                return;
            }

            App.appDllFile = e.Args[0];
            if (!File.Exists(App.appDllFile))
            {
                App.Current.Shutdown();
                return;
            }

            bool recognizerEnabled = false;
            bool.TryParse(e.Args[2], out recognizerEnabled);
            SpeechHelper.Instance.RecognizerEnabled = recognizerEnabled;
            styleSetting.SpeechRecognizer = recognizerEnabled;

            bool fullScreen = false;
            bool.TryParse(e.Args[3], out fullScreen);
            styleSetting.FullScreen = fullScreen;

            bool openSound = false;
            bool.TryParse(e.Args[4], out openSound);
            styleSetting.OpenSound = openSound;

            App.entryType = e.Args[1];
        }
    }
}
