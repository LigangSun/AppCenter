using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;
using System.Threading;
using System.ComponentModel;

namespace SoonLearning.Assessment.Data.Decimal
{
    class FiniteDecimalDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "有限小数练习";
            this.examTitle = "有限小数测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Data.Decimal.FiniteDecimalDocument.xaml";
           //this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
           //     "填空题：",
           //     "（在空格中填入符合条件的数）",
           //     5,
           //     0,
           //     10));         
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                0,
                10));
            //this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.Table,
            //   "表格题：",
            //   "（从表格中选择符合条件的数）",
            //   5,
            //   0,
            //   10));
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
            Random rand = new Random((int)DateTime.Now.Ticks);

            int minValue = 0;
            int maxValue = 10 + 1;
            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            string questionText = string.Format("选出是有限小数的数。");

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                foreach (QuestionOption temp in ObjectCreator.CreateDoubleTextOption(0, 0, minValue + 1, maxValue, "……"))
                {
                    optionList.Add(temp);
                }

                foreach (QuestionOption temp in ObjectCreator.CreateDoubleTextOption(0, 0, minValue + 1, maxValue, "……"))
                {
                    optionList.Add(temp);
                }

                foreach (QuestionOption temp in ObjectCreator.CreateDoubleTextOption(0, 0, minValue + 1, maxValue, ""))
                {
                    temp.IsCorrect = true;
                    optionList.Add(temp);
                }

                foreach (QuestionOption temp in ObjectCreator.CreateDoubleTextOption(minValue, maxValue + 1, minValue + 1, maxValue, "……"))
                {
                    optionList.Add(temp);
                }

                return optionList;
            }
            );

            mcQuestion.RandomOption = true;

            section.QuestionCollection.Add(mcQuestion);

            StringBuilder strBuilder = new StringBuilder();
            foreach (QuestionOption option in mcQuestion.QuestionOptionCollection)
            {
                QuestionContent content = option.OptionContent;
                if (content.QuestionPartCollection[0] is QuestionTextPart)
                {
                    QuestionTextPart decimalPart = content.QuestionPartCollection[0] as QuestionTextPart;
                    if (decimalPart.Text.IndexOf("……") < 0)
                    {
                        strBuilder.AppendLine(string.Format("{0}是有限小数，是正确答案。", decimalPart.Text));
                    }
                }
            }

            mcQuestion.Solution.Content = strBuilder.ToString();
        }

        private void CreateTableQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            // Table Question
            Random rand = new Random((int)DateTime.Now.Ticks);

            int minValue = 0;
            int maxValue = 10 + 1;
            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            string questionText = string.Format("请下表中选出所有的有限小数。");

            TableQuestion tableQuestion = ObjectCreator.CreateTableQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                for (int k = 0; k < 36; k++)
                {
                    int rand0 = rand.Next(0, 2);
                    if (rand0 != 0)
                    {
                        foreach (QuestionOption temp in ObjectCreator.CreateDoubleOption(0, 0, minValue + 1, maxValue))
                        {
                            optionList.Add(temp);
                        }
                    }
                    else
                    {
                        foreach (QuestionOption temp in ObjectCreator.CreateDoubleOption(minValue + 1, maxValue, minValue, maxValue))
                        {
                            temp.IsCorrect = true;
                            optionList.Add(temp);
                        }
                    }
                }

                return optionList;
            });

            tableQuestion.Solution.Content = this.CreateSolution(0);

            section.QuestionCollection.Add(tableQuestion);
        }

        private void CreateFIBQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            int minValue = 0;
            int maxValue = 10 + 1;
            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            int valueA = 0, valueB = 0;

            int[] value = new int[2];
            valueA = rand.Next(minValue, maxValue + 1);
            valueB = rand.Next(minValue + 1, maxValue + 1);

            string questionText = string.Format("写出小数{0}.{1}的整数部分和小数部分", valueA, valueB);

            FIBQuestion fibQuestion = new FIBQuestion();
            fibQuestion.Content.Content = questionText;
            fibQuestion.Content.ContentType = ContentType.Text;
            section.QuestionCollection.Add(fibQuestion);

            value[0] = valueA;
            value[1] = valueB;

            for (int j2 = 0; j2 < 2; j2++)
            {
                QuestionBlank blank = new QuestionBlank();

                QuestionContent blankContent = new QuestionContent();
                blankContent.Content = value[j2].ToString();
                blankContent.ContentType = ContentType.Text;
                blank.ReferenceAnswerList.Add(blankContent);

                fibQuestion.QuestionBlankCollection.Add(blank);

                fibQuestion.Content.Content += blank.PlaceHolder;
            }
        }
        
        private string CreateSolution(int value)
        {
            if (value == 1)
            {
                return "";
            }

            return string.Empty;
        }

        public FiniteDecimalDataCreator()
        {

        }
    }
}
