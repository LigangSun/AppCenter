﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math_Fast.SYSS300.EBSS_171
{
    public class Entry : AssessmentBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 7, 21, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math_Fast.SYSS300.EBSS_171;component/EBSS_171.png"; }
        }

        public override string Id
        {
            get { return "A28D4691-B3C5-4AF2-B6FA-DAD54AA96B77"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "速算方法之25倍速算法"; }
        }

        public override string Description
        {
            get { return "25倍速算法的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\SoonLearning.Math_Fast.SYSS300.EBSS_171");

            DataMgr.Instance.DataCreator = EBSS_171DataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
