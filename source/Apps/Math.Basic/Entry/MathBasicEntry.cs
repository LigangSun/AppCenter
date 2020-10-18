using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Math.Basic.UserControls;
using System.Windows.Media.Imaging;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Math.Basic.Entry
{
    public abstract class MathBasicEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
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

        public IGadgetControl Control
        {
            get { return ControlMgr.Instance.MathBasicControl; }
        }

        public string Name
        {
            get { return "快学(SoonLearning)"; }
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
    }
}
