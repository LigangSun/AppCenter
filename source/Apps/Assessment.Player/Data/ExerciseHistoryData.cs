using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SoonLearning.Assessment.Data;

namespace SoonLearning.Assessment.Player.Data
{
    internal class ExerciseHistoryData : INotifyPropertyChanged
    {
        private string title;
        private DateTime createTime;
        private TimeSpan usedTime;
        private int questionCount;
        private int finishedQuestionCount;
        private string file;
        private Exercise exercise;

        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.OnPropertyChanged("Title");
            }
        }

        public DateTime CreateTime
        {
            get { return this.createTime; }
            set
            {
                this.createTime = value;
                this.OnPropertyChanged("CreateTime");
            }
        }

        public TimeSpan UsedTime
        {
            get { return this.usedTime; }
            set
            {
                this.usedTime = value;
                this.OnPropertyChanged("UsedTime");
            }
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

        public int FinishedQuestionCount
        {
            get { return this.finishedQuestionCount; }
            set
            {
                this.finishedQuestionCount = value;
                this.OnPropertyChanged("FinishedQuestionCount");
            }
        }

        public string File
        {
            get { return this.file; }
        }

        public Exercise Exercise
        {
            get { return this.exercise; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        internal static ExerciseHistoryData FromExercise(Exercise exercise, string file)
        {
            ExerciseHistoryData data = new ExerciseHistoryData();
            data.exercise = exercise;
            data.file = file;
            data.Title = exercise.Title;
            data.CreateTime = exercise.CreateTime;
            data.UsedTime = exercise.UsedTime;
            foreach (Section section in exercise.SectionCollection)
            {
                data.QuestionCount += section.QuestionCollection.Count;
            }

            data.FinishedQuestionCount = exercise.ExerciseResponse.QuestionResponseList.Count;

            return data;
        }
    }
}
