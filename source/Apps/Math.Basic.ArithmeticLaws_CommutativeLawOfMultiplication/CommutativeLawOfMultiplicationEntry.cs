using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.ArithmeticLaws_CommutativeLawOfMultiplication
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.ArithmeticLaws_CommutativeLawOfMultiplication;component/CommutativeLawOfMultiplication.jpg"; }
        }

        public override string Id
        {
            get { return "13045F92-0309-42D1-BFD5-002A1078F8FF"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "运算定律之乘法交换律"; }
        }

        public override string Description
        {
            get { return "乘法交换律的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\ArithmeticLaws\CommutativeLawOfMultiplication");

            DataMgr.Instance.DataCreator = CommutativeLawOfMultiplicationDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
