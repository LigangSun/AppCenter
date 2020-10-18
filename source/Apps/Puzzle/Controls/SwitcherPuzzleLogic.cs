using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.BlockPuzzle.Data;

namespace SoonLearning.BlockPuzzle.Controls
{
    internal class SwitcherPuzzleLogic
    {
        #region Private Data

        private readonly int numRows;
        private readonly int numCols;
        private readonly short[,] cells;
        private const short EMPTY_CELL_ID = -1;

        #endregion

        public SwitcherPuzzleLogic(int numRows, int numCols)
        {
            this.numRows = numRows;
            this.numCols = numCols;
            this.cells = new short[numRows, numCols];

            short tileNumber = 0;
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    cells[r, c] = tileNumber++;
                }
            }
        }

        /// <summary>
        /// Moves the piece
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns>The cell of the newly opened position</returns>
        public void MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            short destCell = cells[toRow, toCol];
            short origCell = cells[fromRow, fromCol];

            cells[toRow, toCol] = origCell;
            cells[fromRow, fromCol] = destCell;
        }

        public void SetPieceValue(int row, int col, int cellNum)
        {
            this.cells[row, col] = (short)cellNum;
        }

        public bool CheckForWin()
        {
            // Just walk through cells and make sure they're all consecutive values
            short tileNumber = 0;
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    if (tileNumber++ != cells[r, c])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public PuzzleCell FindCell(short cellNumber)
        {
            // This is a slow, linear operation, but for the size puzzles we have, it doesn't matter.
            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    if (cells[r, c] == cellNumber)
                    {
                        return new PuzzleCell(r, c, cellNumber);
                    }
                }
            }

            return new PuzzleCell(-1, -1, -1);
        }

        public short[,] GetCells()
        {
            return this.cells;
        }

        public void MixUpPuzzle()
        {
            // Ensure that we can still solve it by only emulating legal moves.
            Random r = new Random();
            int i = 8 * numRows * numCols;  // fairly arbitrary choice of number of moves

            int emptyCol = 0;
            int emptyRow = 0;

            while (i > 0)
            {
                int choice = r.Next(4);
                //   int choice2 = r.Next(_numRows);
                int row = -1;
                int col = -1;

                // 0,1,2,3 - left, right, up, down from empty cell, when possible.  Skip when not.
                switch (choice)
                {
                    case 0:
                        if (emptyCol != 0)
                        {
                            col = emptyCol - 1;
                            row = emptyRow;
                        }
                        break;

                    case 1:
                        if (emptyCol != numCols - 1)
                        {
                            col = emptyCol + 1;
                            row = emptyRow;
                        }
                        break;

                    case 2:
                        if (emptyRow != 0)
                        {
                            row = emptyRow - 1;
                            col = emptyCol;
                        }
                        break;

                    case 3:
                        if (emptyRow != numRows - 1)
                        {
                            row = emptyRow + 1;
                            col = emptyCol;
                        }
                        break;
                }
                if (row != -1)
                {
                    MovePiece(emptyRow, emptyCol, row, col);

                    emptyRow = row;
                    emptyCol = col;

                    i--;
                }
            }
        }
    }
}
