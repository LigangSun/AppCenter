using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public abstract class SelectableQuestion : Question
    {
        private QuestionOptionCollection optionCollection = new QuestionOptionCollection();
        private bool randomOption = false;

        public QuestionOptionCollection QuestionOptionCollection
        {
            get { return this.optionCollection; }
        }

        public bool RandomOption
        {
            get { return this.randomOption; }
            set { this.randomOption = value; }
        }

        protected void InternalClone(SelectableQuestion question)
        {
            question.RandomOption = this.RandomOption;
            foreach (QuestionOption option in this.QuestionOptionCollection)
            {
                question.QuestionOptionCollection.Add(option.Clone() as QuestionOption);
            }
        }
    }
}
