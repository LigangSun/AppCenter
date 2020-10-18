using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using SoonLearning.Math.Data;
using Math.Basic.UserControls;
using Math.Basic.Data;

namespace Math.Basic.Entry
{
    public class UnitEntry : MathBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 2, 8, 0, 0, 0); 

        public override string Title
        {
            get { return "量"; }
        }

        public override string Description
        {
            get { return "量"; }
        }

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/Gadget.Math.Basic;component/Resources/Unit.png"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            DataMgr.Instance.ActiveMathBasicType = MathBasicType.Units;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }

        public override string Id
        {
            get { return "271DD3AA33F64D92B652895A7D4BD43F"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }
    }
}
