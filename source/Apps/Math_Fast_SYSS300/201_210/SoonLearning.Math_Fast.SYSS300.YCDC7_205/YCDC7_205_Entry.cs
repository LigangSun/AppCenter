﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math_Fast.SYSS300.YCDC7_205
{
    public class Entry : AssessmentBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 7, 21, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math_Fast.SYSS300.YCDC7_205;component/YCDC7_205.png"; }
        }

        public override string Id
        {
            get { return "CC8CE5FC-3497-4D5E-9849-FD26C207B714"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "速算方法之以乘代除法（七）"; }
        }

        public override string Description
        {
            get { return "以乘代除法（七）的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\SoonLearning.Math_Fast.SYSS300.YCDC7_205");

            DataMgr.Instance.DataCreator = YCDC7_205DataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
