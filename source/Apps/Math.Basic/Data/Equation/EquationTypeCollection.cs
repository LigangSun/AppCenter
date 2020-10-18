using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Math.Data;

namespace Math.Basic.Data.Equation
{
    internal class EquationTypeCollection : ObservableCollection<MathSubTypeItem>
    {
        internal EquationTypeCollection()
        {
            this.Add(new MathSubTypeItem("方程的定义", MathSubType.DefinitionOfEquation));
            this.Add(new MathSubTypeItem("方程的解", MathSubType.SolutionOfEquation));
            this.Add(new MathSubTypeItem("解方程", MathSubType.SolveEquation));
            this.Add(new MathSubTypeItem("同底数幂的乘法", MathSubType.MulOfSamePowerExponent));
        }
    }
}
