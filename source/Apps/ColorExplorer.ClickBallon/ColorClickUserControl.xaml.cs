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
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.ColorExplorer.ClickBallon
{
    /// <summary>
    /// Interaction logic for ColorClickUserControl.xaml
    /// </summary>
    public partial class ColorClickUserControl : UserControl
    {
        private static ColorClickUserControl instance;

        public static ColorClickUserControl Instance
        {
            get
            {
                if (instance == null)
                    instance = new ColorClickUserControl();

                return instance;
            }
        }

        private PassData currentPass;
        private int currentColorIndex = -1;
        private Storyboard storyboard;
        private int continueCorrectCount = 0;

        public ColorClickUserControl()
        {
            InitializeComponent();

            this.currentPass = new PassData(1.5f, 8, 10, 6,
                string.Empty, string.Empty);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.RandonColorObject();

            Helper.Instance.StartBackgroundMusic(this.currentPass, this.backgroundME);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.storyboard != null)
                this.storyboard.Stop();

            this.backgroundME.Stop();
        }

        private void colorObjectListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.colorObjectListBox.SelectedIndex != this.currentColorIndex)
            {
                if (this.colorObjectListBox.SelectedIndex != -1)
                    this.continueCorrectCount = 0;
                return;
            }

            this.continueCorrectCount++;
            this.colorTextLabel.Visibility = System.Windows.Visibility.Hidden;
            this.AnimateBallon();
        }

        private void backgroundME_MediaEnded(object sender, RoutedEventArgs e)
        {
            this.backgroundME.Position = TimeSpan.Zero;
            this.backgroundME.LoadedBehavior = MediaState.Play;
        }

        private void RandonColorObject()
        {
            Random rand = new Random(DateTime.Now.Second);
            this.currentColorIndex = rand.Next(DataMgr.Instance.ActiveObjectCollection.Count);

            string[] texts = DataMgr.Instance.ActiveObjectCollection[this.currentColorIndex].Text.Split(new char[] { '$' });
            this.colorTextLabel.Content = texts[0];
            this.colorTextLabel.Visibility = System.Windows.Visibility.Visible;

            BitmapImage bi = DataMgr.Instance.GetObjectImage(this.currentColorIndex);
            this.flyingImage.Source = bi;
            this.flyingImage.Width = bi.PixelWidth;
            Canvas.SetTop(this.flyingImage, this.ballonCanvas.ActualHeight);
            Canvas.SetLeft(this.flyingImage, (double)rand.Next((int)(this.ballonCanvas.ActualWidth - this.flyingImage.Width)));

            this.Speek();

            this.colorObjectListBox.SelectedIndex = -1;
        }

        private void Speek()
        {
            string[] texts = DataMgr.Instance.ActiveObjectCollection[this.currentColorIndex].Text.Split(new char[] { '$' });
            SpeechHelper.Instance.SpeakAsync(texts[0], -2);
        }

        private void AnimateBallon()
        {
            this.PlayResultVoice();

            Helper.Instance.PlayObjectCrackMusic(this.currentColorIndex, this.ballonCrackME);

            if (this.storyboard == null)
                this.storyboard = new Storyboard();

            DoubleAnimation doubleAnimate = new DoubleAnimation(-this.flyingImage.Source.Height, new Duration(TimeSpan.FromSeconds(this.currentPass.TimeInterval)));
            doubleAnimate.From = this.ballonCanvas.ActualHeight - this.flyingImage.Source.Height;
            doubleAnimate.FillBehavior = FillBehavior.Stop;
            doubleAnimate.Completed += new EventHandler(doubleAnimate_Completed);

            this.storyboard.Children.Clear();
            this.storyboard.Children.Add(doubleAnimate);
            Storyboard.SetTarget(doubleAnimate, this.flyingImage);
            Storyboard.SetTargetName(doubleAnimate, this.flyingImage.Name);
            Storyboard.SetTargetProperty(doubleAnimate, new PropertyPath(Canvas.TopProperty));

            this.storyboard.Begin();

            this.flyingImage.Visibility = System.Windows.Visibility.Visible;
        }

        private void doubleAnimate_Completed(object sender, EventArgs e)
        {
            this.flyingImage.Visibility = System.Windows.Visibility.Hidden;
            this.RandonColorObject();
        }

        private void PlayResultVoice()
        {
            string text = string.Empty;
            if (this.continueCorrectCount == 1)
                text = "正确";
            else if (this.continueCorrectCount <= 5)
                text = "很好";
            else if (this.continueCorrectCount <= 10)
                text = "很棒";
            else
                text = "聪明";

            SpeechHelper.Instance.SpeakAsync(text, -2);
        }

        public void Restart()
        {
            this.Speek();
        }
    }
}
