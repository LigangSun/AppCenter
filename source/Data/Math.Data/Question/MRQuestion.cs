using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class MRQuestion : SelectableQuestion
    {
        public override QuestionType Type
        {
            get { return QuestionType.MultiResponse; }
        }

        protected override object InternalClone()
        {
            MRQuestion mrQuestion = new MRQuestion();
            base.InternalClone(mrQuestion);
            return mrQuestion;
        }
    }
}
