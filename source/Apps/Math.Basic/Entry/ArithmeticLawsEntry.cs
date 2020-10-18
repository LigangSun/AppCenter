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
    public class ArithmeticLawsEntry : MathBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 2, 5, 0, 0, 0); 

        public override string Title
        {
            get { return "运算法则"; }
        }

        public override string Description
        {
            get { return "运算法则"; }
        }

        public override string Thumbnail
        {
            get { return @"pack:\\application:,,,/Gadget.Math.Basic;component/Resources/arithmeticlaws.png"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            DataMgr.Instance.ActiveMathBasicType = MathBasicType.ArithmeticLaws;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }

        public override string Id
        {
            get { return "DD8AEA212D674CFCA5057B0429B5CB6B"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }
    }
}
