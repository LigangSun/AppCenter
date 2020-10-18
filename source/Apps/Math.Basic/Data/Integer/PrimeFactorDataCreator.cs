using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Math.Data;

namespace Math.Basic.Data
{
    class PrimeFactorDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "质因数练习";
            this.examTitle = "质因数测验";
            this.flowDocumentFile = "Math.Basic.Data.Integer.PrimeFactorDocument.xaml";

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
            Random rand = new Random((int)DateTime.Now.Ticks);

            int minValue = 10;
            int maxValue = 100;

            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            int divValue = rand.Next(minValue, maxValue);
            string questionText = string.Format("请选出至少有两个质因数的数。");

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
                                  int j, k, iFlag = 0;
                                  for (j = 2; j < c / 2 + 1; j++)
                                  {
                                      if (c % j == 0)
                                      {
                                          iFlag = 0;
                                          for (k = 2; k < j / 2 + 1; k++)
                                          {
                                              if (j % k == 0)
                                              {
                                                  iFlag = 1;
                                                  break;
                                              }
                                          }

                                          if (iFlag == 0)
                                              return true;
                                      }
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
                int j, k, iFlag = 0, flag = -1;

                if (value == 0 || value == 1)
                    flag = 0;

                for (j = 2; j < value / 2 + 1; j++)
                {
                    if (value % j == 0)
                    {
                        if (j == 2)
                        {
                            flag = 1;
                            break;
                        }

                        iFlag = 0;
                        for (k = 2; k < j / 2 + 1; k++)
                        {
                            if (j % k == 0)
                            {
                                iFlag = 1;
                                break;
                            }
                        }

                        if (iFlag == 0)
                        {
                            flag = 1;
                            break;
                        }
                    }
                }

                if (flag == 0)
                {
                    strBuilder.AppendLine(string.Format("0和1都不是质因数。"));
                }
                else if (flag == 1)
                {
                    strBuilder.AppendLine(string.Format("{0}有两个质因数，是正确答案。", value));
                }
                else
                {
                    strBuilder.AppendLine(string.Format("{0}没有质因数。", value));
                }
            }
            mcQuestion.Solution.Content = strBuilder.ToString();
        }

        private void CreateTableQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            // Table Question
            Random rand = new Random((int)DateTime.Now.Ticks);

            string questionText = string.Format("请下表中选出至少有两个质因数的数。");

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
                                 int j, k, iFlag = 0;
                                 for (j = 2; j < c / 2 + 1; j++)
                                 {
                                     if (c % j == 0)
                                     {
                                         iFlag = 0;
                                         for (k = 2; k < j / 2 + 1; k++)
                                         {
                                             if (j % k == 0)
                                             {
                                                 iFlag = 1;
                                                 break;
                                             }
                                         }

                                         if (iFlag == 0)
                                             return true;
                                     }
                                 }
                                 return false;
                             }))
                {
                    optionList.Add(option);
                }

                return optionList;
            });

            tableQuestion.Tip = this.CreateTip();

            section.QuestionCollection.Add(tableQuestion);
        }

        private string CreateTip()
        {
            return "1不是质数, 也不是质因数。";
        }

        public PrimeFactorDataCreator()
        {

        }
    }
}