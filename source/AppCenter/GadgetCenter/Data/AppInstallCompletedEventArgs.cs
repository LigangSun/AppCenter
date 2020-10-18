using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppCenter.Data
{
    public class AppInstallCompletedEventArgs : EventArgs
    {
        private Exception error;
        private AppInstallItem itemOnline;

        public Exception Error
        {
            get { return this.error; }
        }

        public AppInstallItem Item
        {
            get { return this.itemOnline; }
        }

        public AppInstallCompletedEventArgs(AppInstallItem item, Exception ex)
        {
            this.itemOnline = item;
            this.error = ex;
        }
    }
}
