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
using SoonLearning.Memorize.Data;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Utility;
using System.ComponentModel;

namespace SoonLearning.Memorize.UI
{
    /// <summary>
    /// Interaction logic for MemorizeStartupUserControl.xaml
    /// </summary>
    public partial class MemorizeStartupUserControl : UserControl
    {
        private static MemorizeStartupUserControl instance;

        internal static MemorizeStartupUserControl Instance
        {
            get
            {
                if (instance == null)
                    instance = new MemorizeStartupUserControl();
                return instance;
            }
        }

        private MediaPlayer backgroundMusicPlayer;
        private EventHandler backgroundMusicEnded;
        private EventHandler<ExceptionEventArgs> backgroundMusicFailed;

        private EventHandler stageDoneHandler;
        private EventHandler timeUsedUpHandler;
        private PropertyChangedEventHandler propertyChangedEventHandler;

        public MemorizeStartupUserControl()
        {
            InitializeComponent();

            this.prepareEventHandlers();
        }

        #region Events

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.memorizeItemGrid.StageDoneEvent += this.stageDoneHandler;
            this.memorizeItemGrid.PropertyChanged += this.propertyChangedEventHandler;
            this.playBackgroundMusic();

            this.playerANameTextBlock.Text = MemorizeDataMgr.Instance.PlayerAName + ":";
            this.playerBNameTextBlock.Text = MemorizeDataMgr.Instance.PlayerBName + ":";

            this.playerAResultListBox.Items.Clear();
            this.playerBResultListBox.Items.Clear();

            if (MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.VsPC ||
                MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.TwoPlayer)
            {
                this.pkPanel.Visibility = System.Windows.Visibility.Visible;
                this.timeControl.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                this.pkPanel.Visibility = System.Windows.Visibility.Hidden;
                this.timeControl.Visibility = System.Windows.Visibility.Visible;
            }

            this.startStage();
            this.timeControl.TimeUsedUpEvent += this.timeUsedUpHandler;
            this.timeControl.Start(60);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.memorizeItemGrid.PropertyChanged -= this.propertyChangedEventHandler;
            this.memorizeItemGrid.StageDoneEvent -= this.stageDoneHandler;

            if (this.backgroundMusicPlayer != null)
            {
                this.backgroundMusicPlayer.Stop();
            }

            this.timeControl.TimeUsedUpEvent -= this.timeUsedUpHandler;
            this.timeControl.Stop();
        }

