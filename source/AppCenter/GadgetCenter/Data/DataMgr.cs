using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Data;
using System.Collections.ObjectModel;
using System.Threading;
using SoonLearning.AppCenter.Utility;
using System.Reflection;
using System.Windows.Shapes;

namespace SoonLearning.AppCenter.Data
{
    public class DataMgr
    {
        private static DataMgr dataMgr;

        public static DataMgr Instance
        {
            get
            {
                Interlocked.CompareExchange<DataMgr>(ref dataMgr, new DataMgr(), null);
                return dataMgr;
            }
        }

        private DataMgr()
        {
            this.loginInfo = LoginInfo.Load();
        }

        internal const string appServiceAddres = @"http://www.soonlearning.com/WebServers/AppServer.asmx";

        private AppItemCollection gadgetItemCollection = new AppItemCollection();
        private MRUAppCollection mruAppCollection = new MRUAppCollection();
        private LoginInfo loginInfo = null;
        private AppService appService;
        private TypeCollection onlineTypeCollection = new TypeCollection();
        private TypeCollection localTypeCollection;
        
        private string logoFolder = string.Empty;
        private string baseFolder = string.Empty;
        private string appInstallConfigFile = string.Empty;
        private string memorizeDataPath = string.Empty;
        private string typeConfigFile = string.Empty;
        private string assessmentDataPath = string.Empty;

        private Dictionary<int, AppItemCollection> appItemCollectionDictionary = new Dictionary<int, AppItemCollection>();
        
        internal AppItemCollection this[int type]
        {
            get
            {
                if (!this.appItemCollectionDictionary.ContainsKey(type))
                    this.appItemCollectionDictionary.Add(type, new AppItemCollection());

                if (this.appItemCollectionDictionary[type].Count == 0 ||
                    !(this.appItemCollectionDictionary[type][this.appItemCollectionDictionary[type].Count - 1] is AddAppItem))
                {
                    this.appItemCollectionDictionary[type].Add(this.createAddAppItem());
                }

                return this.appItemCollectionDictionary[type];
            }
        }

        internal MRUAppCollection MruItems
        {
            get { return this.mruAppCollection; }
        }

        public TypeCollection OnlineTypeCollection
        {
            get { return this.onlineTypeCollection; }
        }

        public TypeCollection LocalTypeCollection
        {
            get
            {
                if (this.localTypeCollection == null)
                {
                    try
                    {
                        if (System.IO.File.Exists(DataMgr.Instance.TypeConfigFile))
                            this.localTypeCollection = SerializerHelper<TypeCollection>.XmlDeserialize(DataMgr.Instance.TypeConfigFile);
                    }
                    catch
                    {
                    }

                    if (this.localTypeCollection == null)
                    {
                        this.localTypeCollection = new TypeCollection();
                        this.localTypeCollection.initDefault();
                    }

                    var query = from temp in this.localTypeCollection
                                where temp.Type == TypeItem.MostRecentUsed
                                select temp;
                    if (query.Count() == 0)
                        this.localTypeCollection.Insert(0, new TypeItem("最近使用", "", "", TypeItem.MostRecentUsed, TypeItem.Root));
                }

                return this.localTypeCollection; 
            }
        }

        public AppMainTagCollection MainTags
        {
            get { return AppMainTagCollection.Instance; }
        }

        public int AppCount
        {
            get
            {
                int count = 0;
                foreach (AppItemCollection collection in this.appItemCollectionDictionary.Values)
                    count += collection.Count;

                return count;
            }
        }

        //internal AppItemCollection MathFastAppItems
        //{
        //    get
        //    {
        //        return this[typeof(MathFastAppItem)];
        //    }
        //}

        //internal AppItemCollection DllAppItems
        //{
        //    get
        //    {
        //        return this[typeof(DllAppItem)];
        //    }
        //}

        //internal AppItemCollection MemorizeAppItems
        //{
        //    get
        //    {
        //        return this[typeof(MemorizeAppItem)];
        //    }
        //}

        internal LoginInfo LoginInfo
        {
            get
            {
                return this.loginInfo;
            }
        }

        internal AppService AppService
        {
            get
            {
                Interlocked.CompareExchange<AppService>(ref this.appService, new AppService(), null);
                this.appService.Endpoint.Address = new System.ServiceModel.EndpointAddress(DataMgr.appServiceAddres);
                return this.appService;
            }
        }

        internal string LogoFolder
        {
            get 
            { 
                if (string.IsNullOrEmpty(this.logoFolder))
                {
                    try
                    {
                        Assembly assembly = Assembly.GetEntryAssembly();
                        this.baseFolder = System.IO.Path.GetDirectoryName(assembly.Location);
                        this.logoFolder = System.IO.Path.Combine(this.baseFolder, "AppLogos");
                    }
                    catch
                    {
                    }
                }
                return this.logoFolder;
            }
        }

        internal string BaseFolder
        {
            get
            {
                if (string.IsNullOrEmpty(this.logoFolder))
                {
                    try
                    {
                        Assembly assembly = Assembly.GetEntryAssembly();
                        this.baseFolder = System.IO.Path.GetDirectoryName(assembly.Location);
                        this.logoFolder = System.IO.Path.Combine(this.baseFolder, "AppLogos");
                    }
                    catch
                    {
                    }
                }
                return this.baseFolder; 
            }
        }

