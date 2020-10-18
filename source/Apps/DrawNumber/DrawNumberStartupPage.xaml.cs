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
using System.Reflection;
using SoonLearning.AppCenter.Controls;
using System.Windows.Threading;
using SoonLearning.AppCenter.Data;
using System.Threading;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.ConnectNumber
{
    /// <summary>
    /// Interaction logic for DrawNumberStartupPage.xaml
    /// </summary>
    public partial class DrawNumberStartupPage : UserControl
    {
        private EventHandler<EventArgs> endLoadDrawNumberItems;

        private NewDrawNumberWindow newDrawNumberWindow;

        private MediaPlayer backgroundMusicPlayer = new MediaPlayer();

        private List<System.IO.FileInfo> backgroundMusicList = new List<System.IO.FileInfo>();

        private EventHandler mediaEndedHandler;
        private OpenSoundStateChangedHandler openSoundStateChangedHandler;
        private SortedList<string, DrawNumberItem> sortedDrawNumberItemList = new SortedList<string, DrawNumberItem>();

        public DrawNumberStartupPage()
        {
            InitializeComponent();

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string musicPath = System.IO.Path.GetDirectoryName(assembly.Location);
                musicPath = System.IO.Path.Combine(musicPath, @"Data\DrawNumber\Music");

                System.IO.DirectoryInfo musicDirectory = new System.IO.DirectoryInfo(musicPath);
                backgroundMusicList.AddRange(musicDirectory.GetFiles("*.mp3"));
                backgroundMusicList.AddRange(musicDirectory.GetFiles("*.aac"));
                backgroundMusicList.AddRange(musicDirectory.GetFiles("*.wav"));
            }
            catch
            {
            }

            this.mediaEndedHandler = (sender, e) =>
            {
                this.PlayBackgroundMusic();
            };

            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.LoadDrawNumberItems(null);
            }),
            DispatcherPriority.Background,
            null);

            openSoundStateChangedHandler = (open) =>
            {
                if (open)
                {
                    this.PlayBackgroundMusic();
                }
                else
                {
                    this.backgroundMusicPlayer.Stop();
                    this.backgroundMusicPlayer.Close();
                }
            };
        }

        internal void LoadDrawNumberItems(EventHandler<EventArgs> endLoading)
        {
            ControlMgr.Instance.DataMgr.Clear();

            this.endLoadDrawNumberItems = endLoading;

            Assembly assembly = Assembly.GetCallingAssembly();
            string dataFolder = System.IO.Path.GetDirectoryName(assembly.Location);
            dataFolder = System.IO.Path.Combine(dataFolder, @"Data\DrawNumber");

            LoadAppDataWindow win = new LoadAppDataWindow(new Uri(@"pack://application:,,,/SoonLearning.ConnectNumber;component/Resources/ConnectNumber.png"));
            win.DataPath = dataFolder;
            win.FileExt = new string[] { "*.dd" };
            win.DataItemLoadedEvent += new DataItemLoadedDelegate(win_DataItemLoadedEvent);
            win.DataLoadCompletedEvent += new DataLoadCompletedDelegate(win_DataLoadCompletedEvent);

            win.StartToLoad();

            this.rootGrid.Children.Clear();
            this.rootGrid.Children.Add(win);
        }

        private ThumbnailItem win_DataItemLoadedEvent(string file)
        {
            DrawNumberItem item = DrawNumberData.LoadDrawNumberItem(file);
            item.DataFile = file;
            string key = item.PointCollection.Count.ToString("000");
            if (this.sortedDrawNumberItemList.ContainsKey(key))
                key += Guid.NewGuid().ToString("N");
            this.sortedDrawNumberItemList.Add(key, item);
            return item;
        }

        private void win_DataLoadCompletedEvent()
        {
            foreach (string key in this.sortedDrawNumberItemList.Keys)
            {
                ControlMgr.Instance.DataMgr.Add(this.sortedDrawNumberItemList[key]);
            }

            this.sortedDrawNumberItemList.Clear();

            if (this.endLoadDrawNumberItems != null)
            {
                this.endLoadDrawNumberItems(this, EventArgs.Empty);
                this.endLoadDrawNumberItems = null;
            }
            else
            {
                this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                    { this.delayLoad(); } ), 
                    DispatcherPriority.Background, 
                    null);
            }
        }

        private void delayLoad()
        {
            this.ShowListPage();
        }

        internal void ShowListPage()
        {
            this.rootGrid.Children.Clear();
            this.rootGrid.Children.Add(ControlMgr.Instance.DrawNumberList);
        }

        internal void ShowDrawNumberPage()
        {
            this.rootGrid.Children.Clear();
            this.rootGrid.Children.Add(ControlMgr.Instance.DrawNumberPage);
        }

        internal void ShowNewDrawNumberPage()
        {
            if (this.newDrawNumberWindow == null)
                this.newDrawNumberWindow = new NewDrawNumberWindow();

            this.rootGrid.Children.Clear();
            this.rootGrid.Children.Add(this.newDrawNumberWindow);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.backgroundMusicPlayer.MediaEnded += this.mediaEndedHandler;
            this.PlayBackgroundMusic();

            UIStyleSetting.Instance.OpenSoundStateChanged += this.openSoundStateChangedHandler;
        }

        private void PlayBackgroundMusic()
        {
            if (this.backgroundMusicList.Count == 0 ||
                !UIStyleSetting.Instance.OpenSound)
                return;
            else
                AudioHelper.StopBackgroundMusic();

            Random rand = new Random(Environment.TickCount);

            this.backgroundMusicPlayer.Open(new Uri(this.backgroundMusicList[rand.Next(this.backgroundMusicList.Count)].FullName, UriKind.Absolute));
            this.backgroundMusicPlayer.Play();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.backgroundMusicPlayer.Stop();
            this.backgroundMusicPlayer.Close();
            this.backgroundMusicPlayer.MediaEnded -= this.mediaEndedHandler;
            UIStyleSetting.Instance.OpenSoundStateChanged -= this.openSoundStateChangedHandler;
        }
    }
}
