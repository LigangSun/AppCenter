using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.Assessment.Player.UserControls;
using SoonLearning.Assessment.Player.Data;
using System.Reflection;
using System.IO;
using SoonLearning.Assessment.Player.Data.Decimal;

namespace SoonLearning.Math.Decimal_InfiniteNonRepeatingDecimal
{
    public class Entry //: AssessmentBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 6, 17, 0, 0, 0); 

        //public override string Thumbnail
        //{
        //    get { return @"pack://application:,,,/SoonLearning.Math.Decimal_InfiniteNonRepeatingDecimal;component/InfiniteNonRepeatingDecimal.png"; }
        //}

        //public override string Id
        //{
        //    get { return "7575C6CC-D988-475B-BA1A-872C747E3CD1"; }
        //}

        //public override DateTime CreateDate
        //{
        //    get { return this.createTime; }
        //}

        //public override string Title
        //{
        //    get { return "无限小数"; }
        //}

        //public override string Description
        //{
        //    get { return "无限小数的练习和测试"; }
        //}

        //public override System.Windows.UIElement GetStartupPage()
        //{
        //    string location = Assembly.GetExecutingAssembly().Location;
        //    DataMgr.Instance.DataFolder = Path.Combine(Path.GetDirectoryName(location), @"Data\Decimal\InfiniteNonRepeatingDecimal");

        // //   DataMgr.Instance.DataCreator = InfiniteNonRepeatingDecimalDataCreator.Instance;
        //    ControlMgr.Instance.Entry = this;
        //    return ControlMgr.Instance.StartupUserControl;
        //}
    }
}
