using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.BlockPuzzle.Properties;
using System.Windows;
using System.Windows.Media.Imaging;
using SoonLearning.AppCenter;
using System.IO;
using SoonLearning.BlockPuzzle.Data;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.BlockPuzzle.Puzzle
{
    public class Puzzle3x3Entry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 10, 4, 8, 0, 0);

        public UIElement GetStartupPage()
        {
            PuzzleSetting.Instance.Rows = 3;
            PuzzleSetting.Instance.Cols = 3;
            PuzzleSetting.Instance.Type = Data.PuzzleType.Normal_3x3;
            ControlMgr.Instance.Entry = this;
            
            return ControlMgr.Instance.PuzzleStartupPage;
        }

        public string Title
        {
            get
            {
                return Resources.puzzle3x3Title;
            }
        }

        public string Description
        {
            get
            {
                return Resources.puzzle3x3Description;
            }
        }

        public string Thumbnail
        {
            get
            {
                return @"pack://application:,,,/SoonLearning.BlockPuzzle;component/Resources/Puzzle_3x3.bmp";
            }
        }

        public int Tag
        {
            get { return 100; }
        }

        public int SubTag
        {
            get { return 103; }
        }

        public string Id
        {
            get { return "AD11378B6B424D18BEA5344B4C171512"; }
        }

        public DateTime CreateDate
        {
            get { return this.createDate; }
        }

        public string Name
        {
            get { return "速学信息科技(SoonLearning)"; }
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
