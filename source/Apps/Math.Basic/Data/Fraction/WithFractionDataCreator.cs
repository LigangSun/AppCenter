using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;

namespace Math.Basic.Data.Fraction
{
    internal class WithFractionDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "分数定义练习";
            this.examTitle = "分数定义测验";
            this.flowDocumentFile = "Math.Basic.Data.Fraction.WithFractionFlowDocument.xaml";

            this.sectionInfoCollection.Add(new SectionBaseInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5));

            this.sectionInfoCollection.Add(new SectionBaseInfo(QuestionType.Table,
                "表格题：",
                "（从表格中选择符合条件的数）",
                5));
        }

        protected override void AppendQuestion(SectionBaseInfo info, Section section)
        {
            throw new NotImplementedException();
        }
    }
}
