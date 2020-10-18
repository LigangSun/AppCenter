using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.AppCenter.Data
{
    public class GadgetItem : MarshalByRefObject
    {
        private string id;
        private DateTime createDate;
        private string title;
        private string description;
        private string thumbnail;
        private GadgetType type;
        private GadgetSubType subType;
        private string fullName;

        private string creatorName;
        private string creatorWebSite;
        private string creatorLogo;

        public string Id
        {
            get { return this.id; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
        }

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

        public GadgetSubType SubType
        {
            get { return this.subType; }
            internal set { this.subType = value; }
        }

        public string FullName
        {
            get { return this.fullName; }
        }

        public string SubTypeTitle
        {
            get
            {
                foreach (TypeItem item in TypeCollection.Instance)
                {
                    foreach (SubTypeItem subItem in item.SubTypeItems)
                    {
                        if (subItem.Type == this.SubType)
                            return subItem.Title;
                    }
                }

                return string.Empty;
            }
        }

        public string AppEntryFile
        {
            get;
            set;
        }

        public IGadgetEntry Entry
        {
            get;
            set;
        }

        public string CreatorName
        {
            get { return this.creatorName; }
        }

        public string CreatorWebSite
        {
            get { return this.creatorWebSite; }
        }

        public string CreatorLogo
        {
            get { return this.creatorLogo; }
        }

        public GadgetItem(string id, 
            string title, 
            string description, 
            DateTime createDate,
            string thumbnailUri,
            GadgetType type,
            GadgetSubType subType, 
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
