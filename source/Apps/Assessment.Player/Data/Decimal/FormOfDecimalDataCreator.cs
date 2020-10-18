using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data;
using System.Threading;
using System.ComponentModel;

namespace SoonLearning.Assessment.Player.Data.Decimal
{
    class FormOfDecimalDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "小数的形式练习";
            this.examTitle = "小数的形式测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Player.Data.Decimal.FormOfDecimalDocument.xaml";

             this.sectionInfoCollection.Add(new SectionBaseInfo(QuestionType.FillInBlank,
                "填空题：",
                "（在空格中填入符合条件的数）",
                5));
             this.sectionInfoCollection.Add(new SectionBaseInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
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
                case QuestionType.FillInBlank:
                    {
                        this.CreateFIBQuestion(info, section);
                    }
                    break;
            }
        }

        private void CreateMCQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            int minValue = 0;
            int maxValue = 10000;
            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            List<int> questionValueList = new List<int>();
            Random rand = new Random((int)DateTime.Now.Ticks);
            int valueA = 0, valueB = 0;
            int minRand = 0, maxRand = 0;
            int minValueTmp = minValue, maxValueTmp = maxValue;

            while (true)
            {
                if (minValueTmp / 10 >= 1)
                {
                    minRand++;
                    minValueTmp /= 10;
                }
                else
                    break;
            }

            while (true)
            {
                if (maxValueTmp / 10 >= 1)
                {
                    maxRand++;
                    maxValueTmp /= 10;
                }
                else
                    break;
            }

            while (true)
            {
                valueA = rand.Next(minRand, maxRand + 1);
                valueB = rand.Next(minRand + 1, maxRand + 1);

                if (valueA != valueB)
                    break;
            }

            double valueP1 = System.Math.Pow(10, valueA);
            double valueP2 = System.Math.Pow(10, valueB);
            double valueD1 = 1 / valueP1, valueD2 = 1 / valueP2;

            questionValueList.Add(valueA);
            string questionText;

            //int valueC1, valueC2;
            int multipleD;

            if (valueD1 > valueD2)
            {
                //valueC1 = valueB;
                //valueC2 = valueA;
                multipleD = (int)(valueD1 / valueD2);
                questionText = string.Format("{0}是{1}的几倍。", valueD1, valueD2);
            }
            else
            {
                //valueC1 = valueA;
                //valueC2 = valueB;
                multipleD = (int)(valueD2 / valueD1);
                questionText = string.Format("{1}是{0}的几倍。", valueD1, valueD2);
            }

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                int valueLst = (int)(System.Math.Pow(10, minRand)), valueMst = (int)(System.Math.Pow(10, maxRand));
                if (minRand - 20 <= 0)
                {
                    valueLst = 0;
                }

                foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                            4, valueLst, valueMst, false, (c => (c == multipleD)), multipleD))
                    optionList.Add(option);

                return optionList;
            }
            );

            section.QuestionCollection.Add(mcQuestion);

            StringBuilder strBuilder = new StringBuilder();
            foreach (QuestionOption option in mcQuestion.QuestionOptionCollection)
            {
                QuestionContent content = option.OptionContent;
                decimal value = System.Convert.ToDecimal(content.Content);
                if (value == multipleD)
                {
                    strBuilder.AppendLine(string.Format("{0}和{1}是{2}倍的关系，是正确答案。", valueD1, valueD2, value));
                }
                else
                {
                    strBuilder.AppendLine(string.Format("{0}和{1}不是{2}倍的关系。", valueD1, valueD2, value));
                }
            }
            mcQuestion.Solution.Content = strBuilder.ToString();
        }

        private void CreateFIBQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            int minValue = 0;
            int maxValue = 1000;
            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            List<int> questionValueList = new List<int>();
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
        
        private string CreateTip()
        {
            return "（在小数里，每相邻两个计数单位之间的进率都是10。小数部分的最高分数单位“十分之一”和整数部分的最低单位“一”之间的进率也是10。）";
        }

        public FormOfDecimalDataCreator()
        {

        }
    }
}
