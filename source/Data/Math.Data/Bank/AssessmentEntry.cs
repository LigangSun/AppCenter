using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.Assessment.Data.Bank
{
    public class AssessmentEntry
    {
        private string id;
        private string name = string.Empty;
        private string description = string.Empty;
        private string thumbnail = string.Empty;

        private int type = 0;
        private int subType = 0;

        private DateTime createDate = DateTime.Now.ToUniversalTime();
        private DateTime modifyDate = DateTime.Now.ToUniversalTime();

        private string version = string.Empty;

        private string creator = string.Empty;
        private string creatorInfo = string.Empty;

        public string Id
        {
            get { return this.id; }
        }

        public string Name
        {
            get { return this.name; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public string Thumbnail
        {
            get { return this.thumbnail; }
        }

        public int Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public int SubType
        {
            get { return this.subType; }
            set { this.subType = value; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate.ToLocalTime(); }
        }

        public DateTime ModifyDate
        {
            get { return this.modifyDate.ToLocalTime(); }
            set { this.modifyDate = value.ToUniversalTime(); }
        }

        public string Version
        {
            get { return this.version; }
        }

        public string Creator
        {
            get { return this.creator; }
        }

        public string CreatorInfo
        {
            get { return this.creatorInfo; }
        }

        public AssessmentEntry(string id, string name, string description,
            string thumbnail, int type, int subType, DateTime createDate,
            DateTime modifyDate, string version, string creator, string creatorInfo)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.thumbnail = thumbnail;
            this.type = type;
            this.subType = subType;
            this.createDate = createDate;
            this.modifyDate = modifyDate;
            this.version = version;
            this.creator = creator;
            this.creatorInfo = creatorInfo;
        }
    }
}
