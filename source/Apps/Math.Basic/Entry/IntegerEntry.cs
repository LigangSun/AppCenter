using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Math.Basic.UserControls;
using SoonLearning.Math.Data;
using Math.Basic.Data;

namespace Math.Basic.Entry
{
    public class IntegerEntry : MathBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 2, 1, 0, 0, 0); 

        public override string Title
        {
            get { return "整数"; }
        }

        public override string Description
        {
            get { return "整数"; }
        }

        public override string Thumbnail
        {
            get { return @"pack:\\application:,,,/Gadget.Math.Basic;component/Resources/integer.png"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            DataMgr.Instance.ActiveMathBasicType = MathBasicType.Integer;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }

        public override string Id
        {
            get { return "E6BE9A1D10BE4D8CA81A89B0438F7963"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }
    }
}
