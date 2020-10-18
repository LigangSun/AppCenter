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
    public class EquationEntry : MathBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 2, 6, 0, 0, 0); 

        public override string Title
        {
            get { return "方程"; }
        }

        public override string Description
        {
            get { return "方程"; }
        }

        public override string Thumbnail
        {
            get { return @"pack:\\application:,,,/Gadget.Math.Basic;component/Resources/Equation.png"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            DataMgr.Instance.ActiveMathBasicType = MathBasicType.Equation;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }

        public override string Id
        {
            get { return "D7F718E6218F4DFB957DB887DB35AA2E"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }
    }
}
