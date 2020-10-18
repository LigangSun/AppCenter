using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SoonLearning.ColorExplorer
{
    public class DataMgr
    {
        private static DataMgr instance;

        public static DataMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataMgr();

                return instance;
            }
        }

        private ObservableCollection<ColorObject> ballonCollection;
        private Dictionary<int, BitmapImage> objectImageDictionary = new Dictionary<int, BitmapImage>();

        private ObservableCollection<ColorObject> BallonCollection
        {
            get
            {
                if (this.ballonCollection == null)
                {
                    this.ballonCollection = new ObservableCollection<ColorObject>();
                    this.ballonCollection.Add(new ColorObject(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/ballon-Red.png", this.GetCrackMusic(0), "红色$红"));
                    this.ballonCollection.Add(new ColorObject(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/ballon-Green.png", this.GetCrackMusic(1), "绿色$绿"));
                    this.ballonCollection.Add(new ColorObject(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/ballon-pink.png", this.GetCrackMusic(2), "粉红色$粉红$粉"));
                    this.ballonCollection.Add(new ColorObject(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/ballon-white.png", this.GetCrackMusic(2), "白色$白"));
                    this.ballonCollection.Add(new ColorObject(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/ballon-yellow.png", this.GetCrackMusic(3), "黄色$黄"));
                    this.ballonCollection.Add(new ColorObject(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/ballon-pale-blue.png", this.GetCrackMusic(1), "蓝色$蓝"));
                    this.ballonCollection.Add(new ColorObject(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/ballon-orange.png", this.GetCrackMusic(2), "橙色$橙$桔"));
                    this.ballonCollection.Add(new ColorObject(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/ballon-black.png", this.GetCrackMusic(2), "黑色$黑"));
                }

                return this.ballonCollection;
            }
        }

        public ObservableCollection<ColorObject> ActiveObjectCollection
        {
            get
            {
                return this.BallonCollection;
            }
        }

        public BitmapImage GetObjectImage(int index)
        {
            if (!this.objectImageDictionary.ContainsKey(index))
            {
                BitmapImage bi = new BitmapImage(new Uri(this.ActiveObjectCollection[index].File));
                bi.DecodePixelHeight = bi.PixelHeight;
                bi.DecodePixelWidth = bi.PixelWidth;
                this.objectImageDictionary.Add(index, bi);
            }

            return this.objectImageDictionary[index];
        }

        private ObservableCollection<PassData> passCollection;
        private int currentPass = -1;

        public ObservableCollection<PassData> PassCollection
        {
            get
            {
                if (this.passCollection == null)
                {
                    this.passCollection = new ObservableCollection<PassData>();

                    // Level 1
                    this.passCollection.Add(new PassData(8f, 8, 10, 6, this.GetBackgroundImage(0), this.GetBackgroundMusic(0)));
                    this.passCollection.Add(new PassData(7f, 7, 10, 6, this.GetBackgroundImage(1), this.GetBackgroundMusic(1)));
                    this.passCollection.Add(new PassData(6f, 6, 10, 6, this.GetBackgroundImage(2), this.GetBackgroundMusic(2)));
                    this.passCollection.Add(new PassData(5f, 5, 12, 8, this.GetBackgroundImage(3), this.GetBackgroundMusic(3)));
                    this.passCollection.Add(new PassData(4f, 4, 14, 9, this.GetBackgroundImage(4), this.GetBackgroundMusic(4)));
                    this.passCollection.Add(new PassData(3f, 3, 16, 10, this.GetBackgroundImage(5), this.GetBackgroundMusic(5)));
                    this.passCollection.Add(new PassData(2f, 2, 18, 11, this.GetBackgroundImage(6), this.GetBackgroundMusic(6)));
                    this.passCollection.Add(new PassData(1.5f, 1.5, 20, 12, this.GetBackgroundImage(7), this.GetBackgroundMusic(7)));

                    // Level 2
                    this.passCollection.Add(new PassData(8f, 4, 10, 6, this.GetBackgroundImage(0), this.GetBackgroundMusic(0)));
                    this.passCollection.Add(new PassData(7f, 3.5, 10, 6, this.GetBackgroundImage(1), this.GetBackgroundMusic(1)));
                    this.passCollection.Add(new PassData(6f, 3, 10, 6, this.GetBackgroundImage(2), this.GetBackgroundMusic(2)));
                    this.passCollection.Add(new PassData(5f, 2.5, 12, 8, this.GetBackgroundImage(3), this.GetBackgroundMusic(3)));
                    this.passCollection.Add(new PassData(4f, 2, 14, 9, this.GetBackgroundImage(4), this.GetBackgroundMusic(4)));
                    this.passCollection.Add(new PassData(3f, 1.5, 16, 10, this.GetBackgroundImage(5), this.GetBackgroundMusic(5)));
                    this.passCollection.Add(new PassData(2f, 1, 18, 11, this.GetBackgroundImage(6), this.GetBackgroundMusic(6)));
                    this.passCollection.Add(new PassData(1f, 0.5, 20, 12, this.GetBackgroundImage(7), this.GetBackgroundMusic(7)));
                }

                return this.passCollection;
            }
        }

        public PassData FistPass
        {
            get
            {
                this.currentPass = -1;
                return NextPass;
            }
        }

        public PassData NextPass
        {
            get
            {
                if (this.currentPass == this.PassCollection.Count - 1)
                    this.currentPass = 0;
                else
                    this.currentPass++;

                return this.PassCollection[this.currentPass];
            }
        }

        public PassData PrePass
        {
            get
            {
                if (this.currentPass == 0)
                    this.currentPass = this.PassCollection.Count - 1;
                else
                    this.currentPass--;

                return this.PassCollection[this.currentPass];
            }
        }

        private List<string> backgroundMusicList;

        private string GetBackgroundMusic(int index)
        {
            if (this.backgroundMusicList == null)
            {
                this.backgroundMusicList = new List<string>();

                Assembly assembly = Assembly.GetCallingAssembly();
                string background = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Voice\ColorExplore\background.mp3");
                this.backgroundMusicList.Add(background);

                background = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\background_1.mp3");
                this.backgroundMusicList.Add(background);

                background = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\background_2.mp3");
                this.backgroundMusicList.Add(background);

                background = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\background_3.mp3");
                this.backgroundMusicList.Add(background);

                background = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\background_4.mp3");
                this.backgroundMusicList.Add(background);

                background = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\background_5.mp3");
                this.backgroundMusicList.Add(background);

                background = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\background_6.mp3");
                this.backgroundMusicList.Add(background);

                background = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\background_7.mp3");
                this.backgroundMusicList.Add(background);
            }

            return this.backgroundMusicList[index];
        }

        private List<string> crackMusicList;

        private string GetCrackMusic(int index)
        {
            if (this.crackMusicList == null)
            {
                this.crackMusicList = new List<string>();

                Assembly assembly = Assembly.GetCallingAssembly();
                string crack = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\1.mp3");
                this.crackMusicList.Add(crack);

                crack = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\2.mp3");
                this.crackMusicList.Add(crack);

                crack = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\3.mp3");
                this.crackMusicList.Add(crack);

                crack = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), @"Data\ColorExplore\4.mp3");
                this.crackMusicList.Add(crack);
            }

            return this.crackMusicList[index];
        }

        private List<string> backgroundImageList;

        private string GetBackgroundImage(int index)
        {
            if (this.backgroundImageList == null)
            {
                this.backgroundImageList = new List<string>();

                this.backgroundImageList.Add(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/background.jpg");
                this.backgroundImageList.Add(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/background_1.jpg");
                this.backgroundImageList.Add(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/background_2.jpg");
                this.backgroundImageList.Add(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/background_3.jpg");
                this.backgroundImageList.Add(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/background_4.jpg");
                this.backgroundImageList.Add(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/background_5.jpg");
                this.backgroundImageList.Add(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/background_6.jpg");
                this.backgroundImageList.Add(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/background_7.jpg");
                this.backgroundImageList.Add(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/background_8.jpg");
                this.backgroundImageList.Add(@"pack://application:,,,/SoonLearning.ColorExplorer;component/Images/background_9.jpg");
            }

            return this.backgroundImageList[index];
        }
    }
}
