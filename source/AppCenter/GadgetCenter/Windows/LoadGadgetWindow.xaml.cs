using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Windows.Resources;
using System.Reflection;
using System.IO;
using System.Threading;
using SoonLearning.AppCenter.Resources;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter.Interfaces;
using System.Diagnostics;
using SoonLearning.Memorize.Data;
using SoonLearning.Assessment.Data.Bank;

namespace SoonLearning.AppCenter.Windows
{
    /// <summary>
    /// Interaction logic for LoadGadgetWindow.xaml
    /// </summary>
    public partial class LoadGadgetWindow : UserControl
    {
        private BackgroundWorker loadGadgetWorker;
        private int totalDllCount = 0;
        private int currentDllIndex = 0;

        public LoadGadgetWindow()
        {
            InitializeComponent();

            this.loadGadgetWorker = new BackgroundWorker();
            this.loadGadgetWorker.WorkerReportsProgress = true;
            this.loadGadgetWorker.DoWork += new DoWorkEventHandler(loadGadgetWorker_DoWork);
            this.loadGadgetWorker.ProgressChanged += new ProgressChangedEventHandler(loadGadgetWorker_ProgressChanged);
            this.loadGadgetWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(loadGadgetWorker_RunWorkerCompleted);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.loadGadgetWorker.RunWorkerAsync();
        }

        private void loadGadgetWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            int index = 0;

            AppDomain exeDomain = AppDomain.CurrentDomain;

            Assembly assembly = Assembly.GetExecutingAssembly();
            string exeAssembly = assembly.FullName;
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetDirectoryName(assembly.Location));
            DirectoryInfo memorizeDataDi = new DirectoryInfo(DataMgr.Instance.MemorizeDataPath);
            DirectoryInfo assessmentDataDi = new DirectoryInfo(DataMgr.Instance.AssessmentDataPath);

            try
            {
                DataMgr.Instance.preLoadApps();
            }
            catch
            {
            }

            try
            {
                AppInstallMgr.Instance.Dispatcher = this.Dispatcher;
                AppInstallMgr.Instance.Init();
            }
            catch
            {
            }

            var newAppQuery = from item in AppInstallMgr.Instance.AppInstallItems
                              where item.State == InstallState.Done
                              select item;

#if _LAUNCH_APP_
            #region Remote Loader

            FileInfo[] appFiles = di.GetFiles("*.dll");

            this.totalDllCount = appFiles.Length;
            this.currentDllIndex = 0;

