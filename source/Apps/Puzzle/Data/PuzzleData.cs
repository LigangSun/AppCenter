using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SoonLearning.BlockPuzzle.Data
{
    internal class PuzzleData
    {
        private PuzzleItem item;
        private byte[] imageData;

        public PuzzleItem PuzzleItem
        {
            get { return this.item; }
        }

        public byte[] ImageData
        {
            get { return this.imageData; }
        }

        public PuzzleData(PuzzleItem item, byte[] imageData)
        {
            this.item = item;
            this.imageData = imageData;
        }

        public void Save(string file)
        {
            using (FileStream fs = File.OpenWrite(file))
            {
                MemoryStream ms = item.Save();
                int itemLen = (int)ms.Length;
                fs.Write(BitConverter.GetBytes(itemLen), 0, 4);

                fs.Write(ms.ToArray(), 0, (int)ms.Length);

                ms.Dispose();

                int imageSize = this.imageData.Length;
                fs.Write(BitConverter.GetBytes(imageSize), 0, 4);
                fs.Write(this.imageData, 0, this.imageData.Length);
            }
        }

        public static PuzzleData Load(string file)
        {
            using (FileStream fs = File.OpenRead(file))
            {
                byte[] itemLenArray = new byte[4];
                fs.Read(itemLenArray, 0, 4);

                int itemLen = BitConverter.ToInt32(itemLenArray, 0);

                byte[] itemData = new byte[itemLen];
                fs.Read(itemData, 0, itemData.Length);

                MemoryStream ms = new MemoryStream(itemData);
                PuzzleItem puzzleItem = PuzzleItem.FromStream(ms);
                ms.Dispose();

                byte[] imageLenArray = new byte[4];
                fs.Read(imageLenArray, 0, 4);

                int imageSize = BitConverter.ToInt32(imageLenArray, 0);

                byte[] imageData = new byte[imageSize];
                fs.Read(imageData, 0, imageSize);

                return new PuzzleData(puzzleItem, imageData);
            }
        }

        public static PuzzleItem LoadPuzzleItem(string file)
        {
            using (FileStream fs = File.OpenRead(file))
            {
                byte[] itemLenArray = new byte[4];
                fs.Read(itemLenArray, 0, 4);

                int itemLen = BitConverter.ToInt32(itemLenArray, 0);

                byte[] itemData = new byte[itemLen];
                fs.Read(itemData, 0, itemData.Length);

                MemoryStream ms = new MemoryStream(itemData);
                PuzzleItem puzzleItem = PuzzleItem.FromStream(ms);
                ms.Dispose();

                return puzzleItem;
            }
        }

        public static byte[] LoadImageData(string file)
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
    }
}
