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

namespace SoonLearning.AppCenter.License
{
    /// <summary>
    /// Interaction logic for GetPreviewLicenseWindow.xaml
    /// </summary>
    public partial class GetPreviewLicenseWindow : Window
    {
        public GetPreviewLicenseWindow()
        {
            InitializeComponent();

            this.keyTextBox.Text = LicenseMgr.Instance.GetHardwardId();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
