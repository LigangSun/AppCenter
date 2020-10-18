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
using System.Threading;
using System.Windows.Threading;
using SoonLearning.AppCenter.Controls;

namespace SoonLearning.AppCenter.UserControls
{
    /// <summary>
    /// Interaction logic for TimeUserControl.xaml
    /// </summary>
    public partial class TimeUserControl : UserControl
    {
        private TimeControlEngine timeCtrlEngine = new TimeControlEngine();
        private int totalTime = 0;

        public event EventHandler TimeUsedUpEvent;

        public TimeUserControl()
        {
            InitializeComponent();
        }

        public void Start(int totalTime)
        {
            this.totalTime = totalTime;
            this.SafeUpdateLeftTimeInfo(this, EventArgs.Empty);
            this.timeCtrlEngine.TimeElapsed += new EventHandler(timeCtrlEngine_TimeElapsed);
            this.timeCtrlEngine.Start();
        }

        public void Stop()
        {
            this.timeCtrlEngine.TimeElapsed -= new EventHandler(timeCtrlEngine_TimeElapsed);
            this.timeCtrlEngine.Stop();
        }

        private void timeCtrlEngine_TimeElapsed(object sender, EventArgs e)
        {
            if (Thread.CurrentThread == this.Dispatcher.Thread)
            {
                this.SafeUpdateLeftTimeInfo(sender, e);
            }
            else
            {
                this.Dispatcher.BeginInvoke(new EventHandler(this.SafeUpdateLeftTimeInfo),
                    DispatcherPriority.Normal,
                    new object[] { sender, e });
            }
        }

        private void SafeUpdateLeftTimeInfo(object sender, EventArgs e)
        {
            int leftTIme = this.totalTime- this.timeCtrlEngine.Elapsed;
            TimeSpan ts = TimeSpan.FromSeconds(leftTIme);
            this.leftTimeLabel.Text = string.Format("{0}:{1}", ts.Minutes.ToString("00"), ts.Seconds.ToString("00"));
            if (ts <= TimeSpan.Zero)
            {
                this.timeCtrlEngine.Stop();
                if (this.TimeUsedUpEvent != null)
                    this.TimeUsedUpEvent(this, EventArgs.Empty);
            }
        }
    }
}
