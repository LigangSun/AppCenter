using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SoonLearning.TeachAppMaker.Data
{
    public abstract class ProjectTreeNode : INotifyPropertyChanged
    {
        public string Name
        {
            get;
            set;
        }

        public string Image
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
