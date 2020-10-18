using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class AboutCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
