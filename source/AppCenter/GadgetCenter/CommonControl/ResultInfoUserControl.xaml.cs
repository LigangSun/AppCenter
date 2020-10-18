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
using System.Timers;
using SoonLearning.AppCenter.Resources;
using System.Windows.Threading;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.Controls
{
    public enum ResultState
    {
        Perfect,
        Pass,
        NotPass,
        Bad
    }

    /// <summary>
    /// Interaction logic for ResultInfoUserControl.xaml
    /// </summary>
    public partial class ResultInfoUserControl : UserControl
    {
        public EventHandler<EventArgs> ResultInfoCompleted;

        private Timer holdTimer;
        private ResultState state = ResultState.Pass;
        private int holdTime = 5;

        public ResultState State
        {
            set
            {
                this.state = value;
            }
            get
            {
                return this.state;
            }
        }

        /// <summary>
        /// In Second
        /// </summary>
        public int HoldTime 
        {
            set 
            { 
                this.holdTime = value;
                this.leftHideTimeTextBlock.Text = this.holdTime.ToString();
            }
        }

        public bool Show
        {
            set
            {
                if (value)
                {
                    this.ShowInfo(); 
                }
            }
        }

        public ResultInfoUserControl()
        {
            InitializeComponent();

            this.holdTimer = new Timer();
            this.holdTimer.Interval = holdTime;
            this.holdTimer.Elapsed += new ElapsedEventHandler(holdTimer_Elapsed);
            this.HoldTime = 5;

            this.ShowInfo();
        }

        private void ShowInfo()
        {
            switch (this.state)
            {
                case ResultState.Perfect:
                    {
                        this.resultImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Images/CleverSmile.png"));
                        this.infoLabel.Text = Strings.Perfect;
                        AudioHelper.PlayPassSound();
                    }
                    break;
                case ResultState.Pass:
                    {
                        this.resultImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Images/BigSmile.png"));
                        this.infoLabel.Text = Strings.Good;
                        AudioHelper.PlayPassSound();
                    }
                    break;
                case ResultState.NotPass:
                    {
                        this.resultImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Images/BadSurprise.png"));
                        this.infoLabel.Text = Strings.NotPass;
                    }
                    break;
                case ResultState.Bad:
                    {
                        this.resultImage.Source = new BitmapImage(new Uri(@"pack://application:,,,/SoonLearning.AppCenter;component/Resources/Images/Cry.png"));
                        this.infoLabel.Text = Strings.Back;
                    }
                    break;
            }

            this.holdTimer.Interval = 1000;
            this.BeginAnimation(UserControl.OpacityProperty, new DoubleAnimation(1.0f, new Duration(TimeSpan.FromSeconds(0.2)), FillBehavior.HoldEnd));
            this.holdTimer.Start();

            GC_UIHelper.ShowMessageWindow(this);
        }

        private void holdTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.holdTime--;
            if (this.holdTime == 0)
            {
                this.holdTimer.Stop();
                this.Dispatcher.BeginInvoke(new EventHandler(SafeHide), new object[] { null, null });
            }
            else
            {
                this.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    this.leftHideTimeTextBlock.Text = this.holdTime.ToString();
                }), DispatcherPriority.Normal, new object[] { });
            }
        }

        private void SafeHide(object sender, EventArgs e)
        {
            this.BeginAnimation(UserControl.OpacityProperty, new DoubleAnimation(0.0f, new Duration(TimeSpan.FromSeconds(0.2)), FillBehavior.HoldEnd));

            GC_UIHelper.CloseMessageWindow();

            if (this.ResultInfoCompleted != null)
                this.ResultInfoCompleted(this, EventArgs.Empty);
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.holdTimer.Stop();
            this.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    this.SafeHide(null, null);
                }),
                DispatcherPriority.Normal,
                null);
        }

        private void UserControl_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.holdTimer.Stop();
            this.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    this.SafeHide(null, null);
                }),
                DispatcherPriority.Normal,
                null);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.holdTimer.Stop();
                this.Dispatcher.BeginInvoke(new System.Threading.ThreadStart(() =>
                {
                    this.SafeHide(null, null);
                }),
                DispatcherPriority.Normal,
                null);
            }
        }
    }
}
