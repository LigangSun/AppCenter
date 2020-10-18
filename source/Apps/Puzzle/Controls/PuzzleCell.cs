using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.BlockPuzzle.Controls
{
    internal struct PuzzleCell
    {
        private int row;
        private int col;
        private int cellNumber;

        public PuzzleCell(int row, int col, int cellNumber)
        {
            this.row = row;
            this.col = col;
            this.cellNumber = cellNumber;
        }

        public int Row
        { 
            get
            {
                return this.row; 
            } 
        }

        public int Col
        { 
            get 
            {
                return this.col; 
            } 
        }

        public int CellNumber 
        { 
            get 
            { 
                return this.cellNumber;
            } 
        }
    }
}
