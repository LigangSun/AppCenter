using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using SoonLearning.BlockPuzzle.Puzzle;

namespace SoonLearning.BlockPuzzle.Data
{
    public delegate void PuzzleItemLoadedDelegate(PuzzleItem item);
    public delegate void PuzzleLoadCompletedDelegate();

    internal class PuzzleDataLoader
    {
        public event PuzzleItemLoadedDelegate PuzzleItemLoadedEvent;
        public event PuzzleLoadCompletedDelegate PuzzleLoadCompletedEvent;

        private BackgroundWorker worker;

        public void LoadAsync(PuzzleType type)
        {
            if (this.worker != null &&
                this.worker.IsBusy)
                return;

            this.worker = new BackgroundWorker();
            this.worker.WorkerSupportsCancellation = true;
            this.worker.WorkerReportsProgress = true;
            this.worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            this.worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            this.worker.RunWorkerAsync(type);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            PuzzleType type = (PuzzleType)e.Argument;

            Assembly assembly = Assembly.GetEntryAssembly();
            string dataFolder = Path.GetDirectoryName(assembly.Location);
            dataFolder = Path.Combine(dataFolder, @"Data\Puzzle\" + PuzzleSetting.Instance.Type.ToString());

            DirectoryInfo di = null;
            if (!Directory.Exists(dataFolder))
            {
                di = Directory.CreateDirectory(dataFolder);
            }  
            else
            {
                di = new DirectoryInfo(dataFolder);
            }

            FileInfo[] fis = di.GetFiles("*.pd");
            foreach (FileInfo fi in fis)
            {
                try
                {
                    PuzzleItem pi = PuzzleData.LoadPuzzleItem(fi.FullName);
                    if (pi.Type != type)
                        continue;
                    pi.ImageFile = fi.FullName;
                    worker.ReportProgress(0, pi);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.PuzzleItemLoadedEvent != null)
                this.PuzzleItemLoadedEvent(e.UserState as PuzzleItem);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.PuzzleLoadCompletedEvent != null)
                this.PuzzleLoadCompletedEvent();
        }
    }
}
