using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace MemorizeAppCreator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.Resources.MergedDictionaries.Add(this.createRD(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Style/Data.xaml"));
            this.Resources.MergedDictionaries.Add(this.createRD(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Style/ColorTheme.xaml"));
            this.Resources.MergedDictionaries.Add(this.createRD(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Style/Icons.xaml"));
            this.Resources.MergedDictionaries.Add(this.createRD(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Style/Strings.xaml"));
            this.Resources.MergedDictionaries.Add(this.createRD(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Style/Button.xaml"));
            this.Resources.MergedDictionaries.Add(this.createRD(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Style/CheckBox.xaml"));
            this.Resources.MergedDictionaries.Add(this.createRD(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Style/Combobox.xaml"));
            this.Resources.MergedDictionaries.Add(this.createRD(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Style/Others.xaml"));
            this.Resources.MergedDictionaries.Add(this.createRD(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Style/ScollBar.xaml"));
            this.Resources.MergedDictionaries.Add(this.createRD(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Style/ListBox.xaml"));
        }

        private ResourceDictionary createRD(string url)
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri(url, UriKind.Absolute);
            return rd;
        }
    }
}
