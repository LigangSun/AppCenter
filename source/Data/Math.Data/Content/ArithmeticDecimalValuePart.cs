using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class ArithmeticDecimalValuePart : QuestionContentPart
    {
        private decimal? value;

        public Decimal? Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        protected override string Prefix
        {
            get { return "_$DECIMAL_"; }
        }

        public ArithmeticDecimalValuePart()
        {
        }

        public ArithmeticDecimalValuePart(decimal value)
        {
            this.value = value;
        }

        public override int CompareTo(QuestionContentPart other)
        {
            if (this == other)
                return 0;

            if (!(other is ArithmeticDecimalValuePart))
                return -1;

            ArithmeticDecimalValuePart valuePart = other as ArithmeticDecimalValuePart;
            if (this.Value == valuePart.value)
                return 0;
            else if (this.value > valuePart.value)
                return 1;

            return -1;
        }

        public override object Clone()
        {
            ArithmeticDecimalValuePart newPart = new ArithmeticDecimalValuePart();
            newPart.Id = this.Id;
            newPart.Value = new decimal?(this.Value.Value);

            return newPart;
        }
    }
}
