using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter;
using System.Windows;
using Gadget.Puzzle.Properties;
using System.Windows.Media.Imaging;
using System.IO;
using Gadget.Puzzle.Data;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Gadget.Puzzle.Puzzle
{
    public class Puzzle2x2Entry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 10, 1, 8, 0, 0);

        public UIElement GetStartupPage()
        {
            PuzzleSetting.Instance.Rows = 2;
            PuzzleSetting.Instance.Cols = 2;
            PuzzleSetting.Instance.Type = Data.PuzzleType.Normal_2x2;
            ControlMgr.Instance.Entry = this;
            //string[] files = Directory.GetFiles(@"F:\百度图片批量下载\托马斯\中尺寸-任何颜色-所有类型-jpg", "*.jpg");
            //foreach (string file in files)
            //{
            //    PuzzleItem.CreatePuzzle(Path.GetFileNameWithoutExtension(file), file, "快学(SoonLearning.com)");
            //}
            return ControlMgr.Instance.PuzzleStartupPage;
        }

        public string Title
        {
            get
            {
                return Resources.puzzle2x2Title;
            }
        }

        public string Description
        {
            get
            {
                return Resources.puzzle2x2Description;
            }
        }

        public string Thumbnail
        {
            get
            {
                return @"pack:\\application:,,,/Puzzle;component/Resources/Puzzle_2x2.bmp";
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
            get { return "C0F9E33FE7C54509BE6FD8F468722FA6"; }
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
