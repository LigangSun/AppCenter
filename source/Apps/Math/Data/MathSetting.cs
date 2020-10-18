using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using SoonLearning.Math.Fast.RapidCalculation;
using SoonLearning.AppCenter.License;
using System.Windows;
using SoonLearning.AppCenter;
using System.Reflection;

namespace SoonLearning.Math.Data
{
    public enum MathType
    {
        Add_Minus,
        Nine_Nine_Table,
        Multiply_Division,
        Add_Minus_Multiply_Division
    }

    public delegate QuestionData CreateQuestionDataDelegate();

    public class MathSetting : INotifyPropertyChanged
    {
        #region Instance

        public static MathSetting Instance
        {
            get
            {
                object instance = App.Current.TryFindResource("mathSetting");
                if (instance == null)
                {
                    ResourceDictionary rd = new ResourceDictionary();
                    rd.Source = new Uri(@"pack://application:,,,/SoonLearning.Math.Fast;component/Resources/DataDictionary.xaml");
                    App.Current.Resources.MergedDictionaries.Add(rd);

                    instance = App.Current.FindResource("mathSetting");
                }

                return instance as MathSetting;
            }
        }

        #endregion

        #region Members

        private ObservableCollection<int> maxNumberCollection = new ObservableCollection<int>();
        private int selectedNumberIndex = 0;
        private int maxQuestionCount = 20;
        private int examTime = 5;
        private ObservableCollection<int> questionCountPerPageCollection = new ObservableCollection<int>();
        private int selectedCountIndex = 0;
        private bool voiceTip = true;
        private bool fullScreen;
        private float pointPerQuestion = 1.0f;

        private int ownerDefinedValue;

        private int passQuizCorrectCount = 20;

        private Dictionary<int, BaseQuestionGenerator> generatorDictionary = new Dictionary<int, BaseQuestionGenerator>();

        private Type type;

        private QuestionData currentQuestionData;

        private QuestionDataCollection questionDataCollection;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event CreateQuestionDataDelegate CreateQuestionData;

        #endregion

        #region Properties

        public ObservableCollection<int> NumberCollection
        {
            get { return this.maxNumberCollection; }
        }

        public int MinNumber
        {
            get;
            set;
        }

        public int MaxNumber
        {
            get;
            set;
        }

        public int SelectedNumberIndex
        {
            get { return this.selectedNumberIndex; }
            set
            {
                this.selectedNumberIndex = value;
                this.OnPropertyChanged("SelectedNumberIndex");
            }
        }

        public int SelectedMaxNumber
        {
            get { return this.maxNumberCollection[this.SelectedNumberIndex]; }
        }

        public int QuestionCount
        {
            get { return this.maxQuestionCount; }
            set 
            { 
                this.maxQuestionCount = value;
                this.OnPropertyChanged("QuestionCount");
                if (this.maxQuestionCount < this.PassQuizCorrectCount)
                    this.PassQuizCorrectCount = value;
            }
        }

        public int PassQuizCorrectCount
        {
            get { return this.passQuizCorrectCount; }
            set
            {
                this.passQuizCorrectCount = value;
                this.OnPropertyChanged("PassQuizCorrectCount");
            }
        }

        public int ExamTime
        {
            get { return this.examTime; }
            set
            {
                this.examTime = value;
                this.OnPropertyChanged("ExamTime");
            }
        }

        public ObservableCollection<int> QuestionCountPerpageCollection
        {
            get { return this.questionCountPerPageCollection; }
        }

        public int SelectedCountIndex
        {
            get { return this.selectedCountIndex; }
            set
            {
                this.selectedCountIndex = value;
                this.OnPropertyChanged("SelectedCountIndex");
            }
        }

        internal int QuestionCountPerPage
        {
            get
            {
                return this.questionCountPerPageCollection[this.SelectedCountIndex];
            }
        }

