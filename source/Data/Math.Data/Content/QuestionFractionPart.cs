using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// QuestionFractionPart类用于表示分子或分母至少有一个是字母或表达式的分数。
    /// </summary>
    public class QuestionFractionPart : QuestionContentPart
    {
        private QuestionContent denominator = new QuestionContent();
        private QuestionContent numerator = new QuestionContent();
        private QuestionContent withPart;

        /// <summary>
        /// 获取或设置分子。
        /// </summary>
        public QuestionContent Numerator
        {
            get { return this.numerator; }
            set { this.numerator = value; }
        }

        /// <summary>
        /// 获取或设置分母。
        /// </summary>
        public QuestionContent Denominator
        {
            get { return this.denominator; }
            set { this.denominator = value; }
        }

        /// <summary>
        /// 获取或设置带分数部分。
        /// </summary>
        public QuestionContent WithPart
        {
            get { return this.withPart; }
            set { this.withPart = value; }
        }
 
        protected override string Prefix
        {
            get { return "_$FRACTION_"; }
        }

        public override int CompareTo(QuestionContentPart other)
        {
            if (this == other)
                return 0;

            if (!(other is QuestionContentPart))
                return -1;

            QuestionFractionPart otherPart = other as QuestionFractionPart;
            int retDenominator = this.denominator.CompareTo(otherPart.Denominator);
            int retNumertor = this.numerator.CompareTo(otherPart.Numerator);

            if (retDenominator == 0 && retNumertor == 0)
            {
                if (this.withPart != null &&
                    otherPart.WithPart != null)
                {
                    int retWithPart = this.withPart.CompareTo(otherPart.WithPart);
                    return retWithPart;
                }
                else if (this.withPart != null ||
                    otherPart.WithPart != null)
                {
                    return -1;
                }
                return 0;
            }

            return -1;
        }

        public override object Clone()
        {
            QuestionFractionPart newPart = new QuestionFractionPart();
            newPart.Id = this.Id;
            newPart.Numerator = this.Numerator.Clone() as QuestionContent;
            newPart.WithPart = this.WithPart.Clone() as QuestionContent;
            newPart.Denominator = this.Denominator.Clone() as QuestionContent;

            return newPart;
        }
    }
}
