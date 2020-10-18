using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SoonLearning.TeachAppMaker.Data;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class OpenProjectCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            OpenWindow openWindow = new OpenWindow();
            if (openWindow.ShowDialog().Value)
            {
                ProjectMgr.Instance.OpenProject(openWindow.File);
            }
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
