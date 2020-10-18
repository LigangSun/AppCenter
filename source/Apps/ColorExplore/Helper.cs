using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Controls;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.ColorExplorer
{
    public class Helper
    {
        private static Helper helper;

        public static Helper Instance
        {
            get
            {
                if (helper == null)
                    helper = new Helper();

                return helper;
            }
        }

        private string objectCrachMusicFile;
        private string backgroundMusicFile;

        static Helper()
        {
        }

        private Helper()
        {
            this.InitMusic();
        }

        private void InitMusic()
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            this.backgroundMusicFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\background.mp3");
            this.objectCrachMusicFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\BallonCrack.mp3");
        }

        public void StartBackgroundMusic(PassData pass, MediaElement me)
        {
            string background = pass.BackgroundMusic;
            if (string.IsNullOrEmpty(background))
            {
                background = this.backgroundMusicFile;
            }

            if (System.IO.File.Exists(background))
            {
                AudioHelper.StopBackgroundMusic();
            }

            if (System.IO.File.Exists(background))
            {
                me.Stop();
                me.Source = new Uri(background, UriKind.Absolute);
                me.Play();
            }
        }

        public void PlayObjectCrackMusic(int index, MediaElement me)
        {
            string crashMusic = DataMgr.Instance.ActiveObjectCollection[index].CrashMusic;
            if (string.IsNullOrEmpty(crashMusic))
                crashMusic = this.objectCrachMusicFile;

            if (System.IO.File.Exists(crashMusic))
            {
                me.Stop();
                me.Source = new Uri(crashMusic, UriKind.Absolute);
                me.Play();
            }
        }
    }
}
