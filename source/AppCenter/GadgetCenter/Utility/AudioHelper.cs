using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Player;
using System.Reflection;
using SoonLearning.AppCenter.Data;
using System.Windows.Media;

namespace SoonLearning.AppCenter.Utility
{
    public class AudioHelper
    {
        public static string[] audioFileList = new string[] {
            @"Resources\Audio\pass.mp3",
            @"Resources\Audio\startup.mp3",
            @"Resources\Audio\click-wator-drop.mp3"
        };

        private static Dictionary<int, AudioPlayer> audioPlayerDictionary = new Dictionary<int, AudioPlayer>();

        private static MediaPlayer backgroundMusicPlayer;

        internal static void Init()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            for (int i = 0; i < audioFileList.Length; i++)
            {
                AudioPlayer player = new AudioPlayer(assembly, "SoonLearning.AppCenter", audioFileList[i]);
                audioPlayerDictionary.Add(i, player);
            }
        }

        public static void PlayPassSound()
        {
            if (!UIStyleSetting.Instance.OpenSound)
                return;

            audioPlayerDictionary[0].Play();
        }

        public static void PlayStartupSound()
        {
            if (!UIStyleSetting.Instance.OpenSound)
                return;

            audioPlayerDictionary[1].Play();
        }

        public static void PlayWaterDropSound()
        {
            audioPlayerDictionary[2].Play();
        }

        private static void RandomPlayBackgroundMusic()
        {
            if (backgroundMusicPlayer != null)
            {
                backgroundMusicPlayer.Stop();
            }
        }
    }
}
