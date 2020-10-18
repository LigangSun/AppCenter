using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Math.Data;

namespace Math.Basic.Data
{
    class CoprimenumbersDataCreator : DataCreator
    {
        private List<int> questionValueList = new List<int>();
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "互质数练习";
            this.examTitle = "互质数测验";
            this.flowDocumentFile = "Math.Basic.Data.Integer.CoprimenumbersDocument.xaml";
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
                    this.CreateMCQuestion(info, section);
                    break;
                case QuestionType.Table:
                    this.CreateTableQuestion(info, section);
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

            if (section.QuestionCollection.Count == 0)
                this.questionValueList.Clear();

            Random rand = new Random((int)DateTime.Now.Ticks);
            int valueA = 0, valueC = 0;

            //valueA = rand.Next(minValue, maxValue);
            while (true)
            {
                int sumCommonDivisor = 0;
                valueA = rand.Next(minValue, maxValue);
                //valueB = rand.Next(minValue, maxValue);

                //if (valueA < valueB)
                //    valueC = valueA;
                //else if (valueA > valueB)
                //    valueC = valueB;
                //else
                //    continue;

                for (int j2 = 1; j2 < valueA; j2++)
                {
                    if (valueA % j2 == 0)
                    {
                        sumCommonDivisor++;
                    }
                }
                if (sumCommonDivisor >= 4)
                    break;
            }

            string questionText = string.Format("请选出与{0}互质的数。", valueA);

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
                            4, minValue, maxValue, false,
                            (c) =>
                            {
                                int sumCommonDivisor = 0;
                                if (valueA < c)
                                    valueC = valueA;
                                else if (valueA > c)
                                    valueC = decimal.ToInt32(c);
                                else
                                    return false;

                                for (int j2 = 1; j2 < valueC; j2++)
                                {
                                    if (valueA % j2 == 0 && c % j2 == 0)
                                    {
                                        sumCommonDivisor++;
                                    }
                                }
                                if (sumCommonDivisor == 1)
                                    return true;
                                return false;
                            }
                            ))
                {
                    optionList.Add(option);
                }

                return optionList;
            }
            );

            section.QuestionCollection.Add(mcQuestion);

            StringBuilder strBuilder = new StringBuilder();
            foreach (QuestionOption option in mcQuestion.QuestionOptionCollection)
            {
                QuestionContent content = option.OptionContent;
                decimal value = System.Convert.ToDecimal(content.Content);
                int sumCommonDivisor = 0;

                if (valueA < value)
                    valueC = valueA;
                else
                    valueC = decimal.ToInt32(value);

                for (int j2 = 1; j2 < valueC; j2++)
                {
                    if (valueA % j2 == 0 && value % j2 == 0)
                    {
                        sumCommonDivisor++;
                    }
                }

                if (sumCommonDivisor == 1)
                {
                    strBuilder.AppendLine(string.Format("{0}和{1}只有公约数1，是正确答案。", valueA, value));
                }
                else
                {
                    strBuilder.AppendLine(string.Format("{0}和{1}有{2}个公约数。", valueA, value, sumCommonDivisor));
                }
            }
            mcQuestion.Solution.Content = strBuilder.ToString();
        }

        private void CreateTableQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            // Table Question
            Section sectionTable = ObjectCreator.CreateSection(sectionInfo.Name, sectionInfo.Description);

            Random rand = new Random((int)DateTime.Now.Ticks);

            int minValue = 10;
            int maxValue = 100;
            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            int valueA = rand.Next(minValue, maxValue);
            string questionText = string.Format("请在下表中选出与{0}互质的数。", valueA);

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
                            36, minValue, maxValue, true,
                            (c) =>
                            {
                                int sumCommonDivisor = 0;
                                int valueC;
                                if (valueA < c)
                                    valueC = valueA;
                                else if (valueA > c)
                                    valueC = decimal.ToInt32(c);
                                else
                                    return false;

                                for (int j2 = 1; j2 < valueC; j2++)
                                {
                                    if (valueA % j2 == 0 && c % j2 == 0)
                                    {
                                        sumCommonDivisor++;
                                    }
                                }
                                if (sumCommonDivisor == 1)
                                    return true;
                                return false;
                            }
                            ))
                    optionList.Add(option);

                return optionList;
            });

            tableQuestion.Tip = this.CreateTip();

            section.QuestionCollection.Add(tableQuestion);
        }

        private string CreateTip()
        {
            return @"1和任何自然数互质。
                	相邻的两个自然数互质。 
                	两个不同的质数互质。 
                	当合数不是质数的倍数时，这个合数和这个质数互质。 
                	两个合数的公约数只有1时，这两个合数互质
                	如果几个数中任意两个都互质，就说这几个数两两互质。 
                	如果两个数是互质数，它们的最大公约数就是1。 
                	如果两个数是互质数，那么这两个数的积就是它们的最小公倍数。
                    ";
        }

        public CoprimenumbersDataCreator()
        {

        }
    }
}
