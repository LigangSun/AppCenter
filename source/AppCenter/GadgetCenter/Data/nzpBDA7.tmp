using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;

namespace GadgetCenter.Data
{
    public enum InstallState
    {
        NotStart,
        Downloading,
        DownloadFail,
        Installing,
        InstallFail,
        UserCancelling,
        UserCancelled,
        Done,
    }

    public class AppInstallItem : INotifyPropertyChanged
    {
        #region Members

        private GadgetItemOnline appItem;
        private WebClient webClient;
        private string locakPackFile;
        private int percent;
        private InstallState state;

        #endregion

        #region Properties

        public GadgetItemOnline AppItem
        {
            get { return this.appItem; }
            set { this.appItem = value; }
        }

        public WebClient WebClient
        {
            get { return this.webClient; }
        }

        public string LocakPackFile
        {
            get { return this.locakPackFile; }
            set { this.locakPackFile = value; }
        }

        public int Percent
        {
            get
            {
                return this.percent;
            }
            set
            {
                this.percent = value;
                this.OnPropertyChanged("Percent");
            }
        }

        public string Image
        {
            get { return this.appItem.Thumbnail; }
        }

        public string Title
        {
            get { return this.appItem.Title; }
        }

        public InstallState State
        {
            get { return this.state; }
            set
            {
                this.state = value;
                this.OnPropertyChanged("State");
            }
        }

        #endregion

        #region Constructor

        public AppInstallItem()
        {
            this.Percent = 0;
            this.State = InstallState.NotStart;
        }

        public AppInstallItem(GadgetItemOnline item, WebClient client, string localFile)
        {
            this.appItem = item;
            this.webClient = client;
            this.locakPackFile = localFile;
            this.Percent = 0;
            this.State = InstallState.NotStart;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion
    }
}
