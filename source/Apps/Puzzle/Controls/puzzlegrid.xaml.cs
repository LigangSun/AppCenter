using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using SoonLearning.BlockPuzzle.Data;
using System.Reflection;
using SoonLearning.BlockPuzzle.Puzzle;
using System.Threading;
using System.Windows.Threading;


namespace SoonLearning.BlockPuzzle.Controls
{
	public partial class PuzzleGrid
	{
        public EventHandler<EventArgs> NewPuzzleDoneEvent;

		#region Grid Setup Logic
		
		public PuzzleGrid()
		{
			InitializeComponent();

			// Centralize handling of all clicks in the puzzle grid.
			this.AddHandler(Button.ClickEvent, new RoutedEventHandler(OnPuzzleButtonClick));
		}

		private void SetupThePuzzleGridStructure()
		{
            _puzzleLogic = new PuzzleLogic(_numRows, _numCols);

			// Define rows and columns in the Grid
			for (int row = 0; row < _numRows; row++)
			{
				RowDefinition r = new RowDefinition();
				r.Height = GridLength.Auto;
                this.puzzleGrid.RowDefinitions.Add(r);
			}

            for (int col = 0; col < _numCols; col++)
            {
                ColumnDefinition c = new ColumnDefinition();
                c.Width = GridLength.Auto;
                this.puzzleGrid.ColumnDefinitions.Add(c);
            }

			Style buttonStyle = (Style)this.Resources["PuzzleButtonStyle"];

			// Now add the buttons in
			int i = 1;
			for (int row = 0; row < _numRows; row++)
			{
				for (int col = 0; col < _numCols; col++)
				{
					// lower right cell is empty
                    if (_numRows != 1 && row == 0 && _numCols != 1 && col == 0)
					{
						continue;
					}
					Button b = new Button();
                    b.FontSize = 24;

                    // Styling comes in only here...
					if (_styling)
					{
						b.Style = buttonStyle;
					}

					b.SetValue(Grid.RowProperty, row);
					b.SetValue(Grid.ColumnProperty, col);
					b.Content = i.ToString();
					i++;
					this.puzzleGrid.Children.Add(b);
				}
			}
		}

		#endregion

		#region Brush Tiling Logic

		private void PuzzleGridLoaded(object sender, RoutedEventArgs e) 
		{
            if (this.puzzleGrid.Children.Count <= 1)
                return;

			if (_styling)
			{
				this.Width = _masterPuzzleSize.Width;
				this.Height = _masterPuzzleSize.Height;

                float edgeSizeH = 1.0f / _numRows;
                float edgeSizeW = 1.0f / _numCols;
                double pieceRowHeight = _masterPuzzleSize.Height / _numRows;
                double pieceColWidth = _masterPuzzleSize.Width / _numCols;

                // Set up viewbox appropriately for each tile.
                foreach (Button btn in this.puzzleGrid.Children)
                {
                    Border root = (Border)btn.Template.FindName("TheTemplateRoot", btn);

                    int row = (int)btn.GetValue(Grid.RowProperty);
                    int col = (int)btn.GetValue(Grid.ColumnProperty);

                    VisualBrush vb = new VisualBrush(_elementForPuzzle);
                    vb.Viewbox = new Rect(col * edgeSizeW, row * edgeSizeH, edgeSizeW, edgeSizeH);
                    vb.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;

                    root.Background = vb;
                    root.Height = pieceRowHeight;
                    root.Width = pieceColWidth;
                }
			}

            if (this.NewPuzzleDoneEvent != null)
                this.NewPuzzleDoneEvent(this, EventArgs.Empty);
		}

		#endregion

		#region Interaction Logic

		private void OnPuzzleButtonClick(object sender, RoutedEventArgs e)
		{
			Button b = e.Source as Button;
			if (b != null)
			{
                ControlMgr.Instance.PlayClickSound();

				int row = (int)b.GetValue(Grid.RowProperty);
				int col = (int)b.GetValue(Grid.ColumnProperty);

				MoveStatus moveStatus = _puzzleLogic.GetMoveStatus(row, col);

                if (MoveMade != null)
                {
                    MoveMade(this, new HandledEventArgs(moveStatus != MoveStatus.BadMove));
                }

                if (moveStatus != MoveStatus.BadMove)
				{
					if (!_styling || !_shouldAnimateInteractions)
					{
						// Not templated, just move piece
						MovePiece(b, row, col);
					}
					else
					{
						AnimatePiece(b, row, col, moveStatus);
					}
				}

			}
		}


