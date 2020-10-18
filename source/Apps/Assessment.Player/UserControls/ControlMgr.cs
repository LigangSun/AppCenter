using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using SoonLearning.AppCenter;
using System.Reflection;
using SoonLearning.Assessment.Data;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Player.Entry;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.Assessment.Player.UserControls
{
    public class ControlMgr
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
            
        }

        private Dictionary<string, UIElement> controlDictionary = new Dictionary<string, UIElement>();
        private IGadgetEntry entry;

        private Dictionary<MathBasicType, string> idDictionary = new Dictionary<MathBasicType, string>();

        public StartupUserControl StartupUserControl
        {
            get
            {
                if (ControlMgr.Instance.Entry == null)
                    return this.GetControl("_AssessmentApp_StartupUserControl", typeof(StartupUserControl).FullName) as StartupUserControl;

                return this.GetControl(ControlMgr.Instance.Entry.ToString() + "_StartupUserControl", 
                    typeof(StartupUserControl).FullName) as StartupUserControl;
            }
        }

        public IGadgetEntry Entry
        {
            get { return this.entry; }
            set { this.entry = value; }
        }

        private UIElement GetControl(string key, string type)
        {
            if (!this.controlDictionary.ContainsKey(key))
            {
                Assembly assembly = Assembly.GetCallingAssembly();
                this.controlDictionary.Add(key, assembly.CreateInstance(type) as UIElement);
            }

            return this.controlDictionary[key];
        }
    }
}
