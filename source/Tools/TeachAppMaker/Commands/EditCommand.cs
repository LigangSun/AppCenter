using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data;
using SoonLearning.TeachAppMaker.Data;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class EditQuestionCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            bool subQuestion = false;

            Question q = null;
            if (parameter != null)
            {
                subQuestion = true;
                q = parameter as Question;
            }
            else
            {
                subQuestion = false;
                q = ProjectMgr.Instance.SelectedQuestion;
            }

            if (q == null)
                return;

            QuestionEditWindow questionEditWindow = new QuestionEditWindow(q, subQuestion, false);
            if (questionEditWindow.ShowDialog().Value)
            {
                ProjectMgr.Instance.Changed = true;
            }
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
            return ProjectMgr.Instance.SelectedQuestion != null;
        }
    }
}
