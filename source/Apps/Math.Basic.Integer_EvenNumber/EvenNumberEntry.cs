using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.Integer_EvenNumber
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0);

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Integer_EvenNumber;component/EvenNumber.png"; }
        }

        public override string Id
        {
            get { return "B5118D58-4204-4005-BDC9-ED2B9EF0CDE9"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "偶数"; }
        }

        public override string Description
        {
            get { return "偶数的学习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\Integer\EvenNumber");

            DataMgr.Instance.DataCreator = EvenNumberDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
