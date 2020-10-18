using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;
using System.ComponentModel;
using System.Threading;

namespace SoonLearning.Assessment.Data.Fraction
{
    internal class DefinitionOfFractionDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "分数定义练习";
            this.examTitle = "分数定义测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Data.Fraction.DefinitionOfFractionFlowDocument.xaml";

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

            int minValue = 1;
            int maxValue = 10;
            int j = 2;
            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            string questionText = "从下面选项中选出分数。";
            List<QuestionContentPart> solutionContentPartList = new List<QuestionContentPart>();
            QuestionContent solution = new QuestionContent();
            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                this.AppendDoubleOption(minValue, maxValue, optionList, solution);
                this.AppendFractionOption(minValue, maxValue, optionList, solution);
                optionList[optionList.Count - 1].IsCorrect = true;
                this.AppendIntegerOption(minValue, maxValue, optionList, solution);

                return optionList;
            }
            );

            mcQuestion.Solution = solution;

            mcQuestion.RandomOption = true;

            section.QuestionCollection.Add(mcQuestion);
        }

        private void CreateTableQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            // Table Question
            Random rand = new Random((int)DateTime.Now.Ticks);

            string questionText = "请下表中选出分数。";

            QuestionContent solution = new QuestionContent();
            TableQuestion tableQuestion = ObjectCreator.CreateTableQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();
                int minValue = 1;
                int maxValue = 20;

                for (int j = 0; j < 36; j++)
                {
                    int randValue = rand.Next(100);
                    int leftValue = randValue % 4;
                    switch (leftValue)
                    {
                        case 0:
                            this.AppendDoubleOption(minValue, maxValue, optionList, solution);
                            break;
                        case 1:
                            this.AppendIntegerOption(minValue, maxValue, optionList, solution);
                            break;
                        case 2:
                        case 3:
                            this.AppendFractionOption(minValue, maxValue, optionList, solution);
                            optionList[optionList.Count - 1].IsCorrect = true;
                            break;
                    }
                }


                return optionList;
            });

            tableQuestion.Solution = solution;

            section.QuestionCollection.Add(tableQuestion);
        }

        protected void AppendDoubleOption(int minValue, int maxValue, List<QuestionOption> optionList, QuestionContent solution)
        {
            foreach (QuestionOption temp in ObjectCreator.CreateDoubleOption(minValue, maxValue, minValue, maxValue))
            {
                optionList.Add(temp);

                ArithmeticDecimalValuePart decimalValuePart = temp.OptionContent.QuestionPartCollection[0] as ArithmeticDecimalValuePart;
                solution.Content += string.Format("{0}是小数，不是正确答案。\n", decimalValuePart.Value);
            }
        }

        protected void AppendIntegerOption(int minValue, int maxValue, List<QuestionOption> optionList, QuestionContent solution)
        {
            foreach (QuestionOption temp in ObjectCreator.CreateIntegerOption(minValue, maxValue))
            {
                optionList.Add(temp);

                ArithmeticDecimalValuePart decimalValuePart = temp.OptionContent.QuestionPartCollection[0] as ArithmeticDecimalValuePart;
                solution.Content += string.Format("{0}是整数，不是正确答案。\n", decimalValuePart.Value);
            }
        }

        protected void AppendFractionOption(int minValue, int maxValue, List<QuestionOption> optionList, QuestionContent solution)
        {
            foreach (QuestionOption temp in ObjectCreator.CreateArithmeticFractionOption(minValue, maxValue, minValue, maxValue))
            {
                optionList.Add(temp);

                ArithmeticFractionValuePart fractionValuePart = temp.OptionContent.QuestionPartCollection[0] as ArithmeticFractionValuePart;
                solution.Content += string.Format("{0}是分数，是正确答案。\n", fractionValuePart.PlaceHolder);
                solution.QuestionPartCollection.Add(fractionValuePart);
            }
        }
    }
}
