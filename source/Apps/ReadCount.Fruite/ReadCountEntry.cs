using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Reflection;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.ReadCount.Fruite.Properties;

namespace SoonLearning.ReadCount.Fruite
{
    public class ReadCountEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2012, 1, 1, 1, 1, 1);

        public string Title
        {
            get { return Resources.CountFruiteTitle; }
        }

        public string Description
        {
            get { return Resources.CountFruiteDescription; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.ReadCount.Fruite;component/Resources/CountFruit.png"; }
        }

        public int Tag
        {
            get { return 200; }
         }

        public System.Windows.UIElement GetStartupPage()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyLocation = System.IO.Path.GetDirectoryName(assembly.Location);
            ReadCountStartupPage.Instance.DataBasePath = System.IO.Path.Combine(assemblyLocation, @"Data\ReadCount\Fruite");
            return ReadCountStartupPage.Instance;
        }

        public bool GoBack()
        {
            return true;
        }

        public int SubTag
        {
            get { return 201; }
        }

        public string Id
        {
            get { return "9482F448717545D0B43C5DC320EFFA03"; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
        }

        public string Name
        {
            get { return "速学信息科技(SoonLearning)"; }
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
            ReadCountStartupPage.Instance.Uninstall();
        }

        public AppCapability Capability
        {
            get { return AppCapability.BackgroundMusic | AppCapability.SpeechRecognization; }
        }
    }
}
