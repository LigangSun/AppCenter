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
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Media.Animation;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.AppCenter.Controls
{
    public delegate ThumbnailItem DataItemLoadedDelegate(string file);
    public delegate void DataLoadCompletedDelegate();

    /// <summary>
    /// Interaction logic for LoadAppDataWindow.xaml
    /// </summary>
    public partial class LoadAppDataWindow
    {
        private string dataPath;
        private List<string> fileExtList = new List<string>();

        public event DataItemLoadedDelegate DataItemLoadedEvent;
        public event DataLoadCompletedDelegate DataLoadCompletedEvent;

        private Uri Thumbnail
        {
            set
            {
                this.iconImage.Source = new BitmapImage(value);
            }
        }

        public string DataPath
        {
            set
            {
                this.dataPath = value;
            }
        }

        public string[] FileExt
        {
            set
            {
                this.fileExtList.Clear();
                this.fileExtList.AddRange(value);
            }
        }

        public LoadAppDataWindow(Uri thumbnail)
        {
            InitializeComponent();

            this.Thumbnail = thumbnail;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = this.TryFindResource("loadStoryboard") as Storyboard;
            if (storyboard != null)
                storyboard.Begin();
        }

        private void delayLoad(object sender, EventArgs e)
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

            if (!Directory.Exists(this.dataPath))
                return;

            DirectoryInfo di = new DirectoryInfo(this.dataPath);
            foreach (string filter in this.fileExtList)
            {
                FileInfo[] fis = di.GetFiles(filter);
                foreach (FileInfo fi in fis)
                {
                    try
                    {
                        worker.ReportProgress(0, fi.FullName);
                        Thread.Sleep(5);
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false, ex.Message);
                    }
                }
            }

            Thread.Sleep(0);
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.DataItemLoadedEvent != null)
            {
                ThumbnailItem item = this.DataItemLoadedEvent(e.UserState as string);
                this.dspPanel.DataContext = item;
            }
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (this.DataLoadCompletedEvent != null)
            {
                this.DataLoadCompletedEvent();
            }

        //    Thread.Sleep(100);

        //    this.enterButton.IsEnabled = true;
        }

        private void enterButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            
        }

        public void StartToLoad()
        {
            this.Dispatcher.BeginInvoke(new EventHandler<EventArgs>(delayLoad), DispatcherPriority.Background, new object[] { null, null }); 
        }
    }
}
