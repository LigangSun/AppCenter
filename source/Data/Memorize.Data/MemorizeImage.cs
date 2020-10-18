using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

namespace SoonLearning.Memorize.Data
{
    public class MemorizeImage : MemorizeObject
    {
        private string url = string.Empty;
        private int count = -1;
        private ImageLayout layout;

        private ObservableCollection<MemorizeImageItem> itemCollection = new ObservableCollection<MemorizeImageItem>();

        public string Url
        {
            get
            { 
                return this.url;
            }
            set
            {
                this.url = value;
                base.OnPropertyChanged("Url");
            }
        }

        public int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        public ImageLayout Layout
        {
            get { return this.layout; }
            set { this.layout = value; }
        }

        public ObservableCollection<MemorizeImageItem> Items
        {
            get { return this.itemCollection; }
        }

        public MemorizeImage()
        {
        }

        public MemorizeImage(string url, int count, ImageLayout layout)
        {
            this.url = url;
            this.count = count;
            this.layout = layout;
        }

        public override void Save(string path)
        {
            try
            {
                this.Url = MemorizeEntry.copyFile(this.url, path);
            }
            catch
            {
            }
        }

        public override void getFullPath(string path)
        {
            this.Url = MemorizeEntry.getFileFullPath(path, this.Url);
        }

        public override string ToString()
        {
            return Path.GetFileName(this.Url);
        }

        public void GenerateItems(int left, int top, int width, int height, int imageWidth)
        {
            this.itemCollection.Clear();
            for (int i = 0; i < count; i++)
            {
                MemorizeImageItem item = new MemorizeImageItem();
                item.Url = this.Url;
                item.Width = imageWidth;
                this.itemCollection.Add(item);
            }
        }

        public void RandomGenerateItems(int left, int top, int width, int height, int imageWidth)
        {
            Random rand = new Random(Environment.TickCount);

            this.itemCollection.Clear();
            for (int i = 0; i < count; i++)
            {
                MemorizeImageItem item = new MemorizeImageItem();
                item.X = rand.Next(left, width);
                item.Y = rand.Next(top, height);
                item.Url = this.Url;
                item.Width = imageWidth;
                this.itemCollection.Add(item);
            }
        }
    }

    public class MemorizeImageItem
    {
        public double X
        {
            get;
            set;
        }

        public double Y
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public double Width
        {
            get;
            set;
        }
    }
}
