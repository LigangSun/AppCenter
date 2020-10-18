using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.TeachAppMaker.Data;
using SoonLearning.Assessment.Data;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class DuplicateCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            if (ProjectMgr.Instance.SelectedQuestion == null)
                return;

            Question newQuestion = ProjectMgr.Instance.SelectedQuestion.Clone() as Question;
            newQuestion.Id = Guid.NewGuid().ToString("N");
            QuestionEditWindow editWindow = new QuestionEditWindow(newQuestion, false, true);
            editWindow.ShowDialog();
            //ProjectMgr.Instance.App.Items.Add(newQuestion);
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
