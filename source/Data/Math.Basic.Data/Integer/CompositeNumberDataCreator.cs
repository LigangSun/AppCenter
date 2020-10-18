using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Math.Data;

namespace SoonLearning.Assessment.Data
{
    class CompositeNumberDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "合数练习";
            this.examTitle = "合数测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Data.Integer.CompositeNumberDocument.xaml";
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

        private void CreateMCQuestion(SectionBaseInfo info, Section section)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            int divValue = rand.Next(2, 9);
            string questionText = string.Format("请选出是合数的数。");

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();
                int minValue = 10;
                int maxValue = 100;
                if (info is SectionValueRangeInfo)
                {
                    SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                    minValue = decimal.ToInt32(rangeInfo.MinValue);
                    maxValue = decimal.ToInt32(rangeInfo.MaxValue);
                }
                foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                            4, minValue, maxValue, false,
                             (c) =>
                             {
                                 for (int j = 2; j < c / 2 + 1; j++)
                                 {
                                     if (c % j == 0)
                                         return true;
                                 }
                                 return false;
                             }))
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
                int flag = 1;
                int j = 1;

                if (value == 0)
                    flag = 0;

                for (j = 2; j < value / 2 + 1; j++)
                {
                    if (value % j == 0)
                    {
                        flag = 2;
                        break;
                    }
                }

                if (flag == 0)
                {
                    strBuilder.AppendLine(string.Format("0 不是合数。"));
                }
                else if (flag == 1)
                {
                    strBuilder.AppendLine(string.Format("{0}只有两个约数{1}，{2}。", value, 1, value));
                }
                else
                {
                    strBuilder.AppendLine(string.Format("{0}有约数{1}，{2}，{3}...，大于2个，是正确答案。", value, 1, value, j));
                }
            }

            mcQuestion.Solution.Content = strBuilder.ToString();
        }

        private void CreateTableQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            string questionText = string.Format("请下表中选出是合数的数。");

            StringBuilder strBuilder = new StringBuilder();
            TableQuestion tableQuestion = ObjectCreator.CreateTableQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();
                int minValue = 10;
                int maxValue = 100;
                if (sectionInfo is SectionValueRangeInfo)
                {
                    SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                    minValue = decimal.ToInt32(rangeInfo.MinValue);
                    maxValue = decimal.ToInt32(rangeInfo.MaxValue);
                }
                foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                            36, minValue, maxValue, true,
                              (c) =>
                              {
                                  for (int j = 2; j < c / 2 + 1; j++)
                                  {
                                      if (c % j == 0)
                                          return true;
                                  }
                                  return false;
                              }))
                {
                    optionList.Add(option);

                    decimal optionValue = Convert.ToDecimal(option.OptionContent.Content);
                    if (optionValue == 0)
                    {
                        strBuilder.AppendLine(string.Format("0 不是合数。"));
                    }
                    else if (!option.IsCorrect)
                    {
                        strBuilder.AppendLine(string.Format("{0}只有两个约数{1}，{2}。", optionValue, 1, optionValue));
                    }
                    else
                    {
                        decimal thirdValue = 0;
                        for (int j = 2; j < optionValue / 2 + 1; j++)
                        {
                            if (optionValue % j == 0)
                            {
                                thirdValue = j;
                                break;
                            }
                        }
                        strBuilder.AppendLine(string.Format("{0}有约数{1}，{2}，{3}...，大于2个，是正确答案。", optionValue, 1, optionValue, thirdValue));
                    }
                }

                return optionList;
            });

            tableQuestion.Solution.Content = strBuilder.ToString();
            tableQuestion.Tip = "合数至少有3个约数。";

            section.QuestionCollection.Add(tableQuestion);
        }

        private string CreateSolution(int divValue)
        {
            if (divValue == 2)
            {
                return "合数至少有3个约数。";
            }

            return string.Empty;
        }

        public CompositeNumberDataCreator()
        {

        }
    }
}
