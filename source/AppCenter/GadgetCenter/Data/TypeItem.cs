using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace GadgetCenter.Data
{
    public class TypeItem
    {
        private string title;
        private string description;
        private string thumbnail;
        private GadgetType type;
        private ObservableCollection<SubTypeItem> subTypeItems = new ObservableCollection<SubTypeItem>();

        public string Title
        {
            get { return this.title; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public string Thumbnail
        {
            get { return this.thumbnail; }
        }

        public GadgetType Type
        {
            get { return this.type; }
        }

        public ObservableCollection<SubTypeItem> SubTypeItems
        {
            get { return this.subTypeItems; }
        }

        public TypeItem(string title, string description, string thumbnailUrl, GadgetType type)
        {
            this.title = title;
            this.description = description;
            this.thumbnail = thumbnailUrl;
            this.type = type;
        }
    }
}
