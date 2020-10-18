using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Memorize.Data;
using System.Windows.Controls;

namespace SoonLearning.Memorize.UI
{
    public class MemorizeControl
    {
        #region Instance

        private static MemorizeControl instance;

        public static MemorizeControl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MemorizeControl();
                }

                return instance;
            }
        }

        private MemorizeControl()
        {
        }

        #endregion

        #region Members 

        private bool testMode;

        #endregion

        #region Properties

        public Control StartupUI
        {
            get { return MemorizeUIContainerUserControl.Instance; }
        }

        public bool TestMode
        {
            set { this.testMode = value; }
            internal get
            { 
                return this.testMode; 
            }
        }

        #endregion

        #region Public methods

        public void Start(MemorizeEntry entry, string userId)
        {
            MemorizeDataMgr.Instance.Entry = entry;
            MemorizeDataMgr.Instance.UserId = userId;
        //    MemorizeStartupUserControl.Instance.startStage();
        }

        public void Uninstall(MemorizeEntry entry, string configFile)
        {
            try
            {
                System.IO.Directory.Delete(System.IO.Path.GetDirectoryName(configFile), true);
            }
            catch
            {
            }
        }

        #endregion
    }
}
