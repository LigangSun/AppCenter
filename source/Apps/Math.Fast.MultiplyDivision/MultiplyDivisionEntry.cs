using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Fast.Properties;
using System.Windows;
using SoonLearning.Math.Data;
using SoonLearning.Math.Fast.RapidCalculation;
using System.Windows.Media.Imaging;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.Math.Fast.MultiplyDivision
{
    public class MultiplyDivisionEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 11, 4, 8, 0, 0);

        public string Title
        {
            get { return Resources.mathMultiplyDivisionTitle; }
        }

        public string Description
        {
            get { return Resources.mathMDDescription; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Fast.MultiplyDivision;component/Images/MD_Title.png"; }
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
            MathAPI.MMInit(IntPtr.Zero);
            ControlMgr.Instance.Entry = this;
            MathSetting.Instance.Type = typeof(MultiplyDivisionQuestionData);
            MathSetting.Instance.CurrentQuestionHeader = MultiplyDivisionQuestionData.header;
            MathSetting.Instance.CreateQuestionData += new CreateQuestionDataDelegate(Instance_CreateQuestionData);
            return ControlMgr.Instance.MathStartupControl;
        }

        public IGadgetControl Control
        {
            get { return RapidGadgetControl.Instance; }
        }

        public string Id
        {
            get { return "D77D4B55CE2E4672B4177D914ABAF975"; }
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
            return new MultiplyDivisionQuestionData();
        }

        public void Uninstall()
        {
            MathSetting.Instance.Uninstall();
        }
    }
}
