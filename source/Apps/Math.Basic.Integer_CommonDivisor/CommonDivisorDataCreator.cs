using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math.Integer_CommonDivisor
{
    public class CommonDivisorDataCreator : DataCreator
    {
        private static CommonDivisorDataCreator creator;

        public static CommonDivisorDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new CommonDivisorDataCreator();

                return creator;
            }
        }

        private List<int> questionValueList = new List<int>();
        private int currentIndex = 2;

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "公约数练习";
            this.examTitle = "公约数测验";
            this.flowDocumentFile = "SoonLearning.Math.Integer_CommonDivisor.CommonDivisorDocument.xaml";
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
                    this.CreateMCQuestion(info, section);
                    break;
                case QuestionType.Table:
                    this.CreateTableQuestion(info, section);
                    break;
                case QuestionType.FillInBlank:
                    this.CreateFIBQuestion(info, section);
                    break;
            }
        }

        private MCQuestion CreateMCQuestion(SectionBaseInfo info, Section section)
        {
            int minValue = 10;
            int maxValue = 100;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            if (section.QuestionCollection.Count == 0)
                this.questionValueList.Clear();
            
            Random rand = new Random((int)DateTime.Now.Ticks);
            int valueA = 0, valueB = 0, valueC = 0;

            int j = 0;
            int flag = 0;

            valueC = rand.Next(minValue / 3, maxValue / 3);
            while (true)
            {
                flag = 0;
                //for (int i2 = 0; i2 < maxValue + minValue; i2++)
                while (true)
                {
                    valueA = rand.Next(valueC, maxValue);
                    valueB = rand.Next(valueC, maxValue);

                    if (valueA % valueC == 0 && valueC >= valueA / valueC &&
                        valueB % valueC == 0 && valueC >= valueB / valueC &&
                        valueA != valueB && valueC != valueA && valueC != valueB &&
                        ((valueA > valueB && valueA % valueB != 0) || (valueB > valueA && valueB % valueA != 0)))
                    {
                        flag = 1;
                        break;
                    }

                    //int lop = 0;
                    //if (valueA > valueB)
                    //    lop = valueB;
                    //else
                    //    lop = valueA;

                    //for (j = lop; j > 1; j--)
                    //{
                    //    if (valueA % j == 0 && valueB % j == 0)
                    //    {
                    //        flag = 1;
                    //        break;
                    //    }
                    //}
                }

                //if (Enumerable.Where<int>(questionValueList, (c => (c == valueA))).Count<int>() > 0)
                //{
                //    Thread.Sleep(10);
                //    continue;
                //}
                break;
            }

            questionValueList.Add(valueA);

            string questionText = string.Format("请找出{0}和{1}的最大公约数。", valueA, valueB);

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                int valueLst = valueC - 20, valueMst = valueC + 20;
                if (j - 20 <= 1)
                {
                    valueLst = 1;
                }

                foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                            4, valueLst, valueMst, false, (c => (c == valueC)), valueC))
                    optionList.Add(option);

                return optionList;
            }
            );

            StringBuilder strBuilder = new StringBuilder();
            foreach (QuestionOption option in mcQuestion.QuestionOptionCollection)
            {
                QuestionContent content = option.OptionContent;
                decimal value = System.Convert.ToDecimal(content.Content);
                if (value == valueC)
                {
                    strBuilder.AppendLine(string.Format(
                        "{0}除以{1}等于{2}，没有余数，并且{3}除以{4}等于{5}，没有余数，{1}是最大公约数，是正确答案。",
                        valueA, value, valueA / value, valueB, value, valueB / value));
                }
                else
                {
                    strBuilder.AppendLine(string.Format("{0}不是{1}和{2}的最大公约数。", value, valueA, valueB));
                }
            }
            mcQuestion.Solution.Content = strBuilder.ToString();

            section.QuestionCollection.Add(mcQuestion);

            return mcQuestion;
        }

        private void CreateTableQuestion(SectionBaseInfo info, Section section)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            if (section.QuestionCollection.Count == 0)
                this.currentIndex = 2;

            int divValue = this.currentIndex;
            string questionText = string.Format("请下表中选出能被{0}公约数的数。", divValue);

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
                if (info is SectionValueRangeInfo)
                {
                    SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                    minValue = decimal.ToInt32(rangeInfo.MinValue);
                    maxValue = decimal.ToInt32(rangeInfo.MaxValue);
                }
                foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                            36, minValue, maxValue, true, (c => ((c % divValue) == 0))))
                            optionList.Add(option);

                return optionList;
            });

            this.currentIndex++;

            tableQuestion.Solution.Content = this.CreateSolution(divValue);

            section.QuestionCollection.Add(tableQuestion);
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

            if (section.QuestionCollection.Count == 0)
                questionValueList.Clear();

            Random rand = new Random((int)DateTime.Now.Ticks);

            int valueA = 0, valueB = 0;

            int value = 0;

            while (true)
            {
                int sumCommonDivisor = 0;
                valueA = rand.Next(minValue, maxValue);
                valueB = rand.Next(minValue, maxValue);

                if (valueA < valueB)
                    value = valueA;
                else if (valueA > valueB)
                    value = valueB;
                else
                    continue;

                for (int j2 = 1; j2 < value; j2++)
                {
                    if (valueA % j2 == 0 && valueB % j2 == 0)
                    {
                        sumCommonDivisor++;
                    }
                }

                if (sumCommonDivisor > 1)
                    break;
            }

            string questionText = string.Format("写出{0}和{1}的所有公约数", valueA, valueB);

            FIBQuestion fibQuestion = new FIBQuestion();
            fibQuestion.Content.Content = questionText;
            fibQuestion.Content.ContentType = ContentType.Text;
            section.QuestionCollection.Add(fibQuestion);

            if (valueA < valueB)
                value = valueA;
            else
                value = valueB;

            for (int j2 = 1; j2 <= value; j2++)
            {
                if (valueA % j2 == 0 && valueB % j2 == 0)
                {
                    QuestionBlank blank = new QuestionBlank();

                    QuestionContent blankContent = new QuestionContent();
                    blankContent.Content = j2.ToString();
                    blankContent.ContentType = ContentType.Text;
                    blank.ReferenceAnswerList.Add(blankContent);

                    fibQuestion.QuestionBlankCollection.Add(blank);

                    fibQuestion.Content.Content += blank.PlaceHolder;
                }
            }
        }

        private string CreateSolution(int divValue)
        {
            if (divValue == 2)
            {
                return "";
            }

            return string.Empty;
        }

        public CommonDivisorDataCreator()
        {

        }
    }
}
