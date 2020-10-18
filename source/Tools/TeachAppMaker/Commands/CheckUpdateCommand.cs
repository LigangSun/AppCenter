using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class CheckUpdateCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
