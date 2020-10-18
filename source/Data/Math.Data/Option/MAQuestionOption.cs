using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class MAQuestionOption : BaseObject , ICloneable
    {
        private QuestionContent leftContent = new QuestionContent();
        private QuestionContent rightContent = new QuestionContent();
        private int index;

        public int Index
        {
            get { return this.index; }
            set
            {
                this.index = value;
                base.OnPropertyChanged("Index");
            }
        }

        public QuestionContent LeftContent
        {
            get { return this.leftContent; }
            set
            {
                this.leftContent = value;
                base.OnPropertyChanged("LeftContent");
            }
        }

        public QuestionContent RightContent
        {
            get { return this.rightContent; }
            set
            {
                this.rightContent = value;
                this.OnPropertyChanged("RightContent");
            }
        }

        public object Clone()
        {
            MAQuestionOption maQuestionOption = new MAQuestionOption();
            maQuestionOption.LeftContent = this.LeftContent.Clone() as QuestionContent;
            maQuestionOption.RightContent = this.RightContent.Clone() as QuestionContent;
            maQuestionOption.Index = this.Index;
            return maQuestionOption;
        }
    }
}
