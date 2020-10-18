using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Math.Data;

namespace SoonLearning.Assessment.Data.Arithmetic
{
    internal class ArithmeticTypeCollection : ObservableCollection<MathSubTypeItem>
    {
        internal ArithmeticTypeCollection()
        {
            this.Add(new MathSubTypeItem("加法", MathSubType.Addition));
            this.Add(new MathSubTypeItem("减法", MathSubType.Subtraction));
            this.Add(new MathSubTypeItem("乘法", MathSubType.Multiplication));
            this.Add(new MathSubTypeItem("除法", MathSubType.Division));
        }
    }
}
