using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;
using SoonLearning.AppCenter;

namespace SoonLearning.ConnectNumber
{
    internal class ControlMgr
    {
        private static ControlMgr instance;

        public static ControlMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ControlMgr();
                }

                return instance;
            }
        }

        private ControlMgr()
        {
        //    ResourceDictionary rd = new ResourceDictionary();
        //    rd.Source = new Uri(@"pack://application:,,,/DrawNumber;component/Resources/DrawNumberDictionary.xaml");
        //    App.Current.Resources.MergedDictionaries.Add(rd);
        }

        private Dictionary<string, UIElement> controlDictionary = new Dictionary<string, UIElement>();
        private DataMgr dataMgr;

        private UIElement GetControl(string key, string type)
        {
            if (!this.controlDictionary.ContainsKey(key))
            {
                Assembly assembly = Assembly.GetCallingAssembly();
                this.controlDictionary.Add(key, assembly.CreateInstance(type) as UIElement);
            }

            return this.controlDictionary[key];
        }

        internal DrawNumberStartupPage DrawNumberStartupPage
        {
            get
            {
                string key = typeof(DrawNumberStartupPage).FullName;
                return GetControl(key, key) as DrawNumberStartupPage;
            }
        }

        internal DrawNumberList DrawNumberList
        {
            get
            {
                string key = typeof(DrawNumberList).FullName;
                return GetControl(key, key) as DrawNumberList;
            }
        }

        internal DrawNumberPage DrawNumberPage
        {
            get
            {
                string key = typeof(DrawNumberPage).FullName;
                return GetControl(key, key) as DrawNumberPage;
            }
        }

        internal DataMgr DataMgr
        {
            get
            {
                if (this.dataMgr == null)
                    this.dataMgr = new DataMgr();
                return this.dataMgr;
            }
        }
    }
}
