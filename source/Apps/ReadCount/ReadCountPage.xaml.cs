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
using System.IO;
using System.Collections;
using System.Speech.Recognition;
using SoonLearning.AppCenter.Utility;
using System.Threading;
using System.Windows.Threading;
using SoonLearning.ReadCount.Data;
using System.ComponentModel;
using System.Windows.Media.Animation;
using SoonLearning.AppCenter.Controls;
using System.Reflection;
using SoonLearning.AppCenter.Player;
using SoonLearning.AppCenter.Data;
using System.Diagnostics;

namespace SoonLearning.ReadCount
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ReadCountPage : UserControl
    {
        private List<Point> goodsPosList = new List<Point>();
        private string basePath;
        private string backgroundImagePath;
        private string goodsImagePath;
        private string backgroundMusicPath;
        private Timer resultTimer;

   //     private AudioPlayer backgroundMusicPlayer;
        private MediaPlayer backgroundMusicPlayer;

        private OpenSoundStateChangedHandler openSoundStateChangedHandler;

        public ReadCountPage()
        {
            InitializeComponent();

            this.PrepareStages();

            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                
            }),
            DispatcherPriority.ApplicationIdle,
            null);

            this.openSoundStateChangedHandler = (open) =>
            {
                if (open)
                {
                    this.PlayNextMusic();
                }
                else
                {
                    this.StopMusic();
                }
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UIStyleSetting.Instance.OpenSoundStateChanged += this.openSoundStateChangedHandler;

            this.startRecognizer();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.stopRecognizer();
            UIStyleSetting.Instance.OpenSoundStateChanged -= this.openSoundStateChangedHandler;
            this.StopMusic();
        }

        private void startRecognizer()
        {
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                try
                {
                    if (ReadCountDataMgr.Instance.PhaseList.Count > 0)
                    {
                        SpeechHelper.Instance.StartRecognizer(ReadCountDataMgr.Instance.PhaseList, speechRecognizer_SpeechRecognized);
                    }
                    else
                    {
                        List<string> textList = new List<string>();
                        for (int i = 0; i <= 10; i++)
                            textList.Add(i.ToString());

                        SpeechHelper.Instance.StartRecognizer(textList, speechRecognizer_SpeechRecognized);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }

                this.PlayNextMusic();
            }),
            DispatcherPriority.ApplicationIdle,
            null);
        }

        private void stopRecognizer()
        {
            SpeechHelper.Instance.EndRecognizer();
        }

        private void PrepareStages()
        {
            this.Dispatcher.Invoke(new ThreadStart(() =>
                {
                    this.backgroundImagePath = System.IO.Path.Combine(ReadCountStartupPage.Instance.DataBasePath, @"Background");
                    this.goodsImagePath = System.IO.Path.Combine(ReadCountStartupPage.Instance.DataBasePath, @"Goods");
                    this.backgroundMusicPath = System.IO.Path.Combine(ReadCountStartupPage.Instance.DataBasePath, @"Music");
                    string configFile = System.IO.Path.Combine(ReadCountStartupPage.Instance.DataBasePath, @"Setting.xml");
                    ReadCountStageCollection stages = SerializerHelper<ReadCountStageCollection>.XmlDeserialize(configFile);
                    ReadCountDataMgr.Instance.StageCollection = stages;

                    this.Next();
                }),
                null);
        }

        private void PlayNextMusic()
        {
            try
            {
                if (!UIStyleSetting.Instance.OpenSound ||
                    !this.IsLoaded)
                    return;

                if (!string.IsNullOrEmpty(ReadCountDataMgr.Instance.CurrentMusic))
                    AudioHelper.StopBackgroundMusic();

                string musicFile = System.IO.Path.Combine(this.backgroundMusicPath, ReadCountDataMgr.Instance.CurrentMusic);
                if (this.backgroundMusicPlayer == null)
                {
                    this.backgroundMusicPlayer = new MediaPlayer();
                    this.backgroundMusicPlayer.MediaEnded += new EventHandler(backgroundMusicPlayer_MediaEnded);
                }
                this.backgroundMusicPlayer.Open(new Uri(musicFile, UriKind.Absolute));
                this.backgroundMusicPlayer.Play();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        private void StopMusic()
        {
            if (this.backgroundMusicPlayer != null)
            {
                this.backgroundMusicPlayer.MediaEnded -= backgroundMusicPlayer_MediaEnded;
                this.backgroundMusicPlayer.Stop();
                this.backgroundMusicPlayer = null;
            }
        }

        private void backgroundMusicPlayer_MediaEnded(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.StopMusic();
                string nextMusic = ReadCountDataMgr.Instance.NextMusic;
                this.PlayNextMusic();
            }),
                DispatcherPriority.Send,
                null);
        }

        private ReadCountStage CreateReadCountStage(string backgroundImage, ReadCountItem[] items)
        {
            ReadCountStage stage = new ReadCountStage(backgroundImage);
            stage.AddItems(items);
            return stage;
        }

        private ReadCountItem CreateReadCountItem(string goodsImage, int maxCount)
        {
            Random rand = new Random(Environment.TickCount);
            ReadCountItem item = new ReadCountItem(goodsImage, rand.Next(maxCount + 1), maxCount, true);
            item.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(item_PropertyChanged);
            return item;
        }

        private void Next()
        {
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    this.Update();
                }),
                DispatcherPriority.Normal,
                null);
        }

        private void Update()
        {
            this.Dispatcher.Invoke(new ThreadStart(() =>
                {
                    this.Clear();
                }),
                null);

            this.rootGrid.DataContext = ReadCountDataMgr.Instance.NextStage;

            foreach (ReadCountItem item in ReadCountDataMgr.Instance.CurrentStage.Items)
            {
                item.PropertyChanged += item_PropertyChanged;
            }

            this.CreateGoodsImage();
            this.UpdateBackgroundImage();

            this.readCountResultListBox.SelectedIndex = 0;

            foreach (object goods in this.goodsCanvas.Children)
            {
                if (goods is Image)
                {
                    Image image = goods as Image;
                    this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                        {
                            Point pt = this.GetImagePoint(image.ActualWidth, image.ActualHeight);
                            Canvas.SetLeft(image, pt.X);
                            Canvas.SetTop(image, pt.Y);

                            DoubleAnimation doubleAnimation = new DoubleAnimation();
                            doubleAnimation.From = 0;
                            doubleAnimation.To = 1;
                            doubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
                            image.BeginAnimation(Image.OpacityProperty, doubleAnimation);

                            image.Visibility = System.Windows.Visibility.Visible;
                        }),
                        DispatcherPriority.Render,
                        null);
                }
            }

            if (UIStyleSetting.Instance.SpeechRecognizer)
            {
                SpeechHelper.Instance.RecognizerEnabled = true;
                SpeechHelper.Instance.EnableRecognizer();
            }
        }

        private void Clear()
        {
            if (ReadCountDataMgr.Instance.CurrentStage == null)
                return;

            this.goodsPosList.Clear();
            this.goodsCanvas.Children.Clear();

            foreach (ReadCountItem item in ReadCountDataMgr.Instance.CurrentStage.Items)
            {
                item.Result = null;
                item.PropertyChanged -= item_PropertyChanged;
            }
        }

        private Point GetImagePoint(double imageWidth, double imageHeight)
        {
            Random rand = new Random(Environment.TickCount);

            int x = rand.Next(10, (int)this.goodsCanvas.ActualWidth - (int)imageWidth - 10);
            int y = rand.Next(10, (int)this.goodsCanvas.ActualHeight - (int)imageHeight - 10);
            Point pt = new Point(x, y);

            bool found = false;
            foreach (Point temp in this.goodsPosList)
            {
                if (pt.X >= temp.X - imageWidth && pt.X <= temp.X + imageWidth &&
                    pt.Y >= temp.Y - imageHeight && pt.Y <= temp.Y + imageHeight)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                Thread.Sleep(10);
                return GetImagePoint(imageWidth, imageHeight);
            }

            this.goodsPosList.Add(pt);

            return pt;
        }

        private void CreateGoodsImage()
        {
            List<Image> imageList = new List<Image>();
            foreach (ReadCountItem item in ReadCountDataMgr.Instance.CurrentStage.Items)
            {
                if (!System.IO.Path.IsPathRooted(item.GoodsImage))
                    item.GoodsImage = System.IO.Path.Combine(this.goodsImagePath, item.GoodsImage);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(item.GoodsImage);
                bi.EndInit();

                if (item.RandomCount)
                {
                    Random rand = new Random(Environment.TickCount);
                    item.Count = rand.Next(1, item.MaxCount + 1);
                }

                for (int i = 0; i < item.Count; i++)
                {
                    Image image = new Image();
                    image.Source = bi;
                    image.Width = 80;
                    image.Visibility = System.Windows.Visibility.Hidden;

                    this.goodsCanvas.Children.Add(image);
                }

                Thread.Sleep(50);
            }
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Result")
            {
                bool allCorrect = true;
                bool foundItem = false;
                foreach (ReadCountItem item in ReadCountDataMgr.Instance.CurrentStage.Items)
                {
                    if (sender == item)
                    {
                        foundItem = true;
                        if (item.IsCorrect != null &&
                            item.IsCorrect.Value &&
                            this.readCountResultListBox.SelectedIndex < this.readCountResultListBox.Items.Count - 1)
                        {
                            this.readCountResultListBox.SelectedIndex++;
                        }
                    }

                    if (item.IsCorrect != null)
                    {
                        if (!item.IsCorrect.Value)
                            allCorrect = item.IsCorrect.Value;
                    }
                    else
                    {
                        allCorrect = false;
                    }
                }

                if (foundItem && allCorrect)
                {
                    this.StartResultTimer();
                }
            }
        }

        private void StartResultTimer()
        {
            if (UIStyleSetting.Instance.SpeechRecognizer)
            {
                SpeechHelper.Instance.RecognizerEnabled = false;
                SpeechHelper.Instance.EnableRecognizer();
            }

            if (resultTimer == null)
            {
                this.resultTimer = new Timer(resultTimerCallback, null, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(-1));
            }
        }

        private void resultTimerCallback(object state)
        {
            this.resultTimer.Dispose();
            this.resultTimer = null;

            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    ResultInfoUserControl resultControl = new ResultInfoUserControl();
                    resultControl.State = ResultState.Pass;
                    resultControl.ResultInfoCompleted += (object sender, EventArgs e) =>
                        {
                            this.Next();
                        };
                    GC_UIHelper.ShowMessageWindow(resultControl);
                }),
                DispatcherPriority.Background,
                null);
        }

        private void speechRecognizer_SpeechRecognized(object sender, RecognizeCompletedEventArgs e)
        {
            ReadCountItem item = this.readCountResultListBox.SelectedItem as ReadCountItem;
            if (item == null || e.Result == null)
                return;

            int count = 0;
            int.TryParse(e.Result.Text, out count);
            item.Result = count;
        }

        private void UpdateBackgroundImage()
        {
            ReadCountStage stage = ReadCountDataMgr.Instance.CurrentStage;
            if (stage == null)
                return;

            if (!System.IO.Path.IsPathRooted(stage.BackgroundImage))
                stage.BackgroundImage = System.IO.Path.Combine(this.backgroundImagePath, stage.BackgroundImage);
        }

        private void readCountResultListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lb = sender as ListBox;
            ReadCountItem readCountItem = lb.SelectedItem as ReadCountItem;
            if (readCountItem == null)
                return;

            ListBoxItem item = lb.ItemContainerGenerator.ContainerFromItem(readCountItem) as ListBoxItem;
            FocusItem(item, "numberTextBox");
            NumberOnlyTextBox textBox = GC_UIHelper.FindChild<NumberOnlyTextBox>(item, "numberTextBox");
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }

        public void FocusItem(ListBoxItem item, string name)
        {
            if (!item.IsLoaded)
            {
                // wait for the item to load so we can find the control to focus 
                RoutedEventHandler onload = null;
                onload = delegate
                {
                    item.Loaded -= onload;
                    FocusItem(item, name);
                };
                item.Loaded += onload;
                return;
            }

            try
            {
                var ctl = item.Template.FindName(name, item) as FrameworkElement;
                ctl.Focus();
            }
            catch
            {
                // focus something else if the template/item wasn't found? 
            }
        } 

    }
}
