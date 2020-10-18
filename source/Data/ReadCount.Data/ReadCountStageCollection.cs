using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace SoonLearning.ReadCount.Data
{
    public class ReadCountStageCollection
    {
        private List<string> backgroundMusicList = new List<string>();
        private List<string> phaseList = new List<string>();
        private ObservableCollection<ReadCountStage> stageCollection = new ObservableCollection<ReadCountStage>();

        public List<string> BackgroundMusicList
        {
            get { return this.backgroundMusicList; }
        }

        public List<string> PhaseList
        {
            get { return this.phaseList; }
        }

        public ObservableCollection<ReadCountStage> StageCollection
        {
            get { return this.stageCollection; }
        }
    }
}
