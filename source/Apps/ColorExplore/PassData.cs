using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.ColorExplorer
{
    public class PassData
    {
        private double timeInterval;
        private double objectInterval;
        private int totalObjectCount;
        private int passObjectCount;

        private string backgroundImage;
        private string backgroundMusic;

        // The object animate time
        public double TimeInterval
        {
            get { return this.timeInterval; }
        }

        // The time interval between two object.
        public double ObjectInterval
        {
            get { return this.objectInterval; }
        }

        public int TotalObjectCount
        {
            get { return this.totalObjectCount; }
        }

        public int PassObjectCount
        {
            get { return this.passObjectCount; }
        }

        public string BackgroundImage
        {
            get { return this.backgroundImage; }
        }

        public string BackgroundMusic
        {
            get { return this.backgroundMusic; }    
        }

        public PassData(double timeInterval, double objectInterval, int total, int pass, string bkImage, string bkMusic)
        {
            this.timeInterval = timeInterval;
            this.objectInterval = objectInterval;
            this.totalObjectCount = total;
            this.passObjectCount = pass;
            this.backgroundImage = bkImage;
            this.backgroundMusic = bkMusic;
        }
    }
}
