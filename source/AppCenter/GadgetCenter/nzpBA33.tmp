using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows;
using GadgetCenter.Data;
using System.Collections.ObjectModel;

namespace GadgetCenter
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

    public interface IGadgetEntry
    {
        string Id
        {
            get;
        }

        DateTime CreateDate
        {
            get;
        }

        string Title
        {
            get;
        }

        string Description
        {
            get;
        }

        BitmapImage Thumbnail
        {
            get;
        }

        GadgetType Tag
        {
            get;
        }

        GadgetSubType SubTag
        {
            get;
        }

        IGadgetControl Control
        {
            get;
        }

        UIElement GetStartupPage();
    }
}
