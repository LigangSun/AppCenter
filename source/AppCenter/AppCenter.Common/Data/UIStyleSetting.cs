using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.IO;
using SoonLearning.AppCenter.Utility;
using System.Threading;

namespace SoonLearning.AppCenter.Data
{
    public delegate void OpenSoundStateChangedHandler(bool open);

    public class UIStyleSetting : INotifyPropertyChanged
    {
        private static UIStyleSetting instance;

        public static UIStyleSetting Instance
        {
            get
            {
                if (instance == null)
                    Load();

                Interlocked.CompareExchange<UIStyleSetting>(ref instance, new UIStyleSetting(), null);
                return instance;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event OpenSoundStateChangedHandler OpenSoundStateChanged;

        private bool openSound = true;
        private bool fullScreen;
        private bool speechRecognizer;

        private int uiStyle = 0;
        private List<string> uiStyleList = new List<string>();
        private List<string> backgroundImageList = new List<string>();

        private static bool loading = false;
        
        public bool OpenSound
        {
            get { return this.openSound; }
            set
            {
                bool temp = openSound;
                this.openSound = value;
                this.OnPropertyChanged("OpenSound");
                this.Save();
                if (this.OpenSoundStateChanged != null &&
                    temp != value)
                {
                    this.OpenSoundStateChanged(value);
                }
            }
        }

        public bool FullScreen
        {
            get { return this.fullScreen; }
            set
            {
                this.fullScreen = value;
                this.OnPropertyChanged("FullScreen");
                this.Save();
            }
        }   

        public bool SpeechRecognizer
        {
            get { return this.speechRecognizer; }
            set
            {
                this.speechRecognizer = value;
                this.OnPropertyChanged("SpeechRecognizer");
                this.Save();
                SpeechHelper.Instance.RecognizerEnabled = value;
                SpeechHelper.Instance.EnableRecognizer();
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
                this.OnPropertyChanged("UIStyle");
            }
        }


        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public UIStyleSetting()
        {
        }

        public void Save()
        {
            if (loading)
                return;

            string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            folder = Path.Combine(folder, "SoonLearning App Center");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string file = Path.Combine(folder, "UIStyleSetting.xml");
            try
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
            catch
            {
                return;
            }
            SerializerHelper<UIStyleSetting>.XmlSerialize(file, this);
        }

        public static void Load()
        {
            loading = true;
            try
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                folder = Path.Combine(folder, "SoonLearning App Center");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
                string file = Path.Combine(folder, "UIStyleSetting.xml");
                instance = SerializerHelper<UIStyleSetting>.XmlDeserialize(file);
            }
            catch
            {
            }
            finally
            {
                loading = false;
            }
        }
    }
}
