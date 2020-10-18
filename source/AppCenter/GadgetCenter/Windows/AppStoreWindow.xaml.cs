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
using SoonLearning.AppCenter.UserControls;
using SoonLearning.AppCenter.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace SoonLearning.AppCenter.Windows
{
    /// <summary>
    /// Interaction logic for AppStoreWindow.xaml
    /// </summary>
    public partial class AppStoreWindow
    {
        public AppStoreWindow()
        {
            InitializeComponent();
        }

        private void AppWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!MainWindow.CanExit)
            {
                this.Hide();
                e.Cancel = true;
                return;
            }

            DataMgr.Instance.LoginInfo.Id = "0";
            this.containerFrame.Content = null;
        }

        private void AppWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.ShowControl(ControlMgr.Instance.AppManagementUserControl);

       //     if (DataMgr.Instance.LoginInfo.Id == "0")
            {
            //    if (!login())
            //        return;
            }

            if (DataMgr.Instance.OnlineTypeCollection.Count == 0)
            {
                BackgroundWorker initAppTypeWorker = new BackgroundWorker();
                initAppTypeWorker.DoWork += ((s1, e1) =>
                {
                    try
                    {
                        string[] result = DataMgr.Instance.AppService.GetAppClass(LoginInfo.GetMD5Hash("$af3#d_&"));
                        e1.Result = result;
                    }
                    catch (Exception ex)
                    {
                        e1.Result = ex;
                    }
                });
                initAppTypeWorker.RunWorkerCompleted += ((s1, e1) =>
                {
                    this.loadingAnimationCtrl.Visibility = System.Windows.Visibility.Hidden;
                    if (e1.Result != null)
                    {
                        if (e1.Result is Exception)
                        {
                            this.loadGadgetInfoTextBlock.Visibility = System.Windows.Visibility.Visible;
                            this.loadGadgetInfoTextBlock.Text = string.Format("加载分类失败：{0}", ((Exception)e1.Result).Message);
                        }
                        else
                        {
                            this.GetMainTypes(e1.Result as string[]);
                            if (this.typeListBox.SelectedIndex < 0)
                            {
                                this.typeListBox.SelectedIndex = 0;
                            }
                        }
                    }
                });
                initAppTypeWorker.RunWorkerAsync();
                this.loadingAnimationCtrl.Visibility = System.Windows.Visibility.Visible;
                this.loadGadgetInfoTextBlock.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                if (this.typeListBox.SelectedIndex < 0)
                    this.typeListBox.SelectedIndex = 0;
            }

            ControlMgr.Instance.AppManagementUserControl.AppItemSelectionChangeEvent += ((item) =>
            {
                if (item == null)
                    return;

                this.ShowControl(ControlMgr.Instance.AppDescriptionUserControl);
                ControlMgr.Instance.AppDescriptionUserControl.ShowAppItem(item);
                this.backButton.IsEnabled = true;
                this.forwardButton.IsEnabled = false;
            });
        }

        private void GetMainTypes(string[] typeList)
        {
            TypeItem allItem = new TypeItem("全部", string.Empty, string.Empty, TypeItem.All, TypeItem.Root);
            DataMgr.Instance.addOnlineTypeItem(allItem);
            allItem.SubTypeItems.Add(new TypeItem("最新", string.Empty, string.Empty, TypeItem.New, TypeItem.All));
            allItem.SubTypeItems.Add(new TypeItem("全部", string.Empty, string.Empty, TypeItem.All, TypeItem.All));

            foreach (string value in typeList)
            {
                string[] temp = value.Split(new char[] { '$' });
                int parentId = Convert.ToInt32(temp[1]);
                if (parentId == TypeItem.Root)
                {
                    TypeItem typeItem = new TypeItem(temp[0] as string,
                        string.Empty,
                        string.Empty,
                        Convert.ToInt32(temp[2]),
                        TypeItem.Root);

                    TypeItem localTypeItem = new TypeItem(temp[0] as string,
                        string.Empty,
                        string.Empty,
                        Convert.ToInt32(temp[2]),
                        TypeItem.Root);

                    DataMgr.Instance.addOnlineTypeItem(typeItem);
                    DataMgr.Instance.addLocalTypeItem(localTypeItem);

                    typeItem.SubTypeItems.Add(new TypeItem("最新", string.Empty, string.Empty, TypeItem.New, typeItem.Type));
                    typeItem.SubTypeItems.Add(new TypeItem("全部", string.Empty, string.Empty, TypeItem.All, typeItem.Type));
                }
            }

            foreach (string value in typeList)
            {
                string[] temp = value.Split(new char[] { '$' });
                int parentId = Convert.ToInt32(temp[1]);
                if (parentId == 0)
                    continue;

                foreach (TypeItem item in DataMgr.Instance.OnlineTypeCollection)
                {
                    if (parentId == item.Type)
                    {
                        item.SubTypeItems.Add(new TypeItem(temp[0] as string,
                            string.Empty,
                            string.Empty,
                            Convert.ToInt32(temp[2]),
                            parentId));

                        DataMgr.Instance.addLocalTypeItem(new TypeItem(temp[0] as string,
                            string.Empty,
                            string.Empty,
                            Convert.ToInt32(temp[2]),
                            parentId));

                        break;
                    }
                }
            }

            this.insertNotApprovedItem();

            // Create a type for installing apps
            TypeItem installingItem = new TypeItem("正在安装", string.Empty, string.Empty, TypeItem.Installing, TypeItem.Root);
            DataMgr.Instance.OnlineTypeCollection.Add(installingItem);

            DataMgr.Instance.saveLocalTypeCollection();
        }

        [Conditional("DEBUG")]
        private void insertNotApprovedItem()
        {
            TypeItem notApprovedItem = new TypeItem("待审核", string.Empty, string.Empty, TypeItem.NotApproved, TypeItem.Root);
            DataMgr.Instance.OnlineTypeCollection.Add(notApprovedItem);

            TypeItem allItem = new TypeItem("全部", string.Empty, string.Empty, TypeItem.All, TypeItem.NotApproved);
            notApprovedItem.SubTypeItems.Add(allItem);
        }

        private void minBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ShowControl(Control ctrl)
        {
            this.containerFrame.Content = ctrl;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.containerFrame.GoBack();
            this.backButton.IsEnabled = this.containerFrame.CanGoBack;
            this.forwardButton.IsEnabled = this.containerFrame.CanGoForward;
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            this.containerFrame.GoForward();
            this.backButton.IsEnabled = this.containerFrame.CanGoBack;
            this.forwardButton.IsEnabled = this.containerFrame.CanGoForward;
        }

        private void typeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataMgr.Instance.MainTags.Clear();
            TypeItem typeItem = this.typeListBox.SelectedItem as TypeItem;
            if (typeItem == null)
                return;

            if (typeItem.Type == TypeItem.Installing) // Show installing list
            {
                this.ShowControl(ControlMgr.Instance.AppInstallingListUserControl);
            }
            else
            {
                ControlMgr.Instance.AppManagementUserControl.TypeId = typeItem.Type;
                foreach (TypeItem subTypeItem in typeItem.SubTypeItems)
                {
                    DataMgr.Instance.MainTags.Add(subTypeItem);
                }

                this.ShowControl(ControlMgr.Instance.AppManagementUserControl);
                ControlMgr.Instance.AppManagementUserControl.SelectSubType();
            }
        }

        internal void SelectInstallTab()
        {
            foreach (TypeItem item in DataMgr.Instance.OnlineTypeCollection)
            {
                if (item.Type == TypeItem.Installing)
                {
                    this.typeListBox.SelectedItem = item;
                    break;
                }
            }
        }

        internal bool login()
        {
            LoginWindow loginCtrl = new LoginWindow();
            loginCtrl.ShowDirectLogin = false;
            bool? ret = loginCtrl.ShowDialog();
            if (ret != null)
                return ret.Value;

            return false;
        }

        private void AppWindow_StateChanged(object sender, EventArgs e)
        {
        }
    }
}
