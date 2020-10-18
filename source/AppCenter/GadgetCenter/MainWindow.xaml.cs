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
using System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Threading;
using System.Threading;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Windows;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.AppCenter.License;
using SoonLearning.AppCenter.UserControls;
using System.ComponentModel;
using System.Speech.Synthesis;

namespace SoonLearning.AppCenter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMessageControl
    {
        private static bool canExit = false;
        private bool disableExit = false;
        private ObservableCollection<AppInstallItem> installedAppItemCollection = new ObservableCollection<AppInstallItem>();

        internal static bool CanExit
        {
            get { return canExit; }
        }

        public MainWindow()
        {
            InitializeComponent();

            //this.Left = (float)SystemInformation.WorkingArea.Left;
            //this.Top = (float)SystemInformation.WorkingArea.Top;
            //this.Width = (float)SystemInformation.WorkingArea.Width;
            //this.Height = (float)SystemInformation.WorkingArea.Height;

        //    this.containerGrid.Width = this.Width;
        //    this.containerGrid.Height = this.Height;
        //    this.containerGrid.Height = this.containerGrid.Width / (this.Width / this.Height);

            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    foreach (string title in GC_UIHelper.GetCurrentUser())
                        this.titleTextBlock.Text = title;
                }),
                DispatcherPriority.Background,
                null);

            LoginWindow loginCtrl = new LoginWindow();
            loginCtrl.ShowDirectLogin = true;
            loginCtrl.ShowDialog();

            this.containerPanel.Content = new LoadGadgetWindow();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (this.disableExit)
            {
                e.Cancel = true;
                return;
            }

            if (!canExit)
            {
                this.ShowMessageWindow(ControlMgr.Instance.ExitUserControl);

                e.Cancel = true;
            }
            else
            {
                AppMgr.Instance.StopAll();
            }

            base.OnClosing(e);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
        //    this.SwitchToFullScreen(UIStyleSetting.Instance.FullScreen);

            AudioHelper.Init();

            //this.containerPanel.MoveInCompletedEvent += moveIn_Completed;
            //this.containerPanel.MoveOutCompletedEvent += moveOut_Completed;
            //this.containerPanel.BackMoveInCompletedEvent += backMoveIn_Completed;
            //this.containerPanel.BackMoveOutCompletedEvent += backMoveOut_Completed;

            this.appaInstallNotifyListBox.ItemsSource = this.installedAppItemCollection;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AppInstallMgr.Instance.AppInstallCompletedEvent += (sender1, e1) =>
            {
                this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    this.AppInstallCompleted(e1);
                }),
                System.Windows.Threading.DispatcherPriority.Background,
                null);
            };

            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.DelayLoad();
            }),
            System.Windows.Threading.DispatcherPriority.ContextIdle,
            null);

            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.StartCheckUpgrade();
            }),
            System.Windows.Threading.DispatcherPriority.ApplicationIdle,
            null);

            try
            {
                SpeechHelper.Instance.SpeechSynthesizer.Volume = 0;
                SpeechHelper.Instance.SpeechSynthesizer.Speak(new Prompt("test"));
            }
            catch
            {
            }
        }

        private void DelayLoad()
        {
            // For Preview Version
            //if (!LicenseMgr.Instance.IsLicenseForCurrentMachine(App.appCenterId))
            //{
            //    PreviewLicenseInputUserControl previewLicenseInputCtrl = new PreviewLicenseInputUserControl();
            //    previewLicenseInputCtrl.LicenseVerifiedEvent += ((obj, args) =>
            //        {
            //            this.containerPanel.Content = new LoadGadgetWindow();
            //        });
            //    ShowMessageWindow(previewLicenseInputCtrl);
            //    return;
            //}
        }

        private void StartCheckUpgrade()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += ((sender, e) =>
                {
                    AudioHelper.UpdateBackgroundMusic(@"http://www.soonlearning.com/AppBackgroundMusic/BackgroundMusicConfig.xml");

                    e.Result = this.CheckUpgrade();
                    Thread.Sleep(1);
                });
            worker.RunWorkerCompleted += ((sender, e) =>
                {
                    string ret = e.Result as string;
                    if (!string.IsNullOrEmpty(ret))
                    {
                        string[] rets = ret.Split(new char[] { ';' });
                        if (rets.Length >= 4)
                        {
                            string message = string.Format("速学应用平台版本{0}{1}已经发布。\n是否要现在升级到该版本？", rets[1].TrimEnd(), rets[2].TrimEnd());
                            UpgradeMessageWindow msgWnd = new UpgradeMessageWindow(message);
                            if (msgWnd.ShowDialog().Value)
                            {
                                Assembly assembly = Assembly.GetEntryAssembly();
                                string upgradeExe = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), "SoonLearning.UpgradeTool.exe");
                                Process.Start(upgradeExe, string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\"",
                                    rets[0].TrimEnd(),
                                    rets[1].TrimEnd(),
                                    rets[2].TrimEnd(),
                                    rets[3].TrimEnd()));

                                this.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                            }
                        }
                    }
                    else
                    {
                    }
                });
            worker.RunWorkerAsync();
        }

        private string CheckUpgrade()
        {
            try
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                return DataMgr.Instance.AppService.CheckVersion(App.appCenterId,
                    assembly.GetName().Version.ToString(),
                    LoginInfo.GetMD5Hash("$af3#d_&"));
            }
            catch (Exception ex)
            {
            }

            return string.Empty;
        }

        private void AppInstallCompleted(AppInstallCompletedEventArgs e)
        {
            this.installedAppItemCollection.Insert(0, e.Item);

            this.appInstallNotifyPopup.PlacementTarget = this.containerGrid;
            this.appInstallNotifyPopup.Placement = PlacementMode.Custom;
            this.appInstallNotifyPopup.StaysOpen = false;
            this.appInstallNotifyPopup.IsOpen = true;

            this.appaInstallNotifyListBox.SelectedIndex = 0;
            
            if (e.Error == null) // Success or Cancel
            {
                if (e.Item.State == InstallState.UserCancelled)
                {
                }
                else
                {
                }
            }
            else // Fail
            {
            }
        }

        private CustomPopupPlacement[] placePopup(Size popupSize, Size targetSize, Point offset)
        {
            CustomPopupPlacement placement1 =
               new CustomPopupPlacement(new Point(targetSize.Width-popupSize.Width-10, targetSize.Height-popupSize.Height-10), PopupPrimaryAxis.Vertical);

            CustomPopupPlacement placement2 =
                new CustomPopupPlacement(new Point(10, 20), PopupPrimaryAxis.Horizontal);

            CustomPopupPlacement[] ttplaces =
                    new CustomPopupPlacement[] { placement1, placement2 };
            return ttplaces;
        }


        internal void StartToUse()
        {
            this.containerPanel.Content = ControlMgr.Instance.GadgetListUserControl;
        //    this.containerPanel.ClearEntry();

            SpeechHelper.Instance.EnableRecognizer();
        }

        internal void LoadGadgetList(SubTypeItem item)
        {
            this.containerPanel.Content = ControlMgr.Instance.SubTypeListUserControl;

            ControlMgr.Instance.SubTypeListUserControl.LoadGadgetList(item);
        }

        internal void LoadGadget(AppItem item)
        {
#if _LAUNCH_APP_
            AppMgr.Instance.Start(item);

            DataMgr.Instance.MruItems.Add(item);

#else
            this.containerPanel.Content = ControlMgr.Instance.GadgetContainerUserControl;
            ControlMgr.Instance.GadgetContainerUserControl.LoadGadget(item);

            DataMgr.Instance.MruItems.Add(item);
#endif
            var newAppQuery = from app in AppInstallMgr.Instance.AppInstallItems
                              where app.State == InstallState.Done && item.Id == app.AppItem.UniqueId
                              select app;

            if (newAppQuery.Count() > 0)
                AppInstallMgr.Instance.Remove(newAppQuery.First());
        }

        internal void ShowSettingPage()
        {
            this.ShowMessageWindow(ControlMgr.Instance.SettingWindow);
        }

        internal void ShowAboutPage()
        {
            this.ShowMessageWindow(ControlMgr.Instance.AboutWindow);
        }

        internal void ShowHelp()
        {
            this.containerPanel.Content = ControlMgr.Instance.HelpWindow;
        }

        internal void ShowAppManagement()
        {
            this.ShowMessageWindow(ControlMgr.Instance.AppManagementUserControl);
        }

        public void SwitchToFullScreen(bool fullScreen)
        {
            return;
            if (fullScreen)
            {
                this.WindowStyle = System.Windows.WindowStyle.None;
            //    this.Topmost = true;
                this.ResizeMode = System.Windows.ResizeMode.NoResize;
                UIStyleSetting.Instance.FullScreen = true;
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
                this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                this.Topmost = false;
                this.ResizeMode = System.Windows.ResizeMode.CanMinimize;
                UIStyleSetting.Instance.FullScreen = false;
            }

            UIStyleSetting.Instance.Save();
        }

        public void EnableSpeechRecognizer(bool enable)
        {
            SpeechHelper.Instance.EnableRecognizer();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            foreach (Window win in App.Current.Windows)
            {
                if (win != this && !(win is AppStoreWindow))
                    win.Activate();
            }
        }

        private void minimax_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void moveIn_Completed(object sender, EventArgs e)
        {
            //if (this.containerPanel.CanGoback)
            //    this.btnPanel.Visibility = System.Windows.Visibility.Visible;
            //else
            //    this.btnPanel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void moveOut_Completed(object sender, EventArgs e)
        {
        }

        private void backMoveIn_Completed(object sender, EventArgs e)
        {
            if (this.containerPanel.Content is GadgetContainerUserControl)
            {
                GadgetContainerUserControl ctrl = this.containerPanel.Content as GadgetContainerUserControl;
                this.EnableBack(ctrl.CanGoback);
                return;
            }

            //if (this.containerPanel.CanGoback)
            //    this.btnPanel.Visibility = System.Windows.Visibility.Visible;
            //else
            //    this.btnPanel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void backMoveOut_Completed(object sender, EventArgs e)
        {

        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.goBack();
        }

        private void containerPanel_LoadCompleted(object sender, NavigationEventArgs e)
        {
        }

        internal void goBack()
        {
        }

        internal void Exit()
        {
            this.CloseMessageWindow();

            canExit = true;
            foreach (Window win in App.Current.Windows)
                win.Close();

            this.Close();
        }

        internal void EnableBack(bool enable)
        {
        }

        private void containerPanel_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void containerPanel_Navigating(object sender, NavigatingCancelEventArgs e)
        {

        }

        private void containerPanel_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {

        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                if (e.OriginalSource is System.Windows.Controls.Primitives.TextBoxBase)
                    return;

                e.Handled = true;
                this.goBack();
            }
            else if (e.Key == Key.F11)
            {
                this.SwitchToFullScreen(!UIStyleSetting.Instance.FullScreen);
            }
            else if (e.Key == Key.Escape)
            {
                this.SwitchToFullScreen(false);
            }
        }

        internal void Home()
        {
            this.containerPanel.Content = ControlMgr.Instance.GadgetListUserControl;
        //    this.containerPanel.ClearEntry();
        }

        private void homeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.containerPanel.Content = ControlMgr.Instance.GadgetListUserControl;
        //    this.containerPanel.ClearEntry();
        }

        public void ShowMessageWindow(System.Windows.Controls.UserControl msgWnd)
        {
            this.disableExit = true;
            this.messagePanel.Children.Clear();
            this.messagePanel.Children.Add(msgWnd);
            this.messagePanel.Visibility = System.Windows.Visibility.Visible;
        }

        public void CloseMessageWindow()
        {
            this.disableExit = false;
            this.messagePanel.Children.Clear();
            this.messagePanel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void appInstallNotifyPopup_Closed(object sender, EventArgs e)
        {
            this.installedAppItemCollection.Clear();
        }
    }
}
