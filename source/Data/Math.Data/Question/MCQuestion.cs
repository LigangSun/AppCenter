using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class MCQuestion : SelectableQuestion
    {
        public override QuestionType Type
        {
            get { return QuestionType.MultiChoice; }
        }

        protected override object InternalClone()
        {
            MCQuestion mcQuestion = new MCQuestion();
            base.InternalClone(mcQuestion);
            return mcQuestion;
        }
    }
}
