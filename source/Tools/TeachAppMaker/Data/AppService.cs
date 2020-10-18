using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SoonLearning.TeachAppMaker
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.soonlearning.com/", ConfigurationName = "AppServerSoap")]
    public interface AppServerSoap
    {
        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/GetLoginName", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        string GetLoginName(string Custid, string password, string Key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/GetAppList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        System.Data.DataTable GetAppList(int page, int pagesize, int userID, int typeId, int subTypeId, string orderBy, ref int totalpage, ref int totalrecord, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/GetOfflineAppList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        System.Data.DataTable GetOfflineAppList(int page, int pagesize, int userID, int typeId, int subTypeId, string orderBy, ref int totalpage, ref int totalrecord, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/Admin_GetNotApprovedAppList", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        System.Data.DataTable Admin_GetNotApprovedAppList(int page, int pagesize, string userName, string password, ref int totalpage, ref int totalrecord, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/Admin_SetAsEditorRecommend", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        void Admin_SetAsEditorRecommend(string appUniqueId, bool recommend, string userName, string password, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/Admin_Approved", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        bool Admin_Approved(string appUniqueId, string userName, string password, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/Admin_Reject", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        bool Admin_Reject(string appUniqueId, string userName, string password, string comment, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/Admin_SetStatus", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        bool Admin_SetStatus(string appUniqueId, string userName, string password, int status, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/GetAppAttachInfo", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        string GetAppAttachInfo(int appId, int userId, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/AddApp", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        void AddApp(APKSoftModel app, string userId, string password, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/GetAppClass", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        string[] GetAppClass(string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/CheckVersion", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        string CheckVersion(string uniqueId, string version, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/GetUserName", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        string GetUserName(int userId, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/IncreaseDownloadCount", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        void IncreaseDownloadCount(string appUniqueId, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/AddTopic", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        void AddTopic(string appUniqueId, int userId, string topic, string key);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/GetAppTopic", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        System.Data.DataTable GetAppTopic(string appUniqueId, string key, int page, int pageSize, ref int totalPage, ref int totalRecord);

        [System.ServiceModel.OperationContractAttribute(Action = "http://www.soonlearning.com/CheckApp", ReplyAction = "*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        int CheckApp(string userId, string password, string appId, string key);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.soonlearning.com/")]
    public partial class APKSoftModel : object, System.ComponentModel.INotifyPropertyChanged
    {

        private int idField;

        private string uniqueIdField;

        private string nameField;

        private int classIDField;

        private int subClassIDField;

        private string versionField;

        private string iCONField;

        private string fileUrlField;

        private int userIDField;

        private string detailField;

        private int isUseField;

        private int isIndexField;

        private int isNewField;

        private decimal priceField;

        private System.DateTime addDateField;

        private int downNumField;

        private int topicNumField;

        private APKAttachModel attachField;

        private string md5CheckField;

        private decimal filesizeField;

        private string sketchField;

        private int statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("Id");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string UniqueId
        {
            get
            {
                return this.uniqueIdField;
            }
            set
            {
                this.uniqueIdField = value;
                this.RaisePropertyChanged("UniqueId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
                this.RaisePropertyChanged("Name");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public int ClassID
        {
            get
            {
                return this.classIDField;
            }
            set
            {
                this.classIDField = value;
                this.RaisePropertyChanged("ClassID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public int SubClassID
        {
            get
            {
                return this.subClassIDField;
            }
            set
            {
                this.subClassIDField = value;
                this.RaisePropertyChanged("SubClassID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
                this.RaisePropertyChanged("Version");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string ICON
        {
            get
            {
                return this.iCONField;
            }
            set
            {
                this.iCONField = value;
                this.RaisePropertyChanged("ICON");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string FileUrl
        {
            get
            {
                return this.fileUrlField;
            }
            set
            {
                this.fileUrlField = value;
                this.RaisePropertyChanged("FileUrl");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public int UserID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
                this.RaisePropertyChanged("UserID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public string Detail
        {
            get
            {
                return this.detailField;
            }
            set
            {
                this.detailField = value;
                this.RaisePropertyChanged("Detail");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public int IsUse
        {
            get
            {
                return this.isUseField;
            }
            set
            {
                this.isUseField = value;
                this.RaisePropertyChanged("IsUse");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public int IsIndex
        {
            get
            {
                return this.isIndexField;
            }
            set
            {
                this.isIndexField = value;
                this.RaisePropertyChanged("IsIndex");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public int IsNew
        {
            get
            {
                return this.isNewField;
            }
            set
            {
                this.isNewField = value;
                this.RaisePropertyChanged("IsNew");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public decimal Price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
                this.RaisePropertyChanged("Price");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public System.DateTime AddDate
        {
            get
            {
                return this.addDateField;
            }
            set
            {
                this.addDateField = value;
                this.RaisePropertyChanged("AddDate");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public int DownNum
        {
            get
            {
                return this.downNumField;
            }
            set
            {
                this.downNumField = value;
                this.RaisePropertyChanged("DownNum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public int TopicNum
        {
            get
            {
                return this.topicNumField;
            }
            set
            {
                this.topicNumField = value;
                this.RaisePropertyChanged("TopicNum");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public APKAttachModel Attach
        {
            get
            {
                return this.attachField;
            }
            set
            {
                this.attachField = value;
                this.RaisePropertyChanged("Attach");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public string Md5Check
        {
            get
            {
                return this.md5CheckField;
            }
            set
            {
                this.md5CheckField = value;
                this.RaisePropertyChanged("Md5Check");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public decimal Filesize
        {
            get
            {
                return this.filesizeField;
            }
            set
            {
                this.filesizeField = value;
                this.RaisePropertyChanged("Filesize");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public string Sketch
        {
            get
            {
                return this.sketchField;
            }
            set
            {
                this.sketchField = value;
                this.RaisePropertyChanged("Sketch");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public int Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
                this.RaisePropertyChanged("Status");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.soonlearning.com/")]
    public partial class APKAttachModel : object, System.ComponentModel.INotifyPropertyChanged
    {

        private int idField;

        private int userIDField;

        private string image1Field;

        private string image2Field;

        private string image3Field;

        private string image4Field;

        private string image5Field;

        private int softIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public int Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("Id");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public int UserID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
                this.RaisePropertyChanged("UserID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Image1
        {
            get
            {
                return this.image1Field;
            }
            set
            {
                this.image1Field = value;
                this.RaisePropertyChanged("Image1");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string Image2
        {
            get
            {
                return this.image2Field;
            }
            set
            {
                this.image2Field = value;
                this.RaisePropertyChanged("Image2");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string Image3
        {
            get
            {
                return this.image3Field;
            }
            set
            {
                this.image3Field = value;
                this.RaisePropertyChanged("Image3");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string Image4
        {
            get
            {
                return this.image4Field;
            }
            set
            {
                this.image4Field = value;
                this.RaisePropertyChanged("Image4");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string Image5
        {
            get
            {
                return this.image5Field;
            }
            set
            {
                this.image5Field = value;
                this.RaisePropertyChanged("Image5");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public int SoftID
        {
            get
            {
                return this.softIDField;
            }
            set
            {
                this.softIDField = value;
                this.RaisePropertyChanged("SoftID");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface AppServerSoapChannel : AppServerSoap, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AppServerSoapClient : System.ServiceModel.ClientBase<AppServerSoap>, AppServerSoap
    {

        public AppServerSoapClient()
        {
        }

        public AppServerSoapClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public AppServerSoapClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public AppServerSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public AppServerSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public string GetLoginName(string Custid, string password, string Key)
        {
            return base.Channel.GetLoginName(Custid, password, Key);
        }

        public System.Data.DataTable GetAppList(int page, int pagesize, int userID, int typeId, int subTypeId, string orderBy, ref int totalpage, ref int totalrecord, string key)
        {
            return base.Channel.GetAppList(page, pagesize, userID, typeId, subTypeId, orderBy, ref totalpage, ref totalrecord, key);
        }

        public System.Data.DataTable GetOfflineAppList(int page, int pagesize, int userID, int typeId, int subTypeId, string orderBy, ref int totalpage, ref int totalrecord, string key)
        {
            return base.Channel.GetOfflineAppList(page, pagesize, userID, typeId, subTypeId, orderBy, ref totalpage, ref totalrecord, key);
        }

        public System.Data.DataTable Admin_GetNotApprovedAppList(int page, int pagesize, string userName, string password, ref int totalpage, ref int totalrecord, string key)
        {
            return base.Channel.Admin_GetNotApprovedAppList(page, pagesize, userName, password, ref totalpage, ref totalrecord, key);
        }

        public void Admin_SetAsEditorRecommend(string appUniqueId, bool recommend, string userName, string password, string key)
        {
            base.Channel.Admin_SetAsEditorRecommend(appUniqueId, recommend, userName, password, key);
        }

        public bool Admin_Approved(string appUniqueId, string userName, string password, string key)
        {
            return base.Channel.Admin_Approved(appUniqueId, userName, password, key);
        }

        public bool Admin_Reject(string appUniqueId, string userName, string password, string comment, string key)
        {
            return base.Channel.Admin_Reject(appUniqueId, userName, password, comment, key);
        }

        public bool Admin_SetStatus(string appUniqueId, string userName, string password, int status, string key)
        {
            return base.Channel.Admin_SetStatus(appUniqueId, userName, password, status, key);
        }

        public string GetAppAttachInfo(int appId, int userId, string key)
        {
            return base.Channel.GetAppAttachInfo(appId, userId, key);
        }

        public void AddApp(APKSoftModel app, string userId, string password, string key)
        {
            base.Channel.AddApp(app, userId, password, key);
        }

        public string[] GetAppClass(string key)
        {
            return base.Channel.GetAppClass(key);
        }

        public string CheckVersion(string uniqueId, string version, string key)
        {
            return base.Channel.CheckVersion(uniqueId, version, key);
        }

        public string GetUserName(int userId, string key)
        {
            return base.Channel.GetUserName(userId, key);
        }

        public void IncreaseDownloadCount(string appUniqueId, string key)
        {
            base.Channel.IncreaseDownloadCount(appUniqueId, key);
        }

        public void AddTopic(string appUniqueId, int userId, string topic, string key)
        {
            base.Channel.AddTopic(appUniqueId, userId, topic, key);
        }

        public System.Data.DataTable GetAppTopic(string appUniqueId, string key, int page, int pageSize, ref int totalPage, ref int totalRecord)
        {
            return base.Channel.GetAppTopic(appUniqueId, key, page, pageSize, ref totalPage, ref totalRecord);
        }

        public int CheckApp(string userId, string password, string appId, string key)
        {
            return base.Channel.CheckApp(userId, password, appId, key);
        }
    }
}
