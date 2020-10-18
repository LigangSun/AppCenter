using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.Integer_PrimeNumber
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0);

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Integer_PrimeNumber;component/PrimeNumber.png"; }
        }

        public override string Id
        {
            get { return "AF0F48A5-3D7F-4136-A2E5-4CCC2C3050E9"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "质数(素数)"; }
        }

        public override string Description
        {
            get { return "质数(素数)"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\Integer\PrimeNumber");

            DataMgr.Instance.DataCreator = PrimeNumberDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
