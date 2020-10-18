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
    public class Puzzle3x2Entry : MarshalByRefObject, IGadgetEntry, IAuthorInfo
    {
        private DateTime createDate = new DateTime(2011, 10, 3, 8, 0, 0);

        public UIElement GetStartupPage()
        {
            PuzzleSetting.Instance.Rows = 2;
            PuzzleSetting.Instance.Cols = 3;
            PuzzleSetting.Instance.Type = Data.PuzzleType.Normal_3x2;
            ControlMgr.Instance.Entry = this;
            //string[] files = Directory.GetFiles(@"F:\Test\Code\Source\Resource\名山", "*.jpg");
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
                return Resources.puzzle3x2Title;
            }
        }

        public string Description
        {
            get
            {
                return Resources.puzzle3x2Description;
            }
        }

        public string Thumbnail
        {
            get
            {
                return @"pack://application:,,,/SoonLearning.BlockPuzzle;component/Resources/Puzzle_3x2.bmp";
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
            get { return "7252CF165E4F449885493568CCBF478A"; }
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
