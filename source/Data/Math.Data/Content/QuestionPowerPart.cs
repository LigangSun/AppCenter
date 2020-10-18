using System;
using System.Collections.Generic;
using System.Text;
using SoonLearning.Assessment.Data;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// QuestionPowerPart用于表示指数
    /// </summary>
    public class QuestionPowerPart : QuestionContentPart
    {
        private QuestionContent questionContent = new QuestionContent();

        /// <summary>
        /// 获取或设置指数的内容。
        /// </summary>
        public QuestionContent Content
        {
            get { return this.questionContent; }
            set { this.questionContent = value; }
        }

        protected override string Prefix
        {
            get { return "_$POWER_"; }
        }

        public override int CompareTo(QuestionContentPart other)
        {
            if (this == other)
                return 0;

            if (!(other is QuestionPowerPart))
                return -1;

            QuestionPowerPart otherPower = other as QuestionPowerPart;
            return this.Content.CompareTo(otherPower.Content);
        }

        public override object Clone()
        {
            QuestionPowerPart newPart = new QuestionPowerPart();
            newPart.Id = this.Id;
            newPart.Content = this.Content.Clone() as QuestionContent;

            return newPart;
        }
    }
}
