using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.Integer_DecompoundPrimeFactor
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0);

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Integer_DecompoundPrimeFactor;component/DecompoundPrimeFactor.png"; }
        }

        public override string Id
        {
            get { return "251B948F-5F32-4CD0-9B42-F0E4D4BEA235"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "分解质因数"; }
        }

        public override string Description
        {
            get { return "分解质因数的学习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\Integer\DecompoundPrimeFactor");

            DataMgr.Instance.DataCreator = DecompoundPrimeFactorDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
