using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Reflection;
using System.Timers;
using SoonLearning.AppCenter.Controls;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter.Utility;

namespace Gadget.ColorExplore
{
    /// <summary>
    /// Interaction logic for BallonUserControl.xaml
    /// </summary>
    public partial class BallonUserControl : UserControl
    {
        private static BallonUserControl instance;

        public static BallonUserControl Instance
        {
            get
            {
                if (instance == null)
                    instance = new BallonUserControl();

                return instance;
            }
        }

        private List<FlyingData> flyingDataList = new List<FlyingData>();

        private PassData currentPass;
        private int generatedCount = 0;
        private int passCount = 0;

        private FlyingData crashingData;

        private Timer generateTimer;

        public BallonUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.currentPass = DataMgr.Instance.FistPass;
            this.generatedCount = 0;
            this.passCount = 0;
            this.StartPass();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.backgroundME.Stop();
            this.ballonCrackME.Stop();
            if (this.generateTimer != null)
                this.generateTimer.Stop();

            foreach (FlyingData data in this.flyingDataList)
            {
                data.Storyboard.Stop();
            }
            this.flyingDataList.Clear();
        }

        internal void Stop()
        {
            SpeechHelper.Instance.EndRecognizer();

            this.backgroundME.Stop();
            this.ballonCrackME.Stop();
            if (this.generateTimer != null)
                this.generateTimer.Stop();

            foreach (FlyingData data in this.flyingDataList)
            {
                data.Storyboard.Stop();
            }
            this.flyingDataList.Clear();
        }

        internal void Restart()
        {
            this.Stop();
            this.StartPass();
        }

        private void StartBackgroundMusic()
        {
            Helper.Instance.StartBackgroundMusic(this.currentPass, this.backgroundME);
        }

        private void PlayBallonCrackMusic(int index)
        {
            Helper.Instance.PlayObjectCrackMusic(index, this.ballonCrackME);
        }

        private void StartPass()
        {
            List<string> textList = new List<string>();
            foreach (ColorObject co in DataMgr.Instance.ActiveObjectCollection)
                textList.Add(co.Text);

            SpeechHelper.Instance.StartRecognizer(textList, speechRecognizer_SpeechRecognized);
            
            string background = this.currentPass.BackgroundImage;
            if (string.IsNullOrEmpty(background))
                background = @"pack:\\application:,,,/ColorExplore;component/Images/background.jpg";
            this.backgroundImage.Source = new BitmapImage(new Uri(background));

            this.StartBackgroundMusic();

            if (this.generateTimer != null)
            {
                this.generateTimer.Stop();
                this.generateTimer.Elapsed -= this.generateTimer_Elapsed;
                this.generateTimer.Dispose();
                this.generateTimer = null;
            }

            if (this.currentPass.ObjectInterval > 0)
            {
                this.generateTimer = new Timer();
                this.generateTimer.Interval = this.currentPass.ObjectInterval * 1000;
                this.generateTimer.Elapsed += new ElapsedEventHandler(generateTimer_Elapsed);
                this.generateTimer.Start();
            }

            this.crashingData = null;
            this.flyingDataList.Clear();
            this.ballonCanvas.Children.Clear();

            this.AnimateBallon();
        }

        private void AnimateBallon()
        {
            if (!this.CheckResult())
                return;

            FlyingData data = this.CreateFlyingObject();
            data.Image.Opacity = 1.0f;

            this.ballonListBox.SelectedIndex = -1;

            DoubleAnimation doubleAnimate =  new DoubleAnimation(-data.Image.Source.Height, new Duration(TimeSpan.FromSeconds(this.currentPass.TimeInterval)));
            doubleAnimate.From = this.ballonCanvas.ActualHeight - data.Image.Source.Height;
            doubleAnimate.FillBehavior = FillBehavior.Stop;
            doubleAnimate.Completed += new EventHandler(doubleAnimate_Completed);

            data.Storyboard.Children.Clear();
            data.Storyboard.Children.Add(doubleAnimate);
            Storyboard.SetTarget(doubleAnimate, data.Image);
            Storyboard.SetTargetName(doubleAnimate, data.Image.Name);
            Storyboard.SetTargetProperty(doubleAnimate, new PropertyPath(Canvas.TopProperty));

            data.Storyboard.Begin();
        }

        private void doubleAnimate_Completed(object sender, EventArgs e)
        {
            if (this.flyingDataList.Count > 0)
            {
                FlyingData data = this.flyingDataList[0];
                data.Storyboard.Stop();
                this.ballonCanvas.Children.Remove(data.Image);
                this.flyingDataList.Remove(data);
            }

            if (this.currentPass.ObjectInterval == 0)
                this.AnimateBallon();
            else
                this.CheckResult();
        }

