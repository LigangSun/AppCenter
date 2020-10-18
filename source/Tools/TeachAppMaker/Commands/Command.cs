using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SoonLearning.TeachAppMaker.Commands
{
    public abstract class Command : ICommand
    {
        public bool CanExecute(object parameter)
        {
           return this.OnCanExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.OnExecute(parameter);
        }

        protected void OnCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
                this.CanExecuteChanged(this, EventArgs.Empty);
        }

        protected abstract void OnExecute(object parameter);
        protected abstract bool OnCanExecute(object parameter);
    }
}
