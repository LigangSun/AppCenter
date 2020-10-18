﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;

namespace SoonLearning.Math.Decimal_FormOfDecimal
{
    public class Entry : AssessmentGradeMathEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0); 

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.Math.Decimal_FormOfDecimal;component/FormOfDecimal.png"; }
        }

        public override string Id
        {
            get { return "56A0D3CB-6C93-45A7-86DB-82EB50A3B96D"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }

        public override string Title
        {
            get { return "小数的形式"; }
        }

        public override string Description
        {
            get { return "小数的形式的练习和测试"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            string location = Assembly.GetExecutingAssembly().Location;
            DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\Decimal\FormOfDecimal");

            DataMgr.Instance.DataCreator = FormOfDecimalDataCreator.Instance;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }
    }
}
