using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;
using SoonLearning.Math.Data;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.Math.Fast.RapidCalculation
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

        public AddMinusStartupControl MathStartupControl
        {
            get
            {
                return GetControl(MathSetting.Instance.Type.ToString() + "_StartupPage", typeof(AddMinusStartupControl).FullName) as AddMinusStartupControl;
            }
        }

        internal AddMinusSettingControl MathSettingControl
        {
            get
            {
                return GetControl(MathSetting.Instance.Type.ToString() + "_SettingPage", typeof(AddMinusSettingControl).FullName) as AddMinusSettingControl;
            }
        }

        internal AddMinusQuestionControl MathQuestionControl
        {
            get
            {
                return GetControl(MathSetting.Instance.Type.ToString() + "_QuestionControl", typeof(AddMinusQuestionControl).FullName) as AddMinusQuestionControl;
            }
        }

        internal AddMinusResultControl MathResultControl
        {
            get
            {
                return GetControl(MathSetting.Instance.Type.ToString() + "_ResultPage", typeof(AddMinusResultControl).FullName) as AddMinusResultControl;
            }
        }

        internal RapidHistoryUserControl HistoryUserControl
        {
            get
            {
                return GetControl(MathSetting.Instance.Type.ToString() + "_HistoryPage", typeof(RapidHistoryUserControl).FullName) as RapidHistoryUserControl;
            }
        }

        internal PrintWindow PrintWindow
        {
            get
            {
                return GetControl(MathSetting.Instance.Type.ToString() + "_PrintWindow", typeof(PrintWindow).FullName) as PrintWindow;
            }
        }

        internal ViewDetailUserControl DetailUserControl
        {
            get
            {
                return GetControl(MathSetting.Instance.Type.ToString() + "_PrintWindow", typeof(ViewDetailUserControl).FullName) as ViewDetailUserControl;
            }
        }

        public IGadgetEntry Entry
        {
            set { this.entry = value; }
            get { return this.entry; }
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
