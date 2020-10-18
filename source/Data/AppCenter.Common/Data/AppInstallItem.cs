using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.Data
{
    public enum InstallState
    {
        NotStart,
        Downloading,
        DownloadFail,
        Installing,
        InstallFail,
        InstallFail_FileInUse,
        InstallFail_RequirePermission,
        InstallFail_UnauthorizedAccess,
        InstallFail_PathTooLongException,
        UserCancelling,
        UserCancelled,
        Done,
    }

    public class AppInstallItem : NotifyPropertyChanged
    {
        #region Members

        private GadgetItemOnline appItem;
        private HttpHelper webClient;
        private string locakPackFile;
        private int percent;
        private InstallState state;
        private Exception lastEx;

        #endregion

        #region Properties

        public GadgetItemOnline AppItem
        {
            get { return this.appItem; }
            set { this.appItem = value; }
        }

        public HttpHelper WebClient
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

        public Exception LastEx
        {
            get { return this.lastEx; }
        }

        #endregion

        #region Constructor

        public AppInstallItem()
        {
            this.Percent = 0;
            this.State = InstallState.NotStart;
        }

        public AppInstallItem(GadgetItemOnline item, HttpHelper client, string localFile)
        {
            this.appItem = item;
            this.webClient = client;
            this.locakPackFile = localFile;
            this.Percent = 0;
            this.State = InstallState.NotStart;
        }

        #endregion

        public void SetHttpHelper(HttpHelper helper)
        {
            this.webClient = helper;
        }

        public void SetLastEx(Exception ex)
        {
            this.lastEx = ex;
        }
    }
}
