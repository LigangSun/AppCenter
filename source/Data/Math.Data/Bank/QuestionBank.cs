using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data.Bank
{
    public class QuestionBank : BaseObject
    {
        private List<Question> questionList = new List<Question>();

        public List<Question> Items
        {
            get { return this.questionList; }
        }
    }
}
