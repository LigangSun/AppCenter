using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GadgetCenter;
using Gadget.ReadCount.Properties;

namespace Gadget.ReadCount
{
    public class CountFruiteEnntry : IGadgetEntry
    {
        public string Title
        {
            get { return Resources.CountFruiteTitle; }
        }

        public string Description
        {
            get { return Resources.CountFruiteDescription; }
        }

        public System.Windows.Media.Imaging.BitmapImage Thumbnail
        {
            get 
            {
                Uri uri = new Uri(@"pack:\\application:,,,/ReadCount;component/Resources/CountFruiteThumbnail.jpg");
                return new System.Windows.Media.Imaging.BitmapImage(uri);
            }
        }

        public GadgetType Tag
        {
            get { return GadgetType.Math; }
        }

        public System.Windows.UIElement GetStartupPage()
        {
            return ReadCountStartupControl.Instance;
        }

        public bool GoBack()
        {
            return true;
        }
    }
}
