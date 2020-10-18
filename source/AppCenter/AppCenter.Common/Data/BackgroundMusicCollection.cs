using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace SoonLearning.AppCenter.Data
{
    public class BackgroundMusicCollection : CollectionBase
    {
        public void Add(BackgroundMusicItem item)
        {
            base.InnerList.Add(item);
        }

        public BackgroundMusicItem this[int index]
        {
            get { return base.InnerList[index] as BackgroundMusicItem; }
            set { base.InnerList[index] = value; }
        }
    }
}
