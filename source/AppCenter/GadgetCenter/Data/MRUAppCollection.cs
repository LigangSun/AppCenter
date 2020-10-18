using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.Data
{
    internal class MRUAppCollection : ObservableCollection<AppItem>
    {
        private string configFile = string.Empty;
        private const int maxMruCount = 20;

        private string ConfigFile
        {
            get
            {
                if (string.IsNullOrEmpty(configFile))
                {
                    Assembly assembly = Assembly.GetEntryAssembly();
                    configFile = Path.Combine(Path.GetDirectoryName(assembly.Location), "MruConfig.xml");
                }

                return configFile;
            }
        }

        private string[] AppIds
        {
            get
            {
                List<string> appIds = new List<string>();
                foreach (AppItem item in this)
                {
                    appIds.Add(item.Id);
                }

                return appIds.ToArray();
            }
        }

        public new void Add(AppItem item)
        {
            var query = from temp in this
                        where temp.Id == item.Id
                        select temp;

            if (query.Count() > 0)
            {
                this.Remove(query.First());
            }
            
            this.Insert(0, item);

            if (this.Items.Count > maxMruCount)
            {
                this.RemoveAt(this.Items.Count - 1);
            }

            this.Save();
        }

        public void UninstallApp(AppItem item)
        {
            if (this.Contains(item))
            {
                this.Remove(item);
            }

            this.Save();
        }

        internal void Load()
        {
            if (!File.Exists(this.ConfigFile))
                return;

            try
            {
                string[] appIds = SerializerHelper<string[]>.XmlDeserialize(this.ConfigFile);
                foreach (var id in appIds)
                {
                    AppItem item = DataMgr.Instance.getAppItemById(id);
                    if (item != null)
                        base.Add(item);
                }
            }
            catch
            {
            }
        }

        internal void Save()
        {
            try
            {
                if (File.Exists(this.ConfigFile))
                    File.Delete(this.ConfigFile);
                SerializerHelper<string[]>.XmlSerialize(this.ConfigFile, this.AppIds);
            }
            catch
            {
            }
        }        
    }
}
