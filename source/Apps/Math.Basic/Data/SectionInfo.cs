using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;
using System.ComponentModel;

namespace Math.Basic.Data
{
    public class SectionBaseInfo : INotifyPropertyChanged
    {
        private QuestionType questionType;
        private string questionName = string.Empty;
        private string questionDescription = string.Empty;
        private int questionCount;

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
