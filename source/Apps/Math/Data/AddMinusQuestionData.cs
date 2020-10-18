using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadgetCenter.Utility;

namespace SoonLearning.Math.Data
{
    public class AddMinusQuestionData : QuestionData
    {
        internal const string header = "{57B12910-739B-4A15-927E-8B0A08B17D67}";

        protected override Question GenerateQuestion()
        {
            int a = 0;
            int b = 0;
            int c = 0;

            MathAPI.MCSelectRandom(ref a, ref b, ref c, this.maxNumber, 0);

            Operator op = Operator.Plus;
            if (a - b == c)
                op = Operator.Minus;

            return new Question_a_b_c(a, b, c, op);
        }

        public AddMinusQuestionData()
        {
        }

        internal AddMinusQuestionData(int maxNumber,
            int questionCount,
            int examTime, 
            float pointPerQuestion,
            int passQuizCorrectCount)
            : base(maxNumber, questionCount, examTime, pointPerQuestion, passQuizCorrectCount)
        {
        }

        protected override string CreateHeader()
        {
            return header;
        }
    }
}