        internal string AppInstallConfigFile
        {
            get
            {
                if (string.IsNullOrEmpty(this.appInstallConfigFile))
                {
                    try
                    {
                        Assembly assembly = Assembly.GetEntryAssembly();
                        string folder = System.IO.Path.GetDirectoryName(assembly.Location);
                        this.appInstallConfigFile = System.IO.Path.Combine(folder, "Install");
                        if (!System.IO.Directory.Exists(this.appInstallConfigFile))
                            System.IO.Directory.CreateDirectory(this.appInstallConfigFile);
                        this.appInstallConfigFile = System.IO.Path.Combine(this.appInstallConfigFile, "AppInstallConfig.xml");
                    }
                    catch
                    {
                    }
                }
                return this.appInstallConfigFile;
            }
        }

        internal string MemorizeDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.memorizeDataPath))
                {
                    this.memorizeDataPath = System.IO.Path.Combine(this.BaseFolder, @"data\Memorize\");
                }

                if (!System.IO.Directory.Exists(this.memorizeDataPath))
                    System.IO.Directory.CreateDirectory(this.memorizeDataPath);

                return this.memorizeDataPath;
            }
        }

        internal string AssessmentDataPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.assessmentDataPath))
                {
                    this.assessmentDataPath = System.IO.Path.Combine(this.BaseFolder, @"data\Assessment\");
                }

                if (!System.IO.Directory.Exists(this.assessmentDataPath))
                    System.IO.Directory.CreateDirectory(this.assessmentDataPath);

                return this.assessmentDataPath;
            }
        }

        internal string TypeConfigFile
        {
            get
            {
                if (string.IsNullOrEmpty(this.typeConfigFile))
                {
                    this.typeConfigFile = System.IO.Path.Combine(this.BaseFolder, @"TypeConfig\config.xml");
                }

                if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(this.typeConfigFile)))
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(this.typeConfigFile));

                return this.typeConfigFile;
            }
        }

        internal AppItemCollection getAppCollectionByType(int type)
        {
            AppItemCollection collection = new AppItemCollection();
            foreach (AppItemCollection temp in this.appItemCollectionDictionary.Values)
            {
                if (temp.Count > 0)
                {
                    if (temp[0].Type == type)
                    {
                        foreach (var app in temp)
                        {
                            if (app is AddAppItem)
                                continue;

                            collection.Add(app);
                        }
                    }
                }
            }

            collection.Add(this.createAddAppItem());

            return collection;
        }

        internal AppItem getAppItemById(string id)
        {
            foreach (AppItemCollection collection in this.appItemCollectionDictionary.Values)
            {
                var query = from item in collection
                            where item.Id == id
                            select item;

                if (query.Count() > 0)
                    return query.First();
            }

            return null;
        }

        internal void addAppItem(AppItem item)
        {
            this[item.SubType].Insert(this[item.SubType].Count-1, item);
        }

        internal void insertAppItem(int index, AppItem item)
        {
            this[item.SubType].Insert(index, item);
        }

        internal void uninstallApp(AppItem item)
        {
            this[item.SubType].UninstallApp(item);
            AppInstallMgr.Instance.Remove(item.Id);
        }

        internal void preLoadApps()
        {
            AppItemCollection collection = new AppItemCollection();
            collection.Load();
        }

        internal void addOnlineTypeItem(TypeItem item)
        {
            if (!this.isTypeItemExist(this.onlineTypeCollection, item))
            {
                this.addTypeItemToCollection(this.onlineTypeCollection, item);
            }
        }

        internal void addLocalTypeItem(TypeItem item)
        {
            if (!this.isTypeItemExist(this.localTypeCollection, item))
            {
                this.addTypeItemToCollection(this.localTypeCollection, item);
            }
        }

        internal void saveLocalTypeCollection()
        {
            try
            {
                string temp = this.TypeConfigFile + ".tmp";
                SerializerHelper<TypeCollection>.XmlSerialize(temp, this.LocalTypeCollection);

                if (System.IO.File.Exists(this.TypeConfigFile))
                {
                    System.IO.File.Delete(this.TypeConfigFile);
                }

                System.IO.File.Move(temp, this.TypeConfigFile);
            }
            catch
            {
            }
        }

        private void addTypeItemToCollection(TypeCollection collection, TypeItem item)
        {
            if (item.ParentType == 0)
            {
                collection.Add(item);
            }
            else
            {
                foreach (TypeItem temp in collection)
                {
                    if (temp.Type == item.ParentType)
                    {
                        temp.SubTypeItems.Add(item);
                        break;
                    }
                }
            }
        }

        private bool isTypeItemExist(TypeCollection collection, TypeItem item)
        {
            foreach (TypeItem temp in collection)
            {
                if (temp.Type == item.Type)
                    return true;

                foreach (TypeItem subTemp in temp.SubTypeItems)
                {
                    if (subTemp.Type == item.Type)
                        return true;
                }
            }

            return false;
        }

        private AddAppItem createAddAppItem()
        {
            AddAppItem item = new AddAppItem();
            item.Thumbnail = @"/Resources/Images/AddApp.png";
            item.Title = "添加应用";
            item.Description = "进入速学应用市场，添加更多精彩应用。";
            return item;
        }
    }
}
