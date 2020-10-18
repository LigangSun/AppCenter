using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoonLearning.Assessment.Player.Data.Fraction
{
    internal class ProperFractionDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "分数定义练习";
            this.examTitle = "分数定义测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Player.Data.Fraction.ProperFractionFlowDocument.xaml";
        }

        protected override void AppendQuestion(SectionBaseInfo info, SoonLearning.Assessment.Data.Section section)
        {
            throw new NotImplementedException();
        }
    }
}
