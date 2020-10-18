using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Memorize.Data;
using System.Reflection;
using SoonLearning.AppCenter.Controls;

namespace SoonLearning.Memorize.UI
{
    public enum ChanllengeMode
    {
        SinglePlayer,
        TwoPlayer,
        VsPC
    }

    internal class MemorizeDataMgr
    {
        #region Instance

        private static MemorizeDataMgr instance;

        public static MemorizeDataMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MemorizeDataMgr();
                }

                return instance;
            }
        }

        private MemorizeDataMgr()
        {
            this.exclusiveStageList.AddRange(new int[] { 2, 7, 11, 13, 17, 19 });
        }

        #endregion

        #region Members

        private MemorizeEntry memorizeEntry;

        private int currentStage = 0;
        private int currentBackgroundMusic = 0;
        private int currentBackgroundImage = 0;
        private string userId = "0";

        private List<int> exclusiveStageList = new List<int>();

        private List<MemorizeObject> memorizeItemList = new List<MemorizeObject>();

        private TimingMode currentTimingMode = TimingMode.Count;
        private int usedTime; // In Second

        internal ChanllengeMode CurrentChanllengeMode
        {
            get;
            set;
        }

        internal string PlayerAName
        {
            get;
            set;
        }

        internal string PlayerBName
        {
            get;
            set;
        }

        private MemorizeStage ActiveStage
        {
            get { return this.memorizeEntry.Stages[this.currentStage]; }
        }

        #endregion

        #region Properties

        internal MemorizeEntry Entry
        {
            get { return this.memorizeEntry; }
            set 
            { 
                this.memorizeEntry = value;
                this.currentStage = 0;
                this.memorizeItemList.Clear();
                this.currentBackgroundMusic = 0;
                this.currentBackgroundImage = 0;

                this.prepareStages();
            }
        }

        internal int CurrentStage
        {
            get { return this.currentStage; }
        }

        internal bool IsLastStage
        {
            get { return this.CurrentStage == this.Entry.Stages.Count-1; }
        }

        internal string CurrentBackgroundMusic
        {
            get
            {
                if (this.memorizeEntry.BackgroundMusics.Count == 0)
                {
                //    Assembly assembly = Assembly.GetEntryAssembly();
                //    return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\Memorize\DefaultData\default_background_music.mp3");
                    return string.Empty;
                }

                int index = this.currentBackgroundMusic;
                if (this.memorizeEntry.RandomBackgroundMusic)
                {
                    Random rand = new Random();
                    index = rand.Next(this.memorizeEntry.BackgroundMusics.Count);
                }

                if (!this.memorizeEntry.RandomBackgroundMusic)
                {
                    this.currentBackgroundMusic++;
                    if (this.currentBackgroundMusic == this.memorizeEntry.BackgroundMusics.Count)
                        this.currentBackgroundMusic = 0;
                }

                return this.memorizeEntry.BackgroundMusics[index];
            }
        }

        internal string CurrentBackgroundImage
        {
            get
            {
                if (this.memorizeEntry == null || this.memorizeEntry.BackgroundImages.Count == 0)
                {
                    return @"pack://application:,,,/SoonLearning.Memorize.UI;component/Resources/default_background_image.jpg";
                }

                if (!string.IsNullOrEmpty(this.ActiveStage.BackgroundImage) &&
                    System.IO.File.Exists(this.ActiveStage.BackgroundImage))
                {
                    return this.ActiveStage.BackgroundImage;
                }

                int index = this.currentBackgroundImage;
                if (this.memorizeEntry.RandomBackgroundImage)
                {
                    Random rand = new Random();
                    index = rand.Next(this.memorizeEntry.BackgroundImages.Count);
                }

                if (!this.memorizeEntry.RandomBackgroundImage)
                {
                    this.currentBackgroundImage++;
                    if (this.currentBackgroundImage == this.memorizeEntry.BackgroundImages.Count)
                        this.currentBackgroundImage = 0;
                }

                return this.memorizeEntry.BackgroundImages[index];
            }
        }

        public string MemorizeItemBackgroundImage
        {
            get
            {
                if (this.memorizeEntry == null || string.IsNullOrEmpty(this.memorizeEntry.MemorizeItemBackground))
                {
                    return @"pack://application:,,,/SoonLearning.Memorize.UI;component/Resources/Icon_Question.png";
                }

                return this.memorizeEntry.MemorizeItemBackground;
            }
        }

        internal string ItemClickSound
        {
            get
            {
                if (this.memorizeEntry == null || string.IsNullOrEmpty(this.memorizeEntry.ItemClickMusic))
                {
                    Assembly assembly = Assembly.GetEntryAssembly();
                    return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\Memorize\DefaultData\default_ItemClick_Sound.mp3");
                }

                return this.memorizeEntry.ItemClickMusic;
            }
        }

        internal string OkSound
        {
            get
            {
                if (this.memorizeEntry == null || string.IsNullOrEmpty(this.memorizeEntry.ItemClickMusic))
                {
                    Assembly assembly = Assembly.GetEntryAssembly();
                    return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\Memorize\DefaultData\ok.mp3");
                }

                return this.memorizeEntry.ItemClickMusic;
            }
        }

        internal string FailSound
        {
            get
            {
                if (this.memorizeEntry == null || string.IsNullOrEmpty(this.memorizeEntry.ItemClickMusic))
                {
                    Assembly assembly = Assembly.GetEntryAssembly();
                    return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\Memorize\DefaultData\fail.mp3");
                }

                return this.memorizeEntry.ItemClickMusic;
            }
        }

        internal List<MemorizeObject> Items
        {
            get
            {
                if (this.memorizeItemList.Count == 0)
                    this.prepareMemorizeItemList();

                return this.memorizeItemList; 
            }
        }

        internal TimingMode CurrentTimingMode
        {
            get { return this.currentTimingMode; }
            set { this.currentTimingMode = value; }
        }

        internal int UsedTime
        {
            get { return this.usedTime; }
            set { this.usedTime = value; }
        }

        internal string UserId
        {
            get { return this.userId; }
            set { this.userId = value; }
        }

        #endregion

        #region Operation Methods

        internal void Restart()
        {
            this.currentStage = 0;
            this.memorizeItemList.Clear();
        }

        internal void MoveToNextStage()
        {
            if (this.currentStage == this.memorizeEntry.Stages.Count - 1)
            {
                this.currentStage = -1;
            }

            this.currentStage++;
            this.memorizeItemList.Clear();
        }

        internal void MoveToPreStage()
        {
            if (this.currentStage == 0)
            {
                this.currentStage = this.memorizeEntry.Stages.Count;
            }

            this.currentStage--;
            this.memorizeItemList.Clear();
        }

        #endregion

        #region Help Methods

        private void prepareMemorizeItemList()
        {
            if (this.ActiveStage is MemorizeStableStage)
            {
                foreach (string id in ((MemorizeStableStage)this.ActiveStage).MemorizeItemIds)
                {
                    var queryItem = from item in this.memorizeEntry.Items
                                    where item.Id == id
                                    select item;
                    this.memorizeItemList.Add(queryItem.First().ObjectA);
                    this.memorizeItemList.Add(queryItem.First().ObjectB);
                }
            }
            else if (this.ActiveStage is MemorizeRandomItemStage)
            {
                List<int> tempIndexList = new List<int>();
                List<int> pickedIndexList = new List<int>();
                for (int i = 0; i < this.memorizeEntry.Items.Count; i++)
                    tempIndexList.Add(i);

                Random rand = new Random(Environment.TickCount);
                for (int i = 0; i < ((MemorizeRandomItemStage)this.ActiveStage).ItemCount; i++)
                {
                    if (tempIndexList.Count == 0)
                        break;

                    int randIndex = rand.Next(tempIndexList.Count);
                    pickedIndexList.Add(tempIndexList[randIndex]);
                    tempIndexList.RemoveAt(randIndex);
                }

                foreach (int index in pickedIndexList)
                {
                    this.memorizeItemList.Add(this.memorizeEntry.Items[index].ObjectA);
                    this.memorizeItemList.Add(this.memorizeEntry.Items[index].ObjectB);
                }
            }
            else if (this.ActiveStage is MemorizeMathStage)
            {
                MemorizeMathStage mathStage = this.ActiveStage as MemorizeMathStage;
                mathStage.GenerateItems();
                foreach (MemorizeItem item in mathStage.Items)
                {
                    this.memorizeItemList.Add(item.ObjectA);
                    this.memorizeItemList.Add(item.ObjectB);
                }
            }

            this.shuffleItems();
        }

        private void shuffleItems()
        {
            Random rand = new Random(Environment.TickCount);
            int randCount = this.memorizeItemList.Count;
            while (true)
            {
                if (randCount <= 1)
                    break;

                int randIndex = rand.Next(randCount - 1);
                var temp = this.memorizeItemList[randCount - 1];
                this.memorizeItemList[randCount - 1] = this.memorizeItemList[randIndex];
                this.memorizeItemList[randIndex] = temp;

                randCount--;
            }
        }

        private void prepareStages()
        {
            if (this.Entry.Stages.Count == 0 &&
                this.Entry.Items.Count >= 3)
            {
                int count = this.Entry.Items.Count;
                if (count > 20)
                    count = 20;
                for (int i = 3; i <= count; i++)
                {
                    if (this.exclusiveStageList.Contains(i))
                        continue;

                    this.Entry.Stages.Add(this.generateStage(i));
                }
            }
            else
            {
                List<MemorizeStage> tempList = new List<MemorizeStage>();
                foreach (var stage in this.Entry.Stages)
                {
                    if (stage is MemorizeRandomItemStage)
                    {
                        MemorizeRandomItemStage randStage = stage as MemorizeRandomItemStage;
                        if (this.exclusiveStageList.Contains(randStage.ItemCount))
                        {
                            tempList.Add(stage);
                        }
                    }
                    else if (stage is MemorizeMathStage)
                    {
                        MemorizeMathStage mathStage = stage as MemorizeMathStage;
                        if (this.exclusiveStageList.Contains(mathStage.ItemCount))
                        {
                            tempList.Add(stage);
                        }
                    }
                }

                foreach (var stage in tempList)
                {
                    this.Entry.Stages.Remove(stage);
                }
            }
        }

        private MemorizeStage generateStage(int itemCount)
        {
            MemorizeRandomItemStage randomStage = new MemorizeRandomItemStage();
            randomStage.ItemCount = itemCount;
            return randomStage;
        }

        #endregion
    }
}