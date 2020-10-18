using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data;
using System.Threading;

namespace SoonLearning.Assessment.Player.Data.Equation
{
    internal class SolutionOfEquationDataCreator : DataCreator
    {
        private List<decimal> questionValueList = new List<decimal>();

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "方程的解练习";
            this.examTitle = "方程的解测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Player.Data.Equation.SolutionOfEquationFlowDocument.xaml";

            SectionValueRangeInfo info = new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                1,
                10);
            this.sectionInfoCollection.Add(info);

            //this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
            //    "填空题：",
            //    "（按照方程的解填空）",
            //    5,
            //    0,
            //    10));

            //this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.Table,
            //    "表格题：",
            //    "（从表格中选择符合方程的解条件的等式）",
            //    5,
            //    0,
            //    10));
        }

        protected override void AppendQuestion(SectionBaseInfo info, SoonLearning.Assessment.Data.Section section)
        {
            switch (info.QuestionType)
            {
                case QuestionType.MultiChoice:
                    this.CreateMCQuestion(info, section);
                    break;
                case QuestionType.FillInBlank:
                    this.CreateFIBQuestion(info, section);
                    break;
                case QuestionType.Table:
                    this.CreateTableQuestion(info, section);
                    break;
            }
        }

        private void CreateMCQuestion(SectionBaseInfo info, Section section)
        {
            int minValue = 10;
            int maxValue = 100;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            decimal valueA = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueB = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueC = rand.Next(minValue, maxValue + 1);

            decimal result = valueA + valueB;

            string questionText = string.Format("从下面选项中选出方程x + {0} = {1} 的解。", valueB, result);

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                //for (int i = 0; i < 3; i++)
                //{
                    decimal d = rand.Next(minValue, maxValue + 1);
                    if (d == valueA)
                    {
                        d = d == 0 ? d + 1 : d - 1;
                    }
                    QuestionOption option = new QuestionOption();
                    option.OptionContent.Content = string.Format("{0}", d);
                    optionList.Add(option);
                //}

                //int tmpminValue, tmpmaxValue;
                //if (d <= 2)
                //{
                //    tmpminValue = decimal.ToInt32(d) + 1;
                //    tmpmaxValue = maxValue;
                //}
                //else
                //{
                //    tmpminValue = decimal.ToInt32(d) + 1;
                //    tmpmaxValue = maxValue;
                //}
                decimal d1;
                while (true)
                {
                    d1 = rand.Next(minValue, maxValue + 1);
                    if (d1 != valueA && d1 != d)
                    {
                        break;
                    }
                }
                QuestionOption option1 = new QuestionOption();
                option1.OptionContent.Content = string.Format("{0}", d1);
                optionList.Add(option1);

                decimal d2;
                while (true)
                {
                    d2 = rand.Next(minValue, maxValue + 1);
                    if (d2 != valueA && d2 != d && d2 != d1)
                    {
                        break;
                    }
                }
                QuestionOption option2 = new QuestionOption();
                option2.OptionContent.Content = string.Format("{0}", d2);
                optionList.Add(option2);

                QuestionOption correctOption = new QuestionOption();
                correctOption.OptionContent.Content = string.Format("{0}", valueA);
                correctOption.IsCorrect = true;
                int correctIndex = rand.Next(100) % 4;
                if (correctIndex == optionList.Count)
                    optionList.Add(correctOption);
                else
                    optionList.Insert(correctIndex, correctOption);

                return optionList;
            }
            );

            mcQuestion.Solution.Content = string.Format("当x={2}，方程x + {0} = {1}等式左右两边相等，因此{2}是方程的解，是正确答案。", valueB, result, valueA);

            section.QuestionCollection.Add(mcQuestion);
        }

        private void CreateFIBQuestion(SectionBaseInfo info, Section section)
        {
            int minValue = 10;
            int maxValue = 100;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            decimal valueA = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueB = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueC = rand.Next(minValue, maxValue + 1);

            decimal result = valueA + valueB + valueC;
            this.questionValueList.Add(result);

            string questionText = string.Format("({0} + {1}) + {2} = ", valueA, valueB, valueC);

            FIBQuestion fibQuestion = new FIBQuestion();
            fibQuestion.Content.Content = questionText;
            fibQuestion.Content.ContentType = ContentType.Text;
            fibQuestion.ShowBlankInContent = true;

            QuestionBlank blankA = new QuestionBlank();
            blankA.MatchOwnRefAnswer = true;
            QuestionContent blankContentA = new QuestionContent();
            blankContentA.Content = valueB.ToString();
            blankContentA.ContentType = ContentType.Text;
            blankA.ReferenceAnswerList.Add(blankContentA);
            fibQuestion.QuestionBlankCollection.Add(blankA);
            fibQuestion.Content.Content += blankA.PlaceHolder;

            fibQuestion.Content.Content += " + ";

            QuestionBlank blankB = new QuestionBlank();
            blankB.MatchOwnRefAnswer = true;
            QuestionContent blankContentB = new QuestionContent();
            blankContentB.Content = valueA.ToString();
            blankContentB.ContentType = ContentType.Text;
            blankB.ReferenceAnswerList.Add(blankContentB);
            fibQuestion.QuestionBlankCollection.Add(blankB);
            fibQuestion.Content.Content += blankB.PlaceHolder;

            fibQuestion.Solution.Content = string.Format("交换两个因数{0}和{1}的位置， 结果是{1}和{0}。", valueA, valueB);

            section.QuestionCollection.Add(fibQuestion);
        }

        private void CreateTableQuestion(SectionBaseInfo info, Section section)
        {
            int minValue = 10;
            int maxValue = 100;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            string questionText = "从表格中选择符合方程的解条件的等式";

            TableQuestion tableQuestion = ObjectCreator.CreateTableQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                for (int j = 0; j < 36; j++)
                {
                    if (rand.Next() % 2 == 0) // Create correct Option
                    {
                        decimal valueA = rand.Next(minValue, maxValue + 1);
                        decimal valueB = rand.Next(minValue, maxValue + 1);
                        QuestionOption option = new QuestionOption();
                        option.IsCorrect = true;
                        option.OptionContent.Content = string.Format("{0}×{1}={1}×{0}", valueA, valueB);
                        optionList.Add(option);
                    }
                    else
                    {
                        decimal valueA = rand.Next(minValue, maxValue + 1);
                        decimal valueB = rand.Next(minValue, maxValue + 1);
                        decimal valueC = rand.Next(minValue, decimal.ToInt32(valueA + valueB + 1));
                        decimal valueD = valueA + valueB - valueC;
                        QuestionOption option = new QuestionOption();
                        option.IsCorrect = (valueC == valueB) ? true : false;
                        option.OptionContent.Content = string.Format("{0}×{1}={2}×{3}", valueA, valueB, valueC, valueD);
                        optionList.Add(option);
                    }
                }

                return optionList;
            }
            );

            //   tableQuestion.Solution.Content = string.Format("加数{0}与加数{1}的和是{2}，所以正确答案是{2}。", valueA, valueB, result);

            section.QuestionCollection.Add(tableQuestion);
        }
    }
}
