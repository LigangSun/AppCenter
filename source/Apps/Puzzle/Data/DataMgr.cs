using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter.Data;
using SoonLearning.BlockPuzzle.Puzzle;
using System.Collections;
using SoonLearning.AppCenter.Player;

namespace SoonLearning.BlockPuzzle.Data
{
    internal class DataMgr
    {
        public DataMgr()
        {
        }

        private ObservableCollection<PuzzleItem> puzzleItemCollection = new ObservableCollection<PuzzleItem>();  
        private int currentIndex = 0;

        public IEnumerable PuzzleItems
        {
            get { return this.puzzleItemCollection; }
        }

        internal PuzzleItem SelectedItem
        {
            get
            {
                if (this.puzzleItemCollection.Count == 0)
                    return null;
                return this.puzzleItemCollection[this.currentIndex];
            }
        }

        internal int CurrentIndex
        {
            get { return this.currentIndex; }
            set { this.currentIndex = value; }
        }

        internal int NextItem
        {
            get
            {
                if (this.currentIndex == this.puzzleItemCollection.Count - 1)
                {
                    this.currentIndex = 0;
                }
                else
                {
                    this.currentIndex++;
                }

                return currentIndex;
            }
        }

        internal int PreItem
        {
            get
            {
                if (this.currentIndex == 0)
                    this.currentIndex = this.puzzleItemCollection.Count - 1;
                else
                    this.currentIndex--;

                return currentIndex;
            }
        }

        internal void Add(PuzzleItem item)
        {
            this.puzzleItemCollection.Add(item);
        }

        internal void Clear()
        {
            this.puzzleItemCollection.Clear();
        }
    }
}
