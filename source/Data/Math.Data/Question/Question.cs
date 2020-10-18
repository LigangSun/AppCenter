using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SoonLearning.Assessment.Data
{
    [XmlInclude(typeof(MCQuestion))]
    [XmlInclude(typeof(MRQuestion))]
    [XmlInclude(typeof(MAQuestion))]
    [XmlInclude(typeof(FIBQuestion))]
    [XmlInclude(typeof(TFQuestion))]
    [XmlInclude(typeof(TableQuestion))]
    [XmlInclude(typeof(ESQuestion))]
    [XmlInclude(typeof(VerticalFormQuestion))]
    [XmlInclude(typeof(CPQuestion))]
    public abstract class Question : BaseObject, ICloneable
    {
        private QuestionContent questionContent = new QuestionContent();
        private DateTime createTime = DateTime.Now.ToUniversalTime();
        private string creator = string.Empty;
        private QuestionContent solution = new QuestionContent();
        private int dificultyLevel = 3; // 1 - 5, 5 is the hardest.

        public QuestionContent Content
        {
            get { return this.questionContent; }
            set { this.questionContent = value; }
        }

        public int DifficultyLevel
        {
            get { return this.dificultyLevel; }
            set
            {
                this.dificultyLevel = value;
                base.OnPropertyChanged("DifficultyLevel");
            }
        }

        public QuestionContent Solution
        {
            get { return this.solution; }
            set { this.solution = value; }
        }

        public string Tip
        {
            get;
            set;
        }

        public DateTime CreateTime
        {
            get { return this.createTime; }
            set { this.createTime = value; }
        }

        public string Creator
        {
            get { return this.creator; }
            set { this.creator = value; }
        }

        public DateTime ModifyTime
        {
            get;
            set;
        }

        // UserId
        public string ModifyBy
        {
            get;
            set;
        }

        public abstract QuestionType Type
        {
            get;
        }

        public object Clone()
        {
            Question newQuestion = this.InternalClone() as Question;
            newQuestion.Content = this.Content.Clone() as QuestionContent;
            newQuestion.CreateTime = this.CreateTime;
            newQuestion.Creator = this.Creator;
            newQuestion.DifficultyLevel = this.DifficultyLevel;
            newQuestion.Id = this.Id;
            newQuestion.ModifyBy = this.ModifyBy;
            newQuestion.ModifyTime = this.ModifyTime;
            newQuestion.Solution = this.Solution.Clone() as QuestionContent;
            newQuestion.Tip = this.Tip;

            return newQuestion;
        }

        protected abstract object InternalClone();
    }
}