		// Assumed to be a valid move.
		private void AnimatePiece(Button b, int row, int col, MoveStatus moveStatus)
		{
            FrameworkElement root = (FrameworkElement)b.Template.FindName("TheTemplateRoot", b);

			double distance;
            bool isMoveHorizontal;

            Debug.Assert(moveStatus != MoveStatus.BadMove);
            if (moveStatus == MoveStatus.Left || moveStatus == MoveStatus.Right)
			{
                isMoveHorizontal = true;
				distance = (moveStatus == MoveStatus.Left ? -1 : 1) * root.Width;
			}
			else
			{
                isMoveHorizontal = false;
                distance = (moveStatus == MoveStatus.Up ? -1 : 1) * root.Height;
			}

			// pull the animation after it's complete, because we move change Grid cells.
			DoubleAnimation slideAnim = new DoubleAnimation(distance, TimeSpan.FromSeconds(0.1), FillBehavior.Stop);
			slideAnim.CurrentStateInvalidated += delegate(object sender2, EventArgs e2)
			{
				// Anonymous delegate -- invoke when done.
				Clock clock = (Clock)sender2;
				if (clock.CurrentState != ClockState.Active)
				{
					// remove the render transform and really move the piece in the Grid.
					MovePiece(b, row, col);
				}
			};

            DependencyProperty directionProperty = 
                isMoveHorizontal ? TranslateTransform.XProperty : TranslateTransform.YProperty;

            root.RenderTransform.BeginAnimation(directionProperty, slideAnim);
		}


		// Assumed to be a valid move.
		private void MovePiece(Button b, int row, int col)
		{
			PuzzleCell newPosition = _puzzleLogic.MovePiece(row, col);

			b.SetValue(Grid.ColumnProperty, newPosition.Col);
			b.SetValue(Grid.RowProperty, newPosition.Row);

			if (_puzzleLogic.CheckForWin())
			{
                this.initPuzzle();

				if (PuzzleWon != null)
				{
					PuzzleWon(this, EventArgs.Empty);
				}
			}
        }

        #endregion

        #region Less Consequential Portions

        public void MixUpPuzzle()
		{
			_puzzleLogic.MixUpPuzzle();

			short cellNumber = 1;
            foreach (Button b in this.puzzleGrid.Children)
			{
				PuzzleCell location = _puzzleLogic.FindCell(cellNumber++);
				b.SetValue(Grid.ColumnProperty, location.Col);
				b.SetValue(Grid.RowProperty, location.Row);
			}
		}

		public void ShowNumbers(bool showNumbers)
		{
			SolidColorBrush nlb = ((SolidColorBrush)this.Resources["NumberLabelBrush"]).Clone();
			nlb.Color = showNumbers ? Colors.Yellow : Colors.Transparent;
            this.Resources["NumberLabelBrush"] = nlb;
		}

        public byte[] GetNumberList()
        {
            List<byte> numList = new List<byte>();

            foreach (short s in this._puzzleLogic.GetCells())
            {
                numList.Add((byte)s);
            }

            return numList.ToArray();
        }

        public byte[][] GetCells()
        {
            short[,] cells = this._puzzleLogic.GetCells();
            byte[][] ret = new byte[this._numCols][];
            for (int c = 0; c < _numCols; c++)
            {
                for (int r = 0; r < _numRows; r++)
                {
                    ret[c][r] = (byte)cells[r, c];
                }
            }

            return ret;
        }

        #endregion

        #region Public Properties

        public int NumRows
		{
			get { return _numRows; }
			set
			{
				// Only support setting this once per PuzzleGrid instance.
				if (_numRows != -1)
				{
					throw new InvalidOperationException("NumRows already initialized for this PuzzleGrid instance.");
				}
				else
				{
					_numRows = value;
					CheckToSetup();
				}
			}
		}

