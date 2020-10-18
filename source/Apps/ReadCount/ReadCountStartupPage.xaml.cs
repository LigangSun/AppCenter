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
using System.Threading;

namespace SoonLearning.ReadCount
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ReadCountStartupPage : UserControl
    {
        private static ReadCountStartupPage startupControl;

        public static ReadCountStartupPage Instance
        {
            get
            {
                Interlocked.CompareExchange<ReadCountStartupPage>(ref startupControl, new ReadCountStartupPage(), null);
                return startupControl;
            }
        }

        private ReadCountPage readCountPage;

        private string dataBasePath = string.Empty;

        public string DataBasePath
        {
            set { this.dataBasePath = value; }
            internal get
            {
                return this.dataBasePath;
            }
        }

        public ReadCountStartupPage()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.readCountPage == null)
            {
                readCountPage = new ReadCountPage();
                rootGrid.Children.Add(readCountPage);
            }
        }

        public void Uninstall()
        {
            try
            {
                System.IO.Directory.Delete(this.DataBasePath, true);
            }
            catch
            {
            }
        }
    }
}
