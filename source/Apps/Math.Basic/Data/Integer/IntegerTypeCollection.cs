using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Math.Data;

namespace Math.Basic.Data
{
    public class IntegerTypeCollection : ObservableCollection<MathSubTypeItem>
    {
        internal IntegerTypeCollection()
        {
            this.Add(new MathSubTypeItem("整除", MathSubType.ExactDivision));
            this.Add(new MathSubTypeItem("约数", MathSubType.Submultiple));
            this.Add(new MathSubTypeItem("倍数", MathSubType.Multiple));
            this.Add(new MathSubTypeItem("奇数", MathSubType.OddNumber));
            this.Add(new MathSubTypeItem("偶数", MathSubType.EvenNumber));
            this.Add(new MathSubTypeItem("质数（素数）", MathSubType.PrimeNumber));
            this.Add(new MathSubTypeItem("合数", MathSubType.CompositeNumber));
            this.Add(new MathSubTypeItem("质因数", MathSubType.PrimeFactor));
            this.Add(new MathSubTypeItem("分解质因数", MathSubType.DecompoundPrimeFactor));
            this.Add(new MathSubTypeItem("公约数", MathSubType.CommonDivisor));
            this.Add(new MathSubTypeItem("公倍数", MathSubType.CommonMultiple));
            this.Add(new MathSubTypeItem("互质数", MathSubType.Coprimenumbers));
        }
    }
}
