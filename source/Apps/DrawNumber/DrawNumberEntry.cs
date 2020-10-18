using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using SoonLearning.AppCenter;
using SoonLearning.ConnectNumber.Properties;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;
using System.Reflection;

namespace SoonLearning.ConnectNumber
{
    public class DrawNumberEntry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 10, 5, 8, 0, 0);

        public string Title
        {
            get { return Resources.DrawNumberTitle; }
        }

        public string Description
        {
            get { return Resources.DrawNumberDescription; }
        }

        public string Thumbnail
        {
            get { return @"pack://application:,,,/SoonLearning.ConnectNumber;component/Resources/ConnectNumber.png"; }
        }

        public int Tag
        {
            get { return 100; }
        }

        public System.Windows.UIElement GetStartupPage()
        {
            return ControlMgr.Instance.DrawNumberStartupPage;
        }

        public int SubTag
        {
            get { return 101; }
        }

        public string Id
        {
            get { return "A3031C587426429D877D68CF9BCC706C"; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
        }

        public string Name
        {
            get { return "速学(SoonLearning)"; }
        }

        public string WebSite
        {
            get { return @"http://www.soonlearning.com"; }
        }

        public string Logo
        {
            get { return string.Empty; }
        }

        public AppCapability Capability
        {
            get { return AppCapability.BackgroundMusic; }
        }

        public void Uninstall()
        {
            try
            {
                Assembly assembly = Assembly.GetCallingAssembly();
                string dataFolder = System.IO.Path.GetDirectoryName(assembly.Location);
                dataFolder = System.IO.Path.Combine(dataFolder, @"Data\DrawNumber");
                System.IO.Directory.Delete(dataFolder, true);
            }
            catch
            {
            }
        }
    }
}