        internal BaseQuestionGenerator QuestionGenerator
        {
            get
            {
                if (!this.generatorDictionary.ContainsKey(this.QuestionCountPerPage))
                {
                    switch (this.QuestionCountPerPage)
                    {
                        case 1:
                            this.generatorDictionary.Add(this.QuestionCountPerPage, new QuestionGenerator1());
                            break;
                        case 3:
                            this.generatorDictionary.Add(this.QuestionCountPerPage, new QuestionGenerator3());
                            break;
                        case 5:
                            this.generatorDictionary.Add(this.QuestionCountPerPage, new QuestionGenerator5());
                            break;
                        case 10:
                            this.generatorDictionary.Add(this.QuestionCountPerPage, new QuestionGenerator10());
                            break;
                        case 20:
                            this.generatorDictionary.Add(this.QuestionCountPerPage, new QuestionGenerator20());
                            break;
                    }
                }

                return this.generatorDictionary[this.QuestionCountPerPage];
            }
        }

        public bool VoiceTip
        {
            get { return this.voiceTip; }
            set
            {
                this.voiceTip = value;
                this.OnPropertyChanged("VoiceTip");
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

        public float PointPerQuestion
        {
            get { return this.pointPerQuestion; }
            set 
            { 
                this.pointPerQuestion = value;
                this.OnPropertyChanged("PointPerQuestion");
            }
        }

        public int MaxQuestionCount
        {
            get { return 100; }
        }

        public int OwnerDefinedNumber
        {
            get { return this.ownerDefinedValue; }
            set
            {
                this.ownerDefinedValue = value;
                this.OnPropertyChanged("OwnerDefinedValue");
            }
        }

        public Type Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = value;
            }
        }

        public QuestionData CurrentQuestionData
        {
            get { return this.currentQuestionData; }
            internal set { this.currentQuestionData = value; }
        }

        public string CurrentQuestionHeader
        {
            get;
            set;
        }

        public bool EnableRange
        {
            get;
            set;
        }

        internal QuestionDataCollection QuestionDataCollection
        {
            get { return this.questionDataCollection; }
        }

        internal string DataFolder
        {
            get
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string dataFolder = System.IO.Path.GetDirectoryName(assembly.Location);
                dataFolder = System.IO.Path.Combine(dataFolder, string.Format("Data\\Rapid\\{0}", MathSetting.Instance.Type.ToString()));
                if (!System.IO.Directory.Exists(dataFolder))
                    System.IO.Directory.CreateDirectory(dataFolder);

                return dataFolder;
            }
        }

        #endregion 

        #region Constructor

        public MathSetting()
        {
            this.questionCountPerPageCollection.Add(1);
            this.questionCountPerPageCollection.Add(3);
            this.questionCountPerPageCollection.Add(5);
        //    this.questionCountPerPageCollection.Add(10);
        //    this.questionCountPerPageCollection.Add(20);

            this.maxNumberCollection.Add(10);
            this.maxNumberCollection.Add(20);
            this.maxNumberCollection.Add(50);
            this.maxNumberCollection.Add(100);
            this.maxNumberCollection.Add(500);
            this.maxNumberCollection.Add(1000);
            this.maxNumberCollection.Add(10000);

            this.MinNumber = 0;
            this.MaxNumber = 10;
            this.EnableRange = true;

            this.questionDataCollection = new QuestionDataCollection();
            this.questionDataCollection.LoadFromFiles();
        }

        #endregion

        internal bool Valid()
        {
            return true;
            if (LicenseMgr.Instance.IsTrialVersion(ControlMgr.Instance.Entry.Id))
            {
                if (this.SelectedMaxNumber != 10 ||
                    this.ExamTime != 5 ||
                    this.QuestionCount != 20)
                {
                    UIHelper.ShowTrialMessage();
                    return false;
                }
            }

            return true;
        }

        internal QuestionData GetQuestionData()
        {
            if (this.CreateQuestionData != null)
            {
                this.currentQuestionData = this.CreateQuestionData();
                this.questionDataCollection.Add(this.currentQuestionData);
            }

            return this.currentQuestionData;
        }

        public void Uninstall()
        {
            try
            {
                System.IO.Directory.Delete(MathSetting.Instance.DataFolder);
            }
            catch
            {
            }
        }

        #region Property Changed

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
