using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.Assessment.Player.Entry
{
    public abstract class AssessmentGradeMathEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
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
            get { return 202; }
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
