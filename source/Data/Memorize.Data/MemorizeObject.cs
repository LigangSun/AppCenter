using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SoonLearning.Memorize.Data
{
    [XmlInclude(typeof(MemorizeImage))]
    [XmlInclude(typeof(MemorizeMusic))]
    [XmlInclude(typeof(MemorizeText))]
    [XmlInclude(typeof(MemorizeReadText))]
    public abstract class MemorizeObject : NotifyPropertyChanged
    {
        private string itemId = string.Empty;

        public abstract void Save(string path);
        public abstract void getFullPath(string path);

        public string ItemId
        {
            get { return this.itemId; }
        }

        internal void setItemId(string id)
        {
            this.itemId = id;
        }
    }
}
