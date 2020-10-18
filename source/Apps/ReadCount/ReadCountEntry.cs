using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using Gadget.ReadCount.Properties;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Gadget.ReadCount
{
    public class ReadCountEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2012, 1, 1, 1, 1, 1);

        public string Title
        {
            get { return Resources.ReadCountTitle; }
        }

        public string Description
        {
            get { return Resources.ReadCountDescription; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/ReadCount;component/Resources/CountFruit.png"; }
        }

        public GadgetType Tag
        {
            get { return GadgetType.Math; }
         }

        public System.Windows.UIElement GetStartupPage()
        {
            return ReadCountStartupPage.Instance;
        }

        public bool GoBack()
        {
            return true;
        }

        public GadgetSubType SubTag
        {
            get { return GadgetSubType.Math_Game; }
        }

        public string Id
        {
            get { return "9482F448717545D0B43C5DC320EFFA03"; }
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
