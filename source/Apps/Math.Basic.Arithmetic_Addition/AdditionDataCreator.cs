﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math.Arithmetic_Addition
{
    public class AdditionDataCreator : DataCreator
    {
        private static AdditionDataCreator creator;

        public static AdditionDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new AdditionDataCreator();

                return creator;
            }
        }

        private List<decimal> questionValueList = new List<decimal>();

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "加法练习";
            this.examTitle = "加法测验";
            this.flowDocumentFile = "SoonLearning.Math.Arithmetic_Addition.AdditionFlowDocument.xaml";

            SectionValueRangeInfo info = new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                0,
                10);
            this.sectionInfoCollection.Add(info);

            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题：",
                "（找出算式中的加数，和）",
                5,
                0,
                10));

            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.Table,
                "表格题：",
                "（从表格中选择符合条件的算式）",
                5,
                0,
                10));
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
            if (section.QuestionCollection.Count == 0)
                this.questionValueList.Clear();

            int minValue = 10;
            int maxValue = 100;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            decimal valueA = rand.Next(minValue, maxValue);
            Thread.Sleep(10);
            decimal valueB = rand.Next(minValue, maxValue);

            bool exist = true;
            for (int i = 0; i < 50; i++)
            {
                if (Enumerable.Where<decimal>(this.questionValueList, (c => (c == valueA + valueB))).Count<decimal>() == 0)
                {
                    exist = false;
                    break;
                }

                valueA = rand.Next(minValue, maxValue);
                Thread.Sleep(10);
                valueB = rand.Next(minValue, maxValue);
            }

            if (exist)
                return;

            decimal result = valueA + valueB;
            this.questionValueList.Add(result);

            string questionText = string.Format("从下面选项中选出两个加数{0}，{1}的和。", valueA, valueB);

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
                            4, minValue, maxValue, false, (c => ((c == result))), result))
                    optionList.Add(option);

                return optionList;
            }
            );

            mcQuestion.Solution.Content = string.Format("加数{0}与加数{1}的和是{2}，所以正确答案是{2}。", valueA, valueB, result);

            section.QuestionCollection.Add(mcQuestion);
        }

        private void CreateFIBQuestion(SectionBaseInfo info, Section section)
        {
            if (section.QuestionCollection.Count == 0)
                this.questionValueList.Clear();

            int minValue = 10;
            int maxValue = 100;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            decimal valueA = rand.Next(minValue, maxValue);
            Thread.Sleep(10);
            decimal valueB = rand.Next(minValue, maxValue);

            bool exist = true;
            for (int i = 0; i < 50; i++)
            {
                if (Enumerable.Where<decimal>(this.questionValueList, (c => (c == valueA + valueB))).Count<decimal>() == 0)
                {
                    exist = false;
                    break;
                }

                valueA = rand.Next(minValue, maxValue);
                Thread.Sleep(10);
                valueB = rand.Next(minValue, maxValue);
            }

            if (exist)
                return;

            decimal result = valueA + valueB;
            this.questionValueList.Add(result);

            string questionText = string.Format("{0} + {1} = {2}。", valueA, valueB, result);

            FIBQuestion fibQuestion = new FIBQuestion();
            fibQuestion.Content.Content = questionText;
            fibQuestion.Content.ContentType = ContentType.Text;
            fibQuestion.ShowBlankInContent = true;

            fibQuestion.Content.Content += "加数";

            QuestionBlank blankA = new QuestionBlank();
            QuestionContent blankContentA = new QuestionContent();
            blankContentA.Content = valueA.ToString();
            blankContentA.ContentType = ContentType.Text;
            blankA.ReferenceAnswerList.Add(blankContentA);
            fibQuestion.QuestionBlankCollection.Add(blankA);
            fibQuestion.Content.Content += blankA.PlaceHolder;

            fibQuestion.Content.Content += "加数";

            QuestionBlank blankB = new QuestionBlank();
            QuestionContent blankContentB = new QuestionContent();
            blankContentB.Content = valueB.ToString();
            blankContentB.ContentType = ContentType.Text;
            blankB.ReferenceAnswerList.Add(blankContentB);
            fibQuestion.QuestionBlankCollection.Add(blankB);
            fibQuestion.Content.Content += blankB.PlaceHolder;

            fibQuestion.Content.Content += "和";

            QuestionBlank blankResult = new QuestionBlank();
            blankResult.MatchOwnRefAnswer = true;
            QuestionContent blankContentResult = new QuestionContent();
            blankContentResult.Content = result.ToString();
            blankContentResult.ContentType = ContentType.Text;
            blankResult.ReferenceAnswerList.Add(blankContentResult);
            fibQuestion.QuestionBlankCollection.Add(blankResult);
            fibQuestion.Content.Content += blankResult.PlaceHolder;

            fibQuestion.Solution.Content = string.Format("一个加数是{0}, 另一个加数是{1}， 和是{2}。", valueA, valueB, result);

            section.QuestionCollection.Add(fibQuestion);
        }

        private void CreateTableQuestion(SectionBaseInfo info, Section section)
        {
            if (section.QuestionCollection.Count == 0)
                this.questionValueList.Clear();

            int minValue = 10;
            int maxValue = 100;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            decimal result = rand.Next(minValue, maxValue);
            
            bool exist = true;
            for (int i = 0; i < 50; i++)
            {
                if (Enumerable.Where<decimal>(this.questionValueList, (c => (c == result))).Count<decimal>() == 0)
                {
                    exist = false;
                    break;
                }

                Thread.Sleep(10);
                result = rand.Next(minValue, maxValue);
            }

            if (exist)
                return;

            this.questionValueList.Add(result);

            string questionText = string.Format("从下面选项中选出两个加数的和是{0}", result);

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
                        decimal valueA = rand.Next(minValue, decimal.ToInt32(result) + 1);
                        decimal valueB = result - valueA;
                        QuestionOption option = new QuestionOption();
                        option.IsCorrect = true;
                        option.OptionContent.Content = string.Format("{0} + {1}", valueA, valueB);
                        optionList.Add(option);
                    }
                    else
                    {
                        decimal valueA = rand.Next(minValue, maxValue);
                        decimal valueB = rand.Next(minValue, maxValue);
                        QuestionOption option = new QuestionOption();
                        option.IsCorrect = (valueA + valueB == result) ? true : false;
                        option.OptionContent.Content = string.Format("{0} + {1}", valueA, valueB);
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
