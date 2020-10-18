using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace SoonLearning.ReadCount.Data
{
    public class ReadCountItem : BaseObject
    {
        private string goodsImage = string.Empty;
        private int count;
        private int? result;
        private bool randomCount = true;
        private int maxCount;

        public string GoodsImage
        {
            get { return this.goodsImage; }
            set { this.goodsImage = value; }
        }

        public int Count
        {
            get { return this.count; }
            set { this.count = value; }
        }

        public int MaxCount
        {
            get { return this.maxCount; }
            set
            {
                this.maxCount = value;
                this.OnPropertyChanged("MaxCount");
            }
        }

        public int? Result
        {
            get { return this.result; }
            set
            {
                this.result = value;
                this.OnPropertyChanged("Result");
                this.OnPropertyChanged("IsCorrect");
            }
        }

        public bool? IsCorrect
        {
            get
            {
                if (this.result == null)
                {
                    return null;
                }

                return this.result == this.Count;
            }
        }

        public bool RandomCount
        {
            get { return this.randomCount; }
            set 
            { 
                this.randomCount = value;
                this.OnPropertyChanged("RandomCount");
            }
        }

        public ReadCountItem()
        {
        }

        public ReadCountItem(string goodsImage, int count, int maxCount, bool randomCount)
        {
            this.goodsImage = goodsImage;
            this.count = count;
            this.maxCount = maxCount;
            this.randomCount = randomCount;
        }
    }
}
