using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter;
using System.Runtime.InteropServices;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;
using System.Windows.Media.Imaging;

namespace Gadget.ColorExplore
{
    public class BallonEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 12, 3, 12, 0, 0);

        public string Title
        {
            get { return "说出颜色"; }
        }

        public string Description
        {
            get { return "Ballon"; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/ColorExplore;component/Images/ColorRecognize.bmp"; }
        }

        public GadgetType Tag
        {
            get { return GadgetType.Explore; }
        }

        public System.Windows.UIElement GetStartupPage()
        {
            return BallonUserControl.Instance;
        }

        public IGadgetControl Control
        {
            get { return BallonGadgetControl.Instance; }
        }


        public GadgetSubType SubTag
        {
            get { return GadgetSubType.Explore_Color; }
        }

        public string Id
        {
            get { return "A19A3144C0B246B8896BA5A0ACBEBE41"; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
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
    }
}
