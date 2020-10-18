using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Math.Basic.Data.Fraction
{
    internal class UnitOfFractionDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "分数定义练习";
            this.examTitle = "分数定义测验";
            this.flowDocumentFile = "Math.Basic.Data.Fraction.UnitOfFractionFlowDocument.xaml";
        }

        protected override void AppendQuestion(SectionBaseInfo info, SoonLearning.Math.Data.Section section)
        {
            throw new NotImplementedException();
        }
    }
}
