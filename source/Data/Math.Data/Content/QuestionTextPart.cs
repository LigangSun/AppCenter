using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class QuestionTextPart : QuestionContentPart
    {
        private string text = @"";

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        protected override string Prefix
        {
            get { return "_$TEXT_"; }
        }

        public QuestionTextPart(string text)
        {
            this.text = text;
        }

        public QuestionTextPart()
        {
        }

        public override int CompareTo(QuestionContentPart other)
        {
            if (this == other)
                return 0;

            if (!(other is QuestionTextPart))
                return -1;

            QuestionTextPart textPart = other as QuestionTextPart;
            return this.text.CompareTo(textPart.Text);
        }

        public override object Clone()
        {
            QuestionTextPart newPart = new QuestionTextPart();
            newPart.Id = this.Id;
            newPart.Text = this.Text;

            return newPart;
        }
    }
}
