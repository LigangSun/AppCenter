using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// In the FIB Question Content the text "_$BLANK_ID$_" will be taken as question blank,
    /// it will be replace by textbox or other blank on UI
    /// </summary>
    public class FIBQuestion : Question
    {
        private string clearContent = string.Empty;

        public QuestionPartCollection QuestionBlankCollection
        {
            get { return this.Content.QuestionPartCollection; }
        }

        public string ClearContent
        {
            get
            {
                if (this.clearContent != string.Empty)
                    return this.clearContent;

                this.clearContent = this.Content.Content;
                int startIndex = 0;
                while (true)
                {
                    startIndex = this.clearContent.IndexOf("_$BLANK_", startIndex);
                    if (startIndex >= 0)
                    {
                        int endIndex = this.clearContent.IndexOf("$_", startIndex);
                        this.clearContent = this.clearContent.Remove(startIndex, endIndex - startIndex + 2);
                    }
                    else
                    {
                        break;
                    }
                }

                return this.clearContent;
            }
        }

        public bool ShowBlankInContent
        {
            get;
            set;
        }

        public override QuestionType Type
        {
            get { return QuestionType.FillInBlank; }
        }

        public FIBQuestion()
        {
            this.ShowBlankInContent = true;
        }

        /// <summary>
        /// Check if the content correct
        /// </summary>
        /// <param name="blankId"></param>
        /// <param name="anyBlank">The content match any blank's ref answer is OK</param>
        /// <returns></returns>
        public bool IsContentCorrect(string blankId, QuestionContent content)
        {
            foreach (QuestionContentPart part in this.QuestionBlankCollection)
            {
                if (part is QuestionBlank)
                {
                    QuestionBlank blank = part as QuestionBlank;
                    if (blank.MatchOwnRefAnswer)
                    {
                        if (blankId != blank.Id)
                            continue;
                    }

                    foreach (QuestionContent answer in blank.ReferenceAnswerList)
                    {
                        if (answer.Content == content.Content)
                            return true;
                    }
                }
            }

            return false;
        }

        public QuestionBlank GetBlank(string blankId)
        {
            foreach (QuestionContentPart part in this.QuestionBlankCollection)
            {
                if (part is QuestionBlank)
                {
                    QuestionBlank blank = part as QuestionBlank;
                    if (blank.Id == blankId)
                        return blank;
                }
            }

            return null;
        }

        protected override object InternalClone()
        {
            FIBQuestion fibQuestion = new FIBQuestion();
            fibQuestion.ShowBlankInContent = this.ShowBlankInContent;
            
            return fibQuestion;
        }
    }
}
