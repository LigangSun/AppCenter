using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.ConnectNumber
{
    public class DrawNumberItem : ThumbnailItem
    {
        private string title;
        private string description;
        private BitmapImage thumbnail;
        private byte[] thumbnailData;
        
        private string creator;
        private DateTime createDate;

        private string dataFile;

        private int canvasWidth;
        private int canvasHeight;

        private Point numberImagePoint;

        private int backgroundImageWidth;
        private int backgroundImageHeight;

        private ObservableCollection<Point> pointCollection = new ObservableCollection<Point>();

        #region ThumbnailItem

        public override string Title
        {
            get { return this.title; }
        }

        public override string Description
        {
            get { return this.description; }
        }

        public override System.Windows.Media.Imaging.BitmapImage Thumbnail
        {
            get
            {
                if (this.thumbnail == null)
                    this.GetThumbnailImage();

                return this.thumbnail;
            }
        }

        #endregion

        public string TitleText
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public string DescriptionText
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public string Creator
        {
            get { return this.creator; }
            set { this.creator = value; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
            set { this.createDate = value; }
        }

        public string DataFile
        {
            get { return this.dataFile; }
            set { this.dataFile = value; }
        }

        public int CanvasWidth
        {
            get { return this.canvasWidth; }
            set { this.canvasWidth = value; }
        }

        public int CanvasHeight
        {
            get { return this.canvasHeight; }
            set { this.canvasHeight = value; }
        }

        public double ForegroundWidth
        {
            get;
            set;
        }

        public double ForegroundHeight
        {
            get;
            set;
        }

        public int BackgroundImageWidth
        {
            get { return this.backgroundImageWidth; }
            set { this.backgroundImageWidth = value; }
        }

        public int BackgroundImageHeight
        {
            get { return this.backgroundImageHeight; }
            set { this.backgroundImageHeight = value; }
        }

        public byte[] ThumbnailData
        {
            get { return this.thumbnailData; }
            set { this.thumbnailData = value; }
        }

        public Point NumberImagePoint
        {
            get { return this.numberImagePoint; }
            set
            {
                this.numberImagePoint = value;
            }
        }

        public ObservableCollection<Point> PointCollection
        {
            get { return this.pointCollection; }
        }

        internal MemoryStream Save()
        {
            MemoryStream ms = new MemoryStream();
            SerializerHelper<DrawNumberItem>.XmlSerialize(ms, this);
            return ms;
        }

        internal static DrawNumberItem FromStream(Stream stream)
        {
            return SerializerHelper<DrawNumberItem>.XmlDeserialize(stream);
        }

        private void GetThumbnailImage()
        {
            if (this.thumbnailData == null)
                return;

            byte[] data = (byte[])this.thumbnailData;
            MemoryStream ms = new MemoryStream(data);

            this.thumbnail = new BitmapImage();
            this.thumbnail.BeginInit();
            this.thumbnail.StreamSource = ms;
            this.thumbnail.EndInit();
        }
    }
}
