using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace SoonLearning.AppCenter.Data
{
    [Serializable]
    public class TypeItem
    {
        public const int Root = 0;
        public const int All = -1;
        public const int New = -2;

        public const int Installing = int.MinValue;
        public const int NotApproved = int.MinValue + 1;

        public const int MostRecentUsed = int.MinValue + 2;

        private string title;
        private string description;
        private string thumbnail;
        private int type;
        private int parentType;
        private ObservableCollection<TypeItem> subTypeItems = new ObservableCollection<TypeItem>();

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        public string Thumbnail
        {
            get { return this.thumbnail; }
            set { this.thumbnail = value; }
        }

        public int Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public int ParentType
        {
            get { return this.parentType; }
            set { this.parentType = value; }
        }

        public ObservableCollection<TypeItem> SubTypeItems
        {
            get { return this.subTypeItems; }
        }

        public int SelectedSubTypeIndex
        {
            get;
            set;
        }

        public TypeItem()
        {
        }

        public TypeItem(string title, string description, string thumbnailUrl, int type, int parentType)
        {
            this.title = title;
            this.description = description;
            this.thumbnail = thumbnailUrl;
            this.type = type;
            this.parentType = parentType;
        }

        public override string ToString()
        {
            return this.Title;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Title", this.Title);
            info.AddValue("Type", this.Type);
            info.AddValue("Description", this.Description);
            info.AddValue("Thumbnail", this.Thumbnail);
            info.AddValue("ParentType", this.ParentType);
            info.AddValue("SubTypes", this.SubTypeItems);
        }
    }
}
