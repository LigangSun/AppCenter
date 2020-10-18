using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.AppCenter.Data;
using SoonLearning.Assessment.Data;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;

namespace SoonLearning.Assessment.Player.Entry
{
    public class PowerExponentEntry// : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime dateTime = new DateTime(2012, 4, 12);
        public string Thumbnail
        {
            get { return @"pack:\\application:,,,/Gadget.SoonLearning.Assessment.Player;component/Resources/integer.png"; }
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
            get { return "速学信息科技(SoonLearning)"; }
        }

        public string WebSite
        {
            get { return @"http://www.soonlearning.com"; }
        }

        public string Logo
        {
            get { return string.Empty; }
        }

        public string Id
        {
            get { return "9485C87DA4DD4C348763E2A5BE9D4C01"; }
        }

        public DateTime CreateDate
        {
            get { return dateTime; }
        }

        public string Title
        {
            get { return "指数幂"; }
        }

        public string Description
        {
            get { return string.Empty; }
        }

        public System.Windows.UIElement GetStartupPage()
        {
            SoonLearning.Assessment.Player.Data.DataMgr.Instance.ActiveMathBasicType = MathBasicType.PowerExponent;
        //    ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
