using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.ColorExplorer.ClickBallon
{
    public class ColorClickEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 12, 2, 8, 0, 0);

        public string Title
        {
            get { return "颜色认知（气球）"; }
        }

        public string Description
        {
            get { return "根据电脑说出的颜色点击对应颜色的气球"; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/FindColor.png"; }
        }

        public int Tag
        {
            get { return 100; }
        }

        public System.Windows.UIElement GetStartupPage()
        {
            return ColorClickUserControl.Instance;
        }

        public int SubTag
        {
            get { return 101; }
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
            get { return "上海速学信息科技有限公司(SoonLearning)"; }
        }

        public string WebSite
        {
            get { return @"http://www.soonlearning.com"; }
        }

        public string Logo
        {
            get { return string.Empty; }
        }

        public void Uninstall()
        {
        }

        public AppCapability Capability
        {
            get { return AppCapability.BackgroundMusic; }
        }
    }
}
