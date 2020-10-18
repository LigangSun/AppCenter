using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class Section : BaseObject
    {
        private QuestionCollection questionCollection = new QuestionCollection();
        private int currentQuestionIndex = -1;

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int QuestionIndex
        {
            get { return this.currentQuestionIndex; }
        }

        public Question CurrentQuestion
        {
            get { return this.questionCollection[this.currentQuestionIndex]; }
        }

        public Question NextQuestion
        {
            get
            {
                if (this.currentQuestionIndex < this.questionCollection.Count - 1)
                {
                    this.currentQuestionIndex++;
                    return this.questionCollection[this.currentQuestionIndex];
                }

                return null;
            }
        }

        public Question PreQuestion
        {
            get
            {
                if (this.currentQuestionIndex > 0)
                {
                    this.currentQuestionIndex--;
                    return this.questionCollection[this.currentQuestionIndex];
                }

                return null;
            }
        }

        public Question LastQuestion
        {
            get
            {
                this.currentQuestionIndex = this.questionCollection.Count - 1;
                return this.questionCollection[this.currentQuestionIndex];
            }
        }

        public Question FirstQuestion
        {
            get
            {
                this.currentQuestionIndex = 0;
                return this.questionCollection[this.currentQuestionIndex];
            }
        }

        public QuestionCollection QuestionCollection
        {
            get { return this.questionCollection; }
        }

        public bool IsLastQuestion
        {
            get { return this.currentQuestionIndex == this.questionCollection.Count - 1; }
        }

        public bool IsFirstQuestion
        {
            get { return this.currentQuestionIndex == 0; }
        }
    }
}
