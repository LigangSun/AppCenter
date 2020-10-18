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
using System.Reflection;

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            AssemblyName assemblyName = assembly.GetName();
            this.showInfo("速学基础教育应用创建工具");
            this.showInfo("版本: " + assemblyName.Version.ToString());
            this.showInfo("上海速学信息科技有限公司 版权所有 @2012");
        }

        private void showInfo(string value)
        {
            this.infoListBox.Items.Add(value);
        }
    }
}
