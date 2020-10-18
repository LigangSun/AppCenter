using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.ArithmeticLaws_DistributiveLawOfMultiplication
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.ArithmeticLaws_DistributiveLawOfMultiplication;component/DistributiveLawOfMultiplication.jpg"; }
        }

        public override string Id
        {
            get { return "BAF64CB5-7CEB-40B9-B31D-2FCEA0856289"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "运算定律之乘法分配律"; }
        }

        public override string Description
        {
            get { return "乘法分配律的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\ArithmeticLaws\DistributiveLawOfMultiplication");

            DataMgr.Instance.DataCreator = DistributiveLawOfMultiplicationDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
