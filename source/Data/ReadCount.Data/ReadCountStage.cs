using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SoonLearning.ReadCount.Data
{
    public class ReadCountStage : BaseObject
    {
        private string id;
        private string backgroundImage = string.Empty;
        private string title = string.Empty;
        private ObservableCollection<ReadCountItem> itemCollection = new ObservableCollection<ReadCountItem>();

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public string BackgroundImage
        {
            get { return this.backgroundImage; }
            set 
            {
                this.backgroundImage = value;
                OnPropertyChanged("BackgroundImage");
            }
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public ObservableCollection<ReadCountItem> Items
        {
            get { return this.itemCollection; }
        }

        public ReadCountStage()
        {
            this.id = Guid.NewGuid().ToString("N");
        }

        public ReadCountStage(string backgroundImage)
        {
            this.backgroundImage = backgroundImage;
        }

        public void AddItems(ReadCountItem[] items)
        {
            foreach (ReadCountItem item in items)
            {
                this.itemCollection.Add(item);
            }
        }

        public override string ToString()
        {
            return this.title;
        }
    }
}
