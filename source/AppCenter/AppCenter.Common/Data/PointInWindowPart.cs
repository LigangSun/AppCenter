using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.AppCenter.Data
{
    internal enum PointInWindowPart
    {
        None = 0,

        InTitle = -1,

        Top = 3,
        Bottom = 6,
        Left = 1,
        Right = 2,

        TopLeft = 4,
        TopRight = 5,
        BottomLeft = 7,
        BottomRight = 8
    }
}
