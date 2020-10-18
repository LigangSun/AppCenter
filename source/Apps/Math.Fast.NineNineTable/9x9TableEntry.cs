using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Fast.Properties;
using System.Windows.Media.Imaging;
using System.Windows;
using SoonLearning.Math.Fast.RapidCalculation;
using SoonLearning.Math.Data;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.Math.Fast.NineNineTable
{
    public class _9x9TableEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 11, 6, 8, 0, 0);

        public string Title
        {
            get { return Resources.NineXNineTableTitle; }
        }

        public string Description
        {
            get { return Resources.NineXNineTableDescription; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Fast.NineNineTable;component/Images/9x9Table.png"; }
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
            MathSetting.Instance.Type = typeof(_9x9QuestionData);
            MathSetting.Instance.EnableRange = false;
            MathSetting.Instance.CurrentQuestionHeader = _9x9QuestionData.header;
            MathSetting.Instance.CreateQuestionData += new CreateQuestionDataDelegate(Instance_CreateQuestionData);
            return ControlMgr.Instance.MathStartupControl;
        }

        public string Id
        {
            get { return "8B36742B04DF4648A29AB8B87DBC1F96"; }
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

        private QuestionData Instance_CreateQuestionData()
        {
            return new _9x9QuestionData();
        }

        public void Uninstall()
        {
            MathSetting.Instance.Uninstall();
        }

        public AppCapability Capability
        {
            get { return AppCapability.None; }
        }
    }
}
