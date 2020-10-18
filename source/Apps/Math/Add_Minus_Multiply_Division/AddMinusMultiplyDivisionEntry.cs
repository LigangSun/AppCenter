using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadgetCenter;
using Gadget.Math.RapidCalculation;
using System.Windows.Media.Imaging;
using System.Windows;
using Gadget.Math.Properties;
using SoonLearning.Math.Data;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Gadget.Math.Add_Minus_Multiply_Division
{
    public class AddMinusMultiplyDivisionEntry : MarshalByRefObject, IGadgetEntry//, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 11, 5, 8, 0, 0);

        public string Title
        {
            get { return Resources.mathAddMinusMultiplyDivisionTitle; }
        }

        public string Description
        {
            get { return Resources.mathAMMDDescription; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/Math;component/Resources/Add_Minus_Multiply_Division.png"; }
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
            MathSetting.Instance.Type = MathType.Add_Minus_Multiply_Division;
            MathAPI.MCInit(IntPtr.Zero);
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
            get { return "0F16FDF1F6854FDAB49603387189FD92"; }
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

        public System.Windows.Media.Imaging.BitmapImage Logo
        {
            get { return null; }
        }
    }
}
