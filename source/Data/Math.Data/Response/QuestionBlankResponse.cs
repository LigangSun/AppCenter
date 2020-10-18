using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class QuestionBlankResponse : Response
    {
        private QuestionContent blankContent = new QuestionContent();

        public QuestionContent BlankContent
        {
            get { return this.blankContent; }
            set { this.blankContent = value; }
        }

        public QuestionBlankResponse(string blankId)
            : base(blankId)
        {
        }
    }
}
