using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;

namespace SoonLearning.Math.Fast.AddMinusMultiplyDivision
{
    public class AddMinusMultiplyDivisionQuestionData: QuestionData
    {
        internal const string header = "{17B9D9E2-028A-4462-AB7C-62C7730C590B}";

        protected override Question GenerateQuestion()
        {
            Random rand = new Random(DateTime.Now.Second);
            if (rand.Next(99) % 2 == 0)
                return GenerateAM();

            return GenerateMD();
        }

        private Question GenerateAM()
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

        private Question GenerateMD()
        {
            double a = 0;
            double b = 0;
            double c = 0;

            MathAPI.MMSelectRandom(ref a, ref b, ref c, this.maxNumber, this.maxNumber, 0);

            Operator op = Operator.Multiply;

            Random key = new Random(DateTime.Now.Second);
            Random rand = new Random(DateTime.Now.Second * key.Next(1234));
            int randOpIndex = rand.Next(99);
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

        public AddMinusMultiplyDivisionQuestionData()
        {
        }

        protected override string CreateHeader()
        {
            return header;
        }

        public override int MaxResultNumber
        {
            get { return MathSetting.Instance.SelectedMaxNumber * MathSetting.Instance.SelectedMaxNumber; }
        }
    }
}
