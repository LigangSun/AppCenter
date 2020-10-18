using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading;
using SoonLearning.AppCenter.Resources;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.AppCenter.Data
{
    public class AppMainTagCollection : ObservableCollection<TypeItem>
    {
        private static AppMainTagCollection instance;

        public static AppMainTagCollection Instance
        {
            get 
            {
                Interlocked.CompareExchange<AppMainTagCollection>(ref instance, new AppMainTagCollection(), null);
                return instance;
            }
        }

        private AppMainTagCollection()
        {
        }
    }
}
