using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.BlockPuzzle.Properties;
using System.Windows;
using System.Windows.Media.Imaging;
using SoonLearning.AppCenter;
using SoonLearning.BlockPuzzle.Data;
using System.IO;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.BlockPuzzle.Puzzle;

namespace SoonLearning.BlockPuzzle._2x3
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
            //string[] files = Directory.GetFiles(@"C:\Users\Ligang\SkyDrive\Content Shared\Puzzle\23", "*.*");
            //foreach (string file in files)
            //{
            //    PuzzleItem.CreatePuzzle(Path.GetFileNameWithoutExtension(file), file, "速学(SoonLearning.com)");
            //}
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
                return @"pack://application:,,,/SoonLearning.BlockPuzzle._2x3;component/Puzzle_2x3.png";
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
            get { return "F26E04C2FCF74EABB9F43AB184B9F1C3"; }
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

        public AppCapability Capability
        {
            get { return AppCapability.BackgroundMusic; }
        }

        public void Uninstall()
        {
            PuzzleSetting.Instance.Uninstall();
        }
    }
}
