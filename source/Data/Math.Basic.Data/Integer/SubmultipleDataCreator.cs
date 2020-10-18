using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;
using System.ComponentModel;
using System.Threading;

namespace SoonLearning.Assessment.Data
{
    internal class SubmultipleDataCreator : DataCreator
    {
        private List<int> questionValueList = new List<int>();

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "约数练习";
            this.examTitle = "约数测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Data.Integer.SubmultipleDocument.xaml";

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
            int tryCount = 100;
            while (true)
            {
                int minValue = 10;
                int maxValue = 100;
                int j = 2;
                if (sectionInfo is SectionValueRangeInfo)
                {
                    SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                    minValue = decimal.ToInt32(rangeInfo.MinValue);
                    maxValue = decimal.ToInt32(rangeInfo.MaxValue);
                }

                int divValue = rand.Next(minValue, maxValue);

                bool validValue = false;
                for (j = 3; j <= divValue / 2 + 1; j++)
                {
                    if (divValue % j == 0)
                    {
                        validValue = true;
                        break;
                    }
                }

                tryCount--;

                if (tryCount == 0)
                    break;

                if (!validValue)
                    continue;

                string questionText = string.Format("请选出自然数{0}的约数。", divValue);

                int inMinValue = 2, inMaxValue = 10;
                if (j / 2 > 2)
                    inMinValue = j / 2;
                if (j * 3 > 10)
                    inMaxValue = j * 3;

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
                                4, inMinValue, inMaxValue, false, (c => ((divValue % c) == 0))))
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
                    if (divValue % value == 0)
                    {
                        strBuilder.AppendLine(string.Format("{0}除以{1}等于{2}，没有余数，是正确答案。", divValue, value, divValue / value));
                    }
                    else
                    {
                        strBuilder.AppendLine(string.Format("{0}除以{1}等于{2}，余数是{3}。", divValue, value, divValue / (int)(value), divValue % value));
                    }
                }
                mcQuestion.Solution.Content = strBuilder.ToString();

                break;
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
                questionValueList.Clear();

            Random rand = new Random((int)DateTime.Now.Ticks);

            int value = 0;
            while (true)
            {
                value = rand.Next(minValue, maxValue);
                if (Enumerable.Where<int>(questionValueList, (c => (c == value))).Count<int>() > 0)
                {
                    Thread.Sleep(10);
                    continue;
                }
                break;
            }

            questionValueList.Add(value);

            string questionText = string.Format("写出自然数{0}的所有正约数。", value);

            FIBQuestion fibQuestion = new FIBQuestion();
            fibQuestion.Content.Content = questionText;
            fibQuestion.Content.ContentType = ContentType.Text;
            fibQuestion.ShowBlankInContent = false;
            section.QuestionCollection.Add(fibQuestion);

            for (int j = 1; j <= value; j++)
            {
                if (value % j == 0)
                {
                    QuestionBlank blank = new QuestionBlank();

                    QuestionContent blankContent = new QuestionContent();
                    blankContent.Content = j.ToString();
                    blankContent.ContentType = ContentType.Text;
                    blank.ReferenceAnswerList.Add(blankContent);

                    fibQuestion.QuestionBlankCollection.Add(blank);

                    fibQuestion.Content.Content += blank.PlaceHolder;
                }
            }
        }

        private string CreateSolution(int divValue)
        {
            return string.Empty;
         //   throw new NotImplementedException();
        }
    }
}
