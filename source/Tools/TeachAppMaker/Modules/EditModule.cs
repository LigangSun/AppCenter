using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SoonLearning.TeachAppMaker.Commands;

namespace SoonLearning.TeachAppMaker.Modules
{
    internal class EditModule
    {
        internal EditModule(UIElement ui)
        {

        }

        private void hookCommands(UIElement ui)
        {
            ui.CommandBindings.Add(new CommandBinding(EditCommands.AddQuestionCommand));
            ui.CommandBindings.Add(new CommandBinding(EditCommands.DuplicateCommand));
            ui.CommandBindings.Add(new CommandBinding(EditCommands.DeleteCommand));
            ui.CommandBindings.Add(new CommandBinding(EditCommands.EditQuestionCommand));
            ui.CommandBindings.Add(new CommandBinding(EditCommands.EditKnowledgeCommand));
            ui.CommandBindings.Add(new CommandBinding(EditCommands.EditAppCommand));
        }
    }
}
