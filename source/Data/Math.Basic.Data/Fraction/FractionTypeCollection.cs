using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Math.Data;

namespace SoonLearning.Assessment.Data.Fraction
{
    internal class FractionTypeCollection : ObservableCollection<MathSubTypeItem>
    {
        internal FractionTypeCollection()
        {
            this.Add(new MathSubTypeItem("分数的定义", MathSubType.DefinitionOfFraction));
            this.Add(new MathSubTypeItem("分数单位", MathSubType.UnitOfFraction));
            this.Add(new MathSubTypeItem("分母分子", MathSubType.DenominatorAndNumerator));
            this.Add(new MathSubTypeItem("真分数", MathSubType.ProperFraction));
            this.Add(new MathSubTypeItem("假分数", MathSubType.ImproperFraction));
            this.Add(new MathSubTypeItem("带分数", MathSubType.WithFraction));
            this.Add(new MathSubTypeItem("最简分数", MathSubType.FractionInLowestTerm));
            this.Add(new MathSubTypeItem("约分", MathSubType.ReductionOfFraction));
            this.Add(new MathSubTypeItem("通分", MathSubType.ReductionToCommonDenomiator));
            this.Add(new MathSubTypeItem("分数和除法的联系", MathSubType.RelationshipInFractionAndDivision));
            this.Add(new MathSubTypeItem("分数和小数的联系", MathSubType.RelationshipInFractionAndDecimal));
            this.Add(new MathSubTypeItem("分数和比的联系", MathSubType.RelationshipInFractionAndRatio));
            this.Add(new MathSubTypeItem("分数和有限小数的联系", MathSubType.RelationshipInFractionAndFiniteDecimal));
            this.Add(new MathSubTypeItem("分数的基本性质", MathSubType.CharacterOfFraction));
            this.Add(new MathSubTypeItem("百分数", MathSubType.Percent));
        }
    }
}
