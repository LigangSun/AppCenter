using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppCenter.Host
{
    public class ShareItem
    {
        private string logo;
        private string title;

        public string Logo
        {
            get { return this.logo; }
        }

        public string Title
        {
            get { return this.title; }
        }

        public ShareItem(string logo, string title)
        {
            this.logo = logo;
            this.title = title;
        }
    }
}
