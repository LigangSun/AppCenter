using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;
using System.Threading;
using System.ComponentModel;

namespace Math.Basic.Data
{
    internal class MultipleDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "倍数练习";
            this.examTitle = "倍数测验";
            this.flowDocumentFile = "Math.Basic.Data.Fraction.MultipleDocument.xaml";

            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.MultiChoice,
               "单选题：",
               "（下面每道题都只有一个选项是正确的）",
               5,
               10,
               100));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.Table,
                "表格题：",
                "（从表格中选择符合条件的数）",
                8,
                10,
                100));
        }

        protected override void AppendQuestion(SectionBaseInfo info, Section section)
        {
            switch (info.QuestionType)
            {
                case QuestionType.MultiChoice:
                    {
                        this.CreateMCQuestion(info, section);
                    }
                    break;
                case QuestionType.Table:
                    {
                        this.CreateTableQuestion(info, section);
                    }
                    break;
            }
        }

        private void CreateMCQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            int minValue = 10;
            int maxValue = 100;
            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            int inMinValue = 2, inMaxValue = 2;
            if (minValue / 10 > inMinValue)
                inMinValue = minValue / 10;
            inMaxValue = maxValue / 10;

            Random rand = new Random((int)DateTime.Now.Ticks);

            int divValue = rand.Next(inMinValue, inMaxValue);
            string questionText = string.Format("请选出是{0}倍数的数。", divValue);

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();
                foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                            4, minValue, maxValue, false, (c => ((c % divValue) == 0))))
                    optionList.Add(option);

                return optionList;
            }
            );

            section.QuestionCollection.Add(mcQuestion);

            StringBuilder strBuilder = new StringBuilder();
            foreach (QuestionOption option in mcQuestion.QuestionOptionCollection)
            {
                QuestionContent content = option.OptionContent;
                decimal value = System.Convert.ToDecimal(content.Content);
                if (value % divValue == 0)
                {
                    strBuilder.AppendLine(string.Format("{1}除以{0}等于{2}，没有余数，是正确答案。", value, divValue, value / divValue));
                }
                else
                {
                    strBuilder.AppendLine(string.Format("{1}除以{0}等于{2}，余数是{3}。", value, divValue, (int)(value) / divValue, value % divValue));
                }
            }
            mcQuestion.Solution.Content = strBuilder.ToString();

        }

        private void CreateTableQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            // Table Question

            int minValue = 10;
            int maxValue = 100;
            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            int inMinValue = 2, inMaxValue = 2;
            if (minValue / 10 > inMinValue)
                inMinValue = minValue / 10;
            inMaxValue = maxValue / 10;

            Random rand = new Random((int)DateTime.Now.Ticks);

            int divValue = rand.Next(2, 10);
            //int divValue = rand.Next(inMinValue, inMaxValue);
            string questionText = string.Format("请下表中选出是{0}倍数的数。", divValue);

            TableQuestion tableQuestion = ObjectCreator.CreateTableQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                            36, minValue, maxValue, true, (c => ((c % divValue) == 0))))
                    optionList.Add(option);

                return optionList;
            });

            tableQuestion.Solution.Content = this.CreateSolution(divValue);

            section.QuestionCollection.Add(tableQuestion);
        }

        private string CreateSolution(int divValue)
        {
            if (divValue == 2)
            {
                return "个位数是0,2,4,6,8的数都是2的倍数。";
            }

            return string.Empty;
        }

        public MultipleDataCreator()
        {

        }
    }
}
