using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SoonLearning.Assessment.Data.Bank;
using System.Collections.ObjectModel;
using SoonLearning.Assessment.Data;
using Microsoft.Win32;
using System.Windows;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using SevenZip;
using System.Windows.Documents;
using HTMLConverter;
using System.Xml;
using System.Net;
using SoonLearning.AppManagementTool;
using System.Diagnostics;

namespace SoonLearning.TeachAppMaker.Data
{
    public class ProjectMgr
    {
        private static ProjectMgr instance;

        public static ProjectMgr Instance
        {
            get
            {
                Interlocked.CompareExchange<ProjectMgr>(ref instance, new ProjectMgr(), null);
                return instance;
            }
        }

        private ProjectMgr()
        {
            this.initTreeNodeData();

            this.loadApps();
        }

        public event Action NewProjectCreated;
        public event Action ProjectOpened;
        public event Action KnowledgeChanged;

        private AssessmentApp app;
        private bool changed;
        private string projectFile;
        private Question selectedQuestion;

        private string dataPath;

        private object appCollectionLocker = new object();
        private ObservableCollection<AppDescription> appCollection = new ObservableCollection<AppDescription>();

        private ObservableCollection<ProjectTreeNode> projectTreeNodeCollection = new ObservableCollection<ProjectTreeNode>();

        public AssessmentApp App
        {
            get { return this.app; }
        }

        public ObservableCollection<ProjectTreeNode> ProjectTreeNodes
        {
            get { return this.projectTreeNodeCollection; }
        }

        internal string ProjectFile
        {
            get { return this.projectFile; }
            set { this.projectFile = value; }
        }

        internal Question SelectedQuestion
        {
            get { return this.selectedQuestion; }
            set { this.selectedQuestion = value; }
        }

        internal bool Changed
        {
            set { this.changed = true; }
        }

        public ObservableCollection<AppDescription> AppItems
        {
            get { return this.appCollection; }
        }

