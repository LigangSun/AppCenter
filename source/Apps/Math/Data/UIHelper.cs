using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Controls;
using SoonLearning.Math.Fast.RapidCalculation;
using System.Windows;
using SoonLearning.AppCenter.Controls;

namespace SoonLearning.Math.Data
{
    internal class UIHelper
    {
        public static void ShowTrialMessage()
        {
            MessageWindow msgWnd = new MessageWindow();
            msgWnd.ShowMessage(string.Format("您所使用的{0}是试用版本，不允许修改设置项！", ControlMgr.Instance.Entry.Title), MessageBoxButton.OK, null);
        }
    }
}
