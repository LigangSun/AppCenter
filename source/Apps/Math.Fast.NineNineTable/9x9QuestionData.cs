using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;

namespace SoonLearning.Math.Fast.NineNineTable
{
    public class _9x9QuestionData : QuestionData
    {
        internal const string header = "{C603D8B9-4428-4FF7-B09F-76A794AB30A1}";

        protected override Question GenerateQuestion()
        {
            double a = 0;
            double b = 0;
            double c = 0;

            MathAPI.MMSelectRandom(ref a, ref b, ref c, 9, 9, 0);

            Operator op = Operator.Multiply;

            return new Question_a_b_c((int)a, (int)b, (int)c, op);
        }

        public _9x9QuestionData()
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
