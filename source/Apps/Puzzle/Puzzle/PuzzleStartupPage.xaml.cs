using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SoonLearning.BlockPuzzle.Data;
using SoonLearning.AppCenter;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.BlockPuzzle.Puzzle
{
    /// <summary>
    /// Interaction logic for Puzzle3x2StartupPage.xaml
    /// </summary>
    public partial class PuzzleStartupPage : UserControl
    {
        private LoadAppDataWindow loadGadgetDataWnd;
        private EventHandler<EventArgs> endLoadPuzzleItems;

        public PuzzleStartupPage()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            this.LoadPuzzleItems(null);
        }

        internal void LoadPuzzleItems(EventHandler<EventArgs> endLoading)
        {
            ControlMgr.Instance.DataMgr.Clear();

            this.endLoadPuzzleItems = endLoading;

            if (this.loadGadgetDataWnd == null)
                loadGadgetDataWnd = new LoadAppDataWindow(new Uri(ControlMgr.Instance.Entry.Thumbnail));
            this.loadGadgetDataWnd.DataPath = PuzzleSetting.Instance.DataFolder;
            this.loadGadgetDataWnd.FileExt = new string[] { "*.pd" };
            this.loadGadgetDataWnd.DataItemLoadedEvent += new DataItemLoadedDelegate(win_DataItemLoadedEvent);
            this.loadGadgetDataWnd.DataLoadCompletedEvent += new DataLoadCompletedDelegate(win_DataLoadCompletedEvent);

            GC_UIHelper.ShowMessageWindow(this.loadGadgetDataWnd);

            this.loadGadgetDataWnd.StartToLoad();
        }

        private ThumbnailItem win_DataItemLoadedEvent(string file)
        {
            PuzzleItem pi = PuzzleData.LoadPuzzleItem(file);
            if (pi.Type != PuzzleSetting.Instance.Type)
                return null;
            pi.ImageFile = file;
            ControlMgr.Instance.DataMgr.Add(pi);
            return pi;
        }

        private void win_DataLoadCompletedEvent()
        {
            if (this.endLoadPuzzleItems != null)
            {
                this.endLoadPuzzleItems(this, EventArgs.Empty);
                this.endLoadPuzzleItems = null;
            }
            else
            {
                this.Dispatcher.BeginInvoke(new ThreadStart(() => {
                        this.delayLoad();
                }),
                    DispatcherPriority.Background,
                    null);
            }
        }

        private void delayLoad()
        {
            this.ShowPuzzleList();            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        internal void ShowPuzzleList()
        {
            this.loadGadgetDataWnd.DataItemLoadedEvent -= new DataItemLoadedDelegate(win_DataItemLoadedEvent);
            this.loadGadgetDataWnd.DataLoadCompletedEvent -= new DataLoadCompletedDelegate(win_DataLoadCompletedEvent);

            GC_UIHelper.CloseMessageWindow();

            this.rootGrid.Children.Clear();
            this.rootGrid.Children.Add(ControlMgr.Instance.PuzzleListPage);
        }

        internal void ShowPuzzle()
        {
            this.loadGadgetDataWnd.DataItemLoadedEvent -= new DataItemLoadedDelegate(win_DataItemLoadedEvent);
            this.loadGadgetDataWnd.DataLoadCompletedEvent -= new DataLoadCompletedDelegate(win_DataLoadCompletedEvent);

            GC_UIHelper.CloseMessageWindow();

            this.rootGrid.Children.Clear();
            this.rootGrid.Children.Add(ControlMgr.Instance.PuzzlePage);
        }

        internal void ShowNewPuzzleWindow()
        {
            this.rootGrid.Children.Clear();
            this.rootGrid.Children.Add(ControlMgr.Instance.NewPuzzleWindow);
        }

        internal bool GoBack()
        {
            //if (this.rootGrid.Children.Count > 0)
            //{
            //    if (this.rootGrid.Children[0] == ControlMgr.Instance.PuzzleListPage)
            //    {
            //        return true;
            //    }
            //    else if (this.rootGrid.Children[0] == ControlMgr.Instance.PuzzlePage)
            //    {
            //        MessageWindow msgWnd = new MessageWindow();
            //        if (msgWnd.ShowMessage(SoonLearning.BlockPuzzle.Properties.Resources.GoBackWarning, MessageBoxButton.OKCancel))
            //        {
            //            this.ShowPuzzleList();
            //            return false;
            //        }
            //    }
            //}

            return true;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
