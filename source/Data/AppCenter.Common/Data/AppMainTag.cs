using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppCenter.Data
{
    public enum MainTagType
    {
        Installed,
        All,
        RecentUpdate,
        MostPopular
    }

    public class AppMainTag
    {
        private string title;
        private MainTagType type;

        public string Title
        {
            get { return this.title; }
        }

        public MainTagType Type
        {
            get { return this.type; }
        }

        public AppMainTag(string title, MainTagType type)
        {
            this.title = title;
            this.type = type;
        }

        public override string ToString()
        {
            return this.Title;
        }
    }
}
