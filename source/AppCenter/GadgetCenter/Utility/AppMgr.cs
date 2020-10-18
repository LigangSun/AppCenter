using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SoonLearning.AppCenter.Data;
using System.Reflection;
using SoonLearning.AppCenter.Win32;

namespace SoonLearning.AppCenter.Utility
{
    internal class AppMgr
    {
        private static AppMgr instance;

        public static AppMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AppMgr();
                }

                return instance;
            }
        }

        private AppMgr()
        {

        }

        private object dicLocker = new object();
        private Dictionary<string, Process> appIdProcessDictionary = new Dictionary<string, Process>();

        public void Start(AppItem item)
        {
            Process process = this.IsAppRunning(item.Id);
            if (process != null)
            {
                this.ActiveApp(process.MainWindowHandle);
                return;
            }

            if (this.appIdProcessDictionary.ContainsKey(item.Id))
            {
                this.appIdProcessDictionary.Remove(item.Id);
            }
            
            foreach (Process p in LaunchApp(item))
            {
                process = p;
            }

            lock (this.dicLocker)
            {
                this.appIdProcessDictionary.Add(item.Id, process);
            }

            if (process == null)
                return;

            this.ActiveApp(process.MainWindowHandle);
        }

        private void ActiveApp(IntPtr handle)
        {
        //    Win32API.SetWindowPos(handle, new IntPtr(-1), 0, 0, 0, 0, Win32API.SWP_NOMOVE | Win32API.SWP_NOSIZE);
            Win32API.SetActiveWindow(handle);
        //    Win32API.SetWindowPos(handle, new IntPtr(-2), 0, 0, 0, 0, Win32API.SWP_NOMOVE | Win32API.SWP_NOSIZE);
        }

        private IEnumerable<Process> LaunchApp(AppItem item)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            string folder = System.IO.Path.GetDirectoryName(currentAssembly.Location);
            Process p = Process.Start(System.IO.Path.Combine(folder, "SoonLearning.AppCenter.Host.exe"), "\"" + item.AppEntryFile + "\" " + item.FullName +
                " " + UIStyleSetting.Instance.SpeechRecognizer.ToString() +
                " " + UIStyleSetting.Instance.FullScreen.ToString() +
                " " + UIStyleSetting.Instance.OpenSound.ToString());
            yield return p;
        }

        private Process IsAppRunning(string appId)
        {
            lock (this.dicLocker)
            {
                if (this.appIdProcessDictionary.ContainsKey(appId))
                {
                    Process appProcess = this.appIdProcessDictionary[appId];
                    try
                    {
                        return Process.GetProcessById(appProcess.Id);
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            return null;
        }

        internal void StopAll()
        {
            foreach (string key in this.appIdProcessDictionary.Keys)
            {
                try
                {
                    this.appIdProcessDictionary[key].Kill();
                }
                catch
                {
                }
            }
        }
    }
}
