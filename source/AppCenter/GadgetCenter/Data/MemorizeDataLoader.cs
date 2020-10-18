using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SoonLearning.Memorize.Data;

namespace SoonLearning.AppCenter.Data
{
    internal class MemorizeDataLoader
    {
        private FileInfo[] memorizeFiles;

        internal int Count
        {
            get
            {
                if (memorizeFiles == null ||
                    memorizeFiles.Length == 0)
                    this.getMemorizeFiles();

                return this.memorizeFiles.Length;
            }
        }

        internal FileInfo[] MemorizeFiles
        {
            get
            {
                if (memorizeFiles == null ||
                    memorizeFiles.Length == 0)
                    this.getMemorizeFiles();

                return this.memorizeFiles;
            }
        }

        public MemorizeDataLoader()
        {
        }

        internal MemorizeEntry Load(string file)
        {
            return MemorizeEntry.Load(file);
        }

        private void getMemorizeFiles()
        {
            DirectoryInfo di = new DirectoryInfo(DataMgr.Instance.MemorizeDataPath);
            memorizeFiles = di.GetFiles("*.mre", SearchOption.AllDirectories);
        }
    }
}
