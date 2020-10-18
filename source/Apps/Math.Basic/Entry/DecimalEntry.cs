using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Math.Basic.Data;
using SoonLearning.Math.Data;
using Math.Basic.UserControls;

namespace Math.Basic.Entry
{
    public class DecimalEntry : MathBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 2, 2, 0, 0, 0); 

        public override string Title
        {
            get { return "小数"; }
        }

        public override string Description
        {
            get { return "小数"; }
        }

        public override string Thumbnail
        {
            get { return @"pack:\\application:,,,/Gadget.Math.Basic;component/Resources/decimal.png"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            DataMgr.Instance.ActiveMathBasicType = MathBasicType.Decimal;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }

        public override string Id
        {
            get { return "8611000CAD4241CFA3FD1660434CD895"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }
    }
}
