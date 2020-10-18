using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data;
using System.ComponentModel;

namespace SoonLearning.Assessment.Player.Data
{
    public class SectionBaseInfo : INotifyPropertyChanged
    {
        private QuestionType questionType;
        private string questionName = string.Empty;
        private string questionDescription = string.Empty;
        private int questionCount;
        private int maxQuestionCount = -1;

        public QuestionType QuestionType
        {
            get { return this.questionType; }
        }

        public string Name
        {
            get { return this.questionName; }
        }

        public string Description
        {
            get { return this.questionDescription; }
        }

        public int QuestionCount
        {
            get { return this.questionCount; }
            set
            {
                this.questionCount = value;
                this.OnPropertyChanged("QuestionCount");
            }
        }

        public int MaxQuestionCount
        {
            get { return this.maxQuestionCount; }
            set
            {
                this.maxQuestionCount = value;
                this.OnPropertyChanged("MaxQuestionCount");
            }
        }

        public SectionBaseInfo(QuestionType type, string name, string description, int count)
        {
            this.questionType = type;
            this.questionName = name;
            this.questionDescription = description;
            this.questionCount = count;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