        public int NumCols
        {
            get { return this._numCols; }
            set
            {
                // Only support setting this once per PuzzleGrid instance.
                if (_numCols != -1)
                {
                    throw new InvalidOperationException("NumRows already initialized for this PuzzleGrid instance.");
                }
                else
                {
                    _numCols = value;
                    CheckToSetup();
                }
            }
        }

		public UIElement ElementToChopUp
		{
			get { return _elementForPuzzle; }
			set
			{
                if (_elementForPuzzle != null)
				{
                    throw new InvalidOperationException("ElementForPuzzle already initialized for this PuzzleGrid instance.");
				}
				else
				{
                    _elementForPuzzle = value;
					CheckToSetup();
				}
			}
		}

		public bool IsApplyingStyle
		{
			get { return _styling; }
			set { _styling = value; }
		}

        public bool ShouldAnimateInteractions
        {
            get { return _shouldAnimateInteractions; }
            set { _shouldAnimateInteractions = value; }
        }

		public Size PuzzleSize
		{
			get { return _masterPuzzleSize; }
			set
			{
				if (!_masterPuzzleSize.IsEmpty)
				{
					throw new InvalidOperationException("SizeForPuzzle already initialized for this PuzzleGrid instance.");
				}
				else
				{
					_masterPuzzleSize = value;
					CheckToSetup();
				}
			}
		}

		public event EventHandler PuzzleWon;
        public event EventHandler<HandledEventArgs> MoveMade;
        
		private void CheckToSetup()
		{
            if (_numRows != -1 && _numCols  != -1 && (!_styling || _elementForPuzzle != null) && !_masterPuzzleSize.IsEmpty)
			{
				SetupThePuzzleGridStructure();
			}
		}

        // from: button indx;
        // to: cell index
        public void MoveTo(int from, int to)
        {
            if (this.puzzleGrid.Children.Count == 1)
                return;

            for (int row = 1; row <= _numRows; row++)
            {
                for (int col = 1; col <= _numCols; col++)
                {
                    if ((row - 1) * _numCols + col == to)
                    {
                        if (from == 0)
                        {
                            this.MovePieceTo(null, row - 1, col - 1);
                        }
                        else
                        {
                            this.MovePieceTo(this.puzzleGrid.Children[from - 1] as Button, row - 1, col - 1);
                        }
                        break;
                    }
                }
            }
        }

        private void MovePieceTo(Button b, int row, int col)
        {
            if (b == null)
            {
                _puzzleLogic.SetPieceValue(row, col, 0);
            }
            else
            {
                int fromRow = (int)b.GetValue(Grid.RowProperty);
                int fromCol = (int)b.GetValue(Grid.ColumnProperty);

                int index = this.puzzleGrid.Children.IndexOf(b);
                _puzzleLogic.SetPieceValue(row, col, index + 1);

                if (row >= 0 && row < _numRows)
                    b.SetValue(Grid.RowProperty, row);

                if (col >= 0 && col < _numCols)
                    b.SetValue(Grid.ColumnProperty, col);
            }

            if (_puzzleLogic.CheckForWin())
            {
                this.initPuzzle();

                this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    if (PuzzleWon != null)
                    {
                        PuzzleWon(this, EventArgs.Empty);
                    }
                }),
                DispatcherPriority.ApplicationIdle,
                null);
            }
        }

        internal void initPuzzle()
        {
            if (this.puzzleGrid.Children.Count == _numRows * _numCols)
                return;

            this.puzzleGrid.Children.Clear();

            this.puzzleGrid.Children.Add(this._elementForPuzzle);

            Grid.SetColumnSpan(this._elementForPuzzle, _numCols);
            Grid.SetRowSpan(this._elementForPuzzle, _numRows);
        }

		#endregion

		#region Private Data

		private PuzzleLogic _puzzleLogic;
		private Size _masterPuzzleSize = Size.Empty;
		private UIElement _elementForPuzzle;
		private int _numRows = -1;
        private int _numCols = -1;
		private bool _styling = true;
        private bool _shouldAnimateInteractions = false;

		#endregion
	}
}