        private void preButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeDataMgr.Instance.MoveToPreStage();
            this.startStage();
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeDataMgr.Instance.MoveToNextStage();
            this.startStage();
        }

        #endregion

        #region Start Stage

        internal void startStage()
        {
            this.memorizeItemGrid.clearInfo();
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.memorizeItemGrid.showMemorizeItems();

                this.stageIndexTextBlock.Text = string.Format("第{0}/{1}关", MemorizeDataMgr.Instance.CurrentStage + 1, MemorizeDataMgr.Instance.Entry.Stages.Count) ;
                this.showBackgroundImage();
            }),
            DispatcherPriority.Background,
            null);
        }

        #endregion

        #region Help Method

        private void prepareEventHandlers()
        {
            this.backgroundMusicEnded = (sender, e) =>
            {
                this.playBackgroundMusic();
            };

            this.backgroundMusicFailed = (sender, e) =>
            {
                this.playBackgroundMusic();
            };

            this.stageDoneHandler = (sender, e) =>
            {
                this.showStageFeedback();
            };

            this.timeUsedUpHandler = (sender, e) =>
            {
                this.timeUsedUp();
            };

            this.propertyChangedEventHandler = (sender, e) =>
            {
                if (e.PropertyName == "UserCorrectCount")
                {
                    if (this.memorizeItemGrid.UserCorrectCount == 0)
                        this.playerAResultListBox.Items.Clear();
                    else
                        this.playerAResultListBox.Items.Add(new ListBoxItem());
                }
                else if (e.PropertyName == "BotCorrectCount")
                {
                    if (this.memorizeItemGrid.BotCorrectCount == 0)
                        this.playerBResultListBox.Items.Clear();
                    else
                        this.playerBResultListBox.Items.Add(new ListBoxItem());
                }

                if (e.PropertyName == "BotTerm")
                {
                    if (this.memorizeItemGrid.BotTerm)
                    {
                        this.playerATermImage.Visibility = System.Windows.Visibility.Hidden;
                        this.playerBTermImage.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        this.playerATermImage.Visibility = System.Windows.Visibility.Visible;
                        this.playerBTermImage.Visibility = System.Windows.Visibility.Hidden;
                    }
                }

                if (e.PropertyName == "PlayerBTerm")
                {
                    if (this.memorizeItemGrid.PlayerBTerm)
                    {
                        this.playerATermImage.Visibility = System.Windows.Visibility.Hidden;
                        this.playerBTermImage.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        this.playerATermImage.Visibility = System.Windows.Visibility.Visible;
                        this.playerBTermImage.Visibility = System.Windows.Visibility.Hidden;
                    }
                }
            };
        }

        private void playBackgroundMusic()
        {
            if (!UIStyleSetting.Instance.OpenSound)
                return;

            string currentBkMusic = MemorizeDataMgr.Instance.CurrentBackgroundMusic;
            if (!string.IsNullOrEmpty(currentBkMusic))
                AudioHelper.StopBackgroundMusic();
            else
                return;

            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                if (this.backgroundMusicPlayer == null)
                {
                    this.backgroundMusicPlayer = new MediaPlayer();
                    this.backgroundMusicPlayer.MediaEnded += this.backgroundMusicEnded;
                    this.backgroundMusicPlayer.MediaFailed += this.backgroundMusicFailed;
                    this.backgroundMusicPlayer.Volume = 0.9;
                }
                else
                {
                    this.backgroundMusicPlayer.Stop();
                }

                this.backgroundMusicPlayer.Open(new Uri(MemorizeDataMgr.Instance.CurrentBackgroundMusic, UriKind.Absolute));
                this.backgroundMusicPlayer.Play();
            }),
            DispatcherPriority.Background,
            null);
        }

        private void showBackgroundImage()
        {
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.backgroundImage.Source = new BitmapImage(new Uri(MemorizeDataMgr.Instance.CurrentBackgroundImage, UriKind.Absolute));
            //    this.backgroundImage.BeginAnimation(Image.OpacityProperty, new DoubleAnimation(0, 1f, new Duration(TimeSpan.FromMilliseconds(300))));
            }),
            DispatcherPriority.Background,
            null);
        }

        private void showStageFeedback()
        {
            Thread.Sleep(200);
        //    DoubleAnimation doubleAnimation = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromMilliseconds(300)));
        //    doubleAnimation.Completed += (sender, e) =>
            {
                this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                //    if (MemorizeDataMgr.Instance.Entry.StageFeedback == null)
                {
                    if (MemorizeDataMgr.Instance.IsLastStage)
                    {
                        MemorizeDataMgr.Instance.UsedTime = this.timeControl.Elapsed;
                        this.timeControl.Stop();
                        MemorizeResultWindow resultWnd = new MemorizeResultWindow();
                        resultWnd.ShowDialog();

                        MemorizeDataMgr.Instance.Restart();
                        this.timeControl.Start(60);
                        this.startStage();
                    }
                    else
                    {
                        ResultWindow resultWnd = new ResultWindow();
                        resultWnd.State = ResultState.Pass;
                        if (MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.VsPC)
                        {
                            if (this.memorizeItemGrid.UserCorrectCount < this.memorizeItemGrid.BotCorrectCount)
                                resultWnd.State = ResultState.NotPass;
                        }
                        else if (MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.TwoPlayer)
                        {
                            StringBuilder message = new StringBuilder();
                            message.AppendLine(string.Format("{0}翻开了{1}组牌, {2}翻开了{3}组牌．", MemorizeDataMgr.Instance.PlayerAName,
                                this.memorizeItemGrid.PlayerACorrectCount,
                                MemorizeDataMgr.Instance.PlayerBName,
                                this.memorizeItemGrid.PlayerBCorrectCount));
                            if (this.memorizeItemGrid.PlayerACorrectCount > this.memorizeItemGrid.PlayerBCorrectCount)
                            {
                                message.AppendLine(string.Format("{0}战胜了{1}．", MemorizeDataMgr.Instance.PlayerAName,
                                    MemorizeDataMgr.Instance.PlayerBName));
                            }
                            else if (this.memorizeItemGrid.PlayerACorrectCount == this.memorizeItemGrid.PlayerBCorrectCount)
                            {
                                message.AppendLine(string.Format("{0}与{1}战成了平手．", MemorizeDataMgr.Instance.PlayerAName,
                                    MemorizeDataMgr.Instance.PlayerBName));
                            }
                            else 
                            {
                                message.AppendLine(string.Format("{0}战胜了{1}．", MemorizeDataMgr.Instance.PlayerBName, MemorizeDataMgr.Instance.PlayerAName));
                            }
                            resultWnd.Message = message.ToString();
                        }
                        resultWnd.ShowMessage((ok) =>
                        {
                            if (ok)
                            {
                                MemorizeDataMgr.Instance.MoveToNextStage();
                            }
                            this.startStage();
                        });
                    }
                }),
                DispatcherPriority.Normal,
                null);
            };

         //   this.backgroundImage.BeginAnimation(Image.OpacityProperty, doubleAnimation);
        }

        private void timeUsedUp()
        {
            this.timeControl.Stop();
            MemorizeResultWindow resultWindow = new MemorizeResultWindow();
            resultWindow.ShowDialog();
            MemorizeDataMgr.Instance.Restart();
            this.timeControl.Start(60);
            this.startStage();
        }

        #endregion

        private void pkModeCheckBox_Click(object sender, RoutedEventArgs e)
        {
        /*    MessageWindow msgWnd = new MessageWindow();
            msgWnd.ShowMessage("切换对战模式，应用需要重新开始，确定要切换吗？", MessageBoxButton.OKCancel, (ok) =>
            {
                if (this.pkModeCheckBox.IsChecked.Value)
                {
                    this.pkPanel.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.pkPanel.Visibility = System.Windows.Visibility.Hidden;
                }
                MemorizeDataMgr.Instance.Entry.IsPkMode = this.pkModeCheckBox.IsChecked.Value;
                MemorizeDataMgr.Instance.Restart();
                this.timeControl.Stop();
                this.timeControl.Start(60);
                this.startStage();
            });*/
        }

        private void oneMinuteRadioButton_Click(object sender, RoutedEventArgs e)
        {
            changeTimeMode(TimingMode.CountDown);
        }

        private void passRadioButton_Click(object sender, RoutedEventArgs e)
        {
            changeTimeMode(TimingMode.Count);
            this.timeControl.Mode = TimingMode.Count;
        }

        private void changeTimeMode(TimingMode mode)
        {
        /*    if (mode == MemorizeDataMgr.Instance.CurrentTimingMode)
                return;

            MessageWindow msgWnd = new MessageWindow();
            msgWnd.ShowMessage("切换计时模式，应用需要重新开始，确定要切换吗？", MessageBoxButton.OKCancel, (ok) =>
            {
                MemorizeDataMgr.Instance.Entry.IsPkMode = this.pkModeCheckBox.IsChecked.Value;
                MemorizeDataMgr.Instance.Restart();
                this.startStage();

                this.timeControl.Mode = mode;
                this.timeControl.Stop();
                this.timeControl.Start(60);

                MemorizeDataMgr.Instance.CurrentTimingMode = mode;
            });*/
        }

        private void restartButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeUIContainerUserControl.Instance.SwitchToSettingPage();
        }
    }
}
