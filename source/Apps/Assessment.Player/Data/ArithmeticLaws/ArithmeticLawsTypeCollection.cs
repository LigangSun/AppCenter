using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Assessment.Data;

namespace SoonLearning.Assessment.Player.Data.ArithmeticLaws
{
    internal class ArithmeticLawsTypeCollection: ObservableCollection<MathSubTypeItem>
    {
        internal ArithmeticLawsTypeCollection()
        {
            this.Add(new MathSubTypeItem("加法交换律", MathSubType.CommutativeLawOfAddition));
            this.Add(new MathSubTypeItem("乘法交换律", MathSubType.CommutativeLawOfMultiplication));
            this.Add(new MathSubTypeItem("加法结合律", MathSubType.AssociativeLawOfAddition));
            this.Add(new MathSubTypeItem("乘法结合律", MathSubType.AssociativeLawOfMultiplication));
            this.Add(new MathSubTypeItem("乘法分配率", MathSubType.DistributiveLawOfMultiplication));
            this.Add(new MathSubTypeItem("减法的性质", MathSubType.CharacterOfSubtraction));
            this.Add(new MathSubTypeItem("除法的性质", MathSubType.CharacterOfDivision));
        }
    }
}
