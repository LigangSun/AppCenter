﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data;
using System.Threading;

namespace SoonLearning.Assessment.Player.Data.Equation
{
    internal class DefinitionOfEquationDataCreator : DataCreator
    {
        private List<decimal> questionValueList = new List<decimal>();

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "方程的定义练习";
            this.examTitle = "方程的定义测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Player.Data.Equation.DefinitionOfEquationFlowDocument.xaml";

            SectionValueRangeInfo info = new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                1,
                10);
            this.sectionInfoCollection.Add(info);

            //this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
            //    "填空题：",
            //    "（按照方程的定义填空）",
            //    5,
            //    0,
            //    10));

            //this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.Table,
            //    "表格题：",
            //    "（从表格中选择符合方程的定义条件的等式）",
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

            decimal result = valueA + valueB + valueC;

            string questionText = "从下面选项中选出符合方程定义的等式。";

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                for (int i = 0; i < 3; i++)
                {
                    decimal a = rand.Next(minValue, maxValue + 1);
                    decimal b = rand.Next(minValue, maxValue + 1);
                    decimal c = rand.Next(minValue, maxValue + 1);
                    decimal d = rand.Next(minValue, maxValue + 1);
                    if (d == c)
                    {
                        d = d == 0 ? d + 1 : d - 1;
                    }

                    QuestionOption option = new QuestionOption();
                    switch (i)
                    {
                        case 0:
                            option.OptionContent.Content = string.Format("{0} + {1} = {2}", a, b, c);
                            break;
                        case 1:
                            option.OptionContent.Content = string.Format("{0} - {1} = {2} + {3}", a, b, c, d);
                            break;
                        case 2:
                            option.OptionContent.Content = string.Format("{0} × {1} = {2} × {3}", a, b, c, d);
                            break;
                        default:
                            break;
                    }
                   optionList.Add(option);
                }

                QuestionOption correctOption = new QuestionOption();
                correctOption.OptionContent.Content = string.Format("x + {0} = {1}", valueA, valueB);
                correctOption.IsCorrect = true;
                int correctIndex = rand.Next(100) % 4;
                if (correctIndex == optionList.Count)
                    optionList.Add(correctOption);
                else
                    optionList.Insert(correctIndex, correctOption);

                return optionList;
            }
            );

            mcQuestion.Solution.Content = string.Format("x + {0} = {1}，在这个等式中，含有未知数x。\n是符合方程的定义，是正确答案。", valueA, valueB);

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

            string questionText = "从表格中选择符合方程的定义条件的等式";

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
