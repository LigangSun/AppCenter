using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.Arithmetic_Division
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Arithmetic_Division;component/Division.png"; }
        }

        public override string Id
        {
            get { return "75B323FA-0B88-443E-9ECA-1C180D9BCDAF"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "四则运算之除法"; }
        }

        public override string Description
        {
            get { return "除法的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\Arithmetic\Division");

            DataMgr.Instance.DataCreator = DivisionDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
