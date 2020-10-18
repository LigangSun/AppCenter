using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Math.Data;

namespace SoonLearning.Assessment.Data
{
    internal class PowerExponentTypeCollection : ObservableCollection<MathSubTypeItem>
    {
        internal PowerExponentTypeCollection()
        {
            this.Add(new MathSubTypeItem("同底数幂乘法", MathSubType.MulOfSamePowerExponent));
        }
    }
}
