using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class ExerciseResponse : Response
    {
        private List<QuestionResponse> questionResponseList = new List<QuestionResponse>();

        public List<QuestionResponse> QuestionResponseList
        {
            get { return this.questionResponseList; }
        }

        public ExerciseResponse(string exerciseId)
            : base(exerciseId)
        {
        }
    }
}
