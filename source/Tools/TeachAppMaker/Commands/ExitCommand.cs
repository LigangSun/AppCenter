using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class ExitCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            App.Current.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Normal);
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
