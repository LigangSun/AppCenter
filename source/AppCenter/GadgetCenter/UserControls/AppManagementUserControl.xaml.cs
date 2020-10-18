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
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.IO;
using System.Data;
using System.Windows.Threading;
using System.ServiceModel.Description;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Windows;

namespace SoonLearning.AppCenter.UserControls
{
    public delegate void AppItemSelectionChangeDelegate(GadgetItemOnline item);

    /// <summary>
    /// Interaction logic for AppManagementUserControl.xaml
    /// </summary>
    public partial class AppManagementUserControl : UserControl
    {
        private ListCollectionView appListCollectionView;
        private List<object> appCollection = new List<object>();
        private List<GadgetItemOnline> gadgetItemOnlineList = new List<GadgetItemOnline>();
        private Dictionary<string, List<GadgetItemOnline>> appItemOnlineDictionary = new Dictionary<string, List<GadgetItemOnline>>();
        private Dictionary<string, int> totalPageDictionary = new Dictionary<string, int>();

        private ObservableCollection<GadgetItemOnline> installingCollection = new ObservableCollection<GadgetItemOnline>();

        public event AppItemSelectionChangeDelegate AppItemSelectionChangeEvent;

        private object appCollectionLocker = new object();

        private bool loadOnlineApps = false;

        private int pageIndex = 1;
        private int totalPage = 0;
        private int totalRecord = 0;
        private ObservableCollection<int> pageIndexCollection = new ObservableCollection<int>();

        private int typeId = 0;
        private int subTypeId = 0;

        private const int maxRetriveCount = 20;

        internal int TypeId
        {
            get { return this.typeId; }
            set { this.typeId = value; }
        }

        internal int SubTypeId
        {
            get { return this.subTypeId; }
            set { this.subTypeId = value; }
        }

        public AppManagementUserControl()
        {
            InitializeComponent();

            this.appListBox.ItemsSource = this.appCollection;

            this.appListCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(this.appCollection);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription();
            groupDescription.PropertyName = "SubTypeName";
            this.appListCollectionView.GroupDescriptions.Add(groupDescription);
            this.FilterApps();

            AppInstallMgr.Instance.Dispatcher = this.Dispatcher;

            this.pageComboBox.ItemsSource = this.pageIndexCollection;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private bool app_Filter(object item)
        {
            TypeItem typeItem = this.headerListBox.SelectedItem as TypeItem;
            if (typeItem == null)
                return false;

            if (item is AppItem)
            {
                AppItem gadgetItem = item as AppItem;
                if (typeItem.Type == gadgetItem.Type)
                    return true;
            }
            else
            {
                GadgetItemOnline onlineItem = item as GadgetItemOnline;               
                if (typeItem.Type == onlineItem.AppSubType ||
                    typeItem.Type < 0)
                    return true;
            }

            return false;
        }

        private void FilterApps()
        {
            if (this.appListCollectionView == null)
                return;

            this.appListCollectionView.Filter -= app_Filter;
            this.appListCollectionView.Filter += new Predicate<object>(app_Filter);
        }

        private void headerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.headerListBox == null || this.headerListBox.SelectedIndex < 0)
                return;

            this.UpdateButtonStatus();

            lock (this.appCollectionLocker)
            {
                this.appCollection.Clear();
            }

            TypeItem mainTag = this.headerListBox.SelectedItem as TypeItem;
            this.pageIndex = 1;
            
          //  this.appItemOnlineDictionary.Clear();
            this.SubTypeId = mainTag.Type;
            
            string key = getKey(this.typeId, this.subTypeId, this.pageIndex);
            if (this.totalPageDictionary.ContainsKey(key))
                this.totalPage = this.totalPageDictionary[key];

            if (this.pageIndex == 1)
            {
                this.pageIndexCollection.Clear();
                for (int i = 0; i < totalPage; i++)
                    this.pageIndexCollection.Add(i + 1);

                if (this.pageIndexCollection.Count > 0)
                    this.pageComboBox.SelectedIndex = 0;
                else
                    LoadOnlineApps();
            }
            else
            {
            }

            this.UpdatePageButtonStatus();
            
            this.FilterApps();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GC_UIHelper.CloseMessageWindow();
        }

