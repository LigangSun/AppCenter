using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;

namespace SoonLearning.Assessment.Data
{
    [XmlInclude(typeof(MCQuestion))]
    [XmlInclude(typeof(FIBQuestion))]
    [XmlInclude(typeof(MRQuestion))]
    [XmlInclude(typeof(ESQuestion))]
    [XmlInclude(typeof(TFQuestion))]
    [XmlInclude(typeof(MAQuestion))]
    [XmlInclude(typeof(TableQuestion))]
    [XmlInclude(typeof(VerticalFormQuestion))]
    [XmlInclude(typeof(CPQuestion))]
    public class QuestionCollection : CollectionBase
    {
        public void Add(Question item)
        {
            base.InnerList.Add(item);
        }

        public void AddRange(ICollection items)
        {
            base.InnerList.AddRange(items);
        }

        public void Remove(Question item)
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

        public bool IsLast(Question item)
        {
            return base.InnerList.IndexOf(item) == base.InnerList.Count - 1;
        }

        public Question this[int index]
        {
            get
            {
                if (this.InnerList.Count == 0)
                    return null;
                return base.InnerList[index] as Question;
            }
            set { base.InnerList[index] = value; }
        }
    }
}
