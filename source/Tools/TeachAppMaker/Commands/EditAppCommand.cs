using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.TeachAppMaker.Data;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class EditAppCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            if (ProjectMgr.Instance.App == null)
                return;

            NewWindow newWindow = new NewWindow();
            newWindow.AssessmentName = ProjectMgr.Instance.App.Name;
            newWindow.Description = ProjectMgr.Instance.App.Description;
            newWindow.Thumbnail = ProjectMgr.Instance.App.Thumbnail;
            if (newWindow.ShowDialog().Value)
            {
                ProjectMgr.Instance.App.Name = newWindow.AssessmentName;
                ProjectMgr.Instance.App.Description = newWindow.Description;
                ProjectMgr.Instance.App.Thumbnail = newWindow.Thumbnail;
                ProjectMgr.Instance.Changed = true;
            }
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
