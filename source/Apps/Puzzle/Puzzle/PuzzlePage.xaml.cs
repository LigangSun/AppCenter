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
using SoonLearning.BlockPuzzle.Data;
using SoonLearning.BlockPuzzle.Controls;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using SoonLearning.AppCenter.Controls;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using SoonLearning.AppCenter.Data;
using System.Windows.Threading;
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.AppCenter.Player;
using SoonLearning.AppCenter;

namespace SoonLearning.BlockPuzzle.Puzzle
{
    /// <summary>
    /// Interaction logic for Puzzle3x2Page.xaml
    /// </summary>
    public partial class PuzzlePage : UserControl
    {
        private PuzzleGrid puzzleGrid;
        private UIElement elementToChopUp;
        private Size puzzleSize;

        private int totalSteps = 0;
        private int usedSteps = 0;
        private int movedSteps = 0;

        private PuzzleItem puzzleItem;
        private System.IO.MemoryStream imageStream;
        private System.IO.MemoryStream puzzleStream;

        private MouseButtonEventHandler puzzleImageMouseButtonEventHandler;

        public PuzzlePage()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            PuzzleControl.Instance.ControlAbility = ControlAbility.CanRestart | 
                ControlAbility.CanShowStageList | 
                ControlAbility.CanSwitchStage |
                ControlAbility.HasHelpContent |
                ControlAbility.CanGoback;

            this.Dispatcher.BeginInvoke(new EventHandler<EventArgs>(delayLoad), DispatcherPriority.Background, new object[] { null, null });

            this.puzzleImageMouseButtonEventHandler = ((sender, e1) =>
            {
                this.mixUp();
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PuzzleControl.Instance.ControlPanlVisible(true);
        }

        private void win_DataLoadCompletedEvent()
        {
            this.Dispatcher.BeginInvoke(new EventHandler<EventArgs>(delayLoad), DispatcherPriority.Background, new object[] { null, null });
        }

        private void delayLoad(object sender, EventArgs e)
        {
            this.ResetPuzzle();
        }

        private void Load(PuzzleItem item)
        {
            this.DataContext = item;
            this.puzzleItem = item;
        }

        internal void ResetPuzzle()
        {
            if (this.puzzleItem == null)
                return;

            if (imageStream != null)
            {
                imageStream.Dispose();
                imageStream = null;
            }

            byte[] imageData = PuzzleData.LoadImageData(this.puzzleItem.ImageFile);

            this.imageStream = new System.IO.MemoryStream(imageData);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = imageStream;
            bi.EndInit();

            Image image = new Image();
            image.Source = bi;
            image.Stretch = Stretch.Uniform;

            this.CalcPuzzleSize(bi.PixelWidth, bi.PixelHeight);

            image.MouseDown += this.puzzleImageMouseButtonEventHandler;

            Border border = new Border();
            border.Style = App.Current.FindResource("puzzleBorderStyle") as Style;

            border.Child = image;
            border.Width = puzzleSize.Width;
            border.Height = puzzleSize.Height;

            this.unsubscribEvent();

            this.puzzleHostingPanel.Children.Clear();
            this.puzzleHostingPanel.Children.Add(border);

            this.statusLabel.Text = "";
            this.answerLabel.Text = "";
            this.totalSteps = -1;
            this.usedSteps = -1;
            this.getAnswerBtn.IsEnabled = false;
            this.nextStepBtn.IsEnabled = false;
            this.movedSteps = 0;
        }

        private void unsubscribEvent()
        {
            if (this.puzzleHostingPanel.Children.Count > 0)
            {
                if (this.puzzleHostingPanel.Children[0] is Border)
                {
                    Border border = this.puzzleHostingPanel.Children[0] as Border;
                    if (border.Child is Image)
                    {
                        Image image = border.Child as Image;
                        image.MouseDown -= this.puzzleImageMouseButtonEventHandler;
                    }
                }
            }
        }

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.mixUp();
        }

