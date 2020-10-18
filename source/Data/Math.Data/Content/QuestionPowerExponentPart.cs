using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// QuestionPowerExponentPart类用于表示指数幂。
    /// </summary>
    public class QuestionPowerExponentPart : QuestionContentPart
    {
        private QuestionContent baseNumber = new QuestionContent();
        private QuestionPowerPart power = new QuestionPowerPart();

        /// <summary>
        /// 获取或设置底数。
        /// </summary>
        public QuestionContent BaseNumber
        {
            get { return this.baseNumber; }
            set { this.baseNumber = value; }
        }

        /// <summary>
        /// 获取或设置指数。
        /// </summary>
        public QuestionPowerPart Power
        {
            get { return this.power; }
            set { this.power = value; }
        }

        protected override string Prefix
        {
            get { return "_$POWEREXPONENT_"; }
        }

        public override int CompareTo(QuestionContentPart other)
        {
            /*
            QuestionPowerExponentPart part = new QuestionPowerExponentPart();
            part.BaseNumber = new QuestionContent();

            QuestionPowerExponentPart part1 = new QuestionPowerExponentPart();
            part.BaseNumber.QuestionPartCollection.Add(part1);
            part.BaseNumber.Content += part1.PlaceHolder;

            part.BaseNumber.Content += "-";

            part.Power.Power
             * */

            if (this == other)
                return 0;

            if (!(other is QuestionPowerExponentPart))
                return -1;

            QuestionPowerExponentPart otherPart = other as QuestionPowerExponentPart;
            int retBase = this.baseNumber.CompareTo(otherPart.BaseNumber);
            int retPower = this.power.CompareTo(otherPart.Power);

            if (retBase == 0 && retPower == 0)
                return 0;

            return -1;
        }

        public override object Clone()
        {
            QuestionPowerExponentPart newPart = new QuestionPowerExponentPart();
            newPart.Id = this.Id;
            newPart.BaseNumber = this.BaseNumber.Clone() as QuestionContent;
            newPart.Power = this.Power.Clone() as QuestionPowerPart;

            return newPart;
        }
    }
}
