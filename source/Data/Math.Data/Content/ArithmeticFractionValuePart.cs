using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// ArithmeticFractionValuePart类代表分子和分母均为数字的分数。
    /// </summary>
    public class ArithmeticFractionValuePart : QuestionContentPart
    {
        private decimal denominator;
        private decimal numerator;

        /// <summary>
        /// 获取或设置分子。
        /// </summary>
        public decimal Numerator
        {
            get { return this.numerator; }
            set { this.numerator = value; }
        }

        /// <summary>
        /// 获取或设置分母。
        /// </summary>
        public decimal Denominator
        {
            get { return this.denominator; }
            set { this.denominator = value; }
        }

        protected override string Prefix
        {
            get { return "_$ARITHMETICFRACTION_"; }
        }

        /// <summary>
        /// 初始化ArithmeticFractionValuePart类的新实例。
        /// </summary>
        public ArithmeticFractionValuePart()
        {
        }

        /// <summary>
        /// 初始化ArithmeticFractionValuePart类的新实例。
        /// </summary>
        /// <param name="numerator">分子的数值。</param>
        /// <param name="denominator">分母的数值。</param>
        public ArithmeticFractionValuePart(decimal numerator, decimal denominator)
        {
            if (denominator == 0)
                throw new NotSupportedException();

            this.denominator = denominator;
            this.numerator = numerator;
        }

        public override int CompareTo(QuestionContentPart other)
        {
            if (!(other is ArithmeticFractionValuePart))
                return -1;

            ArithmeticFractionValuePart otherFraction = other as ArithmeticFractionValuePart;
            double rateThis = decimal.ToDouble(this.Numerator) / decimal.ToDouble(this.Denominator);
            double rateOther = decimal.ToDouble(otherFraction.Numerator) / decimal.ToDouble(otherFraction.Denominator);
            if (rateThis == rateOther)
                return 0;
            if (rateThis > rateOther)
                return 1;

            return -1;
        }

        public override object Clone()
        {
            ArithmeticFractionValuePart newPart = new ArithmeticFractionValuePart();
            newPart.Id = this.Id;
            newPart.Denominator = this.Denominator;
            newPart.Numerator = this.Numerator;
            return newPart;
        }
    }
}
