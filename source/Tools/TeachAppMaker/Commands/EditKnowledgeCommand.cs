using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.TeachAppMaker.Data;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class EditKnowledgeCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            if (ProjectMgr.Instance.App == null)
                return;

            KnowledgeWindow win = new KnowledgeWindow();
            if (win.ShowDialog().Value)
            {
                ProjectMgr.Instance.InvokeKnowledgeChangedEvent();
            }
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
