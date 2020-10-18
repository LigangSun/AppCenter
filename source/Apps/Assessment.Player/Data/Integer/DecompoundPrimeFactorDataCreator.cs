using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Data;

namespace SoonLearning.Assessment.Player.Data
{
    class DecompoundPrimeFactorDataCreator : DataCreator
    {
        private List<int> questionValueList = new List<int>();

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "分解质因数练习";
            this.examTitle = "分解质因数测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Player.Data.Integer.DecompoundPrimeFactorDocument.xaml";

            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.MultiChoice,
                 "单选题：",
                 "（下面每道题都只有一个选项是正确的）",
                 5,
                 10,
                 100));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题：",
                "（在空格中填入符合条件的数）",
                10,
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
                case QuestionType.FillInBlank:
                    {
                        this.CreateFIBQuestion(info, section);
                    }
                    break;
            }
        }

        private void CreateMCQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            //int divValue = rand.Next(2, 9);
            string questionText = string.Format("请选出只能分解出两个质因数相乘的数。");

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
                if (sectionInfo is SectionValueRangeInfo)
                {
                    SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                    minValue = decimal.ToInt32(rangeInfo.MinValue);
                    maxValue = decimal.ToInt32(rangeInfo.MaxValue);
                }
                foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                            4, minValue, maxValue, false,
                             (c) =>
                             {
                                 int j, k, iFlag1 = 0, iFlag2 = 0, j2, k2;
                                 for (j = 2; j < c / 2 + 1; j++)
                                 {
                                     if (c % j == 0)
                                     {
                                         iFlag1 = 0;
                                         iFlag2 = 0;
                                         for (k = 2; k < j / 2 + 1; k++)
                                         {
                                             if (j % k == 0)
                                             {
                                                 iFlag1 = 1;
                                                 break;
                                             }
                                         }

                                         j2 = (int)(c / j);
                                         for (k2 = 2; k2 < j2 / 2 + 1; k2++)
                                         {
                                             if (j2 % k2 == 0)
                                             {
                                                 iFlag2 = 1;
                                                 break;
                                             }
                                         }

                                         if (iFlag1 == 0 && iFlag2 == 0)
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
                int j, k, iFlag1 = 0, iFlag2 = 0, j2 = 0, k2;
                int flag = -1;

                flag = 0;
                for (j = 2; j < value / 2 + 1; j++)
                {
                    if (value % j == 0)
                    {
                        iFlag1 = 0;
                        iFlag2 = 0;
                        for (k = 2; k < j / 2 + 1; k++)
                        {
                            if (j % k == 0)
                            {
                                iFlag1 = 1;
                                break;
                            }
                        }

                        j2 = (int)(value / j);
                        for (k2 = 2; k2 < j2 / 2 + 1; k2++)
                        {
                            if (j2 % k2 == 0)
                            {
                                iFlag2 = 1;
                                break;
                            }
                        }

                        if (iFlag1 == 0 && iFlag2 == 0)
                        {
                            flag = 1;
                            break;
                        }
                    }
                }

                if (flag == 1)
                {
                    strBuilder.AppendLine(string.Format("{0}可分解成质因数{1}，{2}相乘，是正确答案。", value, j, j2));
                }
                else
                {
                    strBuilder.AppendLine(string.Format("{0}不能分解质因数。", value));
                }
            }
        }

        private void CreateFIBQuestion(SectionBaseInfo sectionInfo, Section section)
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

            int value = 0;
            int j = 0, k, iFlag1 = 0, iFlag2 = 0, j2 = 0, k2;
            int flag = 0;

            while (true)
            {
                flag = 0;
                for (int i2 = 0; i2 < maxValue; i2++)
                {
                    value = rand.Next(minValue, maxValue);
                    for (j = 2; j < value / 2 + 1; j++)
                    {
                        if (value % j == 0)
                        {
                            iFlag1 = 0;
                            iFlag2 = 0;
                            for (k = 2; k < j / 2 + 1; k++)
                            {
                                if (j % k == 0)
                                {
                                    iFlag1 = 1;
                                    break;
                                }
                            }
                            j2 = (int)(value / j);
                            for (k2 = 2; k2 < j2 / 2 + 1; k2++)
                            {
                                if (j2 % k2 == 0)
                                {
                                    iFlag2 = 1;
                                    break;
                                }
                            }
                            if (iFlag1 == 0 && iFlag2 == 0)
                            {
                                flag = 1;
                                break;
                            }
                        }
                    }
                    if (flag == 1)
                        break;
                }

                if (Enumerable.Where<int>(questionValueList, (c => (c == value))).Count<int>() > 0)
                {
                    Thread.Sleep(10);
                    continue;
                }
                break;
            }

            questionValueList.Add(value);

            string questionText = string.Format("分解质因数 {0}=", value);

            FIBQuestion fibQuestion = new FIBQuestion();
            fibQuestion.Content.Content = questionText;
            fibQuestion.Content.ContentType = ContentType.Text;
            section.QuestionCollection.Add(fibQuestion);

            QuestionBlank blank1 = new QuestionBlank();
            QuestionContent blankContent1 = new QuestionContent();
            blankContent1.Content = j.ToString();
            blankContent1.ContentType = ContentType.Text;
            blank1.ReferenceAnswerList.Add(blankContent1);
            fibQuestion.QuestionBlankCollection.Add(blank1);

            QuestionBlank blank2 = new QuestionBlank();
            QuestionContent blankContent2 = new QuestionContent();
            blankContent2.Content = j2.ToString();
            blankContent2.ContentType = ContentType.Text;
            blank2.ReferenceAnswerList.Add(blankContent2);
            fibQuestion.QuestionBlankCollection.Add(blank2);

            fibQuestion.Content.Content += string.Format("{0}×{1}", blank1.PlaceHolder, blank2.PlaceHolder);
        }
        
        private string CreateSolution(int divValue)
        {
            if (divValue == 2)
            {
                return "";
            }

            return string.Empty;
        }

        public DecompoundPrimeFactorDataCreator()
        {

        }
    }
}
