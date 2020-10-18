using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gadget.Puzzle.Properties;
using System.Windows;
using System.Windows.Media.Imaging;
using SoonLearning.AppCenter;
using Gadget.Puzzle.Data;
using System.IO;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Gadget.Puzzle.Puzzle
{
    public class Puzzle2x3Entry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 10, 2, 8, 0, 0);

        public UIElement GetStartupPage()
        {
            PuzzleSetting.Instance.Rows = 3;
            PuzzleSetting.Instance.Cols = 2;
            PuzzleSetting.Instance.Type = Data.PuzzleType.Normal_2x3;
            ControlMgr.Instance.Entry = this;
            return ControlMgr.Instance.PuzzleStartupPage;
        }

        public string Title
        {
            get
            {
                return Resources.puzzle2x3Title;
            }
        }

        public string Description
        {
            get
            {
                return Resources.puzzle2x3Description;
            }
        }

        public string Thumbnail
        {
            get
            {
                return @"pack:\\application:,,,/Puzzle;component/Resources/Puzzle_2x3.bmp";
            }
        }

        public GadgetType Tag
        {
            get { return GadgetType.Puzzle; }
        }

        public IGadgetControl Control
        {
            get { return PuzzleControl.Instance; }
        }

        public GadgetSubType SubTag
        {
            get { return GadgetSubType.Puzzle_Block; }
        }

        public string Id
        {
            get { return "F26E04C2FCF74EABB9F43AB184B9F1C3"; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
        }

        public string Name
        {
            get { return "快学(SoonLearning)"; }
        }

        public string WebSite
        {
            get { return @"http://www.soonlearning.com"; }
        }

        public string Logo
        {
            get { return string.Empty; }
        }
    }
}
