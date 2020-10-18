using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppCenter.Upgrade
{
    public class UpgradeInfo
    {
        private List<UpgradeItem> items = new List<UpgradeItem>();

        public string Name
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }

        public List<UpgradeItem> Items
        {
            get
            {
                return this.items;
            }
        }
    }
}
