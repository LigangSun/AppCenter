using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SoonLearning.TeachAppMaker.Data
{
    public class AppDescription : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler temp = this.PropertyChanged;
            if (temp != null)
                temp(this, new PropertyChangedEventArgs(name));
        }

        private string id;
        private string title;
        private string description;
        private DateTime createDate;
        private string file;

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                this.OnPropertyChanged("Id");
            }
        }

        public string Title
        {
            get { return this.title; }
            set
            {
                this.title = value;
                this.OnPropertyChanged("Title");
            }
        }

        public string Description
        {
            get { return this.description; }
            set
            {
                this.description = value;
                this.OnPropertyChanged("Description");
            }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
            set
            {
                this.createDate = value;
                this.OnPropertyChanged("CreateDate");
            }
        }

        internal string File
        {
            get { return this.file; }
            set { this.file = value; }
        }
    }
}
