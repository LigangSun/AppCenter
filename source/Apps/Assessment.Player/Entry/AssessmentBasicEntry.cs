using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.UserControls;
using System.Windows.Media.Imaging;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.Assessment.Player.Entry
{
    /// <summary>
    /// This base entry is for Rapid
    /// </summary>
    public abstract class AssessmentBasicEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        public abstract string Thumbnail
        {
            get;
        }

        public int Tag
        {
            get { return 200; }
        }

        public int SubTag
        {
            get { return 207; }
        }

        public string Name
        {
            get { return "速学(SoonLearning)"; }
        }

        public string WebSite
        {
            get { return @"http://www.soonlearning.com"; }
        }

        public string Logo
        {
            get { return string.Empty; }
        }

        public abstract string Id
        {
            get;
        }

        public abstract DateTime CreateDate
        {
            get;
        }

        public abstract string Title
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public abstract System.Windows.UIElement GetStartupPage();

        public AppCapability Capability
        {
            get { return AppCapability.None; }
        }

        public void Uninstall()
        {
            try
            {
                System.IO.Directory.Delete(SoonLearning.Assessment.Player.Data.DataMgr.Instance.DataFolder, true);
            }
            catch
            {
            }
        }
    }
}
