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
using System.IO;
using System.Reflection;
using System.Windows.Media.Animation;
using SoonLearning.AppCenter.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using SoonLearning.AppCenter.Controls;
using System.Threading;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.UserControls
{
    /// <summary>
    /// Interaction logic for GadgetListUserControl.xaml
    /// </summary>
    public partial class GadgetListUserControl : UserControl
    {
        public GadgetListUserControl()
        {
            InitializeComponent();
        }

        private void DelayLoad(object sender, EventArgs e)
        {
        /*    this.appListBox_RecentUsed.ItemsSource = DataMgr.Instance.MruItems;
            this.memorizeListBox_All.ItemsSource = DataMgr.Instance.MemorizeAppItems;
            this.mathFastListBox_All.ItemsSource = DataMgr.Instance.MathFastAppItems;
            this.appListBox_All.ItemsSource = DataMgr.Instance.DllAppItems;*/

        //    AudioHelper.PlayStartupSound();

            //if (DataMgr.Instance.LocalTypeCollection.Count > 0)
            //{
            //    TreeViewItem treeViewItem = this.typeTreeView.ItemContainerGenerator.ContainerFromItem(DataMgr.Instance.LocalTypeCollection[0]) as TreeViewItem;
            //    if (treeViewItem != null)
            //        treeViewItem.IsSelected = true;
            //}

            
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            this.mainTypeListBox.ItemsSource = DataMgr.Instance.LocalTypeCollection;
            if (this.mainTypeListBox.Items.Count > 0)
            {
                this.mainTypeListBox.SelectedIndex = 0;
            }
        }

        private void settingBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.ShowSettingPage();
        }

        private void aboutBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.ShowAboutPage();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            App.Current.MainWindow.Close();
        }

        private void AnimateText()
        {
        }

        private void thicknessAnimation_Completed(object sender, EventArgs e)
        {
        }

        private void thumbnailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadGadgetList();
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    {
                        this.SubtypeLeft();
                        e.Handled = true;
                    }
                    break;
                case Key.Right:
                    {
                        this.SubtypeRight(); 
                        e.Handled = true;
                    }
                    break;
                case Key.Up:
                    {
                        this.TypeUp();
                        e.Handled = true;
                    }
                    break;
                case Key.Down:
                    {
                        this.TypeDown();
                        e.Handled = true;
                    }
                    break;
                case Key.Enter:
                    {
                        this.LoadGadgetList();
                        e.Handled = true;
                    }
                    break;
            }
        }

        private void TypeUp()
        {
           
        }

        private void TypeDown()
        {
            
        }

        private void SubtypeLeft()
        {
           
        }

        private void SubtypeRight()
        {
 
        }

      //  private void LoadGadgetList()
        //{
        //    if (this.thumbnailList.SelectedItem == null)
        //        return;

        //    if (this.previousSelectedGadget == this.thumbnailList.SelectedIndex)
        //        return;

        //    this.previousSelectedGadget = this.thumbnailList.SelectedIndex;

        //    ListBoxItem gadgetItem = this.thumbnailList.SelectedItem as ListBoxItem;
        //    MainWindow mainWnd = App.Current.MainWindow as MainWindow;
        //    mainWnd.LoadGadgetList(gadgetItem.DataContext as SubTypeItem);
        //}

        private void LoadGadgetList()
        {
        
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();

            if (DataMgr.Instance.AppCount == 0)
            {
                AddAppTipWindow tipWnd = new AddAppTipWindow();
                if (tipWnd.ShowDialog().Value)
                {
                    if (DataMgr.Instance.LoginInfo.Id == "0")
                    {
                        if (!ControlMgr.Instance.AppStoreWindow.login())
                            return;
                    }

                    ControlMgr.Instance.AppStoreWindow.Show();
                    ControlMgr.Instance.AppStoreWindow.WindowState = WindowState.Normal;
                    ControlMgr.Instance.AppStoreWindow.Activate();
                }
            }
        }

        private void managementBtn_Click(object sender, RoutedEventArgs e)
        {
            if (DataMgr.Instance.LoginInfo.Id == "0")
            {
                if (!ControlMgr.Instance.AppStoreWindow.login())
                    return;
            }

            ControlMgr.Instance.AppStoreWindow.Show();
            ControlMgr.Instance.AppStoreWindow.WindowState = WindowState.Normal;
            ControlMgr.Instance.AppStoreWindow.Activate();
        }

        private void appListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox == null)
            {
                return;
            }

            Point clickPoint = e.GetPosition(listBox);
            HitTestResult result = VisualTreeHelper.HitTest(listBox, clickPoint);

            ListBoxItem listBoxItem = GC_UIHelper.FindParent<ListBoxItem>(result.VisualHit);

            if (listBoxItem == null)
                return;

            if (listBoxItem.Content is AddAppItem)
            {
                this.managementBtn_Click(sender, new RoutedEventArgs());
            }
            else if (listBoxItem.Content is AppItem)
            {
                AppItem item = listBoxItem.Content as AppItem;
                MainWindow mainWnd = App.Current.MainWindow as MainWindow;
                mainWnd.LoadGadget(item);

                listBox.SelectedIndex = -1;

                Thread.Sleep(System.Windows.Forms.SystemInformation.DoubleClickTime);
            }
            else if (listBoxItem.Content is TypeItem)
            {
                //TypeItem typeItem = listBoxItem.Content as TypeItem;
                //this.appListStackPanel.Visibility = System.Windows.Visibility.Visible;
                //this.appListBox.Visibility = System.Windows.Visibility.Hidden;
                //if (!string.IsNullOrEmpty(typeItem.Thumbnail))
                //    this.typeThumbnailImage.Source = new BitmapImage(new Uri(typeItem.Thumbnail));
                //this.appListBox_All.ItemsSource = DataMgr.Instance[typeItem.Type];
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.appListBox_All.SelectedIndex < 0)
                return;

            AppItem item = this.appListBox_All.SelectedItem as AppItem;
            if (item == null)
                return;

            MessageWindow messageWnd = new MessageWindow();
            messageWnd.ShowMessage(string.Format("您确定要卸载{0}吗？", item.Title), MessageBoxButton.OKCancel, new MessageWindowCallback((ok) =>
                {
                    if (ok)
                        DataMgr.Instance.uninstallApp(item);
                }));
        }

        private void mruMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //if (this.appListBox_RecentUsed.SelectedIndex < 0)
            //    return;

            //AppItem item = this.appListBox_RecentUsed.SelectedItem as AppItem;
            //if (item == null)
            //    return;

            //MessageWindow messageWnd = new MessageWindow();
            //messageWnd.ShowMessage(string.Format("您确定要卸载{0}吗？", item.Title), MessageBoxButton.OKCancel, new MessageWindowCallback((ok) =>
            //{
            //    if (ok)
            //        DataMgr.Instance.uninstallApp(item);
            //}));
        }

        private void createAppListBoxItem(AppItemCollection collection)
        {
            ListBoxExItem listBoxExItem = new Controls.ListBoxExItem();

            ListBox listBox = new ListBox();
            listBox.BorderBrush = null;
            listBox.BorderThickness = new Thickness(0);
            listBox.Background = Brushes.Transparent;
            listBox.Foreground = null;
            ScrollViewer.SetVerticalScrollBarVisibility(listBox, ScrollBarVisibility.Disabled);
            ScrollViewer.SetHorizontalScrollBarVisibility(listBox, ScrollBarVisibility.Auto);
            listBox.SelectedIndex = -1;
            listBox.ItemsPanel = this.TryFindResource("subTypePanelTemplate") as ItemsPanelTemplate;
            listBox.MouseLeftButtonUp += this.appListBox_MouseLeftButtonUp;
            listBox.Style = this.TryFindResource("ListBoxStyle_SubGroup") as Style;

            MenuItem uninstallMenuItem = new MenuItem();
            uninstallMenuItem.Header = "卸载";
            uninstallMenuItem.Click += MenuItem_Click;

            ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();
            contextMenu.Items.Add(uninstallMenuItem);

            listBoxExItem.Content = listBox;
        //    this.typeTreeView.Items.Add(listBoxExItem);
        }

        private void typeListBox_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //if (this.typeTreeView.SelectedItem == null)
            //    return;

            //TypeItem typeItem = this.typeTreeView.SelectedItem as TypeItem;
            //if (typeItem == null)
            //    return;

            //if (typeItem.ParentType == TypeItem.Root)
            //{
            //    if (typeItem.SubTypeItems.Count > 0)
            //    {
            //        TreeViewItem treeViewItem = this.typeTreeView.ItemContainerGenerator.ContainerFromItem(typeItem) as TreeViewItem;
            //        if (treeViewItem != null)
            //            treeViewItem.IsExpanded = true;

            //        this.appListBox_All.ItemsSource = DataMgr.Instance.getAppCollectionByType(typeItem.Type);
            //    }
            //    if (typeItem.Type == TypeItem.MostRecentUsed)
            //    {
            //        this.appListBox_All.ItemsSource = DataMgr.Instance.MruItems;
            //    }
            //}
            //else
            //{
            //    this.appListBox_All.ItemsSource = DataMgr.Instance[typeItem.Type];
            //}
        }

        private void mainTypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TypeItem typeItem = this.mainTypeListBox.SelectedItem as TypeItem;
            if (typeItem == null)
                return;

            this.subTypeListBox.ItemsSource = typeItem.SubTypeItems;
            if (this.subTypeListBox.Items.Count > 0)
                this.subTypeListBox.SelectedIndex = typeItem.SelectedSubTypeIndex;
            else
            {
                if (typeItem.Type == TypeItem.MostRecentUsed)
                {
                    this.appListBox_All.ItemsSource = DataMgr.Instance.MruItems;
                }
            }
        }

        private void subTypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TypeItem typeItem = getTypeItem();
            if (typeItem == null)
                return;

            if (typeItem.ParentType == TypeItem.Root)
            {
                if (typeItem.SubTypeItems.Count > 0)
                {
                    this.appListBox_All.ItemsSource = DataMgr.Instance.getAppCollectionByType(typeItem.Type);
                }
                if (typeItem.Type == TypeItem.MostRecentUsed)
                {
                    this.appListBox_All.ItemsSource = DataMgr.Instance.MruItems;
                }
            }
            else
            {
                TypeItem mainTypeItem = this.mainTypeListBox.SelectedItem as TypeItem;
                mainTypeItem.SelectedSubTypeIndex = this.subTypeListBox.SelectedIndex;
                this.appListBox_All.ItemsSource = DataMgr.Instance[typeItem.Type];
            }
        }

        private TypeItem getTypeItem()
        {
            TypeItem item = null;
            if (this.subTypeListBox.SelectedItem != null)
            {
                item = this.subTypeListBox.SelectedItem as TypeItem;
            }
            else if (this.mainTypeListBox.SelectedItem != null)
            {
                item = this.mainTypeListBox.SelectedItem as TypeItem;
            }

            return item;
        }
    }
}
