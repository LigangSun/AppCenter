using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using SoonLearning.AppCenter.Data;
using System.Security;
using System.Diagnostics;
using SoonLearning.Memorize.Data;
using SevenZip;

namespace SoonLearning.AppCenter.Utility
{
    internal class AppInstallMgr
    {
        #region Instance

        private static AppInstallMgr instance;

        public static AppInstallMgr Instance
        {
            get
            {
                Interlocked.CompareExchange<AppInstallMgr>(ref instance, new AppInstallMgr(), null);
                return instance;
            }
        }

        private AppInstallMgr()
        {
        }

        #endregion

        #region Members

        private Dispatcher dispatcher;
        private AppInstallItemCollection installingAppCollection = new AppInstallItemCollection();
        private object collectionLocker = new object();

        private const int maxRunningCount = 1;

        private AppInstallItem runningItem = null;

        private string configFile = string.Empty;

        #endregion

        #region Property

        public Dispatcher Dispatcher
        {
            set { this.dispatcher = value; }
        }

        public ObservableCollection<AppInstallItem> AppInstallItems
        {
            get { return this.installingAppCollection; }
        }

        #endregion

        #region Event

        internal EventHandler<AppInstallCompletedEventArgs> AppInstallCompletedEvent;

        #endregion

        #region Init

        internal void Init()
        {
            this.installingAppCollection.Load(DataMgr.Instance.AppInstallConfigFile);
            this.autoStartFirst();
        }

        private void autoStartFirst()
        {
            var query = from item in this.installingAppCollection
                        where item.State != InstallState.Done
                        select item;
            foreach (var item in query)
            {
                HttpHelper helper = new HttpHelper();
                this.InitHttpHelper(helper, item);
                item.SetHttpHelper(helper);
                this.UpdateState(InstallState.Downloading, item);

                this.runningItem = item;
                string tempPackFile = this.GetTempFile(item.AppItem);
                helper.DownloadFileAsync(item.AppItem.PackageUrl, tempPackFile);

                break;
            }
        }

        #endregion

        #region Remove Item

        internal void Remove(AppInstallItem item)
        {
            lock (this.collectionLocker)
            {
                this.installingAppCollection.Remove(item);
                this.installingAppCollection.Save(DataMgr.Instance.AppInstallConfigFile);
            }
        }

