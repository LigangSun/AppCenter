﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math_Fast.SYSS300.DS_150
{
    public class Entry : AssessmentBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 7, 21, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math_Fast.SYSS300.DS_150;component/DS_150.png"; }
        }

        public override string Id
        {
            get { return "014AA180-A479-4465-A8F3-B5E5FB40F058"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "速算方法之倒算法"; }
        }

        public override string Description
        {
            get { return "倒算法的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\SoonLearning.Math_Fast.SYSS300.DS_150");

            DataMgr.Instance.DataCreator = DS_150DataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
