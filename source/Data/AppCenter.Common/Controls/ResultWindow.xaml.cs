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
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Media.Animation;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter.Player;
using System.Reflection;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.AppCenter.Controls
{
    /// <summary>
    /// Interaction logic for ResultWindow.xaml
    /// </summary>
    public partial class ResultWindow
    {
        private ResultState state = ResultState.Pass;

        private int holdTime = 5;
        private Timer holdTimer;
        private int elapsedTime = 0;

        private string passMusic;
        private string failMusic;

        private MessageWindowCallback callback;

        /// <summary>
        /// In Second
        /// </summary>
        public int HoldTime
        {
            set
            {
                this.holdTime = value;
                this.elapsedTime = this.holdTime;
            }
        }

        public ResultState State
        {
            get { return this.state; }
            set
            {
                this.state = value;
            }
        }

        public ResultWindow()
        {
            InitializeComponent();

            this.holdTimer = new Timer();
            this.holdTimer.Interval = 1000;
            this.holdTimer.Elapsed += new ElapsedEventHandler(holdTimer_Elapsed);
        }

        public void ShowMessage(MessageWindowCallback callback)
        {
            this.callback = callback;
            this.ShowInfo();
            UIHelper.ShowMessageWindow(this);
        }

        private void ShowInfo()
        {
            this.Visibility = System.Windows.Visibility.Visible;
            switch (this.state)
            {
                case ResultState.Perfect:
                    {
                        this.resultImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/SoonLearning.AppCenter.Common;component/Images/CleverSmile.png"));
                        this.infoTextBlock.Text = SoonLearning.AppCenter.Properties.Resources.EnterNextStage;
                        this.PlayPassAudio();
                    }
                    break;
                case ResultState.Pass:
                    {
                        this.resultImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/SoonLearning.AppCenter.Common;component/Images/BigSmile.png"));
                        this.infoTextBlock.Text = SoonLearning.AppCenter.Properties.Resources.EnterNextStage;
                        this.PlayPassAudio();
                    }
                    break;
                case ResultState.NotPass:
                    {
                        this.resultImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/SoonLearning.AppCenter.Common;component/Images/BadSurprise.png"));
                        this.infoTextBlock.Text = SoonLearning.AppCenter.Properties.Resources.RetryStage;
                        this.btnPanel.Children.Remove(this.okBtn);
                    }
                    break;
                case ResultState.Bad:
                    {
                        this.resultImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/SoonLearning.AppCenter.Common;component/Images/Cry.png"));
                        this.infoTextBlock.Text = SoonLearning.AppCenter.Properties.Resources.RetryStage;
                        this.btnPanel.Children.Remove(this.okBtn);
                    }
                    break;
            }

            this.elapsedTime = this.holdTime;
            this.elapsedTimeTextBlock.Text = this.elapsedTime.ToString();
            this.BeginAnimation(UserControl.OpacityProperty, new DoubleAnimation(0.0f, 1.0f, new Duration(TimeSpan.FromSeconds(0.2)), FillBehavior.HoldEnd));
            this.holdTimer.Start();
        }

        private void holdTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.elapsedTime--;
            if (this.elapsedTime == 0)
            {
                this.holdTimer.Stop();
                this.Dispatcher.BeginInvoke(new EventHandler(SafeHide), new object[] { null, null });
            }
            else
                this.Dispatcher.BeginInvoke(new EventHandler(SafeElpasedTime), new object[] { null, null });
        }

        private void SafeElpasedTime(object sendre, EventArgs e)
        {
            this.elapsedTimeTextBlock.Text = this.elapsedTime.ToString();
        }

        private void SafeHide(object sender, EventArgs e)
        {
            try
            {
            //    this.BeginAnimation(UserControl.OpacityProperty, new DoubleAnimation(0.0f, new Duration(TimeSpan.FromSeconds(0.2)), FillBehavior.HoldEnd));

                UIHelper.CloseMessageWindow();

                if (state == ResultState.Pass ||
                    state == ResultState.Perfect)
                {
                    if (this.callback != null)
                        this.callback(true);
                }
                else
                {
                    if (this.callback != null)
                        this.callback(false);
                }                
            }
            catch
            {
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.holdTimer.Stop();

            UIHelper.CloseMessageWindow();

            if (this.callback != null)
                this.callback(false);
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            this.holdTimer.Stop();

            UIHelper.CloseMessageWindow();

            if (this.callback != null)
                this.callback(true);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = (Storyboard)this.FindResource("Storyboard1");
            if (storyboard != null)
                storyboard.Begin();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        private void PlayPassAudio()
        {
            AudioHelper.PlayPassSound();
        }
    }
}
