using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.IO;
using System.Threading;

namespace SoonLearning.Math.Data
{
    public class QuestionDataCollection : ObservableCollection<QuestionData>
    {
        private object dataLocker = new object();

        internal void Add(QuestionData data)
        {
            lock (this.dataLocker)
            {
                base.Add(data);
            }
        }

        internal void LoadFromFiles()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            string[] files = Directory.GetFiles(MathSetting.Instance.DataFolder, "*.qdf");
            foreach (string file in files)
            {
                QuestionData data = QuestionData.Load(file);
                worker.ReportProgress(0, data);
            }

            Thread.Sleep(1);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Add(e.UserState as QuestionData);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
    }
}
