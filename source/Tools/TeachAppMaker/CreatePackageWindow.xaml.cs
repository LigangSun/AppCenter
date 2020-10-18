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
using System.Windows.Threading;
using SoonLearning.TeachAppMaker.Data;
using System.Threading;

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for CreatePackageWindow.xaml
    /// </summary>
    public partial class CreatePackageWindow : Window
    {
        private string packageFile = string.Empty;

        internal string PackageFile
        {
            get { return this.packageFile; }
        }

        public CreatePackageWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() => 
            {
                this.packageFile = ProjectMgr.Instance.CreateAppPackage();

                Thread.Sleep(1000);

                if (string.IsNullOrEmpty(this.packageFile))
                {
                    MessageBox.Show("打包应用失败。", "打包", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.DialogResult = false;
                }
                else
                {
                    this.DialogResult = true;
                }
                this.Close();
            }), 
            DispatcherPriority.Background, null);
        }
    }
}
