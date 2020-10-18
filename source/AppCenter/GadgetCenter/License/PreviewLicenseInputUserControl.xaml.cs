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
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.License
{
    /// <summary>
    /// Interaction logic for PreviewLicenseInputUserControl.xaml
    /// </summary>
    public partial class PreviewLicenseInputUserControl : UserControl
    {
        internal EventHandler<EventArgs> LicenseVerifiedEvent;

        public PreviewLicenseInputUserControl()
        {
            InitializeComponent();
        }

        private void getLicenseButton_Click(object sender, RoutedEventArgs e)
        {
            GetPreviewLicenseWindow wnd = new GetPreviewLicenseWindow();
            wnd.ShowDialog();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            GC_UIHelper.CloseMessageWindow();
            App.Current.Shutdown();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            bool ret = LicenseMgr.Instance.IsLicenseForCurrentMachine(this.licenseTextBox.Text, App.appCenterId);
            if (!ret)
            {
                MessageBox.Show("速学应用平台预览版证书格式错误，请访问http://www.soonlearning.com获取证书！", "证书错误", MessageBoxButton.OK, MessageBoxImage.Stop);
                this.licenseTextBox.SelectAll();
                this.licenseTextBox.Focus();
                return;
            }

            LicenseMgr.Instance.SaveLicenseToFile(this.licenseTextBox.Text, App.appCenterId);

            GC_UIHelper.CloseMessageWindow();

            if (this.LicenseVerifiedEvent != null)
            {
                this.LicenseVerifiedEvent(this, EventArgs.Empty);
            }
        }
    }
}
