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
using System.Windows.Threading;
using System.Reflection;
using SoonLearning.BlockPuzzle.Data;
using System.Collections.ObjectModel;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Controls;
using System.Windows.Media.Animation;
using SoonLearning.AppCenter;
using SoonLearning.AppCenter.License;

namespace SoonLearning.BlockPuzzle.Puzzle
{
    /// <summary>
    /// Interaction logic for Puzzle3x2ListPage.xaml
    /// </summary>
    public partial class PuzzleListPage : UserControl
    {
        public PuzzleListPage()
        {
            InitializeComponent();

        //    this.stageListBox.Title = ControlMgr.Instance.Entry.Title;
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new EventHandler<EventArgs>(SafeInit),
                DispatcherPriority.ApplicationIdle,
                new object[] { this, EventArgs.Empty });
        }

        private void SafeInit(object sender, EventArgs e)
        {
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.PuzzleStartupPage.ShowNewPuzzleWindow();
        }

        private void reloadBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.PuzzleStartupPage.LoadPuzzleItems(endLoadPuzzleItems);
        }

        private void endLoadPuzzleItems(object sender, EventArgs e)
        {
            ControlMgr.Instance.PuzzleStartupPage.ShowPuzzleList();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (LicenseMgr.Instance.IsTrialVersion(ControlMgr.Instance.Entry.Id))
            {
                this.newBtn.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                this.newBtn.Visibility = System.Windows.Visibility.Visible;
            }

            PuzzleControl.Instance.ControlPanlVisible(false);

            this.stageListBox.SelectedIndex = -1;
            this.stageListBox.ItemsSource = ControlMgr.Instance.DataMgr.PuzzleItems;

            this.stageListBox.SelectedIndex = ControlMgr.Instance.DataMgr.CurrentIndex;
            
            Storyboard storyboard = this.TryFindResource("loadStoryboard") as Storyboard;
            if (storyboard != null)
                storyboard.Begin();
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.DataMgr.CurrentIndex = this.stageListBox.SelectedIndex;
            ControlMgr.Instance.PuzzleStartupPage.ShowPuzzle();
            ControlMgr.Instance.PuzzlePage.GotoStage(ControlMgr.Instance.DataMgr.CurrentIndex);
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.PuzzleStartupPage.ShowPuzzle();
        }

        private void stageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void stageListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void stageListBox_ThumbnailItemSelectedEvent(int index)
        {
            if (index < 0)
                return;

            ControlMgr.Instance.DataMgr.CurrentIndex = index;

            ControlMgr.Instance.PuzzleStartupPage.ShowPuzzle();
            ControlMgr.Instance.PuzzlePage.GotoStage(index);
        }

        private void stageListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //if (this.stageListBox.SelectedItem == null)
            //    return;

            //ControlMgr.Instance.DataMgr.CurrentIndex = this.stageListBox.SelectedIndex;

            //ControlMgr.Instance.PuzzleStartupPage.ShowPuzzle();
            //ControlMgr.Instance.PuzzlePage.GotoStage(ControlMgr.Instance.DataMgr.CurrentIndex);
        }
    }
}
