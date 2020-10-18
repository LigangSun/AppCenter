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
    public class RelationalExpressionEntry : MathBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 2, 7, 0, 0, 0); 

        public override string Title
        {
            get { return "关系式"; }
        }

        public override string Description
        {
            get { return "关系式"; }
        }

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/Gadget.Math.Basic;component/Resources/RelationalExpression.png"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            DataMgr.Instance.ActiveMathBasicType = MathBasicType.RelationalExpression;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }

        public override string Id
        {
            get { return "39163130456F410282DBE834F83C2B75"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }
    }
}
