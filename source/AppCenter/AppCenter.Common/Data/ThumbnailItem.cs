using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace SoonLearning.AppCenter.Data
{
    public abstract class ThumbnailItem
    {
        public abstract string Title
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public abstract BitmapImage Thumbnail
        {
            get;
        }
    }
}
