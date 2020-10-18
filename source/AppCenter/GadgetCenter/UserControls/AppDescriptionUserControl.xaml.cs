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
using System.ComponentModel;
using System.Data;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter.Windows;
using System.Diagnostics;

namespace SoonLearning.AppCenter.UserControls
{
    /// <summary>
    /// Interaction logic for AppDescriptionUserControl.xaml
    /// </summary>
    public partial class AppDescriptionUserControl : UserControl
    {
        private AppItem appItem = null;

        public AppDescriptionUserControl()
        {
            InitializeComponent();

            AppInstallMgr.Instance.AppInstallCompletedEvent += (s, e) =>
            {
                this.addButton.Content = "打开";
            };
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            GadgetItemOnline item = this.DataContext as GadgetItemOnline;

            this.appItem = DataMgr.Instance.getAppItemById(item.UniqueId);
            var query = from temp in AppInstallMgr.Instance.AppInstallItems
                        where temp.AppItem.UniqueId == item.UniqueId
                        select temp;
            if (appItem != null)
            {
                MainWindow mainWnd = App.Current.MainWindow as MainWindow;
                mainWnd.Activate();
                mainWnd.LoadGadget(this.appItem);

                foreach (Window wnd in App.Current.Windows)
                {
                    if (wnd is AppStoreWindow)
                    {
                        wnd.Close();
                    }
                }
            }
            else
            {
                if (query.Count() > 0)
                {
                    this.addButton.Content = "正在安装";
                }
                //else if (item.Price > 0)
                //{

                //}
                else
                {
                    AppInstallMgr.Instance.Start(this.DataContext as GadgetItemOnline);
                    this.addButton.Content = "正在安装";
                }
            }
        }

        internal void ShowAppItem(GadgetItemOnline item)
        {
            this.DataContext = item;

            this.createTimeTextBlock.Text = item.CreateDate.ToLocalTime().ToString();
            this.priceTextBlock.Text = string.Format("{0}元", item.Price.ToString()) + "（推广期，免费使用）";

            this.ShowCategory(item);

            this.appItem = DataMgr.Instance.getAppItemById(item.UniqueId);
            var query = from temp in AppInstallMgr.Instance.AppInstallItems
                        where temp.AppItem.UniqueId == item.UniqueId
                        select temp;
            if (appItem != null)
            {
                this.addButton.Content = "打开";
            }
            else
            {
                if (query.Count() > 0)
                {
                    this.addButton.Content = "正在安装";
                }
                //else if (item.Price > 0)
                //{
                    
                //}
                else
                {
                    this.addButton.Content = "添加";
                }
            }

            this.snapshotListBox.ItemsSource = null;
            this.authorTextBlock.Text = string.Empty;

            UserPaidInfo userPaidInfo = null;

            if (item.SnapshotList.Count == 0)
            {
                this.IsEnabled = false;

                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerSupportsCancellation = true;
                worker.WorkerReportsProgress = true;
                worker.DoWork += ((s, e) =>
                    {
                        string strRet = DataMgr.Instance.AppService.GetAppAttachInfo(Convert.ToInt32(item.Id), item.CreatorId, LoginInfo.GetMD5Hash("4%!@s*&d"));
                        if (!string.IsNullOrEmpty(strRet))
                        {
                            string[] strRets = strRet.Split(new string[] { "||" }, StringSplitOptions.None);
                            item.Creator = strRets[0];
                            if (strRets.Length > 1)
                            {
                                item.SnapshotList.Add(new AppImage(strRets[1]));
                                if (strRets.Length > 2)
                                    item.SnapshotList.Add(new AppImage(strRets[2]));
                                if (strRets.Length > 3)
                                    item.SnapshotList.Add(new AppImage(strRets[3]));
                                if (strRets.Length > 4)
                                    item.SnapshotList.Add(new AppImage(strRets[4]));
                                if (strRets.Length > 5)
                                    item.SnapshotList.Add(new AppImage(strRets[5]));
                            }
                        }

                        //try
                        //{
                        //    userPaidInfo = DataMgr.Instance.AppService.GetUserPaidInfo(item.UniqueId,
                        //        DataMgr.Instance.LoginInfo.LoginId,
                        //        DataMgr.Instance.LoginInfo.Password,
                        //        LoginInfo.GetMD5Hash("##32*d_&"));
                        //}
                        //catch (Exception ex)
                        //{
                        //    Debug.Assert(false, ex.Message);
                        //}
                    });
                worker.RunWorkerCompleted += ((s, e) =>
                    {
                        this.snapshotListBox.ItemsSource = item.SnapshotList;
                        this.authorTextBlock.Text = item.Creator;
                        this.IsEnabled = true;

                        if (item.Price > 0 && userPaidInfo != null)
                        {
                            // Temp no need paid
                            if (userPaidInfo == null || userPaidInfo.Paid)
                            {
                                this.addButton.Content = "添加";
                            }
                            else
                            {
                                this.addButton.Content = "购买";
                                Style style = App.Current.TryFindResource("ButtonStyle_IconBtn_Single") as Style;
                                if (style != null)
                                    this.addButton.Style = style;
                            }
                        }
                    });
                worker.RunWorkerAsync();
            }
            else
            {
                this.snapshotListBox.ItemsSource = item.SnapshotList;
                this.authorTextBlock.Text = item.Creator;
            }
        }

        private void ShowCategory(GadgetItemOnline item)
        {
            TypeItem typeItem = DataMgr.Instance.LocalTypeCollection.GetById(item.AppType);
            TypeItem subTypeItem = DataMgr.Instance.LocalTypeCollection.GetById(item.AppSubType);
            if (typeItem != null)
                this.categroyTextBlock.Text = typeItem.Title;
            if (subTypeItem != null)
            {
                this.categroyTextBlock.Text += " >> ";
                this.categroyTextBlock.Text += subTypeItem.Title;
            }
        }

        private void AppendImage(GadgetItemOnline item, DataRow dr, string key)
        {
            if (dr[key] != null)
            {
                item.SnapshotList.Add(new AppImage(dr[key].ToString()));
            }
        }
    }
}
