using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace GadgetCenter.Data
{
    public class StageItem
    {
        private string title = string.Empty;
        private string description = string.Empty;
        private BitmapImage bi;

        public string Title
        {
            get { return this.title; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public BitmapImage Thumbnail
        {
            get { return this.bi; }
        }

        public StageItem(string title, string description, BitmapImage bi)
        {
            this.title = title;
            this.description = description;
            this.bi = bi;
        }
    }
}
