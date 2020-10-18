using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using SoonLearning.TeachAppMaker.Commands;

namespace SoonLearning.TeachAppMaker.Modules
{
    class HelpModule
    {
        internal HelpModule(UIElement ui)
        {

        }

        private void hookCommands(UIElement ui)
        {
            ui.CommandBindings.Add(new CommandBinding(HelpCommands.HelpCommand));
            ui.CommandBindings.Add(new CommandBinding(HelpCommands.AboutCommand));
            ui.CommandBindings.Add(new CommandBinding(HelpCommands.CheckUpdateCommand));
        }
    }
}
