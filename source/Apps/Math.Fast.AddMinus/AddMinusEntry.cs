using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Fast.RapidCalculation;
using System.Windows.Media.Imaging;
using System.Windows;
using SoonLearning.Math.Fast.Properties;
using SoonLearning.Math.Data;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.Math.Fast.AddMinus
{
    public class AddMinusEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 11, 3, 8, 0, 0);

        public string Title
        {
            get { return Resources.mathAddMinusTitle; }
        }

        public string Description
        {
            get { return Resources.mathAddMinusDescription; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Fast.AddMinus;component/Images/AM_Icon.png"; }
        }

        public int Tag
        {
            get { return 200; }
        }

        public int SubTag
        {
            get { return 207; }
        }

        public System.Windows.UIElement GetStartupPage()
        {
            MathAPI.MCInit(IntPtr.Zero);
            ControlMgr.Instance.Entry = this;
            MathSetting.Instance.Type = typeof(AddMinusQuestionData);
            MathSetting.Instance.CurrentQuestionHeader = AddMinusQuestionData.header;
            MathSetting.Instance.CreateQuestionData += new CreateQuestionDataDelegate(Instance_CreateQuestionData);
            return ControlMgr.Instance.MathStartupControl;
        }

        public string Id
        {
            get { return "2F3ED5300C0D425C85B9AF2BDF27196F"; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
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

        public AppCapability Capability
        {
            get { return AppCapability.None; }
        }

        private QuestionData Instance_CreateQuestionData()
        {
            return new AddMinusQuestionData();
        }

        public void Uninstall()
        {
            MathSetting.Instance.Uninstall();
        }
    }
}
