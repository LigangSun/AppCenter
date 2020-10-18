using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class SettingCommand : Command
    {
        protected override void OnExecute(object parameter)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.ShowDialog();
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }
    }
}
