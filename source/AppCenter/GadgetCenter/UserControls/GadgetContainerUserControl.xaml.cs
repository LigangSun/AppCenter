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
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Security.Policy;
using System.Timers;
using System.Windows.Threading;
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter.Windows;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter.Resources;
using SoonLearning.Memorize.UI;
using SoonLearning.Memorize.Data;
using SoonLearning.Assessment.Player.Entry;

namespace SoonLearning.AppCenter.UserControls
{
    /// <summary>
    /// Interaction logic for GadgetContainerUserControl.xaml
    /// </summary>
    public partial class GadgetContainerUserControl : UserControl
    {
        private AppItem currentAppItem;
        private Timer appTipTImer;
        private ElapsedEventHandler appTipTimerElapsed;
        private int tipStaySeconds = 5;

        public GadgetContainerUserControl()
        {
            InitializeComponent();

            this.appTipTimerElapsed = ((sender, e) =>
            {
                this.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    this.tipStaySeconds--;
                    if (tipStaySeconds == 0)
                    {
                        this.showApp();
                    }
                    else
                    {
                        this.leftTimeTextBlock.Text = this.tipStaySeconds.ToString();
                    }
                }),
                DispatcherPriority.Background,
                null);
            });
        }

        internal bool CanGoback
        {
            get
            {                
                return true;
            }
        }

        internal bool GoBack()
        {
            return true;
        }

        #region Load In Domain

        //internal void LoadGadget(AppItem item)
        //{
        //    this.appDomain = GC_UIHelper.CreateAppDomain();

        //    Assembly gadgetAssembly = appDomain.Load(System.IO.Path.GetFileNameWithoutExtension(item.AppEntryFile));
        //    this.currentEntry = gadgetAssembly.CreateInstance(item.FullName) as IGadgetEntry;

        //    MainWindow mainWnd = App.Current.MainWindow as MainWindow;
        //    try
        //    {   
        //        this.UpdateControlState();

        //        this.UpdateStageInfo();
        //    }
        //    catch
        //    {
        //        mainWnd.EnableBack(true);
        //        this.RemoveControl(this.stagePanel);
        //        this.RemoveControl(this.stageListButton);
        //        this.RemoveControl(this.restartButton);
        //        this.RemoveControl(this.helpButton);
        //    }

        //    this.gadgetPanel.Children.Clear();
        //    this.gadgetPanel.Children.Add((UIElement)this.currentEntry.GetStartupPage());

        //    this.popupMainMenu();
        //}

        #endregion

        #region Load Directly

        internal void LoadGadget(AppItem item)
        {
            currentAppItem = item;

            if (!this.checkEntry(item))
            {
                MessageWindow msgWnd = new MessageWindow();
                msgWnd.ShowMessage(string.Format("启动应用{0}失败, 请与应用开发者联系。", item.Title), MessageBoxButton.OK, (ok) =>
                {
                    this.goHome();
                });

                return;
            }

            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            try
            {
                this.UpdateStageInfo();
            }
            catch
            {
                mainWnd.EnableBack(true);
            }

            this.DataContext = item;

            this.appTipBorder.Visibility = System.Windows.Visibility.Visible;

            this.tipStaySeconds = 5;
            this.leftTimeTextBlock.Text = this.tipStaySeconds.ToString();

            this.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
            {
                if (this.appTipTImer == null)
                {
                    this.appTipTImer = new Timer();
                    this.appTipTImer.Interval = 1000;
                    this.appTipTImer.Elapsed += this.appTipTimerElapsed;
                    this.appTipTImer.Start();
                }
            }),
            DispatcherPriority.Background,
            null);

            this.gadgetPanel.Child = null;
        }

        private bool checkEntry(AppItem item)
        {
            if (item is MemorizeAppItem)
            {
                MemorizeAppItem memorizeAppItem = item as MemorizeAppItem;
                if (memorizeAppItem.MemorizeEntry == null)
                {
                    try
                    {
                        memorizeAppItem.MemorizeEntry = MemorizeEntry.Load(memorizeAppItem.AppEntryFile);
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false, ex.Message);
                    }
                    finally
                    {
                        
                    }

                    return memorizeAppItem.MemorizeEntry != null;                        
                }
            }
            else if (item is AssessmentAppItem)
            {
                return true;
            }
            else
            {
                if (item.Entry == null)
                {
                    try
                    {
                        Assembly gadgetAssembly = Assembly.LoadFile(item.AppEntryFile);
                        item.Entry = gadgetAssembly.CreateInstance(item.FullName) as IGadgetEntry;
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false, ex.Message);
                    }

                    return item.Entry != null;
                }
            }

            return true;
        }

        #endregion

        private void backToMainBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch
            {
            }

            this.hideMainMenu();

            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.goBack();
        }

        private void helpButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.ShowHelp();
        }

        private void preStateButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateStageInfo();
        }

        private void nextStateButton_Click(object sender, RoutedEventArgs e)
        {
            this.UpdateStageInfo();
        }

        private void stageListButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void OnStageListBack(object sender, EventArgs e)
        {
        }

        private void OnStageListDone(object sender, EventArgs e)
        {

        }

        private void restartButton_Click(object sender, RoutedEventArgs e)
        {
        } 

        private void stageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.UpdateStageInfo();
        }

        private void UpdateStageInfo()
        {
        }

        private void showDoubleAnimationUsingKeyFrames_Completed(object sender, EventArgs e)
        {
            
        }

        private void hideDoubleAnimationUsingKeyFrames_Completed(object sender, EventArgs e)
        {
        //    this.controlPanel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void moreBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            this.popupMainMenu();
        }

        private void moreBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            this.popupMainMenu();
        }

        private void controlPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            this.popupMainMenu();
        }

        private void popupMainMenu()
        {
            Storyboard storyboard = this.FindResource("showPanelStoryboard") as Storyboard;
            storyboard.Begin();
        }

        private void hideMainMenu()
        {
            Storyboard storyboard = this.FindResource("hidePanelStoryboard") as Storyboard;
            storyboard.Begin();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.appTipTImer != null)
            {
                this.appTipTImer.Elapsed -= this.appTipTimerElapsed;
                this.appTipTImer.Stop();
                this.appTipTImer.Dispose();
                this.appTipTImer = null;
            }

            AudioHelper.StopBackgroundMusic();
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            this.goHome();
        }

        private void UserControl_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.appTipBorder.IsVisible)
            {
                this.showApp();
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.appTipBorder.IsVisible)
            {
                this.showApp();
            }
        }

        private void showApp()
        {
            try
            {
                this.appTipTImer.Stop();
                this.appTipBorder.Visibility = System.Windows.Visibility.Hidden;

                this.gadgetPanel.Child = null;
                if (this.currentAppItem is MemorizeAppItem)
                {
                    MemorizeAppItem appItem = this.currentAppItem as MemorizeAppItem;
                    MemorizeControl.Instance.Start(appItem.MemorizeEntry, DataMgr.Instance.LoginInfo.LoginId);
                    this.gadgetPanel.Child = MemorizeControl.Instance.StartupUI;

                    this.soundCheckBox.Visibility = System.Windows.Visibility.Visible;
                    this.speechRecognizerCheckBox.Visibility = System.Windows.Visibility.Hidden;
                    AudioHelper.PlayBackgroundMusic(true);
                }
                else if (this.currentAppItem is AssessmentAppItem)
                {
                    AssessmentAppItem appItem = this.currentAppItem as AssessmentAppItem;
                    AssessmentAppControl.Instance.Start(appItem.AppEntryFile);

                    this.gadgetPanel.Child = AssessmentAppControl.Instance.StartupUI;
                }
                else
                {
                    this.gadgetPanel.Child = (UIElement)this.currentAppItem.Entry.GetStartupPage();
                    if ((this.currentAppItem.Entry.Capability & AppCapability.BackgroundMusic) != 0)
                    {
                        this.soundCheckBox.Visibility = System.Windows.Visibility.Visible;
                        AudioHelper.PlayBackgroundMusic(true);
                    }
                    else
                    {
                        this.soundCheckBox.Visibility = System.Windows.Visibility.Hidden;
                    }

                    if ((this.currentAppItem.Entry.Capability & AppCapability.SpeechRecognization) != 0)
                    {
                        this.speechRecognizerCheckBox.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        this.speechRecognizerCheckBox.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);

                MessageWindow msgWnd = new MessageWindow();
                msgWnd.ShowMessage(string.Format("启动应用{0}失败, 请与应用开发者联系。", this.currentAppItem.Title), MessageBoxButton.OK, (ok) =>
                {
                    this.goHome();
                });
            }
        }

        private void goHome()
        {
            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.Home();
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            SharedWindow sharedWnd = new SharedWindow(this.currentAppItem.Id);
            sharedWnd.ShowDialog();
        }

        private void commentButton_Click(object sender, RoutedEventArgs e)
        {
            CommentWindow commentWnd = new CommentWindow(this.currentAppItem.Id);
            commentWnd.ShowDialog();
        }
    }
}
