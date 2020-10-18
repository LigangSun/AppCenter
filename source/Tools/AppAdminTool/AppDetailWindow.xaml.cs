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
using AppAdminTool.ServiceReference1;

namespace AppAdminTool
{
    /// <summary>
    /// Interaction logic for AppDetailWindow.xaml
    /// </summary>
    public partial class AppDetailWindow : Window
    {
        private APKSoftModel app;
        public AppDetailWindow(APKSoftModel app)
        {
            InitializeComponent();

            this.showProperty("名称", app.Name.ToString());
            this.showProperty("价格", app.Price.ToString());
            this.showProperty("版本", app.Version.ToString());
            this.showProperty("上传时间", app.AddDate.ToString());
            this.showProperty("应用分类", app.ClassID.ToString());
            this.showProperty("应用子分类", app.SubClassID.ToString());
            this.showProperty("短描述", app.Sketch.ToString());
            this.showProperty("描述", app.Detail.ToString());

            if (app.Status == 0)
            {
                this.offlineButton.IsEnabled = false;
                this.onlineButton.IsEnabled = false;
            }
            else if (app.Status == 1)
            {
                this.approveButton.IsEnabled = false;
                this.rejectButton.IsEnabled = false;
                this.onlineButton.IsEnabled = false;
            }
            else if (app.Status == -2)
            {
                this.approveButton.IsEnabled = false;
                this.rejectButton.IsEnabled = false;
                this.offlineButton.IsEnabled = false;
            }

            this.thumbnailImage.Source = new BitmapImage(new Uri(app.ICON, UriKind.Absolute));

            this.app = app;
        }

        private void showProperty(string name, string value)
        {
            this.detailListBox.Items.Add(name + ": " + value);
        }

        private void installButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void approveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.AppService.Admin_Approved(this.app.UniqueId, AdminInformation.adminUserName, AdminInformation.adminPassword, AdminInformation.adminKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "应用管理", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void rejectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommentWindow commendWnd = new CommentWindow();
                if (commendWnd.ShowDialog().Value)
                {
                    MainWindow.AppService.Admin_Reject(this.app.UniqueId,
                        AdminInformation.adminUserName, 
                        AdminInformation.adminPassword,
                        commendWnd.Comment, 
                        AdminInformation.adminKey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "应用管理", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void offlineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.AppService.Admin_SetStatus(this.app.UniqueId, AdminInformation.adminUserName, AdminInformation.adminPassword, -2, AdminInformation.adminKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "应用管理", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void onlineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.AppService.Admin_SetStatus(this.app.UniqueId, AdminInformation.adminUserName, AdminInformation.adminPassword, 1, AdminInformation.adminKey);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "应用管理", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
