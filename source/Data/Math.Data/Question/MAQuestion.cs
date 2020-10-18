using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace SoonLearning.Assessment.Data
{
    public class MAQuestion : Question
    {
        private ObservableCollection<MAQuestionOption> optionList = new ObservableCollection<MAQuestionOption>();

        public override QuestionType Type
        {
            get { return QuestionType.Match; }
        }

        public ObservableCollection<MAQuestionOption> OptionList
        {
            get { return this.optionList; }
        }

        protected override object InternalClone()
        {
            return new MAQuestion();
        }
    }
}
