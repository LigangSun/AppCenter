using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppCenter.Data
{
    public class AppImage
    {
        public string ImageUrl
        {
            get;
            set;
        }

        public AppImage(string url)
        {
            this.ImageUrl = url;
        }

        public AppImage()
        {
            this.ImageUrl = string.Empty;
        }
    }

    public class GadgetItemOnline
    {
        private List<AppImage> snapshotList = new List<AppImage>();

        public string Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string LongDescription
        {
            get;
            set;
        }

        public string Thumbnail
        {
            get;
            set;
        }

        public int AppType
        {
            get;
            set;
        }

        public int AppSubType
        {
            get;
            set;
        }

        public string SubTypeName
        {
            get;
            set;
        }

        public DateTime CreateDate
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }

        public int CreatorId
        {
            get;
            set;
        }

        public string Creator
        {
            get;
            set;
        }

        public string CreatorWebSite
        {
            get;
            set;
        }

        public string CreatorLogo
        {
            get;
            set;
        }

        public string PackageUrl
        {
            get;
            set;
        }

        public List<AppImage> SnapshotList
        {
            get { return this.snapshotList; }
        }

        public string UniqueId
        {
            get;
            set;
        }
    }
}
