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

namespace SoonLearning.Math.Fast.AddMinusMultiplyDivision
{
    public class AddMinusMultiplyDivisionEntry : MarshalByRefObject, IGadgetEntry//, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 11, 5, 8, 0, 0);

        public string Title
        {
            get { return Resources.mathAddMinusMultiplyDivisionTitle; }
        }

        public string Description
        {
            get { return Resources.mathAMMDDescription; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Fast.AddMinusMultiplyDivision;component/Images/Add_Minus_Multiply_Division.png"; }
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
            MathSetting.Instance.Type = typeof(AddMinusMultiplyDivisionQuestionData);
            MathSetting.Instance.CurrentQuestionHeader = AddMinusMultiplyDivisionQuestionData.header;
            MathSetting.Instance.CreateQuestionData += new CreateQuestionDataDelegate(Instance_CreateQuestionData);
            return ControlMgr.Instance.MathStartupControl;
        }

        public string Id
        {
            get { return "0F16FDF1F6854FDAB49603387189FD92"; }
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

        public System.Windows.Media.Imaging.BitmapImage Logo
        {
            get { return null; }
        }

        public AppCapability Capability
        {
            get { return AppCapability.None; }
        }

        private QuestionData Instance_CreateQuestionData()
        {
            return new AddMinusMultiplyDivisionQuestionData();
        }

        public void Uninstall()
        {
            MathSetting.Instance.Uninstall();
        }
    }
}
