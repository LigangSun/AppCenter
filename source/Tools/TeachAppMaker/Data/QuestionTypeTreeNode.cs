using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data;

namespace SoonLearning.TeachAppMaker.Data
{
    public class QuestionTypeTreeNode : ProjectTreeNode
    {
        private int count;

        public int Count
        {
            get { return this.count; }
            set
            {
                this.count = value;
                OnPropertyChanged("Count");
            }
        }

        public QuestionType Type
        {
            get;
            set;
        }
    }
}
