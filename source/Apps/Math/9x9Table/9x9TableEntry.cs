using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadgetCenter;
using Gadget.Math.Properties;
using System.Windows.Media.Imaging;
using System.Windows;
using Gadget.Math.RapidCalculation;
using SoonLearning.Math.Data;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Gadget.Math._9x9Table
{
    public class _9x9TableEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 11, 6, 8, 0, 0);

        public string Title
        {
            get { return Resources.NineXNineTableTitle; }
        }

        public string Description
        {
            get { return Resources.NineXNineTableDescription; }
        }

        public string Thumbnail
        {
            get { return @"pack:\\application:,,,/Math;component/Resources/9x9Table.png"; }
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
            MathSetting.Instance.Type = MathType.Nine_Nine_Table;
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
            get { return "8B36742B04DF4648A29AB8B87DBC1F96"; }
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
