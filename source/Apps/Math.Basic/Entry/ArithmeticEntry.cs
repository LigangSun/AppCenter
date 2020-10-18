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
    public class ArithmeticEntry : MathBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 2, 4, 0, 0, 0);

        public override string Title
        {
            get { return "四则运算"; }
        }

        public override string Description
        {
            get { return "四则运算"; }
        }

        public override string Thumbnail
        {
            get { return @"pack:\\application:,,,/Gadget.Math.Basic;component/Resources/arithmetic.png"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            DataMgr.Instance.ActiveMathBasicType = MathBasicType.Arithmetic;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }

        public override string Id
        {
            get { return "50CAA4200F4F411CA3BABDEF14B657A1"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }
    }
}
