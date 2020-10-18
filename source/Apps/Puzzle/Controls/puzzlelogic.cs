using System;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using SoonLearning.BlockPuzzle.Data;
using System.Threading;

namespace SoonLearning.BlockPuzzle.Controls
{
	class PuzzleLogic
	{
		public PuzzleLogic(int numRows, int numCols)
		{
			_numRows = numRows;
            _numCols = numCols;

            _cells = new short[_numRows, _numCols];

			short tileNumber = 1;
			for (int r = 0; r < _numRows; r++)
			{
                for (int c = 0; c < _numCols; c++)
				{
                    if (r == 0 && c == 0)
                        continue;
					_cells[r, c] = tileNumber++;
				}
			}
			
			// overwrite empty cell
			_emptyRow = 0;
            _emptyCol = 0;
			_cells[_emptyRow, _emptyCol] = EMPTY_CELL_ID;
		}

		public bool IsEmptyCell(int row, int col)
		{
			return (row == _emptyRow && col == _emptyCol);
		}

		public MoveStatus GetMoveStatus(int row, int col)
		{
            int rowDiff = _emptyRow - row;
            int colDiff = _emptyCol - col;

            if (rowDiff == 0 && colDiff == 1)
            {
                return MoveStatus.Right;
            }
            else if (rowDiff == 0 && colDiff == -1)
            {
                return MoveStatus.Left;
            }
            else if (colDiff == 0 && rowDiff == 1)
            {
                return MoveStatus.Down;
            }
            else if (colDiff == 0 && rowDiff == -1)
            {
                return MoveStatus.Up;
            }
            else
            {
                return MoveStatus.BadMove;
            }
		}

		/// <summary>
		/// Moves the piece
		/// </summary>
		/// <param name="row"></param>
		/// <param name="col"></param>
		/// <returns>The cell of the newly opened position</returns>
		public PuzzleCell MovePiece(int row, int col)
		{
		//	Debug.Assert(GetMoveStatus(row, col) != MoveStatus.BadMove);

			PuzzleCell cell = new PuzzleCell(_emptyRow, _emptyCol, EMPTY_CELL_ID);

			short origCell = _cells[row, col];

			_cells[_emptyRow, _emptyCol] = origCell;
			_cells[row, col] = EMPTY_CELL_ID;

			_emptyCol = col;
			_emptyRow = row;

			return cell;
		}

		public bool CheckForWin()
		{
			// Easy out with check for empty cell
        //    if (_emptyRow != 0 || _emptyCol != 0)
			{
		//		return false;
			}

			// Just walk through cells and make sure they're all consecutive values
			short tileNumber = 0;
			for (int r = 0; r < _numRows; r++)
			{
                for (int c = 0; c < _numCols; c++)
				{
					if (tileNumber++ != _cells[r, c])
					{
						// Something is in the wrong place, unless we hit the empty cell.
                        if (!(r == 0 && c == 0))
						{
							return false;
						}
					}
				}
			}

            _emptyCol = 0;
            _emptyRow = 0;

			return true;
		}

		public PuzzleCell FindCell(short cellNumber)
		{
            Debug.Assert(cellNumber < _numRows * _numCols && cellNumber > 0);

			// This is a slow, linear operation, but for the size puzzles we have, it doesn't matter.
			for (int r = 0; r < _numRows; r++)
			{
				for (int c = 0; c < _numCols; c++)
				{
					if (_cells[r, c] == cellNumber)
					{
						return new PuzzleCell(r, c, cellNumber);
					}
				}
			}

			Debug.Assert(false, "Should have found a matching cell");
			return new PuzzleCell(-1, -1, -1);
		}

		public void MixUpPuzzle()
		{
			// Ensure that we can still solve it by only emulating legal moves.
			Random r = new Random();
            int cellCount = _numCols * _numRows;
            int i = 8 * cellCount;  // fairly arbitrary choice of number of moves
            if (i % 2 == 0)
                i += 3;
			while (i > 0)
			{
                Thread.Sleep(0);
                int choice = r.Next(4);
				int row = -1;
				int col = -1;

				// 0,1,2,3 - left, right, up, down from empty cell, when possible.  Skip when not.
				switch (choice)
				{
					case 0:
						if (_emptyCol != 0)
						{
							col = _emptyCol - 1;
							row = _emptyRow;
						}
						break;

					case 1:
						if (_emptyCol != _numCols - 1)
						{
							col = _emptyCol + 1;
							row = _emptyRow;
						}
						break;

					case 2:
						if (_emptyRow != 0)
						{
							row = _emptyRow - 1;
							col = _emptyCol;
						}
						break;

					case 3:
						if (_emptyRow != _numRows - 1)
						{
							row = _emptyRow + 1;
							col = _emptyCol;
						}
						break;
				}
				if (row != -1)
				{
					MovePiece(row, col);
					i--;
				}
			}
        }

        public short[,] GetCells()
        {
            return this._cells;
        }

        public void SetPieceValue(int row, int col, int cellNum)
        {
            if (cellNum == 0)
            {
                this._emptyRow = row;
                this._emptyCol = col;
            }

            this._cells[row, col] = (short)cellNum;
        }

        #region Private Data
        
        private int _emptyCol;
        private int _emptyRow;
        private readonly int _numRows;
        private readonly int _numCols;
        private readonly short[,] _cells;
        private const short EMPTY_CELL_ID = 0;

        #endregion

    }
}
