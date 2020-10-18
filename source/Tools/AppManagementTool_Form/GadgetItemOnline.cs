using System;
using System.Collections.Generic;
using System.Text;

namespace AppManagementTool
{
    public enum GadgetType
    {
        Math,
        Explore,
        Reading,
        Puzzle,
        Memory,

        Other
    }

    public enum GadgetSubType
    {
        Math_Basic,
        Math_Rapid,
        Math_Olympic,
        Math_Game,

        Puzzle_Block,
        Puzzle_Jigsaw,
        Puzzle_Other,

        Explore_Color,
        Explore_Sound,
        Explore_Momery,
        Explore_Other,

        Memory_Color,
        Memory_Sound,

        Reading_Test,

        Other,
    }

    public class GadgetItemOnline
    {
        private string dllFile = string.Empty;

        public GadgetItemOnline(string dllFile)
        {
            this.dllFile = dllFile;
        }

        public GadgetItemOnline()
        {
        }

        public string DllFile
        {
            get { return this.dllFile; }
        }

        public string Id
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Thumbnail
        {
            get;
            set;
        }

        public GadgetType AppType
        {
            get;
            set;
        }

        public GadgetSubType AppSubType
        {
            get;
            set;
        }

        public DateTime CreateDate
        {
            get;
            set;
        }

        public string Version
        {
            get;
            set;
        }

        public string Creator
        {
            get;
            set;
        }

        public string CreatorWebSite
        {
            get;
            set;
        }

        public string CreatorLogo
        {
            get;
            set;
        }

        public string PackageUrl
        {
            get;
            set;
        }
    }
}
