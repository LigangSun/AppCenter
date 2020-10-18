using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Utility;
using System.IO;
using SoonLearning.AppCenter.Data;
using System.Windows.Media.Imaging;
using SoonLearning.BlockPuzzle.Puzzle;
using SoonLearning.AppCenter.Controls;
using System.Windows;
using System.Reflection;

namespace SoonLearning.BlockPuzzle.Data
{
    public enum PuzzleType
    {
        Normal_2x2,
        Normal_2x3,
        Normal_3x2,
        Normal_3x3,
        Normal_4x4,
        Switcher_3x2,
        JigSaw
    }

    public class PuzzleItem : ThumbnailItem
    {
        private string title;
        private string description;
        private string creator;
        private DateTime createDate;
        private PuzzleType type;
        private string imageFile;
        private byte[] thumbnail;
        private BitmapImage bi;

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

        public PuzzleType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public string ImageFile
        {
            get { return this.imageFile; }
            set { this.imageFile = value; }
        }

        public byte[] ThumbnailData
        {
            get { return this.thumbnail; }
            set { this.thumbnail = value; }
        }

        public override string Title
        {
            get { return this.title; }
        }

        public override string Description
        {
            get { return this.description; }
        }

        public override BitmapImage Thumbnail
        {
            get
            {
                if (this.bi == null)
                    this.GetThumbnailImage();
                return this.bi;
            }
        }

        private void GetThumbnailImage()
        {
            byte[] data = (byte[])this.thumbnail;
            MemoryStream ms = new MemoryStream(data);

            bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
        }

        internal static PuzzleItem FromFile(string file)
        {
            return SerializerHelper<PuzzleItem>.XmlDeserialize(file);
        }

        internal void SaveFile(string file)
        {
            SerializerHelper<PuzzleItem>.XmlSerialize(file, this);
        }

        public static PuzzleItem CreatePuzzle(string title, string imageFile, string creator)
        {
            PuzzleItem item = new PuzzleItem();
            item.Type = PuzzleSetting.Instance.Type;
            item.TitleText = title;
            item.DescriptionText = string.Empty;
            item.Creator = creator;
            item.CreateDate = DateTime.Now.ToUniversalTime();
            item.ThumbnailData = GetThumbnailData(imageFile);
            item.ImageFile = System.IO.Path.GetFileName(imageFile);

            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(imageFile);
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, bmp.RawFormat);

            PuzzleData puzzleData = new PuzzleData(item, ms.ToArray());

            ms.Dispose();

            Assembly assembly = Assembly.GetEntryAssembly();
            string dataFolder = System.IO.Path.GetDirectoryName(assembly.Location);
            dataFolder = System.IO.Path.Combine(dataFolder, @"Data\Puzzle\" + PuzzleSetting.Instance.Type.ToString());
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            puzzleData.Save(System.IO.Path.Combine(dataFolder, item.Title + ".pd"));

            item.ImageFile = System.IO.Path.Combine(dataFolder, item.Title + ".pd");

            return item;
        }

        private static byte[] GetThumbnailData(string imageFile)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(imageFile);

            int w = 128;
            int h = 128;
            if (bmp.Width > bmp.Height)
            {
                w = 128;
                h = (int)(w / ((float)bmp.Width / (float)bmp.Height));
            }
            else
            {
                h = 128;
                w = (int)(h * (float)bmp.Width / (float)bmp.Height);
            }

            System.Drawing.Image thumbnail = bmp.GetThumbnailImage(w, h, null, IntPtr.Zero);

            MemoryStream ms = new MemoryStream();
            thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            thumbnail.Dispose();
            bmp.Dispose();

            return ms.ToArray();
        }

        internal MemoryStream Save()
        {
            MemoryStream ms = new MemoryStream();
            SerializerHelper<PuzzleItem>.XmlSerialize(ms, this);
            return ms;
        }

        internal static PuzzleItem FromStream(Stream stream)
        {
            return SerializerHelper<PuzzleItem>.XmlDeserialize(stream);
        }
    }
}
