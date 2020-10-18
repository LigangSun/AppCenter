using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace SoonLearning.AppCenter.Data
{
    public class SubTypeItem : ThumbnailItem
    {
        private string title;
        private string description;
        private BitmapImage thumbnail;
        private GadgetSubType type;
        private GadgetType parentType;

        public override string Title
        {
            get { return this.title; }
        }

        public override string Description
        {
            get { return this.description; }
        }

        public override BitmapImage Thumbnail
        {
            get { return this.thumbnail; }
        }

        public GadgetSubType Type
        {
            get { return this.type; }
        }

        public GadgetType ParentType
        {
            get { return this.parentType; }
        }

        public SubTypeItem(string title, string description, string url, GadgetSubType type, GadgetType parentType)
        {
            this.title = title;
            this.description = description;
            this.thumbnail = new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
            this.type = type;
            this.parentType = parentType;
        }
    }
}
