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
using AppAdminTool.ServiceReference1;
using System.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AppAdminTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static AppServerSoapClient appService;

        internal static AppServerSoapClient AppService
        {
            get
            {
                if (appService == null)
                {
                    appService = new AppServerSoapClient();
                 //   appService.Endpoint.Address = new System.ServiceModel.EndpointAddress("http://www.soonlearning.com/WebServers/appserver.asmx");
                }

                return appService;
            }
        }

        private ObservableCollection<APKSoftModel> notApprovedAppCollection = new ObservableCollection<APKSoftModel>();
        private ObservableCollection<APKSoftModel> onlineAppCollection = new ObservableCollection<APKSoftModel>();
        private ObservableCollection<APKSoftModel> offlineAppCollection = new ObservableCollection<APKSoftModel>();

        public MainWindow()
        {
            InitializeComponent();

            this.notApprovedAppListBox.ItemsSource = notApprovedAppCollection;
            this.onlineAppListBox.ItemsSource = this.onlineAppCollection;
            this.offlineAppListBox.ItemsSource = this.offlineAppCollection;
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int totalPage = 0;
                int totalRecord = 0;
                DataTable dt = AppService.Admin_GetNotApprovedAppList(0, 30,
                    AdminInformation.adminUserName,
                    AdminInformation.adminPassword,
                    ref totalPage, ref totalRecord,
                    AdminInformation.adminKey);

                foreach (var app in getAppFromTable(dt))
                {
                    this.notApprovedAppCollection.Add(app);
                }

                for (int i = 1; i < totalPage; i++)
                {
                    dt = AppService.Admin_GetNotApprovedAppList(i, 30,
                        AdminInformation.adminUserName,
                        AdminInformation.adminPassword,
                        ref totalPage, ref totalRecord,
                        AdminInformation.adminKey);

                    foreach (var app in getAppFromTable(dt))
                    {
                        this.notApprovedAppCollection.Add(app);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "管理应用", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void viewDetailButton_Click(object sender, RoutedEventArgs e)
        {
            APKSoftModel app = this.notApprovedAppListBox.SelectedItem as APKSoftModel;
            if (app == null)
                return;

            AppDetailWindow wnd = new AppDetailWindow(app);
            wnd.ShowDialog();
        }

        private void refreshOnlineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int totalPage = 0;
                int totalRecord = 0;
                DataTable dt = AppService.GetAppList(0, 30,
                    0,
                    0,
                    0,
                    "AddDate",
                    ref totalPage, ref totalRecord,
                    Help.GetMD5Hash("$df@#d^&"));

                foreach (var app in getAppFromTable(dt))
                {
                    this.onlineAppCollection.Add(app);
                }

                for (int i = 1; i < totalPage; i++)
                {
                    dt = AppService.GetAppList(i, 30,
                                0,
                                0,
                                0,
                                "AddDate",
                                ref totalPage, ref totalRecord,
                                Help.GetMD5Hash("$df@#d^&"));

                    foreach (var app in getAppFromTable(dt))
                    {
                        this.onlineAppCollection.Add(app);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "管理应用", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void viewOnlineDetailButton_Click(object sender, RoutedEventArgs e)
        {
            APKSoftModel app = this.onlineAppListBox.SelectedItem as APKSoftModel;
            if (app == null)
                return;

            AppDetailWindow wnd = new AppDetailWindow(app);
            wnd.ShowDialog();
        }

        private void viewOfflineDetailButton_Click(object sender, RoutedEventArgs e)
        {
            APKSoftModel app = this.offlineAppListBox.SelectedItem as APKSoftModel;
            if (app == null)
                return;

            AppDetailWindow wnd = new AppDetailWindow(app);
            wnd.ShowDialog();
        }

        private void refreshOfflineButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int totalPage = 0;
                int totalRecord = 0;
                DataTable dt = AppService.GetOfflineAppList(0, 30,
                    0,
                    0,
                    0,
                    "AddDate",
                    ref totalPage, ref totalRecord,
                    Help.GetMD5Hash("$df@#d^&"));

                foreach (var app in getAppFromTable(dt))
                {
                    this.offlineAppCollection.Add(app);
                }

                for (int i = 1; i < totalPage; i++)
                {
                    dt = AppService.GetOfflineAppList(i, 30,
                                0,
                                0,
                                0,
                                "AddDate",
                                ref totalPage, ref totalRecord,
                                Help.GetMD5Hash("$df@#d^&"));

                    foreach (var app in getAppFromTable(dt))
                    {
                        this.offlineAppCollection.Add(app);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "管理应用", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private IEnumerable<APKSoftModel> getAppFromTable(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                APKSoftModel app = new APKSoftModel();
                app.Id = Convert.ToInt32(dr["ID"]);
                app.ICON = dr["ICON"] as string;
                app.FileUrl = dr["FileUrl"] as string;
                app.Name = dr["Name"] as string;
                app.ClassID = Convert.ToInt32(dr["ClassID"]);
                app.SubClassID = Convert.ToInt32(dr["SubClassID"]);
                app.Sketch = dr["Sketch"] as string;
                app.Detail = dr["Detail"] as string;
                app.AddDate = Convert.ToDateTime(dr["AddDate"]);
                app.UserID = Convert.ToInt32(dr["UserID"]);
                app.Version = dr["Version"] as string;
                app.UniqueId = dr["UniqueId"] as string;
                app.Status = Convert.ToInt32(dr["Status"]);

                yield return app;
            }
        }

        private void sendPlatformUpdateNotifyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AppService.Admin_SendNotify("Admin", Help.GetMD5Hash("SoonLearning_123"),
                                Help.GetMD5Hash("$eg@*d^&"));
            }
            catch (Exception  ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }
    }
}
