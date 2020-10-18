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
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Speech;
using SoonLearning.Memorize.Data;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.Memorize.UI
{
    /// <summary>
    /// Interaction logic for MemorizeGrid.xaml
    /// </summary>
    public partial class MemorizeGrid : INotifyPropertyChanged
    {
        private const int maxRow = 4;
        private const int maxCol = 10;

        private RoutedEventHandler itemClickHandler;
        private EventHandler autoResumeHandler;

        private DispatcherTimer autoResumeTimer;

        private int itemWidth;
        private int itemHeight;

        private Button openedButton1;
        private Button openedButton2;

        private MediaPlayer itemClickMediaPlayer;
        private MediaPlayer audioItemMediaPlayer;

        private MediaPlayer okMediaPlayer;
        private MediaPlayer failMediaPlayer;

        private bool match = false;

        private int userCorrectCount;

        #region Bot

        private int botCorrectCount;
        private bool botTerm;
        private List<Button> openedButtonList = new List<Button>();

        #endregion

        #region Two Player

        private int playerACorrectCount;
        private int playerBCorrectCount;
        private bool playerBTerm;

        #endregion

        private Dictionary<int, Point> preDefinedRowColDictionary = new Dictionary<int, Point>();

        public event EventHandler StageDoneEvent;

        public static RoutedCommand readTextRoutedCommand;
            
        #region Styles

        private Style audioButtonStyle;
        private Style imageButtonStyle;
        private Style imageNoRepeatbuttonStyle;
        private Style textButtonStyle;
        private Style buttonBackStyle;

        private Style AudioButtonStyle
        {
            get
            {
                if (audioButtonStyle == null)
                    audioButtonStyle = FindResource("buttonAudioStyle") as Style;

                return audioButtonStyle;
            }
        }

        private Style ImageButtonStyle
        {
            get
            {
                if (imageButtonStyle == null)
                    imageButtonStyle = FindResource("buttonImageStyle") as Style;

                return imageButtonStyle;
            }
        }

        private Style ImageButtonNoRepeatStyle
        {
            get
            {
                if (imageNoRepeatbuttonStyle == null)
                    imageNoRepeatbuttonStyle = FindResource("buttonImageNoRepeatStyle") as Style;

                return imageNoRepeatbuttonStyle;
            }
        }

        private Style TextButtonStyle
        {
            get
            {
                if (textButtonStyle == null)
                    textButtonStyle = FindResource("buttonTextStyle") as Style;

                return textButtonStyle;
            }
        }

        private Style ButtonBackStyle
        {
            get
            {
                if (buttonBackStyle == null)
                    buttonBackStyle = FindResource("buttonBackStyle") as Style;

                return buttonBackStyle;
            }
        }

        #endregion

        public bool BotTerm
        {
            get { return this.botTerm; }
            set
            {
                this.botTerm = value;
                this.OnPropertyChanged("BotTerm");
            }
        }

        public bool PlayerBTerm
        {
            get { return this.playerBTerm; }
            set
            {
                this.playerBTerm = value;
                this.OnPropertyChanged("PlayerBTerm");
            }
        }

        public int UserCorrectCount
        {
            get
            {
                return this.userCorrectCount;
            }
            set
            {
                this.userCorrectCount = value;
                this.OnPropertyChanged("UserCorrectCount");
            }
        }

        public int BotCorrectCount
        {
            get
            {
                return this.botCorrectCount;
            }
            set
            {
                this.botCorrectCount = value;
                this.OnPropertyChanged("BotCorrectCount");
            }
        }

        public int PlayerACorrectCount
        {
            get { return this.userCorrectCount; }
            set
            {
                this.UserCorrectCount = value;
            }
        }

        public int PlayerBCorrectCount
        {
            get { return this.botCorrectCount; }
            set
            {
                this.BotCorrectCount = value;
            }
        }

        public MemorizeGrid()
        {
            InitializeComponent();

            this.prepareEventHandler();
            this.preparePreDefinedRowCol();
            this.prepareCommand();
        }

        private void preparePreDefinedRowCol()
        {
            this.preDefinedRowColDictionary.Add(4, new Point(2, 2));
            this.preDefinedRowColDictionary.Add(6, new Point(2, 3));
            this.preDefinedRowColDictionary.Add(8, new Point(2, 4));
            this.preDefinedRowColDictionary.Add(10, new Point(2, 5));
            this.preDefinedRowColDictionary.Add(12, new Point(3, 4));
            this.preDefinedRowColDictionary.Add(14, new Point(2, 7));
            this.preDefinedRowColDictionary.Add(16, new Point(4, 4));
            this.preDefinedRowColDictionary.Add(18, new Point(3, 6));
            this.preDefinedRowColDictionary.Add(20, new Point(4, 5));
            this.preDefinedRowColDictionary.Add(22, new Point(2, 11));
            this.preDefinedRowColDictionary.Add(24, new Point(4, 6));
            this.preDefinedRowColDictionary.Add(28, new Point(4, 7));
            this.preDefinedRowColDictionary.Add(30, new Point(3, 10));
            this.preDefinedRowColDictionary.Add(32, new Point(4, 8));
            this.preDefinedRowColDictionary.Add(36, new Point(4, 9));
            this.preDefinedRowColDictionary.Add(40, new Point(4, 10));
        }

        internal void showMemorizeItems()
        {
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.clearInfo();

                int row;
                int col;
                getRowAndCol(out row, out col);

                this.calSize(row, col);
                this.createRowAndCol(row, col);

                this.Visibility = System.Windows.Visibility.Hidden;

                int index = 0;
                for (int r = 0; r < row; r++)
                {
                    for (int c = 0; c < col; c++)
                    {
                        this.createButton(index++, r, c);

                        if (index == MemorizeDataMgr.Instance.Items.Count)
                            break;
                    }
                }

                this.Visibility = System.Windows.Visibility.Visible;

                //this.BeginAnimation(Grid.OpacityProperty, new DoubleAnimation(0, 1.0, new Duration(TimeSpan.FromMilliseconds(500))));

            }),
            DispatcherPriority.Background,
            null);
        }

        private void prepareCommand()
        {
            readTextRoutedCommand = new RoutedCommand("ReadTextRoutedCommand", typeof(MemorizeGrid));
            CommandManager.RegisterClassCommandBinding(typeof(MemorizeGrid),
                new CommandBinding(readTextRoutedCommand, (s, e) =>
                {
                    if (e.Source is Button)
                    {
                        Button btn = e.Source as Button;
                        if (btn.DataContext is MemorizeReadText)
                        {
                            SpeechHelper.Instance.CancelSpeakAsync();
                            MemorizeReadText readText = this.openedButton1.DataContext as MemorizeReadText;
                            SpeechHelper.Instance.SpeakAsync(readText.Text, -4);
                        }
                    }
                }));
        }

        internal void clearInfo()
        {
            if (this.autoResumeTimer != null)
                this.autoResumeTimer.Stop();

            this.BotCorrectCount = 0;
            this.UserCorrectCount = 0;
            this.PlayerACorrectCount = 0;
            this.PlayerBCorrectCount = 0;
            this.PlayerBTerm = false;

            foreach (UIElement ctrl in this.panelGrid.Children)
            {
                if (ctrl is Button)
                    ((Button)ctrl).Click -= this.itemClickHandler;
            }

            this.panelGrid.RowDefinitions.Clear();
            this.panelGrid.ColumnDefinitions.Clear();
            this.panelGrid.Children.Clear();

            this.openedButton1 = null;
            this.openedButton2 = null;
            this.BotTerm = false;
            this.openedButtonList.Clear();
        }

        private void getRowAndCol(out int row, out int col)
        {
            row = 2;
            col = 3;
            if (this.preDefinedRowColDictionary.ContainsKey(MemorizeDataMgr.Instance.Items.Count))
            {
                row = (int)this.preDefinedRowColDictionary[MemorizeDataMgr.Instance.Items.Count].X;
                col = (int)this.preDefinedRowColDictionary[MemorizeDataMgr.Instance.Items.Count].Y;
                return;
            }

            for (row = 2; row <= MemorizeGrid.maxRow; row++)
            {
                int c = MemorizeDataMgr.Instance.Items.Count / row;
                int l = MemorizeDataMgr.Instance.Items.Count % row;
                if (l > 0)
                    continue;

                if (c <= MemorizeGrid.maxCol)
                {
                    col = c;
                    return;
                }
            }
        }

        private void calSize(int row, int col)
        {
            double cellHeight = this.ActualHeight / row;
            double cellWidth = this.ActualWidth / col;

            double ratio = cellWidth / cellHeight;
            if (ratio > 3f / 4f)
            {
                cellWidth = cellHeight / 4f * 3f;
            }
            else
            {
                cellHeight = cellWidth * 4f / 3f;
            }

            this.itemWidth = (int)cellWidth;
            this.itemHeight = (int)cellHeight;

            this.panelGrid.Width = cellWidth * col;
            this.panelGrid.Height = cellHeight * row;
        }

        private void createRowAndCol(int row, int col)
        {
            for (int r = 0; r < row; r++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(this.itemHeight, GridUnitType.Pixel);
                this.panelGrid.RowDefinitions.Add(rowDef);
            }

            for (int c = 0; c < col; c++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(this.itemWidth, GridUnitType.Pixel);
                this.panelGrid.ColumnDefinitions.Add(colDef);
            }
        }

        private void createButton(int index, int row, int col)
        {
            Button btn = new Button();
            btn.DataContext = MemorizeDataMgr.Instance.Entry;
            btn.Tag = MemorizeDataMgr.Instance.Items[index];
            btn.Click += this.itemClickHandler;
            btn.Style = this.ButtonBackStyle;
            Grid.SetRow(btn, row);
            Grid.SetColumn(btn, col);
            this.panelGrid.Children.Add(btn);
        }

        private void prepareEventHandler()
        {
            this.itemClickHandler = (sender, e) =>
            {
                if (this.BotTerm)
                    return;

                if (sender is Button)
                {
                    Button btn = sender as Button;
                    if (btn.Tag is MemorizeObject)
                    {
                        MemorizeObject obj = btn.Tag as MemorizeObject;

                        this.setButtonStyle(btn);
                        this.switchDataContextAndTag(btn);
                        this.checkTwoOpenedButtonResult(match);
                        this.playItemClickSound();

                        this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                        {
                            if (this.openedButton1 != null)
                            {
                                MemorizeObject openingObj = this.openedButton1.DataContext as MemorizeObject;
                                if (openingObj.ItemId == obj.ItemId)
                                {
                                    match = true;
                                    this.autoResumeTimer.Interval = TimeSpan.FromMilliseconds(500);
                                    this.openedButton2 = btn;
                                }
                                else
                                {
                                    match = false;
                                    this.autoResumeTimer.Interval = TimeSpan.FromMilliseconds(1000);
                                    this.openedButton2 = btn;
                                }

                                if (!MemorizeControl.Instance.TestMode)
                                {
                                    if (MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.VsPC)
                                    {
                                        this.BotTerm = !match;
                                    }
                                    else if (MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.TwoPlayer)
                                    {
                                        if (this.PlayerBTerm)
                                            this.PlayerBTerm = match;
                                        else
                                            this.PlayerBTerm = !match;
                                    }

                                    this.autoResumeTimer.Start();
                                }
                                else
                                {
                                    match = false;
                                }

                                this.playMathSound(match);
                            }
                            else
                            {
                                this.openedButton1 = btn;
                                this.autoResumeTimer.Interval = TimeSpan.FromMilliseconds(1000);
                                match = false;
                            }
                        }),
                        DispatcherPriority.Send,
                        null);
                    }
                }
            };

            this.autoResumeHandler = (sender, e) =>
            {
                this.autoResumeTimer.Stop();
                this.checkTwoOpenedButtonResult(match);
            };
        }

        private void setButtonStyle(Button btn)
        {
            MemorizeObject obj = btn.Tag as MemorizeObject;

            if (btn.Tag is MemorizeReadText)
            {
                btn.Style = this.AudioButtonStyle;

                MemorizeReadText memorizeReadText = btn.Tag as MemorizeReadText;

                SpeechHelper.Instance.SpeakAsync(memorizeReadText.Text, -3);
            }
            else if (btn.Tag is MemorizeText)
            {
                btn.Style = this.TextButtonStyle;
            }
            else if (btn.Tag is MemorizeImage)
            {
                MemorizeImage memorizeImage = btn.Tag as MemorizeImage;
                if (memorizeImage.Count >= 0)
                {
                    int imageWidth = (int)(this.itemWidth / 4f);
                    memorizeImage.GenerateItems(0, (this.itemHeight - this.itemWidth) / 2,
                        this.itemWidth - imageWidth, this.itemWidth - imageWidth,
                        imageWidth);
                    btn.Style = this.ImageButtonStyle;
                }
                else
                {
                    btn.Style = this.ImageButtonNoRepeatStyle;
                }
            }
            else if (btn.Tag is MemorizeMusic)
            {
                btn.Style = this.AudioButtonStyle;

                MemorizeMusic memorizeMusic = btn.Tag as MemorizeMusic;

                this.audioItemMediaPlayer.Stop();
                this.audioItemMediaPlayer.Close();
                this.audioItemMediaPlayer.Open(new Uri(memorizeMusic.Url, UriKind.Absolute));
                this.audioItemMediaPlayer.Play();
            }
        }

        private void checkTwoOpenedButtonResult(bool match)
        {
            if (this.openedButton1 != null && this.openedButton2 != null)
            {
                this.autoResumeTimer.Stop();
                if (match)
                {
                    this.matchItem();
                    if (MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.TwoPlayer)
                    {
                        if (this.PlayerBTerm)
                            this.PlayerBCorrectCount++;
                        else
                            this.PlayerACorrectCount++;
                    }
                    else if (MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.VsPC)
                    {
                        if (!this.BotTerm)
                            this.UserCorrectCount++;
                    }
                }
                else
                {
                    this.resumeItem();
                    if (MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.VsPC)
                    {
                        this.botRember();
                    }
                }

                this.openedButton1 = null;
                this.openedButton2 = null;

                if (this.panelGrid.Children.Count == 0)
                    return;

                if (this.BotTerm && 
                    MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.VsPC)
                {
                    this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                    {
                        Thread.Sleep(500);
                        this.botOpen();
                    }),
                    DispatcherPriority.Background,
                    null);
                }
            }
        }

        private void switchDataContextAndTag(Button btn)
        {
            object temp = btn.DataContext;
            btn.DataContext = btn.Tag;
            btn.Tag = temp;
        }

        private void matchItem()
        {
            this.panelGrid.Children.Remove(this.openedButton1);
            this.panelGrid.Children.Remove(this.openedButton2);

            if (MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.VsPC)
            {
                this.openedButtonList.Remove(this.openedButton1);
                this.openedButtonList.Remove(this.openedButton2);
            }

            if (this.panelGrid.Children.Count == 0)
            {
                this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    EventHandler tempHandler = this.StageDoneEvent;
                    if (tempHandler != null)
                        tempHandler(this, EventArgs.Empty);
                }),
                DispatcherPriority.Background,
                null);
            }
        }

        private void resumeItem()
        {
            if (MemorizeControl.Instance.TestMode)
                return;

            this.openedButton1.Style = this.ButtonBackStyle;
            this.openedButton2.Style = this.ButtonBackStyle;

            this.switchDataContextAndTag(this.openedButton1);
            this.switchDataContextAndTag(this.openedButton2);
        }

        private void playItemClickSound()
        {
        //    this.itemClickMediaPlayer.Stop();
        //    this.itemClickMediaPlayer.Play();
        }

        private void playMathSound(bool match)
        {
            if (match)
            {
                this.okMediaPlayer.Stop();
                this.okMediaPlayer.Play();
            }
            else
            {
                this.failMediaPlayer.Stop();
                this.failMediaPlayer.Play();
            }
        }

        private void botRember()
        {
            if (MemorizeDataMgr.Instance.CurrentChanllengeMode == ChanllengeMode.VsPC)
            {
                if (!this.openedButtonList.Contains(this.openedButton1))
                    this.openedButtonList.Add(this.openedButton1);

                if (!this.openedButtonList.Contains(this.openedButton2))
                    this.openedButtonList.Add(this.openedButton2);
            }
        }

        private void botOpen()
        {
            this.match = false;

            Button button1 = null;
            Button button2 = null;
            // Find matched items in bot remembered list
            foreach (var obj in this.openedButtonList)
            {
                var queryItem = from temp in this.openedButtonList
                                where ((MemorizeObject)temp.Tag).ItemId == ((MemorizeObject)obj.Tag).ItemId
                                select temp;
                if (queryItem.Count() > 1)
                {
                    foreach (var item in queryItem)
                    {
                        if (button1 == null)
                        {
                            button1 = item;
                            continue;
                        }
                        if (button2 == null)
                        {
                            button2 = item;
                            break;
                        }
                    }
                    break;
                }
            }

            if (button1 == null && this.openedButtonList.Count > 0)
            {
                button1 = this.openedButtonList[0];
            }

            List<int> indexList = new List<int>();
            for (int i = 0; i < this.panelGrid.Children.Count; i++)
            {
                var queryItem = from temp in this.openedButtonList
                                where temp == this.panelGrid.Children[i]
                                select temp;

                if (queryItem.Count() > 0)
                    continue;

                indexList.Add(i);
            }

            if (button1 == null) // Random button1
            {
                Random rand = new Random(Environment.TickCount);
                int randIndex = rand.Next(indexList.Count);
                button1 = this.panelGrid.Children[indexList[randIndex]] as Button;
                indexList.Remove(randIndex);
            }

            if (button2 == null) // Random button2
            {
                Random rand = new Random(Environment.TickCount);
                int randIndex = rand.Next(indexList.Count);
                button2 = this.panelGrid.Children[indexList[randIndex]] as Button;
                indexList.Remove(randIndex);
            }

            // open button1
            this.setButtonStyle(button1);
            this.setButtonStyle(button2);

            this.match = ((MemorizeObject)button1.Tag).ItemId == ((MemorizeObject)button2.Tag).ItemId;
            this.BotTerm = match;

            if (match)
                this.BotCorrectCount++;

            this.switchDataContextAndTag(button1);
            this.switchDataContextAndTag(button2);

            this.openedButton1 = button1;
            this.openedButton2 = button2;

            this.autoResumeTimer.Interval = TimeSpan.FromSeconds(1);
            this.autoResumeTimer.Start();

            this.playMathSound(match);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.autoResumeTimer == null)
            {
                this.autoResumeTimer = new DispatcherTimer(DispatcherPriority.Normal);
                this.autoResumeTimer.Interval = TimeSpan.FromMilliseconds(1000);
            }

            this.autoResumeTimer.Tick += this.autoResumeHandler;

            if (this.itemClickMediaPlayer == null)
            {
                this.itemClickMediaPlayer = new MediaPlayer();
                try
                {
                    this.itemClickMediaPlayer.Open(new Uri(MemorizeDataMgr.Instance.ItemClickSound, UriKind.Absolute));
                }
                catch
                {
                }
                this.itemClickMediaPlayer.Volume = 1.0;
            }

            if (this.okMediaPlayer == null)
            {
                this.okMediaPlayer = new MediaPlayer();
                try
                {
                    this.okMediaPlayer.Open(new Uri(MemorizeDataMgr.Instance.OkSound, UriKind.Absolute));
                }
                catch
                {
                }
                this.okMediaPlayer.Volume = 1.0;
            }

            if (this.failMediaPlayer == null)
            {
                this.failMediaPlayer = new MediaPlayer();
                try
                {
                    this.failMediaPlayer.Open(new Uri(MemorizeDataMgr.Instance.FailSound, UriKind.Absolute));
                }
                catch
                {
                }
                this.failMediaPlayer.Volume = 1.0;
            }

            if (this.audioItemMediaPlayer == null)
            {
                this.audioItemMediaPlayer = new MediaPlayer();
                this.audioItemMediaPlayer.Volume = 1.0;
            }
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.autoResumeTimer != null)
            {
                this.autoResumeTimer.Tick -= this.autoResumeHandler;
                this.autoResumeTimer.Stop();
            }

            if (this.itemClickMediaPlayer != null)
            {
                this.itemClickMediaPlayer.Stop();
                this.itemClickMediaPlayer.Close();
                this.itemClickMediaPlayer = null;
            }

            if (this.okMediaPlayer != null)
            {
                this.okMediaPlayer.Stop();
                this.okMediaPlayer.Close();
                this.okMediaPlayer = null;
            }

            if (this.failMediaPlayer != null)
            {
                this.failMediaPlayer.Stop();
                this.failMediaPlayer.Close();
                this.failMediaPlayer = null;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
