using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;

namespace SoonLearning.UpgradeTool
{
    internal class DownloadHelper : IDisposable
    {
        private WebClient client;
        private bool disposed;

        public event DownloadProgressChangedEventHandler DownloadProgressChanged;
        public event AsyncCompletedEventHandler DownloadFileCompleted;

        #region File Download Method and Events

        public void DownloadFile(string url, string localFile)
        {
            client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.Headers.Add("SoonLearningPlatformKey", "{1FB07182-8794-47F1-90DB-7B3698AE1E61}");

            client.DownloadFileAsync(new Uri(url), localFile);
        }

        public void Cancel()
        {
            if (!this.disposed)
            {
                if (this.client != null)
                {
                    this.client.CancelAsync();
                }
            }
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgressChangedEventHandler temp = this.DownloadProgressChanged;
            if (temp != null)
            {
                temp(sender, e);
            }
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            AsyncCompletedEventHandler temp = this.DownloadFileCompleted;
            if (temp != null)
            {
                temp(sender, e);
            }
        }

        #endregion

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
                if (client != null)
                {
                    client.Dispose();
                    client = null;
                }
            }

            this.disposed = true;
        }

        ~DownloadHelper()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
