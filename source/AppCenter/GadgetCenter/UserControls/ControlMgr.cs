using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using SoonLearning.AppCenter.Windows;
using System.Reflection;

namespace SoonLearning.AppCenter.UserControls
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

        private Dictionary<string, UserControl> controlDictionary = new Dictionary<string, UserControl>();
        private AppStoreWindow appStoreWindow;

        internal GadgetListUserControl GadgetListUserControl
        {
            get
            {
                foreach (GadgetListUserControl ctrl in GetControl(typeof(GadgetListUserControl).FullName))
                    return ctrl;

                return null;
            }
        }

        internal GadgetContainerUserControl GadgetContainerUserControl
        {
            get
            {
                foreach (GadgetContainerUserControl ctrl in GetControl(typeof(GadgetContainerUserControl).FullName))
                    return ctrl;

                return null;
            }
        }

        internal SubTypeListUserControl SubTypeListUserControl
        {
            get
            {
                foreach (SubTypeListUserControl ctrl in GetControl(typeof(SubTypeListUserControl).FullName))
                    return ctrl;

                return null;
            }
        }

        internal SettingWindow SettingWindow
        {
            get
            {
                foreach (SettingWindow ctrl in GetControl(typeof(SettingWindow).FullName))
                    return ctrl;

                return null;
            }
        }

        internal AboutWindow AboutWindow
        {
            get
            {
                foreach (AboutWindow ctrl in GetControl(typeof(AboutWindow).FullName))
                    return ctrl;

                return null;
            }
        }

        internal ExitUserControl ExitUserControl
        {
            get
            {
                foreach (ExitUserControl ctrl in GetControl(typeof(ExitUserControl).FullName))
                    return ctrl;

                return null;
            }
        }

        internal AppManagementUserControl AppManagementUserControl
        {
            get
            {
                foreach (AppManagementUserControl ctrl in GetControl(typeof(AppManagementUserControl).FullName))
                    return ctrl;

                return null;
            }
        }

        internal AppDescriptionUserControl AppDescriptionUserControl
        {
            get
            {
                foreach (AppDescriptionUserControl ctrl in GetControl(typeof(AppDescriptionUserControl).FullName))
                    return ctrl;

                return null;
            }
        }

        internal AppInstallingListUserControl AppInstallingListUserControl
        {
            get
            {
                foreach (AppInstallingListUserControl ctrl in GetControl(typeof(AppInstallingListUserControl).FullName))
                    return ctrl;

                return null;
            }
        }

        internal HelpWindow HelpWindow
        {
            get
            {
                foreach (HelpWindow ctrl in GetControl(typeof(HelpWindow).FullName))
                    return ctrl;

                return null;
            }
        }

        internal AppStoreWindow AppStoreWindow
        {
            get
            {
                if (this.appStoreWindow == null)
                {
                    this.appStoreWindow = new AppStoreWindow();
                }

                return this.appStoreWindow;
            }
        }

        private IEnumerable<UserControl> GetControl(string type)
        {
            if (!this.controlDictionary.ContainsKey(type))
            {
                Assembly assembly = Assembly.GetCallingAssembly();
                this.controlDictionary.Add(type, assembly.CreateInstance(type) as UserControl);
            }

            yield return this.controlDictionary[type];
        }
    }
}
