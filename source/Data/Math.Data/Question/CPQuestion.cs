using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// CPQuestion类代表复合题。
    /// </summary>
    public class CPQuestion : Question
    {
        private QuestionCollection subQuestionCollection = new QuestionCollection();

        public QuestionCollection SubQuestions
        {
            get { return this.subQuestionCollection; }
        }

        public override QuestionType Type
        {
            get { return QuestionType.Composite; }
        }

        protected override object InternalClone()
        {
            CPQuestion cpQuestion = new CPQuestion();
            foreach (Question q in this.subQuestionCollection)
            {
                cpQuestion.SubQuestions.Add(q.Clone() as Question);
            }

            return new CPQuestion();
        }
    }
}
