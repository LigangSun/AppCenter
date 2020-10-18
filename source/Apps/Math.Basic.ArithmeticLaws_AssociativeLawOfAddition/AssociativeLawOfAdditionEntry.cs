using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.ArithmeticLaws_AssociativeLawOfAddition
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.ArithmeticLaws_AssociativeLawOfAddition;component/AssociativeLawOfAddition.jpg"; }
        }

        public override string Id
        {
            get { return "FE8802EC-E1C5-41B2-B606-3EF07764B198"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "运算定律之加法交换律"; }
        }

        public override string Description
        {
            get { return "加法交换律的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\ArithmeticLaws\AssociativeLawOfAddition");

            DataMgr.Instance.DataCreator = AssociativeLawOfAdditionDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
