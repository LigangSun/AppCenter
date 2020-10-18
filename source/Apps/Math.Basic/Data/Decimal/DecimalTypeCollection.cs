using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Math.Data;

namespace Math.Basic.Data.Decimal
{
    internal class DecimalTypeCollection : ObservableCollection<MathSubTypeItem>
    {
        internal DecimalTypeCollection()
        {
            this.Add(new MathSubTypeItem("小数的意义", MathSubType.DefinitionOfDecimal));
            this.Add(new MathSubTypeItem("小数的形式", MathSubType.FormOfDecimal));
            this.Add(new MathSubTypeItem("纯小数", MathSubType.PureDecimal));
            this.Add(new MathSubTypeItem("带小数", MathSubType.WithDecimal));
            this.Add(new MathSubTypeItem("有限小数", MathSubType.FiniteDecimal));
            this.Add(new MathSubTypeItem("无限小数", MathSubType.InfiniteDecimal));
            this.Add(new MathSubTypeItem("无限不循环小数", MathSubType.InfiniteNonRepeatingDecimal));
            this.Add(new MathSubTypeItem("循环小数", MathSubType.RepeatingDecimal));
            this.Add(new MathSubTypeItem("纯循环小数", MathSubType.PureRepeatingDecimal));
            this.Add(new MathSubTypeItem("混循环小数", MathSubType.MixedRepeatingDecimal));
        }
    }
}
