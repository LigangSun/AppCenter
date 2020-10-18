using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;

namespace SoonLearning.Memorize.Data
{
    public class MemorizeEntry : NotifyPropertyChanged
    {
        private string id = string.Empty;
        private string title = string.Empty;
        private string description = string.Empty;
        private string version = "1.0.0.0";
        private DateTime createDate = DateTime.Now;
        private DateTime modifyDate = DateTime.Now;
        private string thumbnail = string.Empty;
        private string creator = string.Empty;
        private string creatorLogo = string.Empty;
        private string creatorWebsite = string.Empty;

        private int subType = 10299;

        private ObservableCollection<MemorizeItem> memorizeItemList = new ObservableCollection<MemorizeItem>();
        private MemorizeBackgroundObjectList backgroundImageList = new MemorizeBackgroundObjectList();
        private MemorizeBackgroundObjectList backgroundMusicList = new MemorizeBackgroundObjectList();
        private string memorizeItemBackground = string.Empty;
        private string itemClickMusic = string.Empty;
        private MemorizeFeedback stageFeedback = new MemorizeFeedback();
        private MemorizeFeedback itemFeedback = new MemorizeFeedback();
        private bool pkMode = false;
        private ObservableCollection<MemorizeStage> memorizeStageList = new ObservableCollection<MemorizeStage>();

        private bool randomBackgroundImage;
        private bool randomBackgroundMusic;

        private string mreFile = string.Empty;

        public string Id
        {
            get { return this.id; }
            set 
            {
                this.id = value;
            }
        }

