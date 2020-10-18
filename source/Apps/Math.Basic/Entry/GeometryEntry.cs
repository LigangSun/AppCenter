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
    public class GeometryEntry : MathBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 2, 9, 0, 0, 0); 

        public override string Title
        {
            get { return "几何"; }
        }

        public override string Description
        {
            get { return "几何"; }
        }

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/Gadget.Math.Basic;component/Resources/Geometry.png"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            DataMgr.Instance.ActiveMathBasicType = MathBasicType.Geometry;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }

        public override string Id
        {
            get { return "362C686F0BA94DCB82878146AACC36DA"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }
    }
}
