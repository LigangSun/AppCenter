using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SoonLearning.Assessment.Data
{
    public class QuestionOptionCollection : CollectionBase
    {
        public void Add(QuestionOption item)
        {
            base.InnerList.Add(item);
        }

        public void AddRange(ICollection items)
        {
            base.InnerList.AddRange(items);
        }

        public void Insert(int index, QuestionOption item)
        {
            base.InnerList.Insert(index, item);
        }

        public void Remove(QuestionOption item)
        {
            base.InnerList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            base.InnerList.RemoveAt(index);
        }

        public void RemoveRange(int index, int count)
        {
            base.InnerList.RemoveRange(index, count);
        }

        public QuestionOption this[int index]
        {
            get { return base.InnerList[index] as QuestionOption; }
            set { base.InnerList[index] = value; }
        }
    }
}
