using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace SoonLearning.Assessment.Data.Bank
{
    public class AssessmentApp : BaseObject
    {
        private Knowledge knowledge = new Knowledge();
    //    private Dictionary<QuestionType, QuestionBank> questionBankDictionary = new Dictionary<QuestionType,QuestionBank>();
        private ObservableCollection<Question> questionBank = new ObservableCollection<Question>();

        private string name;
        private string description;
        private string thumbnail;
        private DateTime createDate = DateTime.Now.ToUniversalTime();
        private DateTime modifyDate = DateTime.Now.ToUniversalTime();

        private int type;
        private int subType;

        private string version = string.Empty;

        private string creator = string.Empty;
        private string creatorInfo = string.Empty;
        private string creatorWebsite = string.Empty;
        private string creatorLogo = string.Empty;

        public Knowledge Knowledge
        {
            get { return this.knowledge; }
            set { this.knowledge = value; }
        }

        //public QuestionBank this[QuestionType type]
        //{
        //    get 
        //    {
        //        if (!this.questionBankDictionary.ContainsKey(type))
        //            this.questionBankDictionary.Add(type, new QuestionBank());

        //        return this.questionBankDictionary[type];
        //    }
        //    set 
        //    {
        //        this.questionBankDictionary[type] = value;
        //        this.OnPropertyChanged("this");
        //    }
        //}

        public ObservableCollection<Question> Items
        {
            get { return this.questionBank; }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                base.OnPropertyChanged("Name");
            }
        }

        public string Description
        {
            get { return this.description; }
            set
            {
                this.description = value;
                base.OnPropertyChanged("Description");
            }
        }

        public string Thumbnail
        {
            get { return this.thumbnail; }
            set
            {
                this.thumbnail = value;
                base.OnPropertyChanged("Logo");
            }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
            set
            {
                this.createDate = value.ToUniversalTime();
                this.OnPropertyChanged("CreateDate");
            }
        }

        public DateTime ModifyDate
        {
            get { return this.modifyDate; }
            set
            {
                this.modifyDate = value.ToUniversalTime();
                this.OnPropertyChanged("ModifyDate");
            }
        }

        public int Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
                this.OnPropertyChanged("Type");
            }
        }

        public int SubType
        {
            get { return this.subType; }
            set
            {
                this.subType = value;
                this.OnPropertyChanged("SubType");
            }
        }

        public string Version
        {
            get { return this.version; }
            set
            {
                this.version = value;
                this.OnPropertyChanged("Version");
            }
        }

        public string Creator
        {
            get { return this.creator; }
            set
            {
                this.creator = value;
                this.OnPropertyChanged("Creator");
            }
        }

        public string CreatorInfo
        {
            get { return this.creatorInfo; }
            set
            {
                this.creatorInfo = value;
                this.OnPropertyChanged("CreatorInfo");
            }
        }

        public string CreatorWebsite
        {
            get { return this.creatorWebsite; }
            set
            {
                this.creatorWebsite = value;
                this.OnPropertyChanged("CreatorWebsite");
            }
        }

        public string CreatorLogo
        {
            get { return this.creatorLogo; }
            set
            {
                this.creatorLogo = value;
                this.OnPropertyChanged("CreatorLogo");
            }
        }
    }
}
