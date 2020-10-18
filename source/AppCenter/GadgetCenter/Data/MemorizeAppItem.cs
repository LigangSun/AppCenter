using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Memorize.Data;
using System.IO;

namespace SoonLearning.AppCenter.Data
{
    public class MemorizeAppItem : AppItem
    {
        private MemorizeEntry entry;

        public MemorizeEntry MemorizeEntry
        {
            get { return this.entry; }
            set { this.entry = value; }
        }

        public override string AppEntryFile
        {
            get
            {
                if (base.entryFile == null)
                    return string.Empty;
                return Path.Combine(DataMgr.Instance.BaseFolder, base.entryFile);
            }
            set
            {
                if (value.StartsWith(DataMgr.Instance.BaseFolder))
                    base.entryFile = value.Substring(DataMgr.Instance.BaseFolder.Length + 1);
                else
                    base.entryFile = value;
            }
        }
    }
}
