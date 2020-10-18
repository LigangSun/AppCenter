using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.TeachAppMaker.Commands
{
    internal class HelpCommands
    {
        public static readonly HelpCommand HelpCommand;
        public static readonly CheckUpdateCommand CheckUpdateCommand;
        public static readonly AboutCommand AboutCommand;

        static HelpCommands()
        {
            HelpCommand = new HelpCommand();
            CheckUpdateCommand = new CheckUpdateCommand();
            AboutCommand = new AboutCommand();
        }
    }
}
