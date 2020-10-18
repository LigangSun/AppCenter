using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;

namespace SoonLearning.Assessment.Data
{
    public class SectionValueRangeInfo : SectionBaseInfo
    {
        private decimal minValue;
        private decimal maxValue;

        public decimal MinValue
        {
            get { return this.minValue; }
            set
            {
                this.minValue = value;
                base.OnPropertyChanged("MinValue");
            }
        }

        public decimal MaxValue
        {
            get { return this.maxValue; }
            set
            {
                this.maxValue = value;
                base.OnPropertyChanged("MaxValue");
            }
        }

        public SectionValueRangeInfo(QuestionType type, string name, string description, int count, decimal minValue, decimal maxValue)
            : base(type, name, description, count)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }
    }
}
