using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;
using System.ComponentModel;

namespace SoonLearning.AppCenter.Utility
{
    public class HttpHelper : IDisposable
    {
        private bool disposed = false;

        private int retryTimes = 0;
        private bool retryBroken = true;
        private int tryTime = 0;

        private object cancelLocker = typeof(bool);

        private BackgroundWorker worker;

        private const int bufferSize = 4096;

        private Exception lastEx;

        private Dictionary<string, string> headerDictionary = new Dictionary<string, string>();

        public int RetryTimes
        {
            set { this.retryTimes = value; }
        }

        public bool RetryBroken
        {
            set { this.retryBroken = value; }
        }

        public event ProgressChangedEventHandler DownloadProgressChanged;
        public event AsyncCompletedEventHandler DownloadFileCompleted;

        public void DownloadFileAsync(string remoteFile, string localFile)
        {
            worker = new BackgroundWorker();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.DoWork += ((sender, e) =>
            {
                this.downloadFile(remoteFile, localFile, worker, e);
                Thread.Sleep(0);
            });
            worker.ProgressChanged += ((sender, e) =>
            {
                ProgressChangedEventHandler temp = this.DownloadProgressChanged;
                if (temp != null)
                    temp(this, e);
            });
            worker.RunWorkerCompleted += ((sender, e) =>
            {
                AsyncCompletedEventHandler temp = this.DownloadFileCompleted;
                if (temp != null)
                {
                    if (e.Cancelled)
                        temp(this, new AsyncCompletedEventArgs(lastEx, e.Cancelled, null));
                    else
                        temp(this, new AsyncCompletedEventArgs(lastEx, e.Cancelled, e.Result));
                }

                worker.Dispose();
            });
            worker.RunWorkerAsync();
        }

        public void CancelAsync()
        {
            lock (this.cancelLocker)
            {
                if (worker != null && ! worker.CancellationPending)
                    worker.CancelAsync();
            }
        }

        public void Clear()
        {
            
        }

        public void AddHeader(string key, string value)
        {
            if (this.headerDictionary.ContainsKey(key))
                this.headerDictionary[key] = value;
            else
                this.headerDictionary.Add(key, value);
        }

        private void downloadFile(string remoteFile, string localFile, BackgroundWorker worker, DoWorkEventArgs e)
        {
            // Function will return the number of bytes processed  
            // to the caller. Initialize to 0 here.10.  
            long bytesReceived = 0;

            // Assign values to these objects here so that they can  
            // be referenced in the finally block  
            Stream remoteStream = null;
            Stream localStream = null;

            HttpWebResponse response = null;

            bool fail = false;

            // Use a try/catch/finally block as both the WebRequest and Stream 
            // classes throw exceptions upon error  
            try
            {
                // Create a request for the specified remote file name  
                HttpWebRequest request = WebRequest.Create(remoteFile) as HttpWebRequest;
                if (this.IsCancelling())
                {
                    return;
                }

                if (request != null)
                {
                    request.Method = "GET";
                    foreach (string key in this.headerDictionary.Keys)
                    {
                        request.Headers.Add(key, this.headerDictionary[key]);
                    }
                    if (this.retryBroken)
                    {
                        localStream = File.Open(localFile, FileMode.Append, FileAccess.Write);
                        request.AddRange(localStream.Length); // Retry Broken
                        bytesReceived = localStream.Length;
                    }

                    // Send the request to the server and retrieve the   
                    // WebResponse object    
                    response = request.GetResponse() as HttpWebResponse;
                    if (this.IsCancelling())
                    {
                        return;
                    }

                    if (response != null)
                    {
                        // Once the WebResponse object has been retrieved,       
                        // get the stream object associated with the response's data      
                        remoteStream = response.GetResponseStream();
                        // Create the local file  
                        if (localStream == null)
                            localStream = File.Open(localFile, FileMode.CreateNew, FileAccess.Write);

                        if (this.IsCancelling())
                        {
                            return;
                        }

                        // Allocate a 4k buffer       
                        byte[] buffer = new byte[bufferSize];
                        int bytesRead = 0;
                        long totalBytes = response.ContentLength;
                        // Simple do/while loop to read from stream until    
                        // no bytes are returned      
                        do
                        {
                            // Read data (up to 1k) from the stream    
                            bytesRead = remoteStream.Read(buffer, 0, buffer.Length);
                            // Write the data to the local file50.         
                            localStream.Write(buffer, 0, bytesRead);
                            // Increment total bytes processed        
                            bytesReceived += bytesRead;

                            if (worker != null)
                                worker.ReportProgress((int)(bytesReceived * 100 / totalBytes));

                            if (this.IsCancelling())
                            {
                                break;
                            }

                            Thread.Sleep(1);
                        } while (bytesRead > 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                fail = true;
                lastEx = ex;
            }
            finally
            {
                // Close the response and streams objects here 65.   
                // to make sure they're closed even if an exception   
                // is thrown at some point   
                if (response != null)
                    response.Close();
                if (remoteStream != null)
                    remoteStream.Close();
                if (localStream != null)
                    localStream.Close();

                e.Cancel = worker.CancellationPending;
            }

            if (this.IsCancelling())
            {
                return;
            }

            this.tryTime++;
            if (fail)
            {
                if (tryTime < this.retryTimes)
                    this.downloadFile(remoteFile, localFile, worker, e);
            }
            else
            {
                lastEx = null;
            }
        }

        private bool IsCancelling()
        {
            lock (this.cancelLocker)
            {
                if (this.worker == null)
                    return false;

                return this.worker.CancellationPending;
            }
        }

        #region Dispose

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                if (worker != null)
                {
                    worker.Dispose();
                    worker = null;
                }
            }

            this.disposed = true;
        }

        ~HttpHelper()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
