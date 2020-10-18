using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data;
using SoonLearning.TeachAppMaker.Data;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class AddQuestionCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            if (ProjectMgr.Instance.App == null)
                return;

            QuestionType type = (QuestionType)Enum.Parse(typeof(QuestionType), parameter as string);
            Question question = ProjectMgr.Instance.CreateQuestion(type);
            QuestionEditWindow questionEditWindow = new QuestionEditWindow(question, false, false);
            if (questionEditWindow.ShowDialog().Value)
            {
            }
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
