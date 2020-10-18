using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using SoonLearning.BlockPuzzle.Properties;
using SoonLearning.BlockPuzzle.Data;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.BlockPuzzle.Puzzle;

namespace SoonLearning.BlockPuzzle._2x2
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
            //string[] files = Directory.GetFiles(@"D:\SkyDrive同步\SkyDrive\Content Shared\Puzzle\22", "*.*");
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
                return @"pack://application:,,,/SoonLearning.BlockPuzzle._2x2;component/Puzzle_2x2.png";
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
            get { return "C0F9E33FE7C54509BE6FD8F468722FA6"; }
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
