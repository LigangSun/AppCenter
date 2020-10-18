using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.Equation_SolveEquation
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Equation_SolveEquation;component/SolveEquation.jpg"; }
        }

        public override string Id
        {
            get { return "DDCB7B6A-AFC4-4B19-831D-2708FF29BBFB"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "解方程"; }
        }

        public override string Description
        {
            get { return "解方程的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\Equation\SolveEquation");

            DataMgr.Instance.DataCreator = SolveEquationDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
