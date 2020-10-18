using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter;
using System.Windows;
using System.Reflection;
using SoonLearning.BlockPuzzle.Data;
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.AppCenter.Player;

namespace SoonLearning.BlockPuzzle.Puzzle
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

        private AudioPlayer blockMoveSound;

        private ControlMgr()
        {
            ResourceDictionary rd = new ResourceDictionary();
            rd.Source = new Uri(@"pack://application:,,,/SoonLearning.BlockPuzzle;component/Resources/PuzzleDictionary.xaml");
            App.Current.Resources.MergedDictionaries.Add(rd);

            if (blockMoveSound == null)
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                blockMoveSound = new AudioPlayer(assembly, "SoonLearning.BlockPuzzle", @"Resources\click.mp3");
            }
        }

        private Dictionary<string, UIElement> controlDictionary = new Dictionary<string, UIElement>();
        private Dictionary<string, DataMgr> dataMgrDictionary = new Dictionary<string, DataMgr>();
        private IGadgetEntry entry;

        public PuzzleStartupPage PuzzleStartupPage
        {
            get
            {
                PuzzleStartupPage startupPage = this.GetControl(PuzzleSetting.Instance.Type.ToString() + "_PuzzleStartupPage", typeof(PuzzleStartupPage).FullName) as PuzzleStartupPage;
                return startupPage;
            }
        }


        public PuzzleListPage PuzzleListPage
        {
            get
            {
                return this.GetControl(PuzzleSetting.Instance.Type.ToString() + "_ListPage", typeof(PuzzleListPage).FullName) as PuzzleListPage;
            }
        }

        internal PuzzlePage PuzzlePage
        {
            get
            {
                return this.GetControl(PuzzleSetting.Instance.Type.ToString() + "_PuzzlePage", typeof(PuzzlePage).FullName) as PuzzlePage;
            }
        }

        internal NewPuzzleWindow NewPuzzleWindow
        {
            get
            {
                return this.GetControl("NewPuzzleWin", typeof(NewPuzzleWindow).FullName) as NewPuzzleWindow;
            }
        }

        internal DataMgr DataMgr
        {
            get
            {
                return this.GetDataMgr(PuzzleSetting.Instance.Type.ToString() + "_DataMgr", typeof(DataMgr).FullName);
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

        private DataMgr GetDataMgr(string key, string type)
        {
            if (!this.dataMgrDictionary.ContainsKey(key))
            {
                Assembly assembly = Assembly.GetCallingAssembly();
                this.dataMgrDictionary.Add(key, assembly.CreateInstance(type) as DataMgr);
            }

            return this.dataMgrDictionary[key];
        }

        internal void PlayClickSound()
        {
            if (SoonLearning.AppCenter.Data.UIStyleSetting.Instance.OpenSound)
                blockMoveSound.Play();
        }
    }
}
