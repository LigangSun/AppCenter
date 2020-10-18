using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading;

namespace SoonLearning.Math.Presentation.Bridge
{
    public class Data : INotifyPropertyChanged
    {
        #region Instance

        private static Data instance;

        public static Data Instance
        {
            get
            {
                Interlocked.CompareExchange<Data>(ref instance, new Data(), null);
                return instance;
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        private decimal trainLength;
        private decimal bridgeLength;
        private decimal trainSpeed; // km/h
        private decimal duration; // In min

        public decimal TrainLength
        {
            get { return this.trainLength; }
            set
            {
                this.trainLength = value;
                this.OnPropertyChanged("TrainLength");
            }
        }

        public decimal BridgeLength
        {
            get { return this.bridgeLength; }
            set
            {
                this.bridgeLength = value;
                this.OnPropertyChanged("BridgeLength");
            }
        }

        public decimal TrainSpeed
        {
            get { return this.trainSpeed; }
            set
            {
                this.trainSpeed = value;
                this.OnPropertyChanged("TrainSpeed");
            }
        }

        public decimal Duration
        {
            get { return this.duration; }
            set
            {
                this.duration = value;
                this.OnPropertyChanged("Duration");
            }
        }

        internal bool IsValid
        {
            get
            {
                int i = 0;
                if (this.TrainLength == 0)
                    i++;

                if (this.TrainSpeed == 0)
                    i++;

                if (this.BridgeLength == 0)
                    i++;

                if (this.Duration == 0)
                    i++;

                if (i >= 2)
                    return false;

                return true;
            }
        }

        internal decimal calDuration()
        {
            int i = 0;
            if (this.TrainLength != 0)
                i++;

            if (this.TrainSpeed != 0)
                i++;

            if (this.BridgeLength != 0)
                i++;

            if (this.Duration != 0)
                i++;

            if (i == 4 || this.Duration == 0)
                this.Duration = new decimal(decimal.ToDouble(this.TrainLength + this.BridgeLength) / decimal.ToDouble(TrainSpeed));

            if (this.TrainLength == 0)
            {
                this.TrainLength = this.Duration * this.TrainSpeed - this.BridgeLength;
            }

            if (this.BridgeLength == 0)
            {
                this.BridgeLength = this.Duration * this.TrainSpeed - this.TrainLength;
            }

            if (this.TrainSpeed == 0)
            {
                this.TrainSpeed = new decimal(decimal.ToDouble(this.TrainLength + this.BridgeLength) / decimal.ToDouble(this.Duration));
            }

            return this.Duration;
        }
    }
}
