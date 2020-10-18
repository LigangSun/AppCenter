using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.Integer_Coprimenumbers
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0);

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Integer_Coprimenumbers;component/Coprimenumbers.png"; }
        }

        public override string Id
        {
            get { return "9C8DF0D3-EC5E-4813-80FA-8E306EE91D5B"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "互质数"; }
        }

        public override string Description
        {
            get { return "互质数的学习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\Integer\Coprimenumbers");

            DataMgr.Instance.DataCreator = CoprimenumbersDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
