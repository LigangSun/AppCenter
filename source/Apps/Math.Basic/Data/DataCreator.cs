﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using SoonLearning.Math.Data;
using System.Reflection;
using System.IO;
using System.Windows.Markup;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Threading;
using System.Diagnostics;

namespace Math.Basic.Data
{
    internal delegate void CreateDataProgress(BaseObject data);
    internal delegate void CreateDataCompleted(BaseObject data);

    internal abstract class DataCreator
    {
        public event CreateDataProgress CreateDataProgressEvent;
        public event CreateDataCompleted CreateDataCompletedEvent;

        protected string exerciseTitle = string.Empty;
        protected string exerciseDescription = string.Empty;
        protected string examTitle = string.Empty;
        protected string examDescription = string.Empty;
        protected string flowDocumentFile = string.Empty;

        protected ObservableCollection<SectionBaseInfo> sectionInfoCollection = new ObservableCollection<SectionBaseInfo>();

        public ObservableCollection<SectionBaseInfo> SectionInfoCollection
        {
            get { return this.sectionInfoCollection; }
        }

        public DataCreator()
        {
            this.PrepareSectionInfoCollection();
        }

        protected abstract void PrepareSectionInfoCollection();

        internal virtual FlowDocument GetKnowledgeDefinition()
        {
            return this.GetFlowDocument(this.flowDocumentFile);
        }

        // This is an Async method
        protected virtual void GetExercise(BackgroundWorker worker)
        {
            Exercise exercise = ObjectCreator.CreateExercise(this.exerciseTitle, this.exerciseDescription);

            foreach (SectionBaseInfo info in this.sectionInfoCollection)
            {
                if (info.QuestionCount == 0)
                    continue;

                exercise.SectionCollection.Add(this.CreateSection(info, worker));
            }

            worker.ReportProgress(100, exercise);
        }

        // This is an Async method
        protected virtual void GetExam(BackgroundWorker worker)
        {
            Exam exam = ObjectCreator.CreateExam(this.examTitle, this.examDescription, 300);

            foreach (SectionBaseInfo info in this.sectionInfoCollection)
            {
                if (info.QuestionCount == 0)
                    continue;
                exam.SectionCollection.Add(this.CreateSection(info, worker));
            }

            worker.ReportProgress(100, exam);
        }

        protected virtual SoonLearning.Math.Data.Section CreateSection(SectionBaseInfo info, BackgroundWorker worker)
        {
            SoonLearning.Math.Data.Section section = ObjectCreator.CreateSection(info.Name, info.Description);

            Random rand = new Random((int)DateTime.Now.Ticks);
            int generatedCount = 0;
            while (true)
            {
                if (generatedCount == info.QuestionCount)
                    break;

                try
                {
                    AppendQuestion(info, section);
                }
                catch (Exception ex)
                {
                    Debug.Assert(false, ex.Message);
                }
                
                worker.ReportProgress(0, null);
                Debug.WriteLine(generatedCount);

                generatedCount++;

                Thread.Sleep(50);
            }

            return section;
        }

        protected abstract void AppendQuestion(SectionBaseInfo info, SoonLearning.Math.Data.Section section);

        public void GenerateExercise()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
        }

        public void GenerateExam()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += new DoWorkEventHandler(examWorker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
        }

        private void examWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            this.GetExam(worker);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            this.GetExercise(worker);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
                this.NotifyCreateDataCompleted(e.UserState as BaseObject);
            else
                this.NotifyCreateDataProgress(e.UserState as BaseObject);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }

        private void NotifyCreateDataCompleted(BaseObject obj)
        {
            if (this.CreateDataCompletedEvent != null)
            {
                this.CreateDataCompletedEvent(obj);
            }
        }

        private void NotifyCreateDataProgress(BaseObject obj)
        {
            if (this.CreateDataProgressEvent != null)
            {
                this.CreateDataProgressEvent(obj);
            }
        }

        protected virtual FlowDocument GetFlowDocument(string file)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Stream stream = executingAssembly.GetManifestResourceStream(file);
            FlowDocument doc = (FlowDocument)XamlReader.Load(stream);
            return doc;
        }
    }
}
