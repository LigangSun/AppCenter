﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math_Fast.SYSS300.LBSS1_188
{
    public class Entry : AssessmentBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 7, 21, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math_Fast.SYSS300.LBSS1_188;component/LBSS1_188.png"; }
        }

        public override string Id
        {
            get { return "86E47BC0-F4CD-4891-B0DE-E8B599F12290"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "速算方法之625倍速算法（一）"; }
        }

        public override string Description
        {
            get { return "625倍速算法（一）的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\SoonLearning.Math_Fast.SYSS300.LBSS1_188");

            DataMgr.Instance.DataCreator = LBSS1_188DataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
