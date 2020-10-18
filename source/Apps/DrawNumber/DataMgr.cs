using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.ConnectNumber
{
    internal class DataMgr
    {
        internal DataMgr()
        {
        }

        private ObservableCollection<DrawNumberItem> drawNumberCollection = new ObservableCollection<DrawNumberItem>();
        private int currentIndex = 0;

        public IEnumerable DrawNumberItems
        {
            get { return this.drawNumberCollection; }
        }

        internal DrawNumberItem SelectedItem
        {
            get
            {
                if (this.drawNumberCollection.Count == 0)
                    return null;
                return this.drawNumberCollection[this.currentIndex];
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
                if (this.currentIndex == this.drawNumberCollection.Count - 1)
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
                    this.currentIndex = this.drawNumberCollection.Count - 1;
                else
                    this.currentIndex--;

                return currentIndex;
            }
        }

        private ThumbnailItem win_DataItemLoadedEvent(string file)
        {
            DrawNumberItem item = DrawNumberData.LoadDrawNumberItem(file);
            item.DataFile = file;
            this.drawNumberCollection.Add(item);
            return item;
        }

        internal void Add(DrawNumberItem item)
        {
            this.drawNumberCollection.Add(item);
        }

        internal void Clear()
        {
            this.drawNumberCollection.Clear();
        }
    }
}