        private void ballonListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.ballonListBox.SelectedIndex < 0)
                return;

            foreach (FlyingData data in this.flyingDataList)
            {
                if (data.Index == this.ballonListBox.SelectedIndex)
                {
                    this.ballonCrack(data);
                    break;
                }
            }

            this.ballonListBox.SelectedIndex = -1;
        }

        private void ballonCrack(FlyingData data)
        {
            this.crashingData = data;

            this.flyingDataList.Remove(this.crashingData);

            this.passCount++;

            data.Storyboard.Pause();

            data.Storyboard.Children.Clear();

            DoubleAnimation scaleXAnimation = new DoubleAnimation(1.5f, new Duration(TimeSpan.FromSeconds(0.5)));
            DoubleAnimation scaleYAnimation = new DoubleAnimation(1.5f, new Duration(TimeSpan.FromSeconds(0.5)));
            scaleXAnimation.From = 1.2f;
            scaleYAnimation.From = 1.2f;

            DoubleAnimation doubleAnimation = new DoubleAnimation(0.0f, new Duration(TimeSpan.FromSeconds(0.3)));
            doubleAnimation.From = 1.0f;
            doubleAnimation.Completed += new EventHandler(crackAnimation_Completed);

            data.Storyboard.Children.Clear();
            data.Storyboard.Children.Add(doubleAnimation);
        //    this.flyStoryboard.Children.Add(scaleXAnimation);
        //    this.flyStoryboard.Children.Add(scaleYAnimation);

            Storyboard.SetTarget(doubleAnimation, data.Image);
            Storyboard.SetTargetName(doubleAnimation, data.Image.Name);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Image.OpacityProperty));

            //Storyboard.SetTarget(scaleXAnimation, this.ballonImage);
            //Storyboard.SetTargetName(scaleXAnimation, this.ballonImage.Name);
            //Storyboard.SetTargetProperty(scaleXAnimation, new PropertyPath(ScaleTransform.ScaleXProperty));

            //Storyboard.SetTarget(scaleYAnimation, this.ballonImage);
            //Storyboard.SetTargetName(scaleYAnimation, this.ballonImage.Name);
            //Storyboard.SetTargetProperty(scaleYAnimation, new PropertyPath(ScaleTransform.ScaleYProperty));

            data.Storyboard.Begin();

            this.PlayBallonCrackMusic(data.Index);
        }

        private void crackAnimation_Completed(object sender, EventArgs e)
        {
            this.Dispatcher.BeginInvoke(new EventHandler<EventArgs>(crackDone), DispatcherPriority.ApplicationIdle, new object[] { sender, e });
        }

        private void crackDone(object sender, EventArgs e)
        {
            this.crashingData.Storyboard.Stop();

            this.ballonCanvas.Children.Remove(this.crashingData.Image);
            this.crashingData = null;

            if (this.currentPass.ObjectInterval == 0)
                this.AnimateBallon();
            else
                this.CheckResult();
        }

        private void backgroundME_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.backgroundME.Position = TimeSpan.Zero;
            this.backgroundME.LoadedBehavior = MediaState.Play;
        }

        private bool CheckResult()
        {
            if (this.generatedCount == this.currentPass.TotalObjectCount)
            {
                if (this.generateTimer != null)
                    this.generateTimer.Stop();
            }

            if (this.generatedCount == this.currentPass.TotalObjectCount &&
                this.flyingDataList.Count == 0)
            {
                ResultWindow wnd = new ResultWindow();
                if (this.passCount >= this.currentPass.PassObjectCount)
                    wnd.State = SoonLearning.AppCenter.Controls.ResultState.Pass;
                else
                    wnd.State = SoonLearning.AppCenter.Controls.ResultState.NotPass;

                wnd.ShowMessage(MessageWindowCallback);
                
                return false;
            }

            return true;
        }

        private void MessageWindowCallback(bool ok)
        {
            if (ok)
            {
                this.currentPass = DataMgr.Instance.NextPass;
                this.generatedCount = 0;
                this.passCount = 0;
                this.StartPass();
            }
            else
            {
                this.generatedCount = 0;
                this.passCount = 0;
                this.StartPass();
            }
        }

        private FlyingData CreateFlyingObject()
        {
            Image img = new Image();
            img.RenderTransform = new ScaleTransform(1.5f, 1.5f);

            Random rand = new Random(DateTime.Now.Second);
            int index = rand.Next(DataMgr.Instance.ActiveObjectCollection.Count);
            img.Source = new BitmapImage(new Uri(DataMgr.Instance.ActiveObjectCollection[index].File));

            img.Name = string.Format("image_{0}", Guid.NewGuid().ToString("N"));

            FlyingData data = new FlyingData(index, img);
            this.flyingDataList.Add(data);

            this.ballonCanvas.Children.Add(img);

            Canvas.SetTop(img, this.ballonCanvas.ActualHeight);
            Canvas.SetLeft(img, (double)rand.Next((int)(this.ballonCanvas.ActualWidth - img.Source.Width)));

            this.generatedCount++;

            return data;
        }

        private void speechRecognizer_SpeechRecognized(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Result == null)
                return;

            foreach (FlyingData data in this.flyingDataList)
            {
                string text = DataMgr.Instance.ActiveObjectCollection[data.Index].Text;
                if (text.Equals(e.Result.Text, StringComparison.OrdinalIgnoreCase))
                {
                    this.ballonCrack(data);
                    break;
                }
            }
        }

        private void generateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (this.generatedCount == this.currentPass.TotalObjectCount)
                return;

            this.Dispatcher.Invoke(new EventHandler<EventArgs>(safeGenerate), DispatcherPriority.Normal, new object[] { sender, EventArgs.Empty });
        }

        private void safeGenerate(object sender, EventArgs e)
        {
            this.AnimateBallon();
        }
    }
}
