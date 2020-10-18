using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class SelectableQuestionResponse : QuestionResponse
    {
        private List<string> optionIdList = new List<string>();

        public List<string> OptionIdList
        {
            get { return this.optionIdList; }
        }

        public SelectableQuestionResponse(string questionId)
            : base(questionId)
        {
        }
    }
}
