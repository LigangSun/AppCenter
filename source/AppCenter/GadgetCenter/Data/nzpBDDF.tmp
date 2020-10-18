using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Media;
using GadgetCenter.Utility;
using System.Windows.Forms;
using System.IO;
using System.Windows;

namespace GadgetCenter.Data
{
    public class UIStyleSetting : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Color startColor = Colors.Black;//Colors.Maroon;
        private Color endColor = Colors.Black;//Colors.Red;

        private Color foregroundColor = Colors.White;

        private string backgroundImage = @"pack://application:,,,/Resources/Images/ThumbnailBackground.png";

        private bool openSound = true;
        private bool fullScreen;
        private bool speechRecognizer;

        private int thumbnailWidth = 138;
        private int thumbnailHeight = 128;

        private int thumbnailHighlightWidth = 160;
        private int thumbnailHighlightHeight = 150;

        private int uiStyle = 0;
        private List<string> uiStyleList = new List<string>();

        private int backgroundImageIndex = 0;
        private List<string> backgroundImageList = new List<string>();

        private Thickness mainBorderThickness;

        public Color StartColor
        {
            get { return this.startColor; }
            set
            {
                this.startColor = value;
                this.OnPropertyChanged("StartColor");
            }
        }

        public Color EndColor
        {
            get { return this.endColor; }
            set
            {
                this.endColor = value;
                this.OnPropertyChanged("EndColor");
            }
        }

        public Color ForegroundColor
        {
            get { return this.foregroundColor; }
            set
            {
                this.foregroundColor = value;
                this.OnPropertyChanged("ForegroundColor");
            }
        }

        public string BackgroundImage
        {
            get { return this.backgroundImageList[this.backgroundImageIndex]; }
            set
            {
                this.backgroundImage = value;
                this.OnPropertyChanged("BackgroundImage");
            }
        }

        public bool OpenSound
        {
            get { return this.openSound; }
            set
            {
                this.openSound = value;
                this.OnPropertyChanged("OpenSound");
            }
        }

        public bool FullScreen
        {
            get { return this.fullScreen; }
            set
            {
                this.fullScreen = value;
                this.OnPropertyChanged("FullScreen");
            }
        }   

        public bool SpeechRecognizer
        {
            get { return this.speechRecognizer; }
            set
            {
                this.speechRecognizer = value;
                this.OnPropertyChanged("SpeechRecognizer");
            }
        }

        public int ThumbnailWidth
        {
            get { return this.thumbnailWidth; }
            set
            {
                this.thumbnailWidth = value;
                this.OnPropertyChanged("ThumbnailWidth");
            }
        }

        public int ThumbnailHeight
        {
            get { return this.thumbnailHeight; }
            set
            {
                this.thumbnailHeight = value;
                this.OnPropertyChanged("ThumbnailHeight");
            }
        }

        public int ThumbnailHighlightWidth
        {
            get { return this.thumbnailHighlightWidth; }
            set
            {
                this.thumbnailHighlightWidth = value;
                this.OnPropertyChanged("ThumbnailHighlightWidth");
            }
        }

        public int ThumbnailHighlightHeight
        {
            get { return this.thumbnailHighlightHeight; }
            set
            {
                this.thumbnailHighlightHeight = value;
                this.OnPropertyChanged("ThumbnailHighlightHeight");
            }
        }

        public int UIStyle
        {
            get
            {
                return this.uiStyle;
            }
            set
            {
                this.uiStyle = value;
                App app = App.Current as App;
                app.ApplySkin(new Uri(this.uiStyleList[this.uiStyle], UriKind.Relative));
            }
        }

        public int BackgroundImageIndex
        {
            get { return this.backgroundImageIndex; }
            set
            {
                this.backgroundImageIndex = value;
                this.BackgroundImage = this.backgroundImageList[this.backgroundImageIndex];
            }
        }

        internal List<string> BackgroundImageList
        {
            get { return this.backgroundImageList; }
        }

        public Thickness MainBorderThickness
        {
            get { return this.mainBorderThickness; }
            set
            {
                this.mainBorderThickness = value;
                this.OnPropertyChanged("MainBorderThickness");
            }
        }

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public UIStyleSetting()
        {
            this.uiStyleList.Add(@"/Resources/Gray/GrayStyle.xaml");
            this.uiStyleList.Add(@"/Resources/Black/BlackStyle.xaml");

            this.backgroundImageList.Add(@"/Resources/Images/ThumbnailBackground.png");
            this.backgroundImageList.Add(@"/Resources/Images/ThumbnailBackground_Green.png");
            this.backgroundImageList.Add(@"/Resources/Images/ThumbnailBackground_Flower.png");
        }

        public void Save()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            folder = Path.Combine(folder, "Education Gadget Center");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string file = Path.Combine(folder, "UIStyleSetting.xml");
            if (File.Exists(file))
                File.Delete(file);
            SerializerHelper<UIStyleSetting>.XmlSerialize(file, this);
        }

        public static UIStyleSetting Load()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            folder = Path.Combine(folder, "Education Gadget Center");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string file = Path.Combine(folder, "UIStyleSetting.xml");
            return SerializerHelper<UIStyleSetting>.XmlDeserialize(file);
        }
    }
}
