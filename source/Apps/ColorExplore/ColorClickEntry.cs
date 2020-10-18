using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Gadget.ColorExplore
{
    public class ColorClickEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 12, 2, 8, 0, 0);

        public string Title
        {
            get { return "颜色认知"; }
        }

        public string Description
        {
            get { return "颜色认知"; }
        }

        public string Thumbnail
        {
            get { return @"pack:\\application:,,,/ColorExplore;component/Images/FindColor.bmp"; }
        }

        public GadgetType Tag
        {
            get { return GadgetType.Explore; }
        }

        public System.Windows.UIElement GetStartupPage()
        {
            return ColorClickUserControl.Instance;
        }

        public IGadgetControl Control
        {
            get { return ColorClickGadgetControl.Instance; }
        }

        public GadgetSubType SubTag
        {
            get { return GadgetSubType.Explore_Color; }
        }

        public string Id
        {
            get { return "A51FFC6ADBF94F8988C7D5A353A2A6C4"; }
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
