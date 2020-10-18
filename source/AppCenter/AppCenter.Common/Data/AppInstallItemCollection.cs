using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.Data
{
    public class AppInstallItemCollection : ObservableCollection<AppInstallItem>
    {
        public void Save(string file)
        {
            try
            {
                if (System.IO.File.Exists(file))
                    System.IO.File.Delete(file);
            }
            catch
            {
            }
            SerializerHelper<AppInstallItemCollection>.XmlSerialize(file, this);
        }

        public void Load(string file)
        {
            this.Clear();

            AppInstallItemCollection collection = SerializerHelper<AppInstallItemCollection>.XmlDeserialize(file);

            var itemsQuery = from item in collection
                             select item;
            foreach (var item in itemsQuery)
            {
                this.Add(item);
            }
        }
    }
}
