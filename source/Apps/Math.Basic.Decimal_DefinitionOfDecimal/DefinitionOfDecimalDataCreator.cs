using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math.Decimal_DefinitionOfDecimal
{
    public class DefinitionOfDecimalDataCreator : DataCreator
    {
        private static DefinitionOfDecimalDataCreator creator;

        public static DefinitionOfDecimalDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new DefinitionOfDecimalDataCreator();

                return creator;
            }
        }

        private List<int> questionValueList = new List<int>();

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "小数的意义练习";
            this.examTitle = "小数的意义测验";
            this.flowDocumentFile = "SoonLearning.Math.Decimal_DefinitionOfDecimal.DefinitionOfDecimalDocument.xaml";

            this.sectionInfoCollection.Add(new SectionBaseInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5));
            //this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
            //    "填空题：",
            //    "（在空格中填入符合条件的数）",
            //    10,
            //    10,
            //    100));
        }

        protected override void AppendQuestion(SectionBaseInfo info, Section section)
        {
            switch (info.QuestionType)
            {
                case QuestionType.MultiChoice:
                    this.CreateMCQuestion(info, section);
                    break;
            }
        }

        private void CreateMCQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            int minValue = 10;
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

            valueB = rand.Next(minRand, maxRand + 1);
            valueA = (int)(System.Math.Pow(10, valueB));

            questionValueList.Add(valueA);

            string questionText = string.Format("把1分成{0}份，请问可以用几位小数表示。", valueA);

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                int valueLst = valueB - 2, valueMst = valueB + 3;
                if (minRand - 20 <= 1)
                {
                    valueLst = 1;
                }

                foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                            4, valueLst, valueMst, false, (c => (c == valueB)), valueB))
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
                if (value == valueB)
                {
                    strBuilder.AppendLine(string.Format("把1分成{0}份，可以用{1}位小数表示，是正确答案。", valueA, value));
                }
                else
                {
                    strBuilder.AppendLine(string.Format("把1分成{0}份，不可以用{1}位小数表示。", valueA, value));
                }
            }
            mcQuestion.Solution.Content = strBuilder.ToString();
        }

        public DefinitionOfDecimalDataCreator()
        {

        }
    }
}
