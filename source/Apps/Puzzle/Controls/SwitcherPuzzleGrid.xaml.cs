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
using System.ComponentModel;
using System.Windows.Media.Animation;
using SoonLearning.BlockPuzzle.Data;
using System.Diagnostics;
using System.Threading;

namespace SoonLearning.BlockPuzzle.Controls
{
    /// <summary>
    /// Interaction logic for PuzzleGrid.xaml
    /// </summary>
    public partial class SwitcherPuzzleGrid : Grid
    {
        #region Private Data

        private SwitcherPuzzleLogic puzzleLogic;
        private Size masterPuzzleSize = Size.Empty;
        private UIElement elementForPuzzle;
        private int numRows = -1;
        private int numCols = -1;
        private bool styling = true;
        private bool shouldAnimateInteractions = false;

        private Button movingBtn1;
        private Button movingBtn2;
        private Vector movingVec1;
        private Vector movingVec2;

        private Storyboard slideStoryboard = new Storyboard();
        private Storyboard slideStoryboard1 = new Storyboard();

        #endregion

        #region Grid Setup Logic

        public SwitcherPuzzleGrid()
        {
            InitializeComponent();

            this.AddHandler(Button.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(OnPuzzleButtonMouseLeftButtonDown));
            this.AddHandler(Button.PreviewMouseMoveEvent, new RoutedEventHandler(OnPuzzleButtonMouseMove));
            this.AddHandler(Button.PreviewMouseLeftButtonUpEvent, new RoutedEventHandler(OnPuzzleButtonMouseLeftButtonUp));
        }

