using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;
using System.Windows.Media.Imaging;

namespace SoonLearning.Assessment.Data
{
    public class MathSubTypeItem
    {
        private string title;
        private string description = string.Empty;
        private BitmapImage bi;
        private MathSubType mathSubType;

        public string Title
        {
            get { return this.title; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public System.Windows.Media.Imaging.BitmapImage Thumbnail
        {
            get { return this.bi; }
        }

        public MathSubType Type
        {
            get { return this.mathSubType; }
        }

        public MathSubTypeItem(string title, MathSubType type)
        {
            this.title = title;
            this.mathSubType = type;
        }
    }
}
