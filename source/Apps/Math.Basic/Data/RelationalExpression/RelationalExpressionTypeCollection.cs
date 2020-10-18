using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Math.Data;

namespace Math.Basic.Data.RelationalExpression
{
    internal class RelationalExpressionTypeCollection : ObservableCollection<MathSubTypeItem>
    {
        internal RelationalExpressionTypeCollection()
        {
            this.Add(new MathSubTypeItem("速度问题", MathSubType.ProblemsOfSpeed));
            this.Add(new MathSubTypeItem("工作问题", MathSubType.ProblemsOfWork));
            this.Add(new MathSubTypeItem("价格问题", MathSubType.ProblemsOfPrice));
        }
    }
}