        internal void Remove(string appId)
        {
            lock (this.collectionLocker)
            {
                foreach (AppInstallItem item in this.installingAppCollection)
                {
                    if (item.AppItem.UniqueId == appId)
                    {
                        this.installingAppCollection.Remove(item);
                        this.installingAppCollection.Save(DataMgr.Instance.AppInstallConfigFile);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Start a new App installation

        internal void Start(GadgetItemOnline item)
        {
            lock (this.collectionLocker)
            {
                if (Enumerable.Count<AppInstallItem>(this.installingAppCollection, (c) => (c.AppItem.Id == item.Id)) > 0)
                {
                    return;
                }
            }

            string tempPackFile = this.GetTempFile(item);

            HttpHelper helper = new HttpHelper();

            AppInstallItem appInstallItem = new AppInstallItem(item, helper, tempPackFile);
            lock (this.collectionLocker)
            {
                this.installingAppCollection.Add(appInstallItem);
            }

            this.InitHttpHelper(helper, appInstallItem);

            if (this.runningItem != null)
                return;

            this.UpdateState(InstallState.Downloading, appInstallItem);

            this.runningItem = appInstallItem;
            try
            {
                this.runningItem.WebClient.DownloadFileAsync(this.runningItem.AppItem.PackageUrl, this.runningItem.LocakPackFile);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                this.UpdateState(InstallState.DownloadFail, this.runningItem);
                this.runningItem = null;
                this.Next();
            }
        }

        private void Next()
        {
            this.runningItem = null;
            lock (this.collectionLocker)
            {
                if (this.installingAppCollection.Count > 0)
                {
                    var query = from item in this.installingAppCollection
                                where item.State == InstallState.NotStart
                                select item;
                    foreach (AppInstallItem item in query)
                    {
                        this.runningItem = item;
                        break;
                    }
                }
            }

            if (this.runningItem != null)
            {
                if (this.runningItem.WebClient == null)
                {
                    HttpHelper helper = new HttpHelper();
                    this.InitHttpHelper(helper, this.runningItem);
                    this.runningItem.SetHttpHelper(helper);
                }

                this.UpdateState(InstallState.Downloading, this.runningItem);
                try
                {
                    this.runningItem.WebClient.DownloadFileAsync(this.runningItem.AppItem.PackageUrl, this.runningItem.LocakPackFile);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                    this.UpdateState(InstallState.DownloadFail, this.runningItem);
                    this.runningItem = null;
                    this.Next();
                }
            }
        }

        private void InitHttpHelper(HttpHelper helper, AppInstallItem appInstallItem)
        {
            helper.RetryBroken = true;
            helper.RetryTimes = 10;
            helper.AddHeader("SoonLearningPlatformKey", "{1FB07182-8794-47F1-90DB-7B3698AE1E61}");
            helper.DownloadProgressChanged += ((sender, e) =>
            {
                if (this.dispatcher != null)
                {
                    this.UpdateProgress(e.ProgressPercentage, appInstallItem);
                }
            });
            helper.DownloadFileCompleted += ((sender, e) =>
            {
                if (e.Cancelled)
                {
                    this.UpdateState(InstallState.UserCancelled, appInstallItem);
                    this.InvokeInstallCompletedEvent(appInstallItem, e.Error);
                }
                else if (e.Error != null)
                {
                    appInstallItem.SetLastEx(e.Error);
                    this.UpdateState(InstallState.DownloadFail, appInstallItem);
                //    this.installingAppCollection.Remove(appInstallItem);
                    this.InvokeInstallCompletedEvent(appInstallItem, e.Error);
                }
                else
                {
                    this.Install(appInstallItem);
                }

                this.runningItem = null;
                this.Next();
            });
        }

        #endregion

        #region Install App

        private void Install(AppInstallItem item)
        {
            FileStream fs = null;
            try
            {
                this.UpdateState(InstallState.Installing, item);

                this.UpdateProgress(0, item);
                fs = File.OpenRead(item.LocakPackFile);

                // Check File Header
                string fileHeader = this.ReadString(fs);
                this.VerifyFileHeader(fileHeader);

                // Check AppTitle and AppId
                string appId = this.ReadString(fs);
                string appTitle = this.ReadString(fs);
                if (appTitle != item.AppItem.Title ||
                    appId != item.AppItem.Id)
                {
                //    throw new NotSupportedException();
                }

                string appFolder = this.GetAppFolder(fileHeader, appId);

                // Description
                string description = this.ReadString(fs);

                // Logos
                string thumbnail = this.ExtractLogo(fs, appId);

                // Entry File;
                string appEntryFile = this.ReadString(fs);
                string fullName = this.ReadString(fs);

                int type = BitConverter.ToInt32(this.ReadBytes(fs), 0);
                int subType = BitConverter.ToInt32(this.ReadBytes(fs), 0);

                AppItem appItem = this.CreateAppItem(fileHeader, type, subType);

                appItem.Id = appId;
                appItem.Title = appTitle;
                appItem.Description = description;
                appItem.Thumbnail = thumbnail;
                appItem.AppEntryFile = appEntryFile;
                appItem.FullName = fullName;
                appItem.Type = type;
                appItem.SubType = subType;

                appItem.CreateDate = DateTime.Parse(this.ReadString(fs));

                appItem.CreatorName = this.ReadString(fs);
                appItem.CreatorLogo = this.ReadString(fs);
                appItem.CreatorWebSite = this.ReadString(fs);

                appItem.AddedTime = DateTime.Now.ToUniversalTime();

                // Get File Count
                string countString = this.ReadString(fs);
                int count = Convert.ToInt32(countString);
                for (int i = 0; i < count; i++)
                {
                    // Get FileName and Data
                    string fileName = this.ReadString(fs);
                    byte[] fileData = this.ReadBytes(fs);
                    if (string.IsNullOrEmpty(fileName))
                        continue;

                    appItem.RelatedFiles.Add(fileName);

                    // Create File Path
                    string filePath = appFolder + Path.GetDirectoryName(fileName);
                    if (!Directory.Exists(filePath))
                        Directory.CreateDirectory(filePath);

                    // Create File Name
                    filePath = Path.Combine(filePath, Path.GetFileName(fileName));

                    if (File.Exists(filePath))
                    {
                    }

                    try
                    {
                        if (fileData.Length > 0)
                            File.WriteAllBytes(filePath, fileData);
                    }
                    catch
                    {
                    }

                    if (item.State == InstallState.UserCancelling)
                    {
                        this.UpdateState(InstallState.UserCancelled, item);
                        break;
                    }

                    this.UpdateProgress((int)((i + 1 * 100) / count), item);
                }

                if (appItem is MemorizeAppItem)
                {
                    MemorizeAppItem memorizeAppItem = appItem as MemorizeAppItem;
                    try
                    {
                        memorizeAppItem.MemorizeEntry = MemorizeEntry.Load(System.IO.Path.Combine(appFolder, System.IO.Path.GetFileName(appItem.AppEntryFile)));
                        if (memorizeAppItem.MemorizeEntry.SubType == 0)
                            memorizeAppItem.SubType = 10299;
                        else
                            memorizeAppItem.SubType = memorizeAppItem.MemorizeEntry.SubType;
                        memorizeAppItem.Type = 102;
                    }
                    catch (Exception e)
                    {
                        this.UpdateState(InstallState.InstallFail, item);
                        this.InvokeInstallCompletedEvent(item, e);
                        return;
                    }
                }
                else if (appItem is AssessmentAppItem)
                {
                    AssessmentAppItem assessmentAppItem = appItem as AssessmentAppItem;
                    try
                    {
                        string folder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                        if (UIHelper.Is64BitOS())
                            SevenZipExtractor.SetLibraryPath(System.IO.Path.Combine(folder, "ThirdParty\\7z\\7z64.dll"));
                        else
                            SevenZipExtractor.SetLibraryPath(System.IO.Path.Combine(folder, "ThirdParty\\7z\\7z.dll"));

                        string archiveFile = Path.Combine(folder, @"data\assessment\" + assessmentAppItem.Id + @"\" + assessmentAppItem.Id + ".zip");

                        assessmentAppItem.AppEntryFile = Path.Combine(folder, @"data\assessment\" + assessmentAppItem.Id + @"\" + assessmentAppItem.Id + ".sla");
                        using (SevenZipExtractor extractor = new SevenZipExtractor(archiveFile, "$L&%$D123@3$5^7*"))
                        {
                            string targetFolder = Path.Combine(folder, @"data\assessment\" + assessmentAppItem.Id);
                            for (int i = 0; i < extractor.FilesCount; i++)
                            {
                                string fileName = extractor.ArchiveFileNames[i];
                                if (!extractor.ArchiveFileNames[i].StartsWith("\\"))
                                    fileName = "\\" + extractor.ArchiveFileNames[i];
                                string targetFile = targetFolder + fileName;
                                FileStream targetFs = File.Open(targetFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                                extractor.ExtractFile(i, targetFs);
                                targetFs.Close();
                            }
                        }

                        File.Delete(archiveFile);
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false, ex.Message);
                        this.UpdateState(InstallState.InstallFail, item);
                        this.InvokeInstallCompletedEvent(item, ex);
                        return;
                    }
                }

                if (DataMgr.Instance.getAppItemById(appItem.Id) == null)
                    DataMgr.Instance.insertAppItem(0, appItem);
               
                this.UpdateProgress(100, item);
                this.UpdateState(InstallState.Done, item);

                this.InvokeInstallCompletedEvent(item, null);

          //      this.Remove(item);
 
                DataMgr.Instance.AppService.IncreaseDownloadCount(appItem.Id, LoginInfo.GetMD5Hash("#$32*d_&"));
            }
            catch (Exception ex)
            {
                ProcessWriteFileEx(ex, item);
                this.InvokeInstallCompletedEvent(item, ex);
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }

                try
                {
                    File.Delete(item.LocakPackFile);
                }
                catch
                {
                }
            }
        }

        private void VerifyFileHeader(string fileHeader)
        {
            if (fileHeader != "{B5F6844E-984C-4129-8D19-79FDEFBDD5DC}" &&
                fileHeader != "{BE4A1507-5B37-42EA-9E08-43EF4F363C42}" &&
                fileHeader != "{8D2C2705-B49D-4730-BF39-B0E7E0E09172}")
            {
                throw new NotSupportedException();
            }
        }

        private string GetAppFolder(string fileHeader, string appId)
        {
            string appFolder = DataMgr.Instance.BaseFolder;
            if (fileHeader == "{B5F6844E-984C-4129-8D19-79FDEFBDD5DC}")
            {
            }
            else if (fileHeader == "{8D2C2705-B49D-4730-BF39-B0E7E0E09172}")
            {
                appFolder = System.IO.Path.Combine(appFolder, string.Format(@"Data\Assessment\{0}", appId));
                if (!System.IO.Directory.Exists(appFolder))
                    System.IO.Directory.CreateDirectory(appFolder);
            }
            else if (fileHeader == "{BE4A1507-5B37-42EA-9E08-43EF4F363C42}")
            {
                appFolder = System.IO.Path.Combine(appFolder, string.Format(@"Data\Memorize\{0}", appId));
                if (!System.IO.Directory.Exists(appFolder))
                    System.IO.Directory.CreateDirectory(appFolder);
            }

            return appFolder;
        }

        private AppItem CreateAppItem(string fileHeader, int type, int subType)
        {
            AppItem item = null;
            if (fileHeader == "{BE4A1507-5B37-42EA-9E08-43EF4F363C42}")
                item = new MemorizeAppItem();
            else if (fileHeader == "{8D2C2705-B49D-4730-BF39-B0E7E0E09172}")
            {
                item = new AssessmentAppItem();
            }
            else
            {
                if (type == 200 &&
                    subType == 207)
                    item = new MathFastAppItem();
                else
                    item = new DllAppItem();
            }

            return item;
        }

        private void ProcessWriteFileEx(Exception ex, AppInstallItem item)
        {
            item.SetLastEx(ex);
            this.UpdateProgress(0, item);
            if (ex is PathTooLongException)
            {
                this.UpdateState(InstallState.InstallFail_PathTooLongException, item);
            }
            else if (ex is IOException)
            {
                this.UpdateState(InstallState.InstallFail_FileInUse, item);
            }
            else if (ex is UnauthorizedAccessException)
            {
                this.UpdateState(InstallState.InstallFail_UnauthorizedAccess, item);
            }
            else if (ex is SecurityException)
            {
                this.UpdateState(InstallState.InstallFail_RequirePermission, item);
            }
            else
            {
                this.UpdateState(InstallState.InstallFail, item);
            }
        }

        private string ExtractLogo(Stream stream, string id)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string logoExt = this.ReadString(stream);

            string thumbnail = System.IO.Path.GetDirectoryName(assembly.Location);
            thumbnail = System.IO.Path.Combine(thumbnail, @"AppLogos\");
            if (!Directory.Exists(thumbnail))
                Directory.CreateDirectory(thumbnail);
            thumbnail += id;
            thumbnail += logoExt;

            try
            {
                File.WriteAllBytes(thumbnail, this.ReadBytes(stream));
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            return thumbnail;
        }

        private string ReadString(Stream stream)
        {
            byte[] textDataLength = new byte[4];
            stream.Read(textDataLength, 0, 4);

            byte[] textData = new byte[BitConverter.ToInt32(textDataLength, 0)];
            stream.Read(textData, 0, textData.Length);
            return Encoding.UTF8.GetString(textData);
        }

        private byte[] ReadBytes(Stream stream)
        {
            byte[] dataLength = new byte[4];
            stream.Read(dataLength, 0, 4);

            byte[] data = new byte[BitConverter.ToInt32(dataLength, 0)];
            stream.Read(data, 0, data.Length);
            return data;
        }

        #endregion

        #region Update State and Progress

        private void UpdateState(InstallState state, AppInstallItem item)
        {
            this.dispatcher.BeginInvoke(new ThreadStart(() => { 
                item.State = state;
                if (state == InstallState.Done)
                {
                    item.WebClient.Clear();
                    lock (this.collectionLocker)
                    {
                        this.Remove(item);
                    //    this.installingAppCollection.Add(item);
                    }
                }

                lock (this.collectionLocker)
                {
                    this.installingAppCollection.Save(DataMgr.Instance.AppInstallConfigFile);
                }
            }), 
                DispatcherPriority.Send, 
                null);
        }

        private void UpdateProgress(int percent, AppInstallItem item)
        {
            this.dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                if (item.Percent != percent)
                    item.Percent = percent;
            }),
                DispatcherPriority.Normal,
                null);
        }

        #endregion

        #region Cancel Installation

        internal void Cancel(string appId)
        {
            AppInstallItem item = this.GetAppInstallItemById(appId);
            if (item != null)
            {
                if (item.State == InstallState.Downloading)
                {
                    if (item.WebClient != null)
                        item.WebClient.CancelAsync();
                    this.UpdateState(InstallState.UserCancelling, item);
                }
                else if (item.State == InstallState.Installing)
                {
                }
                else if (item.State == InstallState.InstallFail ||
                    item.State == InstallState.DownloadFail)
                {
                    item.WebClient.Clear();
                    lock (this.collectionLocker)
                    {
                     //   this.installingAppCollection.Remove(item);
                    }
                }
            }
        }

        #endregion

        #region Retry Installation

        internal void Retry(string appId)
        {
            AppInstallItem item = this.GetAppInstallItemById(appId);
            if (item != null)
            {
                this.UpdateState(InstallState.NotStart, item);

                if (item.WebClient == null)
                {
                    HttpHelper helper = new HttpHelper();
                    this.InitHttpHelper(helper, item);
                    item.SetHttpHelper(helper);                    
                }

                this.runningItem = item;

                this.UpdateState(InstallState.Downloading, item);
                item.WebClient.DownloadFileAsync(item.AppItem.PackageUrl, this.GetTempFile(item.AppItem));
            }
        }

        #endregion

        #region Get AppInstallItem

        private AppInstallItem GetAppInstallItemById(string appId)
        {
            lock (this.collectionLocker)
            {
                return Enumerable.First<AppInstallItem>(this.installingAppCollection, (c) => (c.AppItem.Id == appId));
            }
        }

        #endregion

        #region Get Temp File

        private string GetTempFile(GadgetItemOnline item)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string tempFolder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), "temp");
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            return System.IO.Path.Combine(tempFolder, item.Id + ".zip");
        }

        #endregion

        #region Invoke Install Completed Event

        private void InvokeInstallCompletedEvent(AppInstallItem item, Exception ex)
        {
            EventHandler<AppInstallCompletedEventArgs> temp = this.AppInstallCompletedEvent;
            if (temp != null)
                temp(this, new AppInstallCompletedEventArgs(item, ex));
        }

        #endregion
    }
}
