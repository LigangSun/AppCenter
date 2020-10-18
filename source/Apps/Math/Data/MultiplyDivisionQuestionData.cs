using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadgetCenter.Utility;

namespace SoonLearning.Math.Data
{
    public class MultiplyDivisionQuestionData : QuestionData
    {
        internal const string header = "{712F5A1F-2CB6-4160-9A45-862564A4903D}";

        protected override Question GenerateQuestion()
        {
            double a = 0;
            double b = 0;
            double c = 0;

            MathAPI.MMSelectRandom(ref a, ref b, ref c, this.maxNumber, this.maxNumber, 0);

            Operator op = Operator.Multiply;

            Random key = new Random();
            Random rand = new Random(4 * key.Next());
            int randOpIndex = rand.Next();
            randOpIndex = randOpIndex % 2;
            if (randOpIndex == 0)
                op = Operator.Multiply;
            else
            {
                op = Operator.Division;
                double temp = a;
                a = c;
                c = temp;
            }

            return new Question_a_b_c((int)a, (int)b, (int)c, op);
        }

        public MultiplyDivisionQuestionData()
        {
        }

        internal MultiplyDivisionQuestionData(int maxNumber,
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
