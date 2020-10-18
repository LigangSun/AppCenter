using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.ArithmeticLaws_CharacterOfDivision
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.ArithmeticLaws_CharacterOfDivision;component/CharacterOfDivision.jpg"; }
        }

        public override string Id
        {
            get { return "B4665349-7817-4CBD-ADBF-72A726F437BF"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "运算定律之除法的性质"; }
        }

        public override string Description
        {
            get { return "除法的性质的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\ArithmeticLaws\CharacterOfDivision");

            DataMgr.Instance.DataCreator = CharacterOfDivisionDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
