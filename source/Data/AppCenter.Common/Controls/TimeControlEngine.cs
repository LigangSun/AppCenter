using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace SoonLearning.AppCenter.Controls
{
    public class TimeControlEngine
    {
        public event EventHandler TimeElapsed;

        private int startTickCount;
        private Timer timer;
        private int elapsed;

        private int pauseElapsed = 0;

        public int Elapsed // in Second
        {
            get 
            { 
                return this.elapsed;
            }
            private set
            {
                if (value != this.elapsed)
                {
                    this.elapsed = this.pauseElapsed + value;
                    if (this.TimeElapsed != null)
                        this.TimeElapsed(this, EventArgs.Empty);
                }
            }
        }

        public void Start()
        {
            if (this.timer == null)
            {
                this.timer = new Timer();
                this.timer.Interval = 100;
                this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            }

            this.startTickCount = Environment.TickCount;
            this.timer.Start();
        }

        public void Stop()
        {
            if (this.timer != null)
            {
                this.timer.Stop();
                this.timer.Dispose();
                this.timer = null;
            }
        }

        public void Pause()
        {
            if (this.timer != null)
            {
                this.pauseElapsed = this.Elapsed;
                this.timer.Stop();
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Elapsed = (Environment.TickCount - this.startTickCount) / 1000;
        }
    }
}
