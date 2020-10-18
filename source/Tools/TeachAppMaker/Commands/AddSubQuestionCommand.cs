using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.TeachAppMaker.Data;
using SoonLearning.Assessment.Data;
using SoonLearning.TeachAppMaker.Dialogs;
using System.Collections.ObjectModel;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class AddSubQuestionCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            if (ProjectMgr.Instance.App == null)
                return;

            ObservableCollection<Question> subQuestionCollection = parameter as ObservableCollection<Question>;
            if (subQuestionCollection == null)
                return;

            SubQuestionTypeDialog subQuestionTypeDlg = new SubQuestionTypeDialog();
            if (subQuestionTypeDlg.ShowDialog().Value)
            {
                QuestionType type = subQuestionTypeDlg.SelectedQuestionType;
                Question question = ProjectMgr.Instance.CreateQuestion(type);
                QuestionEditWindow questionEditWindow = new QuestionEditWindow(question, true, false);
                if (questionEditWindow.ShowDialog().Value)
                {
                    subQuestionCollection.Add(question);
                    ProjectMgr.Instance.Changed = true;
                }
            }
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
