using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;

namespace SoonLearning.Math.Data
{
    public abstract class Question : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int index;

        public int Index
        {
            get { return this.index; }
            set
            {
                this.index = value;
                OnPropertyChanged("Index");
            }
        }

        public abstract int Type
        {
            get;
        }

        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public abstract void Save(Stream stream);

        protected abstract void LoadQuestion(Stream stream);

        public static Question Load(Stream stream)
        {
            Question question = null;

            int type = (int)QuestionData.ReadValue(stream);
            if (type == Question_a_b_c.type)
                question = new Question_a_b_c();

            question.LoadQuestion(stream);

            return question;
        }
    }
}