        private void CreatePuzzle()
        {
            this.puzzleHostingPanel.Children.Clear();

            this.puzzleGrid = new PuzzleGrid();
            this.puzzleGrid.PuzzleWon += delegate(object sender, EventArgs e)
            {
                this.getAnswerBtn.IsEnabled = false;
                this.nextStepBtn.IsEnabled = false;
                this.showNumbersCheckBox.IsEnabled = false;

                this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    ResultWindow win = new ResultWindow();
                    win.State = ResultState.Pass;
                    win.ShowMessage(MessageWindowCallback);
                }),
                DispatcherPriority.ApplicationIdle,
                null);
            };

            if (puzzleStream != null)
            {
                puzzleStream.Dispose();
                puzzleStream = null;
            }

            byte[] imageData = PuzzleData.LoadImageData(this.puzzleItem.ImageFile);

            if (this.puzzleStream != null)
            {
                this.puzzleStream.Dispose();
                this.puzzleStream = null;
            }

            this.puzzleStream = new System.IO.MemoryStream(imageData);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = this.puzzleStream;
            bi.EndInit();

            Image image = new Image();
            image.Source = bi;

            image.Width = puzzleSize.Width;
            image.Height = puzzleSize.Height;

            this.elementToChopUp = image;

            this.puzzleGrid.MoveMade += new EventHandler<HandledEventArgs>(OnMoveMade);

            this.puzzleGrid.IsApplyingStyle = true;
            this.puzzleGrid.NumRows = PuzzleSetting.Instance.Rows;
            this.puzzleGrid.NumCols = PuzzleSetting.Instance.Cols;

            this.puzzleGrid.ElementToChopUp = elementToChopUp;
            this.puzzleGrid.PuzzleSize = puzzleSize;

            this.puzzleGrid.ShowNumbers(this.showNumbersCheckBox.IsChecked.Value);

            this.puzzleGrid.ShouldAnimateInteractions = true;

            this.showNumbersCheckBox.IsEnabled = true;

            puzzleHostingPanel.Children.Add(this.puzzleGrid);
        }

        private void MessageWindowCallback(bool ok)
        {
            if (ok)
            {
                this.NextStage();
                return;
            }
            else
            {
                this.ResetPuzzle();
                this.mixUp();
            }
        }

        private void CalcPuzzleSize(double imageWidth, double imageHeight)
        {
            double gridWidth = this.puzzleHostingPanel.ActualWidth;
            double gridHeight = this.puzzleHostingPanel.ActualHeight;
            double ratio = gridWidth / gridHeight;
            double imageRatio = imageWidth / imageHeight;
            if (imageWidth > this.puzzleHostingPanel.ActualWidth &&
                imageHeight > this.puzzleHostingPanel.ActualHeight)
            {
                if (ratio > imageRatio)
                {
                    this.puzzleSize.Height = gridHeight;
                    this.puzzleSize.Width = this.puzzleSize.Height * imageRatio;
                }
                else
                {
                    this.puzzleSize.Width = gridWidth;
                    this.puzzleSize.Height = this.puzzleSize.Width * imageRatio;
                }
            }
            else if (imageWidth > this.puzzleHostingPanel.ActualWidth)
            {
                this.puzzleSize.Width = gridWidth;
                this.puzzleSize.Height = this.puzzleSize.Width / imageRatio;
            }
            else if (imageHeight > this.puzzleHostingPanel.ActualHeight)
            {
                this.puzzleSize.Height = gridHeight;
                this.puzzleSize.Width = this.puzzleSize.Height * imageRatio;
            }
            else
            {
                this.puzzleSize.Width = imageWidth;
                this.puzzleSize.Height = imageHeight;
            }

            Storyboard storyboard = (Storyboard)this.FindResource("resetStoryboard");
            storyboard.Begin();
        }

        private void OnMoveMade(object sender, HandledEventArgs e)
        {
            this.movedSteps++;
            this.statusLabel.Text = string.Format(SoonLearning.BlockPuzzle.Properties.Resources.movedStepCount, this.movedSteps);

            this.totalSteps = 0;
            this.getAnswerBtn.IsEnabled = true;
            this.nextStepBtn.IsEnabled = false;
            this.answerLabel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void showNumbersCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            puzzleGrid.ShowNumbers(cb.IsChecked.Value);
        }

        private void nextStepBtn_Click(object sender, RoutedEventArgs e)
        {
            MoveToNextStep();

            this.answerLabel.Text = string.Format(SoonLearning.BlockPuzzle.Properties.Resources.StepsOfTotal, this.totalSteps - usedSteps, this.totalSteps);
        }

        private void getAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
        //    IntPtr gcPtr = new IntPtr();
        //    PuzzleMxNLib.PGInit(gcPtr);

            byte[] nums = this.puzzleGrid.GetNumberList();
            byte[] retNums = new byte[nums.Length];
            for (int i = 0; i < retNums.Length; i++)
                retNums[i] = (byte)i;

            int ret = PuzzleMxNLib.PGGetAnswer(nums, ref retNums, PuzzleSetting.Instance.Cols, PuzzleSetting.Instance.Rows);
            if (ret > 0)
                this.nextStepBtn.IsEnabled = true;
            else
                this.nextStepBtn.IsEnabled = false;

            this.totalSteps = ret;
            this.usedSteps = ret;

            if (this.totalSteps > 0)
                this.totalSteps--;

            this.answerLabel.Visibility = System.Windows.Visibility.Visible;
            this.answerLabel.Text = string.Format(SoonLearning.BlockPuzzle.Properties.Resources.StepsOfTotal, 0, this.totalSteps);

            for (int i = 0; i < retNums.Length; i++)
                retNums[i]++;

            MoveToNextStep();
        }

        private void MoveToNextStep()
        {
            if (this.totalSteps <= 0 || usedSteps <= 0)
                return;

            byte[] nums = this.puzzleGrid.GetNumberList();
            
            PuzzleMxNLib.PGGetNextMovePath(PuzzleSetting.Instance.Cols,
                PuzzleSetting.Instance.Rows, usedSteps--, ref nums);

            for (int i = 0; i < nums.Length; i++)
            {
                this.puzzleGrid.MoveTo(nums[i], i + 1);
            }
        }

        private void mixUpBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    this.mixUp();
                }),
                DispatcherPriority.Normal,
                null);
        }

        private void mixUp()
        {
            this.CreatePuzzle();

            this.puzzleGrid.NewPuzzleDoneEvent += delegate(object s, EventArgs args)
            {
                this.puzzleGrid.MixUpPuzzle();

                this.getAnswerBtn.IsEnabled = true;
                this.nextStepBtn.IsEnabled = false;
            //    this.resetBtn.IsEnabled = true;
            };

            this.statusLabel.Text = "";
            this.answerLabel.Text = "";
            this.totalSteps = -1;
            this.usedSteps = -1;
            this.nextStepBtn.IsEnabled = false;
        //    this.resetBtn.IsEnabled = true;
            this.movedSteps = 0;
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            this.ResetPuzzle();
        }

        internal void NextStage()
        {
            this.GotoStage(ControlMgr.Instance.DataMgr.NextItem);
        }

        internal void PreStage()
        {
            this.GotoStage(ControlMgr.Instance.DataMgr.PreItem);
        }

        internal void GotoStage(int index)
        {
            if (index < 0)
                return;

            this.Load(ControlMgr.Instance.DataMgr.SelectedItem);
            this.ResetPuzzle();

            PuzzleControl.Instance.SelectedStage = index;
        }

        internal void Restart()
        {
            this.mixUp();
        }

        private void puzzleListBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.PuzzleStartupPage.ShowPuzzleList();
        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            this.NextStage();
        }

        private void prePage_Click(object sender, RoutedEventArgs e)
        {
            this.PreStage();
        }
    }
}
