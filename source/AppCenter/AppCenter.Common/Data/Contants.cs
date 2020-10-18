using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppCenter.Data
{
    public enum GadgetType
    {
        None,

        Math,
        Explore,
        Reading,
        Puzzle,
        Memory,

        Other
    }

    public enum GadgetSubType
    {
        None,

        Math_Basic,
        Math_Junior,
        Math_Rapid,
        Math_Olympic,
        Math_Game,

        Puzzle_Block,
        Puzzle_Jigsaw,
        Puzzle_Other,

        Explore_Color,
        Explore_Sound,
        Explore_Other,

        Memory_Color,
        Memory_Sound,
        Memory_Other,

        Reading_Test,

        Other,
    }
}
