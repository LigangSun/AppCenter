using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// QuestionBlank类用来表示试题中的空格。
    /// </summary>
    public class QuestionBlank : QuestionContentPart
    {
        private List<QuestionContent> referenceAnswerList = new List<QuestionContent>(); 
        private int index;

        /// <summary>
        /// 获取该空格的参考答案列表
        /// </summary>
        public List<QuestionContent> ReferenceAnswerList
        {
            get { return referenceAnswerList; }
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
        /// 获取或设置该空格是否只匹配自己的参考答案。
        /// 默认值为false，表示该空格的答案也可以匹配其他空格的参考答案。
        /// </summary>
        public bool MatchOwnRefAnswer
        {
            get;
            set;
        }

        protected override string Prefix
        {
            get { return "_$BLANK_"; }
        }

        public override int CompareTo(QuestionContentPart other)
        {
            if (other == this)
                return 0;

            return -1;
        }

        public override object Clone()
        {
            QuestionBlank newBlank = new QuestionBlank();
            newBlank.Id = this.Id;
            newBlank.MatchOwnRefAnswer = this.MatchOwnRefAnswer;
            foreach (QuestionContent content in newBlank.ReferenceAnswerList)
            {
                QuestionContent newContent = content.Clone() as QuestionContent;
                newBlank.ReferenceAnswerList.Add(newContent);
            }

            return newBlank;
        }
    }
}
