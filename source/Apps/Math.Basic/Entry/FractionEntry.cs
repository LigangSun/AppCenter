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
    public class FractionEntry : MathBasicEntry
    {
        private DateTime createTime = new DateTime(2012, 2, 3, 0, 0, 0); 

        public override string Title
        {
            get { return "分数"; }
        }

        public override string Description
        {
            get { return "分数"; }
        }

        public override string Thumbnail
        {
            get { return @"pack://application:,,,/Gadget.Math.Basic;component/Resources/Fraction.png"; }
        }

        public override System.Windows.UIElement GetStartupPage()
        {
            DataMgr.Instance.ActiveMathBasicType = MathBasicType.Fraction;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.StartupUserControl;
        }

        public override string Id
        {
            get { return "C53B156461CB4363ABF36382D8D92159"; }
        }

        public override DateTime CreateDate
        {
            get { return this.createTime; }
        }
    }
}
