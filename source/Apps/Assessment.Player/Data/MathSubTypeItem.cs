using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter.Data;
using System.Windows.Media.Imaging;
using SoonLearning.Assessment.Data;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.Assessment.Player.Data
{
    public class MathSubTypeItem : ThumbnailItem
    {
        private string title;
        private string description = string.Empty;
        private BitmapImage bi;
        private MathSubType mathSubType;

        public override string Title
        {
            get { return this.title; }
        }

        public override string Description
        {
            get { return this.description; }
        }

        public override System.Windows.Media.Imaging.BitmapImage Thumbnail
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
