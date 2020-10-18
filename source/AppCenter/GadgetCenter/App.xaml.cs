using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using SoonLearning.AppCenter.Data;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;
using SoonLearning.AppCenter.License;
using System.Text;
using SoonLearning.AppCenter.Windows;
using System.Threading;
using System.Globalization;

namespace SoonLearning.AppCenter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private bool uiStyleApplied = false;
        internal static string appCenterId = "CD7A915052E545A68614FC7DFAB59775";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!LicenseMgr.Instance.Init())
            {
                MessageBox.Show("找不到速学应用平台证书文件，请访问http://www.soonlearning.com获取！", "证书错误", MessageBoxButton.OK, MessageBoxImage.Stop);
                App.Current.Shutdown();
                return;
            }

            Thread.CurrentThread.CurrentUICulture = new CultureInfo("zh-CN");

            this.DispatcherUnhandledException += ((sender1, e1) =>
                {
                    StringBuilder strMsg = new StringBuilder();
                    strMsg.AppendLine("抱歉，速学应用平台发生异常:");
                    if (e1.Exception != null)
                    {
                        if (e1.Exception.InnerException != null)
                        {
                            strMsg.AppendLine(e1.Exception.InnerException.Message);
                            strMsg.AppendLine(e1.Exception.InnerException.StackTrace);
                        }
                        else
                        {
                            strMsg.AppendLine(e1.Exception.Message);
                            strMsg.AppendLine(e1.Exception.StackTrace);
                        }                        
                    }
                    MessageBox.Show(strMsg.ToString(), "未处理的异常", MessageBoxButton.OK, MessageBoxImage.Error);
                    App.Current.Shutdown();
                });

            //try
            //{
            //    uiStyleSetting = UIStyleSetting.Load();
            //}
            //catch
            //{
            //    uiStyleSetting = new UIStyleSetting();
            //    uiStyleSetting.UIStyle = 1;
            //}

            if (!uiStyleApplied)
            {
             //   uiStyleSetting.UIStyle = 1;
            }

         //   this.ApplySkin(new Uri(@"/Resources/Black/BlackStyle.xaml", UriKind.Relative));

        //    App.Current.MainWindow = new MainWindow();
        //    App.Current.MainWindow.Show();
        }

        public void ApplySkin(Uri skinDictionaryUri)
        {
            ResourceDictionary skinDict = Application.LoadComponent(skinDictionaryUri) as ResourceDictionary;

            Collection<ResourceDictionary> mergedDicts = base.Resources.MergedDictionaries;

            if (mergedDicts.Count > 0)
                mergedDicts.RemoveAt(0);

            mergedDicts.Insert(0, skinDict);

            uiStyleApplied = true;
        }
    }
}
