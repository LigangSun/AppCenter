using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// QuestionOption类表示选择题的选项。
    /// </summary>
    public class QuestionOption : BaseObject, ICloneable
    {
        private QuestionContent content = new QuestionContent();
        private bool correct;
        private int index;

        /// <summary>
        /// 获取或设置选项的内容。
        /// </summary>
        public QuestionContent OptionContent
        {
            get { return this.content; }
            set { this.content = value; }
        }

        public int Index
        {
            get { return this.index; }
            set
            {
                this.index = value;
                base.OnPropertyChanged("Index");
            }
        }

        /// <summary>
        /// 获取或设置该选项是否为正确答案。
        /// </summary>
        public bool IsCorrect
        {
            get { return this.correct; }
            set
            {
                this.correct = value;
                base.OnPropertyChanged("IsCorrect");
            }
        }

        public object Clone()
        {
            QuestionOption newOption = new QuestionOption();
            newOption.Id = this.Id;
            newOption.index = this.Index;
            newOption.IsCorrect = this.IsCorrect;
            newOption.OptionContent = this.OptionContent.Clone() as QuestionContent;
           
            return newOption;
        }
    }
}
