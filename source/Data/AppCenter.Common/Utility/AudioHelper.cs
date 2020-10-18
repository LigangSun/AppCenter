using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Player;
using System.Reflection;
using SoonLearning.AppCenter.Data;
using System.Windows.Media;
using System.IO;

namespace SoonLearning.AppCenter.Utility
{
    public class AudioHelper
    {
        public static string[] audioFileList = new string[] {
            @"Audio\pass.mp3",
            @"Audio\startup.mp3",
            @"Audio\click-wator-drop.mp3"
        };

        private static bool random = false;

        private static List<string> backgroundMusicList = new List<string>();
        private static int currentBackgroundMusicIndex = 0;
        private static string defaultBackgroundMusic;
        private static Dictionary<int, AudioPlayer> audioPlayerDictionary = new Dictionary<int, AudioPlayer>();

        private static BackgroundMusicCollection onlineBackgroundMusicCollection;
        private static int downloadingIndex = 0;

        private static object backgroundMusicListLocker = new object();

        private static MediaPlayer backgroundMusicPlayer;
        private static EventHandler backgroundMusicEnded;

        private static string backgroundMusicFolder;

        public static void Init()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            for (int i = 0; i < audioFileList.Length; i++)
            {
                AudioPlayer player = new AudioPlayer(assembly, "SoonLearning.AppCenter", audioFileList[i]);
                audioPlayerDictionary.Add(i, player);
            }

            backgroundMusicEnded = (sender, e) =>
            {
                PlayBackgroundMusic(random);
            };

            UIStyleSetting.Instance.OpenSoundStateChanged += (open) =>
            {
                if (open)
                {
                    PlayBackgroundMusic(random);
                }
                else
                {
                    StopBackgroundMusic();
                }
            };

            backgroundMusicFolder = System.IO.Path.GetDirectoryName(assembly.Location);
            backgroundMusicFolder = System.IO.Path.Combine(backgroundMusicFolder, "BackgroundMusic");
            if (!Directory.Exists(backgroundMusicFolder))
                Directory.CreateDirectory(backgroundMusicFolder);

            string[] mp3Files = System.IO.Directory.GetFiles(backgroundMusicFolder, "*.mp3");
            foreach (var file in mp3Files)
            {
                backgroundMusicList.Add(file);
            }

            string[] wavFiles = System.IO.Directory.GetFiles(backgroundMusicFolder, "*.wav");
            foreach (var file in wavFiles)
            {
                backgroundMusicList.Add(file);
            }

            string[] wmaFiles = System.IO.Directory.GetFiles(backgroundMusicFolder, "*.wma");
            foreach (var file in wmaFiles)
            {
                backgroundMusicList.Add(file);
            }

            string[] aacFiles = System.IO.Directory.GetFiles(backgroundMusicFolder, "*.aac");
            foreach (var file in aacFiles)
            {
                backgroundMusicList.Add(file);
            }
        }

        public static void Clear()
        {
            if (!string.IsNullOrEmpty(defaultBackgroundMusic))
            {
                try
                {
                    File.Delete(defaultBackgroundMusic);
                }
                catch
                { }
            }

            if (backgroundMusicPlayer != null)
                backgroundMusicEnded -= backgroundMusicEnded;
            StopBackgroundMusic();
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

        public static void PlayBackgroundMusic(bool random)
        {
            AudioHelper.random = random;

            if (!UIStyleSetting.Instance.OpenSound)
                return;

            if (backgroundMusicPlayer == null)
            {
                backgroundMusicPlayer = new MediaPlayer();
                backgroundMusicPlayer.Volume = 1.0;
                backgroundMusicPlayer.MediaEnded += backgroundMusicEnded;
            }

            backgroundMusicPlayer.Stop();
            backgroundMusicPlayer.Close();

            int index = currentBackgroundMusicIndex;
            if (random)
            {
                Random rand = new Random(Environment.TickCount);
                index = rand.Next(backgroundMusicList.Count);
            }
            else
            {
                if (currentBackgroundMusicIndex < backgroundMusicList.Count - 1)
                    currentBackgroundMusicIndex++;
                else
                    currentBackgroundMusicIndex = 0;
            }

            string file = getBackgroundMusicFile(index);
            if (!string.IsNullOrEmpty(file))
            {
                backgroundMusicPlayer.Open(new Uri(file, UriKind.Absolute));
                backgroundMusicPlayer.Play();
            }
        }

        public static void StopBackgroundMusic()
        {
            if (backgroundMusicPlayer != null)
            {
                backgroundMusicPlayer.Stop();
                backgroundMusicPlayer.Close();
            }
        }

        private static string getBackgroundMusicFile(int index)
        {
            if (backgroundMusicList.Count == 0)
            {
                if (!string.IsNullOrEmpty(defaultBackgroundMusic))
                    return defaultBackgroundMusic;

                try
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    Stream s = assembly.GetManifestResourceStream("SoonLearning.AppCenter.Audio.default_background_music.mp3");

                    string tempFile = Path.GetTempFileName();
                    Path.ChangeExtension(tempFile, "mp3");

                    FileStream fs = File.OpenWrite(tempFile);
                    CopyStream(s, fs);
                    fs.Close();
                    s.Close();

                    defaultBackgroundMusic = tempFile;

                    return tempFile;
                }
                catch
                {
                }
            }
            else
            {
                return backgroundMusicList[index];
            }

            return string.Empty;
        }

        private static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        public static void UpdateBackgroundMusic(string configFileUrl)
        {
            string localConfigFile = Path.GetTempFileName();

            DownloadHelper helper = new DownloadHelper();
            helper.DownloadFileCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    startSyncBackgroundMusicFile(localConfigFile);
                }
            };
            helper.DownloadProgressChanged += (sender, e) =>
            {
            };

            helper.DownloadFile(configFileUrl, localConfigFile);
        }

        private static void startSyncBackgroundMusicFile(string localConfigFile)
        {
            try
            {
                onlineBackgroundMusicCollection = SerializerHelper<BackgroundMusicCollection>.XmlDeserialize(localConfigFile);
                if (onlineBackgroundMusicCollection.Count > 0)
                {
                    downloadingIndex = 0;
                    downloadMusicFile(onlineBackgroundMusicCollection[downloadingIndex]);
                }
            }
            catch
            {
            }
        }

        private static bool downloadMusicFile(BackgroundMusicItem item)
        {
            string localMusicFile = Path.Combine(backgroundMusicFolder, item.FileName);

            DownloadHelper helper = new DownloadHelper();
            helper.DownloadFileCompleted += (sender, e) =>
            {
            //    if (e.Error == null)
                {
                    lock (backgroundMusicListLocker)
                    {
                        backgroundMusicList.Add(localMusicFile);
                    }

                    downloadingIndex++;
                    if (downloadingIndex >= onlineBackgroundMusicCollection.Count)
                        return;

                    downloadMusicFile(onlineBackgroundMusicCollection[downloadingIndex]);
                }
            };
            helper.DownloadProgressChanged += (sender, e) =>
            {
            };

            if (File.Exists(localMusicFile))
            {
                downloadingIndex++;
                if (downloadingIndex >= onlineBackgroundMusicCollection.Count)
                    return false;

                downloadMusicFile(onlineBackgroundMusicCollection[downloadingIndex]);
                return true;
            }

            helper.DownloadFile(item.Url, localMusicFile);

            return true;
        }
    }
}
