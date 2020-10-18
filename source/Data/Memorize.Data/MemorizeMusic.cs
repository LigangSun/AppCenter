using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SoonLearning.Memorize.Data
{
    public class MemorizeMusic : MemorizeObject
    {
        private string url = string.Empty;

        public string Url
        {
            get { return this.url; }
            set { this.url = value; }
        }

        public MemorizeMusic()
        {
        }

        public MemorizeMusic(string url)
        {
            this.url = url;
        }

        public override void Save(string path)
        {
            try
            {
                this.url = MemorizeEntry.copyFile(this.url, path);
            }
            catch
            {
            }
        }

        public override void getFullPath(string path)
        {
            this.Url = MemorizeEntry.getFileFullPath(path, this.Url);
        }

        public override string ToString()
        {
            return this.Url;
        }
    }
}
