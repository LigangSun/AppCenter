using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace SoonLearning.Memorize.Data
{
    [XmlInclude(typeof(MemorizeRandomItemStage))]
    [XmlInclude(typeof(MemorizeStableStage))]
    [XmlInclude(typeof(MemorizeMathStage))]
    public abstract class MemorizeStage : NotifyPropertyChanged
    {
        private string backgroundImage = string.Empty;

        public string BackgroundImage
        {
            get { return this.backgroundImage; }
            set { this.backgroundImage = value; }
        }

        internal void getFullPath(string path)
        {
            this.BackgroundImage = MemorizeEntry.getFileFullPath(path, this.BackgroundImage);
        }
    }
}
