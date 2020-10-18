using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// This kind of question is especially for math
    /// </summary>
    public class VerticalFormQuestion : Question
    {
        public override QuestionType Type
        {
            get { return QuestionType.VerticalForm; }
        }

        protected override object InternalClone()
        {
            return new VerticalFormQuestion();
        }
    }
}
