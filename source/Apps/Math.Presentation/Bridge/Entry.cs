using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.Math.Presentation.Bridge
{
    public class Entry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createData = new DateTime(2012, 9, 2, 12, 0, 0);

        public AppCapability Capability
        {
            get { return AppCapability.BackgroundMusic; }
        }

        public DateTime CreateDate
        {
            get { return this.createData; }
        }

        public string Description
        {
            get { return "以动画的方式演示火车过桥问题，并对过桥问题进行详细讲解。"; }
        }

        public System.Windows.UIElement GetStartupPage()
        {
            return MainUserControl.Instance;
        }

        public string Id
        {
            get { return "E23542934BBD44DABF66B00165FB88E5"; }
        }

        public int SubTag
        {
            get { return 202; }
        }

        public int Tag
        {
            get { return 200; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Presentation.Bridge;component/Images/Train.png"; }
        }

        public string Title
        {
            get { return "过桥问题"; }
        }

        public void Uninstall()
        {
        }

        public string Logo
        {
            get { return string.Empty; }
        }

        public string Name
        {
            get { return "速学科技"; }
        }

        public string WebSite
        {
            get { return "http://www.soonlearning.com"; }
        }
    }
}
