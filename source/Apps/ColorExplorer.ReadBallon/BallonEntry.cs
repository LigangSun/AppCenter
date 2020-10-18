using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;
using System.Windows.Media.Imaging;

namespace SoonLearning.ColorExplorer.ReadBallon
{
    public class BallonEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 12, 3, 12, 0, 0);

        public string Title
        {
            get { return "说出颜色（气球）"; }
        }

        public string Description
        {
            get { return "说出飞向天空的气球的颜色"; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/ColorRecognize.png"; }
        }

        public int Tag
        {
            get { return 100; }
        }

        public System.Windows.UIElement GetStartupPage()
        {
            return BallonUserControl.Instance;
        }

        public int SubTag
        {
            get { return 101; }
        }

        public string Id
        {
            get { return "A19A3144C0B246B8896BA5A0ACBEBE41"; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
        }

        public string Name
        {
            get { return "上海速学信息科技有限公司(SoonLearning)"; }
        }

        public string WebSite
        {
            get { return @"http://www.soonlearning.com"; }
        }

        public string Logo
        {
            get { return string.Empty; }
        }

        public void Uninstall()
        {
        }

        public AppCapability Capability
        {
            get { return AppCapability.SpeechRecognization; }
        }
    }
}
