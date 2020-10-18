using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SoonLearning.TeachAppMaker.Commands;

namespace SoonLearning.TeachAppMaker.Modules
{
    internal class ProjectModule
    {
        internal ProjectModule(UIElement ui)
        {

        }

        private void hookCommands(UIElement ui)
        {
            ui.CommandBindings.Add(new CommandBinding(ProjectCommands.NewProjectCommand));
            ui.CommandBindings.Add(new CommandBinding(ProjectCommands.OpenProjectCommand));
            ui.CommandBindings.Add(new CommandBinding(ProjectCommands.SaveProjectCommand));
            ui.CommandBindings.Add(new CommandBinding(ProjectCommands.SettingCommand));
            ui.CommandBindings.Add(new CommandBinding(ProjectCommands.ExitCommand));
        }
    }
}
