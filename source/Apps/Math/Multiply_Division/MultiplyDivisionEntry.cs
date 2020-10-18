using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadgetCenter;
using Gadget.Math.Properties;
using System.Windows;
using SoonLearning.Math.Data;
using Gadget.Math.RapidCalculation;
using System.Windows.Media.Imaging;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Gadget.Math.Multiply_Division
{
    public class MultiplyDivisionEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 11, 4, 8, 0, 0);

        public string Title
        {
            get { return Resources.mathMultiplyDivisionTitle; }
        }

        public string Description
        {
            get { return Resources.mathMDDescription; }
        }

        public string Thumbnail
        {
            get { return @"pack:\\application:,,,/Math;component/Resources/MD_Title.png"; }
        }

        public GadgetType Tag
        {
            get { return GadgetType.Math; }
        }

        public System.Windows.UIElement GetStartupPage()
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri(@"pack://application:,,,/Math;component/Resources/DataDictionary.xaml");
            App.Current.Resources.MergedDictionaries.Add(rd);
            MathSetting.Instance.Type = MathType.Multiply_Division;
            MathAPI.MMInit(IntPtr.Zero);
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.MathStartupControl;
        }

        public IGadgetControl Control
        {
            get { return RapidGadgetControl.Instance; }
        }

        public GadgetSubType SubTag
        {
            get { return GadgetSubType.Math_Rapid; }
        }

        public string Id
        {
            get { return "D77D4B55CE2E4672B4177D914ABAF975"; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
        }

        public string Name
        {
            get { return "快学(SoonLearning)"; }
        }

        public string WebSite
        {
            get { return @"http://www.soonlearning.com"; }
        }

        public string Logo
        {
            get { return string.Empty; }
        }
    }
}
