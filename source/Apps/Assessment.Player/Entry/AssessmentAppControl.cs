using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using SoonLearning.Assessment.Data.Bank;
using SoonLearning.Assessment.Player.UserControls;
using System.Windows.Controls;
using SoonLearning.AppCenter.Utility;
using SoonLearning.Assessment.Player.Data;

namespace SoonLearning.Assessment.Player.Entry
{
    public class AssessmentAppControl
    {
        #region Instance

        private static AssessmentAppControl instance;

        public static AssessmentAppControl Instance
        {
            get
            {
                Interlocked.CompareExchange<AssessmentAppControl>(ref instance, new AssessmentAppControl(), null);
                return instance;
            }
        }

        private AssessmentAppControl()
        {
        }

        #endregion

        #region Members

        private string assessmentFile = string.Empty;

        #endregion

        #region Properties

        public Control StartupUI
        {
            get { return ControlMgr.Instance.StartupUserControl; }
        }

        internal string AssessmentFile
        {
            get { return this.assessmentFile; }
        }

        #endregion

        #region Public methods

        public void Start(string assessmentFile)
        {
            this.assessmentFile = assessmentFile;
            AssessmentDataCreator dataCreator = new AssessmentDataCreator(SerializerHelper<AssessmentApp>.XmlDeserialize(assessmentFile));
            DataMgr.Instance.DataCreator = dataCreator;
        }

        public void Uninstall(string assessmentFile)
        {
            try
            {
                System.IO.Directory.Delete(System.IO.Path.GetDirectoryName(assessmentFile), true);
            }
            catch
            {
            }
        }

        #endregion
    }
}
