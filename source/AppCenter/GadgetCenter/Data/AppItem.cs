using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;
using System.IO;
using System.Reflection;

namespace SoonLearning.AppCenter.Data
{
    public class AppItem : MarshalByRefObject
    {
        private string id;
        private DateTime createDate;
        private DateTime addedTime;
        private DateTime lastUsedTime;
        private int usedCount;
        private int state;

        private string title;
        private string description;
        private string thumbnail;
        private int type;
        private int subType;
        private string fullName;

        protected string entryFile;

        private List<string> relatedFileList = new List<string>();

        private string creatorName;
        private string creatorWebSite;
        private string creatorLogo;

        private bool isNew = false;

        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
            set { this.createDate = value; }
        }

        public DateTime AddedTime
        {
            get { return this.addedTime; }
            set { this.addedTime = value; }
        }

        public DateTime LastUsedTime
        {
            get { return this.lastUsedTime; }
            set { this.lastUsedTime = value; }
        }

        public int UsedCount
        {
            get { return this.usedCount; }
            set { this.usedCount = value; }
        }

        public int State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        public List<string> RelatedFiles
        {
            get { return this.relatedFileList; }
        }

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
            //get 
            //{
            //    if (this.thumbnail == null)
            //        return string.Empty;

            //    return Path.Combine(DataMgr.Instance.LogoFolder, this.thumbnail); 
            //}
            //set
            //{
            //    this.thumbnail = Path.GetFileName(value); 
            //}
        }

        public int Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public int SubType
        {
            get { return this.subType; }
            internal set { this.subType = value; }
        }

        public string FullName
        {
            get { return this.fullName; }
            set { this.fullName = value; }
        }

        public string SubTypeTitle
        {
            get
            {
                foreach (TypeItem item in DataMgr.Instance.LocalTypeCollection)
                {
                    foreach (TypeItem subItem in item.SubTypeItems)
                    {
                        if (subItem.Type == this.SubType)
                            return subItem.Title;
                    }
                }

                return string.Empty;
            }
        }

        public virtual string AppEntryFile
        {
            get 
            {
                if (this.entryFile == null)
                    return string.Empty;
                return Path.Combine(DataMgr.Instance.BaseFolder, this.entryFile);
            }
            set { this.entryFile = Path.GetFileName(value); }
        }

        public string CreatorName
        {
            get { return this.creatorName; }
            set { this.creatorName = value; }
        }

        public string CreatorWebSite
        {
            get { return this.creatorWebSite; }
            set { this.creatorWebSite = value; }
        }

        public string CreatorLogo
        {
            get { return this.creatorLogo; }
            set { this.creatorLogo = value; }
        }

        public bool IsNew
        {
            get { return this.isNew; }
            set { this.isNew = value; }
        }

        public IGadgetEntry Entry
        {
            get;
            set;
        }

        public AppItem()
        {
        }

        public AppItem(string id, 
            string title, 
            string description, 
            DateTime createDate,
            string thumbnailUri,
            int type,
            int subType, 
            string fullName,
            string creatorName,
            string creatorWebSite,
            string creatorLogo)
        {
            this.id = id;
            this.title = title;
            this.description = description;
            this.createDate = createDate;
            this.thumbnail = thumbnailUri;
            this.type = type;
            this.subType = subType;
            this.fullName = fullName;
            this.creatorName = creatorName;
            this.creatorWebSite = creatorWebSite;
            this.creatorLogo = creatorLogo;
        }
    }
}
