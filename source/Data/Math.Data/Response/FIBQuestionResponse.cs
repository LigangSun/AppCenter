using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class FIBQuestionResponse : QuestionResponse
    {
        private List<QuestionBlankResponse> blankResponseList = new List<QuestionBlankResponse>();

        public List<QuestionBlankResponse> BlankResponseList
        {
            get { return this.blankResponseList; }
        }

        public QuestionBlankResponse GetBlankResponse(string blankId, bool createNotExist)
        {
            QuestionBlankResponse blankResponse = null;

            bool found = false;
            foreach (QuestionBlankResponse response in this.blankResponseList)
            {
                if (response.ObjectId == blankId)
                {
                    blankResponse = response;
                    break;
                }
            }

            if (blankResponse == null && createNotExist)
            {
                blankResponse = new QuestionBlankResponse(blankId);
                this.blankResponseList.Add(blankResponse);
            }

            return blankResponse;
        }

        public FIBQuestionResponse()
            : base(string.Empty)
        {
        }

        public FIBQuestionResponse(string questionId)
            : base(questionId)
        {
        }
    }
}
