using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SoonLearning.TeachAppMaker.Data;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class NewProjectCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            NewWindow newWindow = new NewWindow();
            if (newWindow.ShowDialog().Value)
            {
                ProjectMgr.Instance.NewProject(newWindow.AssessmentName, newWindow.Description, newWindow.Thumbnail);
            }
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
