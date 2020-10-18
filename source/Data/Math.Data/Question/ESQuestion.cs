using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// ESQuestion类代表简答题。
    /// </summary>
    public class ESQuestion : Question
    {
        private QuestionContent referenceAnswer = new QuestionContent();

        public override QuestionType Type
        {
            get { return QuestionType.Essay; }
        }

        public QuestionContent ReferenceAnswer
        {
            get { return this.referenceAnswer; }
            set
            {
                this.referenceAnswer = value;
                this.OnPropertyChanged("ReferenceAnswer");
            }
        }

        protected override object InternalClone()
        {
            ESQuestion esQuestion = new ESQuestion();
            esQuestion.ReferenceAnswer = this.ReferenceAnswer.Clone() as QuestionContent;
            return esQuestion;
        }
    }
}
