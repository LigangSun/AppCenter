using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// ArithmeticSignPart类用于替代试卷中的的各种运算符号。
    /// </summary>
    public class ArithmeticSignPart : QuestionContentPart
    {
        private Symbol symbol;

        /// <summary>
        /// 获取或设置符号。
        /// </summary>
        public Symbol Symbol
        {
            get { return this.symbol; }
            set { this.symbol = value; }
        }

        protected override string Prefix
        {
            get { return "_$SIGN_"; }
        }

        /// <summary>
        /// 初始化ArithmeticSignPart类的新实例。
        /// </summary>
        public ArithmeticSignPart()
        {
        }

        /// <summary>
        /// 初始化ArithmeticSignPart类的新实例。
        /// </summary>
        /// <param name="symbol">符号的常量。</param>
        public ArithmeticSignPart(Symbol symbol)
        {
            this.symbol = symbol;
        }

        public override int CompareTo(QuestionContentPart other)
        {
            if (this == other)
                return 0;

            if (!(other is ArithmeticSignPart))
                return -1;

            ArithmeticSignPart signOther = other as ArithmeticSignPart;
            return (this.Symbol - signOther.Symbol);
        }

        public override object Clone()
        {
            ArithmeticSignPart part = new ArithmeticSignPart();
            part.Id = this.Id;
            part.Symbol = this.Symbol;

            return part;
        }
    }
}
