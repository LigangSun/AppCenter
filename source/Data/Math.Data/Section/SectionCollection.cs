using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SoonLearning.Assessment.Data
{
    public class SectionCollection : CollectionBase
    {
        public void Add(Section item)
        {
            base.InnerList.Add(item);
        }

        public void AddRange(ICollection items)
        {
            base.InnerList.AddRange(items);
        }

        public void Remove(Section item)
        {
            base.InnerList.Remove(item);
        }

        public void RemoveRange(int index, int count)
        {
            base.InnerList.RemoveRange(index, count);
        }

        public Section this[int index]
        {
            get { return base.InnerList[index] as Section; }
            set { base.InnerList[index] = value; }
        }
    }
}