        private void SetupThePuzzleGridStructure()
        {
            puzzleLogic = new SwitcherPuzzleLogic(numRows, numCols);

            // Define rows and columns in the Grid
            for (int row = 0; row < numRows; row++)
            {
                RowDefinition r = new RowDefinition();
                r.Height = GridLength.Auto;
                this.RowDefinitions.Add(r);
            }

            for (int col = 0; col < numCols; col++)
            {
                ColumnDefinition c = new ColumnDefinition();
                c.Width = GridLength.Auto;
                this.ColumnDefinitions.Add(c);
            }

            // Now add the buttons in
            int index = 1;
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    CreatePuzzleButton(index++, row, col);
                }
            }
        }

        private void CreatePuzzleButton(int index, int row, int col)
        {
            Style buttonStyle = (Style)this.Resources["PuzzleButtonStyle"];

            Button b = new Button();
            b.FontSize = 24;

            // Styling comes in only here...
            if (styling)
            {
                b.Style = buttonStyle;
            }

            b.SetValue(Grid.RowProperty, row);
            b.SetValue(Grid.ColumnProperty, col);
            b.Content = index.ToString();
            b.Name = string.Format("puzzle_btn_{0}", index);

            this.Children.Add(b);
        }

        #endregion

        #region Brush Tiling Logic

        private void PuzzleGridLoaded(object sender, RoutedEventArgs e)
        {
            if (styling)
            {
                this.Width = masterPuzzleSize.Width;
                this.Height = masterPuzzleSize.Height;

                float edgeSizeH = 1.0f / numRows;
                float edgeSizeW = 1.0f / numCols;
                double pieceRowHeight = masterPuzzleSize.Height / numRows;
                double pieceColWidth = masterPuzzleSize.Width / numCols;

                // Set up viewbox appropriately for each tile.
                foreach (Button btn in this.Children)
                {
                    Border root = (Border)btn.Template.FindName("TheTemplateRoot", btn);

                    int row = (int)btn.GetValue(Grid.RowProperty);
                    int col = (int)btn.GetValue(Grid.ColumnProperty);

                    VisualBrush vb = new VisualBrush(elementForPuzzle);
                    vb.Viewbox = new Rect(col * edgeSizeW, row * edgeSizeH, edgeSizeW, edgeSizeH);
                    vb.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;

                    root.Background = vb;
                    root.Height = pieceRowHeight;
                    root.Width = pieceColWidth;
                }
            }
        }

        #endregion

        #region Interaction Logic

        private Point buttonMoveStartPos;

        private void OnPuzzleButtonMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Button b = e.Source as Button;
            if (b != null)
            {
                buttonMoveStartPos = Mouse.GetPosition(this);
            }
        }

        private void OnPuzzleButtonMouseMove(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;

            Button b = e.Source as Button;
            if (b != null)
            {
                Point curPt = Mouse.GetPosition(this);
                Vector deltaVc = curPt - buttonMoveStartPos;

                if (Math.Abs(deltaVc.X) > Math.Abs(deltaVc.Y))
                {
                    b.RenderTransform = new TranslateTransform(deltaVc.X, 0);
                }
                else
                {
                    b.RenderTransform = new TranslateTransform(0, deltaVc.Y);
                }
            }
        }

        private void OnPuzzleButtonMouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            Button b = e.Source as Button;
            if (b != null)
            {
                Point curPt = Mouse.GetPosition(this);
                Vector deltaVc = curPt - buttonMoveStartPos;

                int row = (int)b.GetValue(Grid.RowProperty);
                int col = (int)b.GetValue(Grid.ColumnProperty);

                if (Math.Abs(deltaVc.X) > Math.Abs(deltaVc.Y))
                {
                    b.RenderTransform = new TranslateTransform(deltaVc.X, 0);
                    if (Math.Abs(deltaVc.X) >= b.ActualWidth / 2)
                    {
                        if (deltaVc.X > 0)
                        {
                            if (col == this.numCols - 1)
                            {
                                b.RenderTransform = null;
                                return;
                            }
                            AnimatePiece(b, row, col + 1, MoveStatus.Right);
                        }
                        else
                        {
                            if (col == 0)
                            {
                                b.RenderTransform = null;
                                return;
                            }
                            AnimatePiece(b, row, col - 1, MoveStatus.Left);
                        }
                    }
                    else
                    {
                        b.RenderTransform = null;
                    }
                }
                else
                {
                    b.RenderTransform = new TranslateTransform(0, deltaVc.Y);
                    if (Math.Abs(deltaVc.Y) >= b.ActualHeight / 2)
                    {
                        if (deltaVc.Y > 0)
                        {
                            if (row == this.numRows - 1)
                            {
                                b.RenderTransform = null;
                                return;
                            }
                            AnimatePiece(b, row + 1, col, MoveStatus.Down);
                        }
                        else
                        {
                            if (row == 0)
                            {
                                b.RenderTransform = null;
                                return;
                            }
                            AnimatePiece(b, row - 1, col, MoveStatus.Up);
                        }
                    }
                    else
                    {
                        b.RenderTransform = null;
                    }
                }
            }
        }


        // Assumed to be a valid move.
        private void AnimatePiece(Button b, int row, int col, MoveStatus moveStatus)
        {
            if (MoveMade != null)
            {
                MoveMade(this, new HandledEventArgs(moveStatus != MoveStatus.BadMove));
            }

            FrameworkElement root = (FrameworkElement)b.Template.FindName("TheTemplateRoot", b);

            double distance;
            bool isMoveHorizontal;

            TranslateTransform trans = b.RenderTransform as TranslateTransform;

            Debug.Assert(moveStatus != MoveStatus.BadMove);
            if (moveStatus == MoveStatus.Left || moveStatus == MoveStatus.Right)
            {
                isMoveHorizontal = true;
                double x = 0;
                if (trans != null)
                    x = trans.X;
                if (moveStatus == MoveStatus.Left)
                    distance = -(root.Width + x);
                else
                    distance = root.Width - x;
            }
            else
            {
                isMoveHorizontal = false;
                double y = 0;
                if (trans != null)
                    y = trans.Y;
                if (moveStatus == MoveStatus.Up)
                    distance = -(root.Height + y);
                else
                    distance = root.Height - y;
            }

            // pull the animation after it's complete, because we move change Grid cells.
            DoubleAnimation slideAnim = new DoubleAnimation(distance, TimeSpan.FromSeconds(0.3), FillBehavior.Stop);
            slideAnim.Completed += new EventHandler(slideAnim_Completed);
            this.movingBtn1 = b;
            this.movingVec1 = new Vector(row, col);

            foreach (Button btn in this.Children)
            {
                if (isMoveHorizontal)
                {
                    if ((int)btn.GetValue(Grid.ColumnProperty) == col &&
                        (int)btn.GetValue(Grid.RowProperty) == row)
                    {
                        if (moveStatus == MoveStatus.Right)
                            AnimatePiece2(btn, row, col - 1, MoveStatus.Left);
                        else
                            AnimatePiece2(btn, row, col + 1, MoveStatus.Right);
                        break;
                    }
                }
                else
                {
                    if ((int)btn.GetValue(Grid.RowProperty) == row &&
                        (int)btn.GetValue(Grid.ColumnProperty) == col)
                    {
                        if (moveStatus == MoveStatus.Down)
                            AnimatePiece2(btn, row - 1, col, MoveStatus.Up);
                        else
                            AnimatePiece2(btn, row + 1, col, MoveStatus.Down);
                        break;
                    }
                }
            }

            DependencyProperty directionProperty =
                isMoveHorizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;

            this.slideStoryboard.Children.Add(slideAnim);
            Storyboard.SetTarget(slideAnim, b);
            Storyboard.SetTargetName(slideAnim, b.Name);
            Storyboard.SetTargetProperty(slideAnim, new PropertyPath(directionProperty));

            this.slideStoryboard.Begin();
        }

        private void AnimatePiece2(Button b, int row, int col, MoveStatus moveStatus)
        {
            if (MoveMade != null)
            {
                MoveMade(this, new HandledEventArgs(moveStatus != MoveStatus.BadMove));
            }

            FrameworkElement root = (FrameworkElement)b.Template.FindName("TheTemplateRoot", b);

            double distance;
            bool isMoveHorizontal;

            TranslateTransform trans = b.RenderTransform as TranslateTransform;

            Debug.Assert(moveStatus != MoveStatus.BadMove);
            if (moveStatus == MoveStatus.Left || moveStatus == MoveStatus.Right)
            {
                isMoveHorizontal = true;
                double x = 0;
                if (trans != null)
                    x = trans.X;
                if (moveStatus == MoveStatus.Left)
                    distance = -(root.Width + x);
                else
                    distance = root.Width - x;
            }
            else
            {
                isMoveHorizontal = false;
                double y = 0;
                if (trans != null)
                    y = trans.Y;
                if (moveStatus == MoveStatus.Up)
                    distance = -(root.Height + y);
                else
                    distance = root.Height - y;
            }

            // pull the animation after it's complete, because we move change Grid cells.
            DoubleAnimation slideAnim = new DoubleAnimation(distance, TimeSpan.FromSeconds(0.3), FillBehavior.Stop);
            slideAnim.Completed += new EventHandler(slideAnim_Completed2);
            this.movingBtn2 = b;
            this.movingVec2 = new Vector(row, col);

            DependencyProperty directionProperty =
                isMoveHorizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;

            this.slideStoryboard1.Children.Add(slideAnim);
            Storyboard.SetTarget(slideAnim, b);
            Storyboard.SetTargetName(slideAnim, b.Name);
            Storyboard.SetTargetProperty(slideAnim, new PropertyPath(directionProperty));

            this.slideStoryboard1.Begin();
        }

        private void slideAnim_Completed(object sender, EventArgs e)
        {
            if (this.movingBtn1 != null)
            {
                this.movingBtn1.RenderTransform = null;
                this.MovePieceTo(this.movingBtn1, (int)this.movingVec1.X, (int)this.movingVec1.Y);

                this.slideStoryboard.Children.Clear();
            }
        }

        private void slideAnim_Completed2(object sender, EventArgs e)
        {
            if (this.movingBtn2 != null)
            {
                this.movingBtn2.RenderTransform = null;
                this.MovePieceTo(this.movingBtn2, (int)this.movingVec2.X, (int)this.movingVec2.Y);

                this.slideStoryboard1.Children.Clear();
            }
        } 

        // Assumed to be a valid move.
        private void MovePiece(Button b, int row, int col)
        {
            int fromRow = (int)b.GetValue(Grid.RowProperty);
            int fromCol = (int)b.GetValue(Grid.ColumnProperty);

            puzzleLogic.MovePiece(fromRow, fromCol, row, col);

            b.SetValue(Grid.ColumnProperty, col);
            b.SetValue(Grid.RowProperty, row);

            if (puzzleLogic.CheckForWin())
            {
                if (PuzzleWon != null)
                {
                    PuzzleWon(this, EventArgs.Empty);
                }
            }
        }

        private void MovePieceTo(Button b, int row, int col)
        {
            int fromRow = (int)b.GetValue(Grid.RowProperty);
            int fromCol = (int)b.GetValue(Grid.ColumnProperty);

            int index = this.Children.IndexOf(b);
            puzzleLogic.SetPieceValue(row, col, index);

            if (row >= 0 && row < numRows)
                b.SetValue(Grid.RowProperty, row);

            if (col >= 0 && col < numCols)
                b.SetValue(Grid.ColumnProperty, col);

            if (puzzleLogic.CheckForWin())
            {
                if (PuzzleWon != null)
                {
                    PuzzleWon(this, EventArgs.Empty);
                }
            }
        }

        // from: button indx;
        // to: cell index
        public void MoveTo(int from, int to)
        {
            for (int row = 1; row <= numRows; row++)
            {
                for (int col = 1; col <= numCols; col++)
                {
                    if ((row - 1) * numCols + col == to)
                    {
                        this.MovePieceTo(this.Children[from] as Button, row - 1, col - 1);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Less Consequential Portions

        public void MixUpPuzzle()
        {
            puzzleLogic.MixUpPuzzle();

            short cellNumber = 0;
            foreach (Button b in this.Children)
            {
                PuzzleCell location = puzzleLogic.FindCell(cellNumber++);
                b.SetValue(Grid.ColumnProperty, location.Col);
                b.SetValue(Grid.RowProperty, location.Row);
            }
        }

        public byte[] GetNumberList()
        {
            List<byte> numList = new List<byte>();

            foreach (short s in this.puzzleLogic.GetCells())
            {
                numList.Add((byte)s);
            }

            return numList.ToArray();
        }

        public void ShowNumbers(bool showNumbers)
        {
            SolidColorBrush nlb = ((SolidColorBrush)this.Resources["NumberLabelBrush"]).Clone();
            nlb.Color = showNumbers ? Colors.Yellow : Colors.Transparent;
            this.Resources["NumberLabelBrush"] = nlb;
        }

        #endregion

        #region Public Properties

        public int NumRows
        {
            get { return numRows; }
            set
            {
                // Only support setting this once per PuzzleGrid instance.
                if (numRows != -1)
                {
                    throw new InvalidOperationException("NumRows already initialized for this PuzzleGrid instance.");
                }
                else
                {
                    numRows = value;
                    CheckToSetup();
                }
            }
        }

        public int NumCols
        {
            get { return this.numCols; }
            set
            {
                // Only support setting this once per PuzzleGrid instance.
                if (numCols != -1)
                {
                    throw new InvalidOperationException("NumRows already initialized for this PuzzleGrid instance.");
                }
                else
                {
                    numCols = value;
                    CheckToSetup();
                }
            }
        }

        public UIElement ElementToChopUp
        {
            get { return elementForPuzzle; }
            set
            {
                if (elementForPuzzle != null)
                {
                    throw new InvalidOperationException("ElementForPuzzle already initialized for this PuzzleGrid instance.");
                }
                else
                {
                    elementForPuzzle = value;
                    CheckToSetup();
                }
            }
        }

        public bool IsApplyingStyle
        {
            get { return styling; }
            set { styling = value; }
        }

        public bool ShouldAnimateInteractions
        {
            get { return shouldAnimateInteractions; }
            set { shouldAnimateInteractions = value; }
        }

        public Size PuzzleSize
        {
            get { return masterPuzzleSize; }
            set
            {
                if (!masterPuzzleSize.IsEmpty)
                {
                    throw new InvalidOperationException("SizeForPuzzle already initialized for this PuzzleGrid instance.");
                }
                else
                {
                    masterPuzzleSize = value;
                    CheckToSetup();
                }
            }
        }

        public event EventHandler PuzzleWon;
        public event EventHandler<HandledEventArgs> MoveMade;

        private void CheckToSetup()
        {
            if (numRows != -1 && (!styling || elementForPuzzle != null) && !masterPuzzleSize.IsEmpty)
            {
                SetupThePuzzleGridStructure();
            }
        }


        #endregion
    }
}
