using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.TeachAppMaker.Data;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class DeleteCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            ProjectMgr.Instance.DeleteSelectedQuestion();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
