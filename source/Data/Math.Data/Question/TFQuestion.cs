using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class TFQuestion : SelectableQuestion
    {
        public override QuestionType Type
        {
            get { return QuestionType.TrueFalse; }
        }

        protected override object InternalClone()
        {
            TFQuestion tfQuestion = new TFQuestion();
            base.InternalClone(tfQuestion);
            return tfQuestion;
        }

        public TFQuestion()
        {
            
        }
    }
}