            foreach (FileInfo fi in appFiles)
            {
                this.currentDllIndex++;

                LocalLoader localLoader = null;
                
                try
                {
                    localLoader = new LocalLoader(System.IO.Path.GetDirectoryName(assembly.Location));
                    localLoader.LoadAssembly(fi.FullName);
                    foreach (string entryName in localLoader.GetSubclasses(typeof(MarshalByRefObject).FullName))
                    {
                        MarshalByRefObject entryObject = localLoader.CreateInstance(entryName,
                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.CreateInstance,
                            new object[] { });

                        if (entryObject is IGadgetEntry)
                        {
                            IGadgetEntry entry = entryObject as IGadgetEntry;
                            if (DataMgr.Instance.GadgetItems.IsAppExist(entry.Id))
                            {
                                worker.ReportProgress(index++, entry.Title);
                                Thread.Sleep(1);
                                break;
                            }

                            string name = string.Empty;
                            string website = string.Empty;
                            string logo = string.Empty;

                            if (entry is IAuthorInfo)
                            {
                                IAuthorInfo authorInfo = entry as IAuthorInfo;
                                name = authorInfo.Name;
                                website = authorInfo.WebSite;
                                logo = authorInfo.Logo;
                            }

                            string thumbnail = System.IO.Path.GetDirectoryName(assembly.Location);
                            thumbnail = System.IO.Path.Combine(thumbnail, @"AppLogos\");
                            thumbnail += entry.Id;
                            thumbnail += System.IO.Path.GetExtension(entry.Thumbnail);

                            if (!File.Exists(thumbnail))
                            {
                                ExtractLogo(entry.Thumbnail, entry.Id, assembly.Location);
                            }

                            AppItem item =
                                (AppItem)exeDomain.CreateInstanceAndUnwrap(
                                    exeAssembly,
                                    typeof(AppItem).FullName,
                                    true,
                                    BindingFlags.Default,
                                    null,
                                    new object[] { entry.Id,
                                                entry.Title, 
                                                entry.Description, 
                                                entry.CreateDate,
                                                thumbnail,
                                                entry.Tag,
                                                entry.SubTag, 
                                                entryName,
                                                name,
                                                website,
                                                logo},
                                    null,
                                    null
                                );

                            item.AppEntryFile = fi.FullName;

                            //  GadgetItem item = new GadgetItem();
                            DataMgr.Instance.GadgetItems.Add(item);

                            worker.ReportProgress(index++, entry.Title);

                            Thread.Sleep(1);

                            break;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (localLoader != null)
                    {
                        localLoader.Unload();
                        localLoader = null;
                    }
                }
            }

            this.currentDllIndex = this.totalDllCount;

            #endregion
#else
             
            //////////////////////////////////////////////////////////
            FileInfo[] appFiles = di.GetFiles("*.dll");
            FileInfo[] memorizeDataFiles = memorizeDataDi.GetFiles("*.mre", SearchOption.AllDirectories);
            FileInfo[] assessmentAppFiles = assessmentDataDi.GetFiles("*.sla", SearchOption.AllDirectories);

            this.totalDllCount = appFiles.Length + memorizeDataFiles.Length + assessmentAppFiles.Length;
            this.currentDllIndex = 0;
            foreach (FileInfo fi in appFiles)
            {
                try
                {
                    this.currentDllIndex++;

                    Assembly gadgetAssembly = Assembly.LoadFrom(fi.FullName);

                    bool found = false;

                    //   Assembly gadgetAssembly = Assembly.LoadFile(fi.FullName);
                    Module[] modules = gadgetAssembly.GetModules();
                    foreach (Module module in modules)
                    {
                        Type[] types = module.GetTypes();
                        foreach (Type type in types)
                        {
                            if (type.IsClass && !type.IsAbstract)
                            {
                                Type entryInterface = type.GetInterface("IGadgetEntry");
                                if (entryInterface != null)
                                {
                                    IGadgetEntry entry = gadgetAssembly.CreateInstance(type.FullName) as IGadgetEntry;
                                    if (DataMgr.Instance.getAppItemById(entry.Id) != null)
                                    {
                                        worker.ReportProgress(index++, entry.Title);
                                        Thread.Sleep(1);
                                        found = true;
                                        break;
                                    }

                                    string name = string.Empty;
                                    string website = string.Empty;
                                    string logo = string.Empty;

                                    if (entry is IAuthorInfo)
                                    {
                                        IAuthorInfo authorInfo = entry as IAuthorInfo;
                                        name = authorInfo.Name;
                                        website = authorInfo.WebSite;
                                        logo = authorInfo.Logo;
                                    }

                                    string thumbnail = entry.Thumbnail;
                                    string thumbnailFile = System.IO.Path.GetDirectoryName(assembly.Location);
                                    thumbnailFile = System.IO.Path.Combine(thumbnail, @"AppLogos\");
                                    thumbnailFile += entry.Id;
                                    thumbnailFile += System.IO.Path.GetExtension(entry.Thumbnail);

                                    if (File.Exists(thumbnailFile))
                                    {
                                        try
                                        {
                                            File.Delete(thumbnailFile);
                                        }
                                        catch
                                        {
                                            
                                        }
                                    }

                                 //   if (entry.Tag == 200 &&
                                 //       entry.SubTag == 202)
                                 //       thumbnail = @"pack://application:,,,/Resources/Images/GradeMath.png";

                                    AppItem item = null;

                                    if (entry.Tag == 200 &&
                                        entry.SubTag == 207)
                                    {
                                        item = new MathFastAppItem(entry.Id,
                                                entry.Title,
                                                entry.Description,
                                                entry.CreateDate,
                                                thumbnail,
                                                entry.Tag,
                                                entry.SubTag,
                                                type.FullName,
                                                name,
                                                website,
                                                logo);
                                    }
                                    else
                                    {
                                        item = new DllAppItem(entry.Id,
                                                entry.Title,
                                                entry.Description,
                                                entry.CreateDate,
                                                thumbnail,
                                                entry.Tag,
                                                entry.SubTag,
                                                type.FullName,
                                                name,
                                                website,
                                                logo);
                                    }

                                    var matchAppId = from app in newAppQuery
                                                     where app.AppItem.UniqueId == entry.Id
                                                     select app.AppItem.Id;

                                    if (matchAppId.Count() > 0)
                                    {
                                        item.IsNew = true;
                                    }

                                    item.Entry = entry;
                                    item.AppEntryFile = fi.FullName;

                                    //  GadgetItem item = new GadgetItem();
                                    DataMgr.Instance.addAppItem(item);

                                    worker.ReportProgress(index++, entry.Title);

                                    Thread.Sleep(1);

                                    found = true;

                                    break;
                                } // end if
                            } // end if
                        } // end foreach

                        if (found)
                            break;
                    }
                }
                catch
                {
                }
            }

            foreach (FileInfo fi in memorizeDataFiles)
            {
                try
                {
                    MemorizeEntry entry = MemorizeEntry.Load(fi.FullName);
                    MemorizeAppItem item = new MemorizeAppItem();
                    item.Id = entry.Id;
                    item.Title = entry.Title;
                    item.Description = entry.Description;
                    item.Thumbnail = entry.Thumbnail;
                    item.CreateDate = entry.CreateDate;
                    item.CreatorName = entry.Creator;
                    item.CreatorLogo = entry.CreatorLogo;
                    item.CreatorWebSite = entry.CreatorWebsite;
                    item.MemorizeEntry = entry;
                    if (entry.SubType == 0)
                        item.SubType = 10299;
                    else
                        item.SubType = entry.SubType;
                    item.Type = 102;
                    item.AppEntryFile = fi.FullName;

                    DataMgr.Instance.addAppItem(item);

                    worker.ReportProgress(index++, entry.Title);

                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }

            foreach (FileInfo fi in assessmentAppFiles)
            {
                try
                {
                    AssessmentApp app = SerializerHelper<AssessmentApp>.XmlDeserialize(fi.FullName);

                    AssessmentAppItem item = new AssessmentAppItem();
                    item.Id = app.Id;
                    item.Title = app.Name;
                    item.Description = app.Description;
                    item.Thumbnail = app.Thumbnail;
                    item.Type = 200;// app.Type;
                    item.SubType = 201;// app.SubType;
                    item.CreateDate = app.CreateDate.ToLocalTime();
                    item.AppEntryFile = fi.FullName;
                    item.CreatorName = app.Creator;
                    item.CreatorWebSite = app.CreatorWebsite;
                    item.CreatorLogo = app.CreatorLogo;

                    DataMgr.Instance.addAppItem(item);

                    worker.ReportProgress(index++, item.Title);

                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }

            this.currentDllIndex = this.totalDllCount;
#endif

            try
            {
                DataMgr.Instance.MruItems.Load();
            }
            catch
            {
            }
        }

        private static string ExtractLogo(string logo, string id, string location)
        {
            try
            {
                string thumbnail = System.IO.Path.GetDirectoryName(location);
                thumbnail = System.IO.Path.Combine(thumbnail, @"AppLogos\");
                if (!Directory.Exists(thumbnail))
                    Directory.CreateDirectory(thumbnail);
                thumbnail += id;
                thumbnail += System.IO.Path.GetExtension(logo);

                int index = logo.LastIndexOf(';');
                string logoFile = System.IO.Path.GetFileNameWithoutExtension(location) + logo.Substring(index + ";component".Length, logo.Length - index - ";component".Length);
                logoFile = logoFile.Replace('/', '.');
                // /Gadget.Math.Basic;component/Resources/decimal.png
                string logoName = System.IO.Path.GetFileName(logo);

                StreamResourceInfo sri = Application.GetResourceStream(new Uri(logo));
                Stream stream = sri.Stream;

                if (stream != null)
                {
                    FileStream fs = null;
                    try
                    {
                        fs = File.OpenWrite(thumbnail);
                        while (true)
                        {
                            byte[] data = new byte[1024];
                            int len = stream.Read(data, 0, 1024);
                            fs.Write(data, 0, len);
                            if (len < 1024)
                                break;
                        }
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Dispose();
                            fs = null;
                        }
                    }
                }

                if (stream != null)
                    stream.Close();

                return thumbnail;
            }
            catch
            {
            }

            return string.Empty;
        }

        private void loadGadgetWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.loadIndexTextBox.Text = string.Format(Strings.LoadingIndex, e.ProgressPercentage + 1, e.UserState as string);

            this.loadProgressBar.Value = this.currentDllIndex * 100 / this.totalDllCount;
        }

        private void loadGadgetWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.loadIndexTextBox.Text = string.Format(Strings.LoadingDone, DataMgr.Instance.AppCount);
            this.enterButton.IsEnabled = true;
        //    this.enableSpeechRecognizerButton.IsEnabled = true;
            this.enterButton.Focus();

            this.loadProgressBar.Value = 100;
        }

        private void enterButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.StartToUse();
        }

        private void enableSpeechRecognizerButton_Click(object sender, RoutedEventArgs e)
        {
            UIStyleSetting.Instance.SpeechRecognizer = true;
            SpeechHelper.Instance.RecognizerEnabled = true;
            SpeechHelper.Instance.EnableRecognizer();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
