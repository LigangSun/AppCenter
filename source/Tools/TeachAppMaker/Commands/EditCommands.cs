using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class EditCommands
    {
        public static readonly AddQuestionCommand AddQuestionCommand;
        public static readonly AddSubQuestionCommand AddSubQuestionCommand;
        public static readonly EditQuestionCommand EditQuestionCommand;
        public static readonly EditKnowledgeCommand EditKnowledgeCommand;
        public static readonly DeleteCommand DeleteCommand;
        public static readonly DuplicateCommand DuplicateCommand;
        public static readonly EditAppCommand EditAppCommand;

        static EditCommands()
        {
            AddQuestionCommand = new AddQuestionCommand();
            AddSubQuestionCommand = new AddSubQuestionCommand();
            EditQuestionCommand = new EditQuestionCommand();
            EditKnowledgeCommand = new EditKnowledgeCommand();
            DeleteCommand = new DeleteCommand();
            DuplicateCommand = new DuplicateCommand();
            EditAppCommand = new EditAppCommand();
        }
    }
}
