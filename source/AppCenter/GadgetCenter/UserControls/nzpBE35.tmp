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
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Utility;
using System.Threading;
using System.ComponentModel;

namespace SoonLearning.AppCenter.UserControls
{
    /// <summary>
    /// Interaction logic for AppInstallingListUserControl.xaml
    /// </summary>
    public partial class AppInstallingListUserControl : UserControl
    {
        private PropertyChangedEventHandler AppInstallItemPropertyChangedHandler;

        public AppInstallingListUserControl()
        {
            InitializeComponent();

            this.AppInstallItemPropertyChangedHandler = ((s, e1) =>
            {
                if (e1.PropertyName == "State")
                {
                    this.UpdateButtonState(s as AppInstallItem);
                }
            });
        }

        private void cancelPenddingAppButton_Click(object sender, RoutedEventArgs e)
        {
            AppInstallItem item = this.pendingListBox.SelectedItem as AppInstallItem;
            if (item == null)
                return;

            AppInstallMgr.Instance.Cancel(item.AppItem.Id);
        }

        private void pendingListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AppInstallItem item = this.pendingListBox.SelectedItem as AppInstallItem;
            if (item == null)
            {
                this.cancelPenddingAppButton.IsEnabled = false;
                this.retryButton.IsEnabled = false;
                this.deleteButton.IsEnabled = false;
            }
            else
            {
                item.PropertyChanged -= this.AppInstallItemPropertyChangedHandler;
                item.PropertyChanged += this.AppInstallItemPropertyChangedHandler;

                this.UpdateButtonState(item);
            }
        }

        private void UpdateButtonState(AppInstallItem item)
        {
            this.cancelPenddingAppButton.IsEnabled = true;
            if (item.State == InstallState.DownloadFail ||
                (item.State >= InstallState.InstallFail &&
                item.State <= InstallState.InstallFail_PathTooLongException))
            {
                this.retryButton.IsEnabled = true;
            }
            else
            {
                this.retryButton.IsEnabled = false;
            }

            if (item.State != InstallState.Downloading &&
                item.State != InstallState.Installing)
            {
                this.cancelPenddingAppButton.IsEnabled = false;
            }

            if (item.State != InstallState.Installing &&
                item.State != InstallState.Downloading)
                this.deleteButton.IsEnabled = true;
            else
                this.deleteButton.IsEnabled = false;
        }

        private void retryButton_Click(object sender, RoutedEventArgs e)
        {
            AppInstallItem item = this.pendingListBox.SelectedItem as AppInstallItem;
            if (item == null)
                return;

            AppInstallMgr.Instance.Retry(item.AppItem.Id);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            AppInstallItem item = this.pendingListBox.SelectedItem as AppInstallItem;
            if (item == null)
                return;

            AppInstallMgr.Instance.Remove(item);
        }
    }
}
