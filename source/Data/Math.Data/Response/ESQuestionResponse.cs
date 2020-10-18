using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class ESQuestionResponse : QuestionResponse
    {
        private QuestionFlowDocumentPart answer = new QuestionFlowDocumentPart();

        public QuestionFlowDocumentPart Answer
        {
            get { return this.answer; }
            set { this.answer = value; }
        }

        public ESQuestionResponse(string questinoId)
            : base(questinoId)
        {
        }
    }
}