        internal string DataPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.dataPath))
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    this.dataPath = Path.GetDirectoryName(assembly.Location);
                    this.dataPath = Path.Combine(dataPath, "Data");
                }

                if (!Directory.Exists(this.dataPath))
                    Directory.CreateDirectory(this.dataPath);

                return this.dataPath;
            }
        }

        internal string ImagePath
        {
            get
            {
                string imagePath = System.IO.Path.Combine(this.DataPath, this.App.Id);
                if (!Directory.Exists(imagePath))
                    Directory.CreateDirectory(imagePath);
                return imagePath;
            }
        }

        private void initTreeNodeData()
        {
            projectTreeNodeCollection.Add(new KnowledgeTreeNode() { Name = "知识点", Image = "pack://application:,,,/Resources/Images/question.png" });
            projectTreeNodeCollection.Add(new QuestionBankTreeNode() { Name = "题库", Image = "pack://application:,,,/Resources/Images/QuestionBank.png"} );
        }

        internal void NewProject(string title, string description, string thumbnail)
        {
            if (this.changed)
            {
                MessageBoxResult ret = MessageBox.Show("当前项目已经被修改，再打开新项目前是否要保存当前项目的修改？", "速学应用创建工具", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    this.SaveProject();
                }
                else if (ret == MessageBoxResult.No)
                {
                }
                else if (ret == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            this.app = new AssessmentApp();
            this.app.Name = title;
            this.app.Description = description;
            this.app.Thumbnail = thumbnail;
            this.ProjectFile = string.Empty;

            Action temp = this.NewProjectCreated;
            if (temp != null)
            {
                temp();
            }
        }

        internal bool SaveProject()
        {
            if (this.App == null)
                return true;

            try
            {
                if (string.IsNullOrEmpty(this.ProjectFile))
                {
                    this.ProjectFile = Path.Combine(this.DataPath, this.App.Id + ".sla");
                }

                if (File.Exists(this.ProjectFile))
                    File.Delete(this.ProjectFile);

                SerializerHelper<AssessmentApp>.XmlSerialize(this.ProjectFile, this.App);
                this.changed = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("保存应用{0}失败。", this.App.Name));
                return false;
            }

            try
            {
                var query = from temp in this.appCollection
                            where this.App.Id == temp.Id
                            select temp;
                if (query.Count() == 0)
                {
                    lock (this.appCollectionLocker)
                    {
                        this.appCollection.Add(new AppDescription()
                        {
                            Id = this.App.Id,
                            Title = this.App.Name,
                            Description = this.App.Description,
                            CreateDate = this.App.CreateDate,
                            File = this.ProjectFile
                        });
                    }
                }
            }
            catch
            {
            }

            return true;
        }

        internal void OpenProject(string file)
        {
            if (this.changed)
            {
                MessageBoxResult ret = MessageBox.Show("当前项目已经被修改，再打开新项目前是否要保存当前项目的修改？", "速学应用创建工具", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    this.SaveProject();
                }
                else if (ret == MessageBoxResult.No)
                {
                }
                else if (ret == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            this.app = SerializerHelper<AssessmentApp>.XmlDeserialize(file);
            this.ProjectFile = file;

            this.InvokeProjectOpenedEvent();
        }

        internal bool CloseProject()
        {
            if (this.changed)
            {
                MessageBoxResult ret = MessageBox.Show("当前项目已经被修改，再打开新项目前是否要保存当前项目的修改？", "速学应用创建工具", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    this.SaveProject();
                }
                else if (ret == MessageBoxResult.No)
                {
                }
                else if (ret == MessageBoxResult.Cancel)
                {
                    return true;
                }
            }

            return false;
        }

        internal string CreateAppPackage()
        {
            if (!this.SaveProject())
                return string.Empty;

            List<string> fileList = new List<string>();

            foreach (var file in GetImageFiles())
                fileList.Add(file);

            // Install to local
            RegistryKey subKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\上海速学信息科技有限公司(SoonLearning.com)\速学应用平台");
            if (subKey != null)
            {
                string appPath = subKey.GetValue("Path") as string;
                if (!string.IsNullOrEmpty(appPath))
                {
                    string dataPath = System.IO.Path.Combine(appPath, @"data\Assessment");
                    if (!Directory.Exists(dataPath))
                        Directory.CreateDirectory(dataPath);

                    string targetPath = System.IO.Path.Combine(dataPath, this.App.Id);
                    if (!Directory.Exists(targetPath))
                        Directory.CreateDirectory(targetPath);

                    string targetImagePath = System.IO.Path.Combine(targetPath, "Images");
                    if (!Directory.Exists(targetImagePath))
                        Directory.CreateDirectory(targetImagePath);

                    foreach (string imageFile in fileList)
                    {
                        try
                        {
                            File.Copy(imageFile, Path.Combine(targetImagePath, Path.GetFileName(imageFile)));
                        }
                        catch
                        {
                        }
                    }

                    try
                    {
                        File.Copy(this.ProjectFile, Path.Combine(targetPath, Path.GetFileName(this.ProjectFile)));
                    }
                    catch
                    {
                    }
                }
            }


       //     fileList.Add(this.ProjectFile);

       //     string[] imageFiles = Directory.GetFiles(this.ImagePath, "*.*");
       //     fileList.AddRange(imageFiles);

            string folder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (Helper.Is64BitOS())
                SevenZipCompressor.SetLibraryPath(System.IO.Path.Combine(folder, "ThirdParty\\7z\\7z64.dll"));
            else
                SevenZipCompressor.SetLibraryPath(System.IO.Path.Combine(folder, "ThirdParty\\7z\\7z.dll"));

            SevenZipCompressor cmp;

            try
            {
                cmp = new SevenZipCompressor();
                cmp.ArchiveFormat = OutArchiveFormat.Zip;
                cmp.CompressionLevel = CompressionLevel.Normal;
                cmp.CompressionMode = CompressionMode.Create;
                cmp.FastCompression = false;

                cmp.TempFolderPath = this.DataPath;
                string archiveFile = System.IO.Path.Combine(this.DataPath, this.App.Id + ".zip");
                if (System.IO.File.Exists(archiveFile))
                    System.IO.File.Delete(archiveFile);

                if (fileList.Count > 0)
                {
                    cmp.CompressFilesEncrypted(archiveFile, getTempPath().Length, "$L&%$D123@3$5^7*", fileList.ToArray());
                    cmp.CompressionMode = CompressionMode.Append;
                }

                cmp.CompressFilesEncrypted(archiveFile, Path.GetDirectoryName(this.ProjectFile).Length, "$L&%$D123@3$5^7*", new string[] {this.ProjectFile});

                foreach (string file in fileList)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                    }
                }

                string slpFile = Path.Combine(ProjectMgr.Instance.DataPath, this.App.Id + ".slp");
                PackageCreator.CreateTeachAppPackage(this.App.Id,
                    this.App.Name,
                    this.App.Description,
                    this.App.Thumbnail,
                    this.App.Type,
                    this.App.SubType,
                    this.App.CreateDate,
                    this.App.Creator,
                    this.App.CreatorLogo,
                    this.App.CreatorWebsite,
                    archiveFile,
                    slpFile);

                return slpFile;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            return string.Empty;
        }

        private IEnumerable<string> GetImageFiles()
        {
            foreach (var img in GetImageFiles(this.app.Knowledge.Content.Content))
                yield return img;

            foreach (var question in this.App.Items)
            {
                foreach (var src in GetQuestionImageFiles(question))
                    yield return src;
            }
        }

        private IEnumerable<string> GetQuestionImageFiles(Question question)
        {
            foreach (string src in GetImageFiles(question.Content.Content))
                yield return src;

            foreach (string src in GetImageFiles(question.Solution.Content))
                yield return src;

            if (question is SelectableQuestion)
            {
                SelectableQuestion selectableQuestion = question as SelectableQuestion;
                foreach (QuestionOption option in selectableQuestion.QuestionOptionCollection)
                {
                    foreach (string optionImageSrc in GetImageFiles(option.OptionContent.Content))
                        yield return optionImageSrc;
                }
            }
            else if (question is FIBQuestion)
            {
                FIBQuestion fibQuestion = question as FIBQuestion;
                foreach (QuestionBlank blank in fibQuestion.QuestionBlankCollection)
                {
                    foreach (QuestionContent refAnswer in blank.ReferenceAnswerList)
                    {
                        foreach (string blankImageSrc in GetImageFiles(refAnswer.Content))
                        {
                            yield return blankImageSrc;
                        }
                    }
                }
            }
            else if (question is MAQuestion)
            {
                MAQuestion maQuestion = question as MAQuestion;
                foreach (MAQuestionOption option in maQuestion.OptionList)
                {
                    foreach (var optionImageSrc in GetImageFiles(option.LeftContent.Content))
                        yield return optionImageSrc;

                    foreach (var optionImageSrc in GetImageFiles(option.RightContent.Content))
                        yield return optionImageSrc;
                }
            }
            else if (question is ESQuestion)
            {
                ESQuestion esQuestion = question as ESQuestion;
                foreach (var refAnswerImageSrc in GetImageFiles(esQuestion.ReferenceAnswer.Content))
                    yield return refAnswerImageSrc;
            }
            else if (question is CPQuestion)
            {
                CPQuestion cpQuestion = question as CPQuestion;
                foreach (var subQuestion in cpQuestion.SubQuestions)
                {
                    foreach (var subQuestionImageSrc in GetQuestionImageFiles(subQuestion as Question))
                        yield return subQuestionImageSrc;
                }
            }
        }

        private List<string> GetImageFiles(string xaml)
        {
            List<string> imageList = new List<string>();
            if (string.IsNullOrEmpty(xaml))
                return imageList;

            System.Xml.XmlDocument doc = new XmlDocument();
            doc.Load(new XmlTextReader(new StringReader(xaml)));
            XmlNodeList imageNodeList = doc.GetElementsByTagName("Image");
            foreach (XmlNode imageNode in imageNodeList)
            {
                XmlAttribute sourceAttribute = imageNode.Attributes["Source"];
                WebClient webClient = new WebClient();
                string tempFile = getTempFile(sourceAttribute.Value);
                webClient.DownloadFile(sourceAttribute.Value, getTempFile(sourceAttribute.Value));
                webClient.Dispose();
                imageList.Add(tempFile);
            }

            return imageList;
        }

        private string getTempFile(string file)
        {
            string tempPath = Path.Combine(getTempPath(), "Images");
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            string fileName = System.IO.Path.GetFileName(file);
            return Path.Combine(tempPath, fileName);
        }

        private string getTempPath()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string tempPath = Path.Combine(Path.GetDirectoryName(assembly.Location), "TempFiles");
            return tempPath;
        }

        internal void DeleteSelectedQuestion()
        {
            if (MessageBox.Show("您确定要删除选中的试题吗？", "速学应用创建工具", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.App.Items.Remove(this.SelectedQuestion);
            }        
        }

        internal Question CreateQuestion(QuestionType type)
        {
            Question question = null;
            switch (type)
            {
                case QuestionType.MultiChoice:
                    {
                        MCQuestion mcQuestion = new MCQuestion();
                        for (int i = 0; i < 4; i++)
                        {
                            QuestionOption option = new QuestionOption();
                            option.Index = i;
                            mcQuestion.QuestionOptionCollection.Add(option);
                        }
                        question = mcQuestion;
                    }
                    break;
                case QuestionType.MultiResponse:
                    {
                        MRQuestion mrQuestion = new MRQuestion();
                        for (int i = 0; i < 4; i++)
                        {
                            QuestionOption option = new QuestionOption();
                            option.Index = i;
                            mrQuestion.QuestionOptionCollection.Add(option);
                        }
                        question = mrQuestion;
                    }
                    break;
                case QuestionType.Essay:
                    {
                        ESQuestion esQuestion = new ESQuestion();
                        question = esQuestion;
                    }
                    break;
                case QuestionType.TrueFalse:
                    {
                        TFQuestion tfQuestion = new TFQuestion();
                        QuestionOption optionT = new QuestionOption();
                        optionT.Index = 0;
                        QuestionOption optionF = new QuestionOption();
                        optionF.Index = 1;
                        tfQuestion.QuestionOptionCollection.Add(optionT);
                        tfQuestion.QuestionOptionCollection.Add(optionF);
                        tfQuestion.RandomOption = true;

                        question = tfQuestion;
                    }
                    break;
                case QuestionType.Match:
                    {
                        MAQuestion maQuestion = new MAQuestion();
                        for (int i = 0; i < 4; i++)
                        {
                            MAQuestionOption option = new MAQuestionOption();
                            option.Index = i;
                            maQuestion.OptionList.Add(option);
                        }
                        question = maQuestion;
                    }
                    break;
                case QuestionType.FillInBlank:
                    {
                        FIBQuestion fibQuestion = new FIBQuestion();
                        question = fibQuestion;
                    }
                    break;
                case QuestionType.Composite:
                    {
                        CPQuestion cpQuestion = new CPQuestion();
                        question = cpQuestion;
                    }
                    break;
            }

            question.DifficultyLevel = 3;
            question.CreateTime = DateTime.Now.ToUniversalTime();
            question.ModifyTime = DateTime.Now.ToUniversalTime();

            return question;
        }

        internal QuestionOption CreateOption(int index)
        {
            QuestionOption option = new QuestionOption();
            option.Index = index;
            return option;
        }

        internal MAQuestionOption CreateMAOption(int index)
        {
            MAQuestionOption option = new MAQuestionOption();
            option.Index = index;
            return option;
        }

        #region Invoke Events

        internal void InvokeKnowledgeChangedEvent()
        {
            this.Changed = true;
            Action temp = this.KnowledgeChanged;
            if (temp != null)
                temp();
        }

        internal void InvokeProjectOpenedEvent()
        {
            Action temp = this.ProjectOpened;
            if (temp != null)
                temp();
        }

        #endregion

        #region Load Apps

        private void loadApps()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                string[] files = Directory.GetFiles(this.DataPath, "*.sla");
                foreach (var file in files)
                {
                    try
                    {
                        AssessmentApp app = SerializerHelper<AssessmentApp>.XmlDeserialize(file);
                        worker.ReportProgress(0, new AppDescription()
                        {
                            Id = app.Id,
                            Title = app.Name,
                            Description = app.Description,
                            CreateDate = app.CreateDate,
                            File = file
                        });
                    }
                    catch
                    {
                    }

                    Thread.Sleep(0);
                }

                Thread.Sleep(0);
            };
            worker.ProgressChanged += (s, e) =>
            {
                if (e.UserState != null)
                {
                    lock (this.appCollectionLocker)
                    {
                        this.appCollection.Add(e.UserState as AppDescription);
                    }
                }
            };
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
        }

        #endregion
    }
}
