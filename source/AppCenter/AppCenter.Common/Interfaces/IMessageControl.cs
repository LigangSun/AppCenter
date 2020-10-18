using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppCenter.Interfaces
{
    public interface IMessageControl
    {
        void ShowMessageWindow(System.Windows.Controls.UserControl msgWnd);
        
        void CloseMessageWindow();        
    }
}