        private void typeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.FilterApps();
        }

        private void LoadOnlineApps()
        {
            string key = getKey(this.typeId, this.subTypeId, this.pageIndex);
            if (this.appItemOnlineDictionary.ContainsKey(key))
            {
                if (this.appItemOnlineDictionary[key].Count != 0)
                {
                    lock (this.appCollectionLocker)
                    {
                        this.appCollection.Clear();
                        this.appCollection.AddRange(this.appItemOnlineDictionary[key]);
                    }

                    this.FilterApps();

                    return;
                }
            }

            if (this.loadOnlineApps)
                return;

#if DEBUG
            if (TypeItem.NotApproved == this.typeId)
            {
                AdminLoginWindow adminLoginWindow = new AdminLoginWindow();
                if (!adminLoginWindow.ShowDialog().Value)
                {
                    return;
                }
            }
#endif

            this.loadOnlineApps = true;

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();

            this.loadingAnimationCtrl.Visibility = System.Windows.Visibility.Visible;
            this.loadGadgetInfoTextBlock.Visibility = System.Windows.Visibility.Hidden;
        }

        private bool login()
        {
            LoginWindow loginCtrl = new LoginWindow();
            bool? ret = loginCtrl.ShowDialog();
            if (ret != null)
                return ret.Value;

            return false;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string method = "DownNum";
                if (this.SubTypeId == TypeItem.New ||
                    this.TypeId == TypeItem.New)
                    method = "AddDate";

                DataTable dt = null;
                if (this.TypeId != TypeItem.NotApproved)
                {
                    dt = DataMgr.Instance.AppService.GetAppList(pageIndex,
                             maxRetriveCount,
                             Convert.ToInt32(DataMgr.Instance.LoginInfo.Id),
                             this.TypeId,
                             this.SubTypeId,
                             method,
                             ref totalPage,
                             ref totalRecord,
                             LoginInfo.GetMD5Hash("$df@#d^&"));
                }
                else
                {
#if DEBUG
                    dt = this.getNotApproveApps();
#endif
                }

                List<GadgetItemOnline> tempList = new List<GadgetItemOnline>();
                foreach (DataRow dr in dt.Rows)
                {
                    Debug.WriteLine("{0}", dr);
                    GadgetItemOnline itemOnline = new GadgetItemOnline();
                    itemOnline.Id = dr["ID"].ToString();
                    itemOnline.Thumbnail = dr["ICON"] as string;
                    itemOnline.PackageUrl = dr["FileUrl"] as string;
                    itemOnline.Title = dr["Name"] as string;
                    itemOnline.AppType = Convert.ToInt32(dr["ClassID"]);
                    itemOnline.AppSubType = Convert.ToInt32(dr["SubClassID"]);
                    itemOnline.Description = dr["Sketch"] as string;
                    itemOnline.LongDescription = dr["Detail"] as string;
                    itemOnline.CreateDate = Convert.ToDateTime(dr["AddDate"]);
                    itemOnline.CreatorId = Convert.ToInt32(dr["UserID"]);
                    itemOnline.Version = dr["Version"] as string;
                    itemOnline.UniqueId = dr["UniqueId"] as string;
                    itemOnline.Price = Convert.ToSingle(dr["Price"]);
                    if (!string.IsNullOrEmpty(itemOnline.UniqueId))
                        itemOnline.UniqueId = itemOnline.UniqueId.Trim();

                    TypeItem typeItem = DataMgr.Instance.LocalTypeCollection.GetById(itemOnline.AppSubType);
                    if (typeItem != null)
                        itemOnline.SubTypeName = typeItem.Title;
                    else
                        itemOnline.SubTypeName = string.Empty;

                    //if (itemOnline.AppSubType == 202 &&
                    //    itemOnline.AppType == 200)
                    //    itemOnline.Thumbnail = @"pack://application:,,,/Resources/Images/GradeMath.png";

                    //if (itemOnline.AppSubType == 207 &&
                    //   itemOnline.AppType == 200)
                    //    itemOnline.Thumbnail = @"pack://application:,,,/Resources/Images/FastCalc.png";

                    var queryItem = from item in tempList
                                    where item.UniqueId == itemOnline.UniqueId
                                    select item;
                    if (queryItem.Count() == 0)
                        tempList.Add(itemOnline);
                }

                string key = getKey(typeId, subTypeId, pageIndex);
                if (this.appItemOnlineDictionary.ContainsKey(key))
                {
                    this.totalPageDictionary[key] = this.totalPage;
                    this.appItemOnlineDictionary[key] = tempList;
                }
                else
                {
                    this.totalPageDictionary.Add(key, this.totalPage);
                    this.appItemOnlineDictionary.Add(key, tempList);
                }

                foreach (DataColumn column in dt.Columns)
                    Console.WriteLine(column.ColumnName);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            Thread.Sleep(0);
        }

#if DEBUG
        private DataTable getNotApproveApps()
        {
            return DataMgr.Instance.AppService.Admin_GetNotApprovedAppList(this.pageIndex, 30,
                    AdminInformation.adminUserName,
                    AdminInformation.adminPassword,
                    ref totalPage, ref totalRecord,
                    AdminInformation.adminKey);
        }