        public string Title
        {
            get { return this.title; }
            set 
            {
                this.title = value;
                base.OnPropertyChanged("Title");
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

        public string Version
        {
            get { return this.version; }
            set 
            { 
                this.version = value;
                base.OnPropertyChanged("Version");
            }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
            set { this.createDate = value; }
        }

        public DateTime ModifyDate
        {
            get { return this.modifyDate; }
            set { this.modifyDate = value; }
        }

        public string Thumbnail
        {
            get
            {
                return this.thumbnail; 
            }
            set
            { 
                this.thumbnail = value;
                base.OnPropertyChanged("Thumbnail");
            }
        }

        public string Creator
        {
            get { return this.creator; }
            set 
            { 
                this.creator = value;
                base.OnPropertyChanged("Creator");
            }
        }

        public string CreatorLogo
        {
            get
            {
                return this.creatorLogo;
            }
            set
            {
                this.creatorLogo = value;
                this.OnPropertyChanged("CreatorLogo");
            }
        }

        public string CreatorWebsite
        {
            get { return this.creatorWebsite; }
            set 
            { 
                this.creatorWebsite = value;
                base.OnPropertyChanged("CreatorWebsite");
            }
        }

        public int SubType
        {
            get { return this.subType; }
            set { this.subType = value; }
        }

        public ObservableCollection<MemorizeItem> Items
        {
            get { return this.memorizeItemList; }
        }

        public MemorizeBackgroundObjectList BackgroundImages
        {
            get { return this.backgroundImageList; }
        }

        public MemorizeBackgroundObjectList BackgroundMusics
        {
            get { return this.backgroundMusicList; }
        }

        public string MemorizeItemBackground
        {
            get 
            {
                return memorizeItemBackground;
            }
            set 
            { 
                this.memorizeItemBackground = value;
                base.OnPropertyChanged("MemorizeItemBackground");
            }
        }

        public string ItemClickMusic
        {
            get 
            {
                return this.itemClickMusic; 
            }
            set
            { 
                this.itemClickMusic = value;
                base.OnPropertyChanged("ItemClickMusic");
            }
        }

        public MemorizeFeedback StageFeedback
        {
            get { return this.stageFeedback; }
            set { this.stageFeedback = value; }
        }

        public MemorizeFeedback ItemFeedback
        {
            get { return this.itemFeedback; }
            set { this.itemFeedback = value; }
        }

        public bool IsPkMode
        {
            get { return this.pkMode; }
            set
            {
                this.pkMode = value;
                base.OnPropertyChanged("IsPkMode");
            }
        }

        public ObservableCollection<MemorizeStage> Stages
        {
            get { return this.memorizeStageList; }
        }

        public bool RandomBackgroundImage
        {
            get 
            {
                return this.randomBackgroundImage; 
            }
            set
            {
                this.randomBackgroundImage = value;
                base.OnPropertyChanged("RandomBackgroundImage");
            }
        }

        public bool RandomBackgroundMusic
        {
            get { return this.randomBackgroundMusic; }
            set
            {
                this.randomBackgroundMusic = value;
                base.OnPropertyChanged("RandomBackgroundMusic");
            }
        }

        public MemorizeEntry()
        {
            this.id = Guid.NewGuid().ToString("N");
        }

        public void Save(string file)
        {
            string path = System.IO.Path.GetDirectoryName(file);
            this.saveData(path);

            File.Delete(file);
            Helper<MemorizeEntry>.XmlSerialize(file, this);
            this.mreFile = file;
        }

        public static MemorizeEntry Load(string file)
        {
            string path = Path.GetDirectoryName(file);
            string backgroundImagePath = System.IO.Path.Combine(path, "BackgroundImage");
            string backgroundMusicPath = Path.Combine(path, "BackgroundMusic");
            string memorizeItemPath = Path.Combine(path, "MemorizeItems");
            string feedbackPath = Path.Combine(path, "Feedback");
            staticCheckPath(backgroundImagePath);
            staticCheckPath(backgroundMusicPath);
            staticCheckPath(memorizeItemPath);
            staticCheckPath(feedbackPath);

            var entry = Helper<MemorizeEntry>.XmlDeserialize(file);

            for (int i = 0; i < entry.BackgroundImages.Count; i++)
            {
                entry.BackgroundImages[i] = getFileFullPath(backgroundImagePath, entry.BackgroundImages[i]);
            }

            for (int i = 0; i < entry.BackgroundMusics.Count; i++)
            {
                entry.BackgroundMusics[i] = getFileFullPath(backgroundMusicPath, entry.BackgroundMusics[i]);
            }

            entry.MemorizeItemBackground = getFileFullPath(memorizeItemPath, entry.MemorizeItemBackground);
            entry.ItemClickMusic = getFileFullPath(memorizeItemPath, entry.ItemClickMusic);
            entry.Thumbnail = getFileFullPath(memorizeItemPath, entry.Thumbnail);
            entry.CreatorLogo = getFileFullPath(memorizeItemPath, entry.CreatorLogo);

            if (entry.ItemFeedback != null)
                entry.ItemFeedback.getFullPath(feedbackPath);

            if (entry.StageFeedback != null)
                entry.StageFeedback.getFullPath(feedbackPath);

            foreach (MemorizeItem item in entry.Items)
            {
                item.ObjectA.getFullPath(memorizeItemPath);
                item.ObjectB.getFullPath(memorizeItemPath);
            }

            foreach (MemorizeStage stage in entry.Stages)
            {
                stage.getFullPath(backgroundImagePath);
            }

            entry.mreFile = file;

            return entry;
        }

        private void saveData(string path)
        {
            string backgroundImagePath = System.IO.Path.Combine(path, "BackgroundImage");
            string backgroundMusicPath = Path.Combine(path, "BackgroundMusic");
            string memorizeItemPath = Path.Combine(path, "MemorizeItems");
            string feedbackPath = Path.Combine(path, "Feedback");
            staticCheckPath(backgroundImagePath);
            staticCheckPath(backgroundMusicPath);
            staticCheckPath(memorizeItemPath);
            staticCheckPath(feedbackPath);

            List<string> tempFileList = new List<string>();
            foreach (string file in this.backgroundImageList)
            {
                tempFileList.Add(copyFile(file, backgroundImagePath));
            }

            this.backgroundImageList.Clear();
            foreach (var temp in tempFileList)
                this.backgroundImageList.Add(temp);

            foreach (string file in this.backgroundMusicList)
            {
                tempFileList.Add(copyFile(file, backgroundMusicPath));
            }

            this.backgroundMusicList.Clear();
            foreach (var temp in tempFileList)
                this.backgroundMusicList.Add(temp);

            this.MemorizeItemBackground = copyFile(this.MemorizeItemBackground, memorizeItemPath);
            this.ItemClickMusic = copyFile(this.itemClickMusic, memorizeItemPath);
            this.Thumbnail = copyFile(this.Thumbnail, memorizeItemPath);
            this.CreatorLogo = copyFile(this.CreatorLogo, memorizeItemPath);

            if (this.itemFeedback != null)
                this.itemFeedback.Save(feedbackPath);

            if (this.stageFeedback != null)
                this.stageFeedback.Save(feedbackPath);

            foreach (MemorizeItem item in this.Items)
            {
                item.ObjectA.Save(memorizeItemPath);
                item.ObjectB.Save(memorizeItemPath);
            }
        }

        private static void staticCheckPath(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        internal static string getFileFullPath(string path, string file)
        {
            if (string.IsNullOrEmpty(file))
                return string.Empty;

            staticCheckPath(path);
            return Path.Combine(path, Path.GetFileName(file));
        }

        internal static string copyFile(string srcFile, string destPath)
        {
            try
            {
                if (!File.Exists(srcFile))
                    return string.Empty;

                string destFile = Path.Combine(destPath, Path.GetFileName(srcFile));
                if (srcFile.Equals(destFile, StringComparison.OrdinalIgnoreCase))
                    return Path.GetFileName(destFile);
                else
                {
                    destFile = createUniqueFileName(destPath, Path.GetFileNameWithoutExtension(srcFile), Path.GetExtension(srcFile), 0);
                }

                File.Copy(srcFile, destFile, true);
                return destFile;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            return string.Empty;
        }

        private static string createUniqueFileName(string destPath, string name, string ext, int index = 0)
        {
            string fullName = Path.Combine(destPath, string.Format("{0}_{1}", name, index) + ext);
            if (!File.Exists(fullName))
            {
                return fullName;
            }

            return createUniqueFileName(destPath, name, ext, index + 1);
        }
    }
}
