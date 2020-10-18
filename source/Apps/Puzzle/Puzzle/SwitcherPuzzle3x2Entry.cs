using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.AppCenter;
using System.Windows.Media.Imaging;
using System.Windows;
using SoonLearning.BlockPuzzle.Properties;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Interfaces;

namespace SoonLearning.BlockPuzzle.Puzzle3x2
{
    internal class SwitcherPuzzle3x2Entry// : IGadgetEntry
    {
        private SwitcherPuzzle3x2StartupPage startPage;

        public UIElement GetStartupPage()
        {
            if (this.startPage == null)
                this.startPage = new SwitcherPuzzle3x2StartupPage();
            return this.startPage;
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

        public BitmapImage Thumbnail
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/SoonLearning.BlockPuzzle;component/Resources/Puzzle.png"));
            }
        }

        public GadgetType Tag
        {
            get { return GadgetType.Puzzle; }
        }

        public IGadgetControl Control
        {
            get { throw new NotImplementedException(); }
        }

        public GadgetSubType SubTag
        {
            get { return GadgetSubType.Puzzle_Block; }
        }
    }
}
