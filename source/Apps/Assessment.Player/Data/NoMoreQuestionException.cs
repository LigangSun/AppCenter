﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.Assessment.Player.Data
{
    public class NoMoreQuestionException : Exception
    {
        public NoMoreQuestionException()
            : base("No More question can be generated!")
        {
        }
    }
}
