using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data.Bank
{
    public class Knowledge : BaseObject
    {
        private QuestionContent content;

        public QuestionContent Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
    }
}
