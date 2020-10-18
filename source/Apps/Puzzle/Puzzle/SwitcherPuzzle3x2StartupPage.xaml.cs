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
using SoonLearning.BlockPuzzle.Controls;
using SoonLearning.BlockPuzzle.Data;
using System.Windows.Media.Effects;
using System.Runtime.InteropServices;
using System.ComponentModel;
using SoonLearning.BlockPuzzle.Properties;
using SoonLearning.BlockPuzzle.Puzzle;

namespace SoonLearning.BlockPuzzle.Puzzle3x2
{
    /// <summary>
    /// Interaction logic for Puzzle3x2StartupPagexaml.xaml
    /// </summary>
    public partial class SwitcherPuzzle3x2StartupPage : UserControl
    {
        private UIElement elementToChopUp;
        private SwitcherPuzzleGrid puzzleGrid = null;
        private Size puzzleSize;
        private bool stylingPuzzle = true;

        private int totalSteps = 0;
        private int usedSteps = 0;
        private int movedSteps = 0;

        private System.Windows.Forms.Label imgLabel;

        public SwitcherPuzzle3x2StartupPage()
        {
            InitializeComponent();
        }

        private void NewPuzzleGrid()
        {
            if (puzzleGrid != null)
            {
                puzzleHostingPanel.Children.Remove(puzzleGrid);
            }
            puzzleGrid = new SwitcherPuzzleGrid();
            puzzleGrid.PuzzleWon += delegate(object sender, EventArgs e)
            {
                this.gotItImageBorder.Visibility = System.Windows.Visibility.Visible;
            };

            puzzleGrid.MoveMade += new EventHandler<HandledEventArgs>(OnMoveMade);

            puzzleGrid.IsApplyingStyle = stylingPuzzle;
            puzzleGrid.NumRows = 2;
            puzzleGrid.NumCols = 3;

            puzzleGrid.ElementToChopUp = elementToChopUp;
            puzzleGrid.PuzzleSize = puzzleSize;

            puzzleGrid.ShowNumbers(ChkShowNumbers.IsChecked.Value);

            puzzleGrid.ShouldAnimateInteractions = true;

            puzzleHostingPanel.Children.Add(puzzleGrid);
        }

        private void OnMoveMade(object sender, HandledEventArgs e)
        {
            this.movedSteps++;
            this.ShowMovedSteps();
        }

        private void mixUpBtn_Click(object sender, RoutedEventArgs e)
        {
            this.gotItImageBorder.Visibility = System.Windows.Visibility.Hidden;

            puzzleGrid.MixUpPuzzle();

            this.statusLabel.Content = "";
            this.answerLabel.Content = "";
            this.totalSteps = -1;
            this.usedSteps = -1;
            this.nextStepBtn.IsEnabled = false;
        }

        private void resetBtn_Click(object sender, RoutedEventArgs e)
        {
            this.gotItImageBorder.Visibility = System.Windows.Visibility.Hidden;

            NewPuzzleGrid();

            this.statusLabel.Content = "";
            this.answerLabel.Content = "";
            this.totalSteps = -1;
            this.usedSteps = -1;
            this.nextStepBtn.IsEnabled = false;
            this.movedSteps = 0;
        }

        private void getAnswerBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextStepBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void ChkShowNumbers_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            puzzleGrid.ShowNumbers(cb.IsChecked.Value);
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new EventHandler<EventArgs>(DelayLoad),
                System.Windows.Threading.DispatcherPriority.ApplicationIdle,
                new object[] { this, EventArgs.Empty });
        }

        private void DelayLoad(object sender, EventArgs e)
        {
            Image masterImage = (Image)this.Resources["MasterImage"];
            elementToChopUp = masterImage;
            BitmapSource bitmap = (BitmapSource)masterImage.Source;
            puzzleSize = new Size(bitmap.PixelWidth / 1.8, bitmap.PixelHeight / 1.8);

            NewPuzzleGrid();

            if (this.imgLabel == null)
            {
                this.imgLabel = new System.Windows.Forms.Label();
                this.imgLabel.Visible = true;
                this.gotItImageHost.Child = this.imgLabel;
            }
        }

        private void ShowMovedSteps()
        {
            this.statusLabel.Content = string.Format(SoonLearning.BlockPuzzle.Properties.Resources.movedStepCount, this.movedSteps / 2);
        }

        internal bool GoBack()
        {
            return true;
        }
    }
}