#endif

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                lock (this.appCollectionLocker)
                {
                    string key = getKey(this.typeId, this.subTypeId, this.pageIndex);
                    if (this.totalPageDictionary.ContainsKey(key))
                        this.totalPage = this.totalPageDictionary[key];

                    if (this.pageIndex == 1)
                    {
                        this.pageIndexCollection.Clear();
                        for (int i = 0; i < totalPage; i++)
                            this.pageIndexCollection.Add(i + 1);
                        if (this.pageIndexCollection.Count > 0)
                            this.pageComboBox.SelectedIndex = 0;
                    }

                    this.appCollection.Clear();
                    if (this.appItemOnlineDictionary.ContainsKey(key))
                        this.appCollection.AddRange(this.appItemOnlineDictionary[key]);

                    this.UpdatePageButtonStatus();
                }

                this.FilterApps();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            if (e.Result != null)
            {
                this.loadGadgetInfoTextBlock.Visibility = System.Windows.Visibility.Visible;
                this.loadGadgetInfoTextBlock.Text = ((Exception)e.Result).Message;
            }
            
            this.loadingAnimationCtrl.Visibility = System.Windows.Visibility.Hidden;

            this.loadOnlineApps = false;
        }

        private void uninstallButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void installButton_Click(object sender, RoutedEventArgs e)
        {
            this.StartInstall();
            this.openInstallList();
        }

        private void viewInstallingListButton_Click(object sender, RoutedEventArgs e)
        {
            this.openInstallList();
        }

        private void openInstallList()
        {
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    this.penddingPopup.PlacementTarget = this.viewInstallingListButton;
                    this.penddingPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
                    this.penddingPopup.IsOpen = true;
                }),
                DispatcherPriority.Normal,
                null);
        }

        private void appListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateButtonStatus();

            if (this.appListBox == null || this.appListBox.SelectedIndex < 0)
                return;

            AppItemSelectionChangeDelegate temp = this.AppItemSelectionChangeEvent;
            if (temp != null)
            {
                temp(this.appListBox.SelectedItem as GadgetItemOnline);
            }

            this.appListBox.SelectedIndex = -1;
        }

        private void UpdateButtonStatus()
        {
            if (this.installButton == null)
                return;

            this.installButton.IsEnabled = (this.appListBox.SelectedIndex >= 0);
        }

        private void StartInstall()
        {
            GadgetItemOnline item = this.appListBox.SelectedItem as GadgetItemOnline;
            if (item == null)
                return;

            AppInstallMgr.Instance.Start(item);
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
        }

        private void closePopupButton_Click(object sender, RoutedEventArgs e)
        {
            this.penddingPopup.IsOpen = false;
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
            }
            else
            {
                this.cancelPenddingAppButton.IsEnabled = true;
                if (item.State == InstallState.DownloadFail ||
                    item.State == InstallState.InstallFail)
                {
                    this.retryButton.IsEnabled = true;
                }
                else
                {
                    this.retryButton.IsEnabled = false;
                }
            }
        }

        private void retryButton_Click(object sender, RoutedEventArgs e)
        {
            AppInstallItem item = this.pendingListBox.SelectedItem as AppInstallItem;
            if (item == null)
                return;

            AppInstallMgr.Instance.Retry(item.AppItem.Id);
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
         //   this.penddingPopup.IsOpen = false;
        }

        private void appListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void pageIndexListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void prePageButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.pageComboBox.SelectedIndex <= 0)
                return;

            this.pageComboBox.SelectedIndex--;
            this.UpdatePageButtonStatus();
        }

        private void pageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.pageComboBox.SelectedIndex < 0)
                return;

            //if (this.pageIndex == this.pageComboBox.SelectedIndex + 1)
            //    return;

            this.pageIndex = this.pageComboBox.SelectedIndex + 1;
            this.LoadOnlineApps();
        }

        private void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.pageComboBox.Items.Count == 0)
                return;

            this.pageComboBox.SelectedIndex++;
            this.UpdatePageButtonStatus();
        }

        private void UpdatePageButtonStatus()
        {
            if (this.pageComboBox.Items.Count == 0)
            {
                this.prePageButton.IsEnabled = false;
                this.nextPageButton.IsEnabled = false;
                return;
            }

            if (this.pageComboBox.SelectedIndex == 0)
                this.prePageButton.IsEnabled = false;
            else
                this.prePageButton.IsEnabled = true;

            if (this.pageComboBox.SelectedIndex == this.pageComboBox.Items.Count - 1)
                this.nextPageButton.IsEnabled = false;
            else
                this.nextPageButton.IsEnabled = true;
        }

        internal void SelectSubType()
        {
            if (this.headerListBox.SelectedIndex < 0 && 
                this.headerListBox.Items.Count > 0)
            {
                this.headerListBox.SelectedIndex = 0;
            }
        }

        private string getKey(int type, int subType, int pageIndex)
        {
            return string.Format("{0}_{1}_{2}", type, subType, pageIndex);
        }
    }
}
