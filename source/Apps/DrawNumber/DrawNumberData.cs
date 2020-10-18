using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SoonLearning.ConnectNumber
{
    public class DrawNumberData
    {
        private DrawNumberItem item;
        private byte[] backgroundImageData;
        private byte[] drawNumberImageData;

        public DrawNumberItem DrawNumberItem
        {
            get { return this.item; }
        }

        public byte[] BackgroundImageData
        {
            get { return this.backgroundImageData; }
            set { this.backgroundImageData = value; }
        }

        public byte[] DrawNumberImageData
        {
            get { return this.drawNumberImageData; }
            set { this.drawNumberImageData = value; }
        }

        public DrawNumberData()
        {
            this.item = new DrawNumberItem();
        }

        public DrawNumberData(DrawNumberItem item, byte[] bkImageData, byte[] drawNumberImageData)
        {
            this.item = item;
            this.backgroundImageData = bkImageData;
            this.drawNumberImageData = drawNumberImageData;
        }

        public void Save(string file)
        {
            string fullPath = file;
            if (File.Exists(file))
            {
                int index = fullPath.LastIndexOf('.');
                fullPath = fullPath.Insert(index, Guid.NewGuid().ToString("N"));
            }

            this.item.DataFile = fullPath;

            using (FileStream fs = File.OpenWrite(fullPath))
            {
                MemoryStream ms = item.Save();
                int itemLen = (int)ms.Length;
                fs.Write(BitConverter.GetBytes(itemLen), 0, 4);

                fs.Write(ms.ToArray(), 0, (int)ms.Length);

                ms.Dispose();
                
                int imageSize = this.backgroundImageData.Length;
                fs.Write(BitConverter.GetBytes(imageSize), 0, 4);
                fs.Write(this.backgroundImageData, 0, this.backgroundImageData.Length);

                imageSize = this.drawNumberImageData.Length;
                fs.Write(BitConverter.GetBytes(imageSize), 0, 4);
                fs.Write(this.drawNumberImageData, 0, this.drawNumberImageData.Length);
            }
        }

        public static DrawNumberData Load(string file)
        {
            using (FileStream fs = File.OpenRead(file))
            {
                byte[] itemLenArray = new byte[4];
                fs.Read(itemLenArray, 0, 4);

                int itemLen = BitConverter.ToInt32(itemLenArray, 0);

                byte[] itemData = new byte[itemLen];
                fs.Read(itemData, 0, itemData.Length);

                MemoryStream ms = new MemoryStream(itemData);
                DrawNumberItem puzzleItem = DrawNumberItem.FromStream(ms);
                ms.Dispose();

                byte[] imageLenArray = new byte[4];
                fs.Read(imageLenArray, 0, 4);

                int imageSize = BitConverter.ToInt32(imageLenArray, 0);

                byte[] imageData = new byte[imageSize];
                fs.Read(imageData, 0, imageSize);

                fs.Read(imageLenArray, 0, 4);

                imageSize = BitConverter.ToInt32(imageLenArray, 0);

                byte[] numberImageData = new byte[imageSize];
                fs.Read(numberImageData, 0, imageSize);

                return new DrawNumberData(puzzleItem, imageData, numberImageData);
            }
        }

        public static DrawNumberItem LoadDrawNumberItem(string file)
        {
            using (FileStream fs = File.OpenRead(file))
            {
                byte[] itemLenArray = new byte[4];
                fs.Read(itemLenArray, 0, 4);

                int itemLen = BitConverter.ToInt32(itemLenArray, 0);

                byte[] itemData = new byte[itemLen];
                fs.Read(itemData, 0, itemData.Length);

                MemoryStream ms = new MemoryStream(itemData);
                DrawNumberItem item = DrawNumberItem.FromStream(ms);
                ms.Dispose();

                return item;
            }
        }

        public static byte[] LoadBackgroundImageData(string file)
        {
            using (FileStream fs = File.OpenRead(file))
            {
                byte[] itemLenArray = new byte[4];
                fs.Read(itemLenArray, 0, 4);

                int itemLen = BitConverter.ToInt32(itemLenArray, 0);

                fs.Seek(itemLen, SeekOrigin.Current);

                byte[] imageLenArray = new byte[4];
                fs.Read(imageLenArray, 0, 4);

                int imageSize = BitConverter.ToInt32(imageLenArray, 0);

                byte[] imageData = new byte[imageSize];
                fs.Read(imageData, 0, imageSize);

                return imageData;
            }
        }

        public static byte[] LoadNumberImageData(string file)
        {
            using (FileStream fs = File.OpenRead(file))
            {
                byte[] itemLenArray = new byte[4];
                fs.Read(itemLenArray, 0, 4);

                int itemLen = BitConverter.ToInt32(itemLenArray, 0);

                fs.Seek(itemLen, SeekOrigin.Current);

                byte[] imageLenArray = new byte[4];
                fs.Read(imageLenArray, 0, 4);

                int imageSize = BitConverter.ToInt32(imageLenArray, 0);

                fs.Seek(imageSize, SeekOrigin.Current);

                fs.Read(imageLenArray, 0, 4);

                imageSize = BitConverter.ToInt32(imageLenArray, 0);

                byte[] imageData = new byte[imageSize];
                fs.Read(imageData, 0, imageSize);

                return imageData;
            }
        }
    }
}
