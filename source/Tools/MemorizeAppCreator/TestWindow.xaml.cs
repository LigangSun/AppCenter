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
using SoonLearning.Memorize.Data;
using SoonLearning.Memorize.UI;

namespace MemorizeAppCreator
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        private string projectFile;

        public TestWindow(string file)
        {
            InitializeComponent();
            this.projectFile = file;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                MemorizeEntry entry = MemorizeEntry.Load(this.projectFile);
                MemorizeControl.Instance.TestMode = false;
                MemorizeControl.Instance.Start(entry, "0");
                this.rootGrid.Children.Add(MemorizeControl.Instance.StartupUI);
            }
            catch (Exception ex)
            {
                StringBuilder strBuilder = new StringBuilder();
                strBuilder.AppendLine("测试失败");
                strBuilder.AppendLine(ex.Message);
                MessageBox.Show(strBuilder.ToString(), "记忆工具", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
