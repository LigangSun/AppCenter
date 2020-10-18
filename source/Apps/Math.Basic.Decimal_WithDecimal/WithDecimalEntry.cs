using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.Decimal_WithDecimal
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Decimal_WithDecimal;component/WithDecimal.png"; }
        }

        public override string Id
        {
            get { return "E0B92CDE-1331-47CA-A1D6-93F67E81F0DE"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "带小数"; }
        }

        public override string Description
        {
            get { return "带小数的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\Decimal\WithDecimal");

            DataMgr.Instance.DataCreator = WithDecimalDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
