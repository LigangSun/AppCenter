﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math_Fast.SYSS300.YCDJ2_147
{
    public class Entry : AssessmentBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 7, 21, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math_Fast.SYSS300.YCDJ2_147;component/YCDJ2_147.png"; }
        }

        public override string Id
        {
            get { return "BBA7DB9E-134D-4E59-9A81-E4FD19F6DA13"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "速算方法之依次得积法（二）"; }
        }

        public override string Description
        {
            get { return "依次得积法（二）的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\SoonLearning.Math_Fast.SYSS300.YCDJ2_147");

            DataMgr.Instance.DataCreator = YCDJ2_147DataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
