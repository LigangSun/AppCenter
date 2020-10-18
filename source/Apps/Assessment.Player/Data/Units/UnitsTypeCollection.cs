using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Assessment.Data;

namespace SoonLearning.Assessment.Player.Data.Units
{
    internal class UnitsTypeCollection : ObservableCollection<MathSubTypeItem>
    {
        internal UnitsTypeCollection()
        {
            this.Add(new MathSubTypeItem("长度单位", MathSubType.UnitsOfLength));
            this.Add(new MathSubTypeItem("面积单位", MathSubType.UnitsOfArea));
            this.Add(new MathSubTypeItem("体积（容积）单位", MathSubType.UnitsOfVolume));
            this.Add(new MathSubTypeItem("质量单位", MathSubType.UnitsOfWeight));
            this.Add(new MathSubTypeItem("时间单位", MathSubType.UnitsOfTime));
            this.Add(new MathSubTypeItem("月", MathSubType.Month));
            this.Add(new MathSubTypeItem("年", MathSubType.Year));
            this.Add(new MathSubTypeItem("名数", MathSubType.ConcreteNumber));
        }
    }
}
