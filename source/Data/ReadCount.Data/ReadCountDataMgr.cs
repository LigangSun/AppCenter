using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SoonLearning.ReadCount.Data
{
    public class ReadCountDataMgr : INotifyPropertyChanged
    {
        private static ReadCountDataMgr instance;

        public static ReadCountDataMgr Instance
        {
            get
            {
                Interlocked.CompareExchange<ReadCountDataMgr>(ref instance, new ReadCountDataMgr(), null);
                return instance;
            }
        }

        private ReadCountStageCollection stageCollection = new ReadCountStageCollection();
        private int stageIndex = -1;
        private int musicIndex = -1;

        public ReadCountStageCollection StageCollection
        {
            set { this.stageCollection = value; }
        }

        //public List<string> MusicList
        //{
        //    get { return stageCollection.BackgroundMusicList; }
        //}

        public List<string> PhaseList
        {
            get { return this.stageCollection.PhaseList; }
        }

        public string NextMusic
        {
            get
            {
                if (this.stageCollection.BackgroundMusicList.Count == 0)
                    return string.Empty;

                if (this.musicIndex == this.stageCollection.BackgroundMusicList.Count - 1)
                    this.musicIndex = -1;

                this.musicIndex++;
                return this.stageCollection.BackgroundMusicList[this.musicIndex];
            }
        }

        public string CurrentMusic
        {
            get
            {
                if (this.musicIndex >= this.stageCollection.BackgroundMusicList.Count ||
                    this.stageCollection.BackgroundMusicList.Count == 0)
                    return string.Empty;
                if (this.musicIndex == -1)
                    this.musicIndex = 0;
                return this.stageCollection.BackgroundMusicList[this.musicIndex];
            }
        }

        public string PreviousMusic
        {
            get
            {
                if (this.stageCollection.BackgroundMusicList.Count == 0)
                    return string.Empty;

                if (this.musicIndex == 0)
                    this.musicIndex = stageCollection.BackgroundMusicList.Count;

                this.musicIndex--;
                return this.stageCollection.BackgroundMusicList[this.musicIndex];
            }
        }

        public ReadCountStage NextStage
        {
            get 
            {
                if (this.StageIndex == stageCollection.StageCollection.Count - 1)                
                    this.stageIndex = -1;

                this.StageIndex++;
                return this.stageCollection.StageCollection[this.StageIndex];
            }
        }

        public ReadCountStage PreStage
        {
            get
            {
                if (this.StageIndex == 0)
                    this.stageIndex = stageCollection.StageCollection.Count;

                this.StageIndex--;
                return this.stageCollection.StageCollection[this.StageIndex];
            }
        }

        public ReadCountStage CurrentStage
        {
            get 
            {
                if (this.StageIndex < 0 || this.StageIndex >= this.stageCollection.StageCollection.Count)
                    return null;

                return this.stageCollection.StageCollection[this.StageIndex]; 
            }
        }

        public int StageIndex
        {
            get
            {
                return this.stageIndex;
            }
            set
            {
                this.stageIndex = value;
                this.OnPropertyChanged("StageIndex");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
