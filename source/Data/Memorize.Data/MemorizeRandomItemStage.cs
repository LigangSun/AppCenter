using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.Memorize.Data
{
    public class MemorizeRandomItemStage : MemorizeStage
    {
        private int itemCount = 0;

        public int ItemCount
        {
            get { return this.itemCount; }
            set
            {
                this.itemCount = value;
                base.OnPropertyChanged("ItemCount");
            }
        }

        public override string ToString()
        {
            return string.Format("随机内容关卡， 记忆物品数:{0}", this.ItemCount);
        }
    }
}
