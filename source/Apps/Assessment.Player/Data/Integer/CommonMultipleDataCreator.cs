using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data;
using System.Threading;
using System.ComponentModel;

namespace SoonLearning.Assessment.Player.Data
{
    internal class CommonMultipleDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "公倍数练习";
            this.examTitle = "公倍数测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Player.Data.Integer.CommonMultipleDocument.xaml";
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
                case QuestionType.FillInBlank:
                    this.CreeateFIBQuestion(info, section);
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
            int valueA = 0, valueB = 0, valueC = 0;

            int value;

            while (true)
            {
                int j2 = 0;
                valueA = rand.Next(minValue, maxValue);
                valueB = rand.Next(minValue, maxValue);

                if (valueA > valueB)
                    value = valueA;
                else if (valueA < valueB)
                    value = valueB;
                else
                    continue;

                j2 = value;
                while (true)
                {
                    if (j2 % valueA == 0 && j2 % valueB == 0)
                    {
                        valueC = j2;
                        break;
                    }
                    j2++;
                }
                break;
            }

            string questionText = string.Format("请选{0}和{1}的最小公倍数。", valueA, valueB);

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
                            4, valueC / 2, valueC * 2, false,
                            (c => (c == valueC)), valueC))
                    optionList.Add(option);

                return optionList;
            }
            );

            section.QuestionCollection.Add(mcQuestion);

            StringBuilder strBuilder = new StringBuilder();
            foreach (QuestionOption option in mcQuestion.QuestionOptionCollection)
            {
                QuestionContent content = option.OptionContent;
                decimal valueO = System.Convert.ToDecimal(content.Content);
                if (valueC == valueO)
                {
                    strBuilder.AppendLine(string.Format("{0}能同时被{1}和{2}整除，且是所有能被整除的数里面最小的数，是正确答案。", valueO, valueA, valueB));
                }
                else
                {
                    if (valueO % valueA == 0 && valueO % valueB == 0)
                        strBuilder.AppendLine(string.Format("{0}能同时被{1}和{2}整除，但不是所有能被整除的数里面最小的数。", valueO, valueA, valueB));
                    else
                        strBuilder.AppendLine(string.Format("{0}不能同时被{1}和{2}整除。", valueO, valueA, valueB));
                }
            }
            mcQuestion.Solution.Content = strBuilder.ToString();
        }

        private Section CreateTableSection(SectionBaseInfo sectionInfo, BackgroundWorker worker)
        {
            // Table Question
            Section sectionTable = ObjectCreator.CreateSection(sectionInfo.Name, sectionInfo.Description);

            Random rand = new Random((int)DateTime.Now.Ticks);

            for (int i = 2; i < sectionInfo.QuestionCount + 2; i++)
            {
                int divValue = i;
                string questionText = string.Format("请下表中选出能被{0}公倍数的数。", divValue);

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
                    if (sectionInfo is SectionValueRangeInfo)
                    {
                        SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                        minValue = decimal.ToInt32(rangeInfo.MinValue);
                        maxValue = decimal.ToInt32(rangeInfo.MaxValue);
                    }
                    foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                                36, minValue, maxValue, true, (c => ((c % divValue) == 0))))
                        optionList.Add(option);

                    return optionList;
                });

                tableQuestion.Solution.Content = this.CreateSolution(divValue);

                sectionTable.QuestionCollection.Add(tableQuestion);

                worker.ReportProgress(0, tableQuestion);
            }

            return sectionTable;
        }

        private void CreeateFIBQuestion(SectionBaseInfo info, Section section)
        {
            int minValue = 10;
            int maxValue = 100;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            List<int> questionValueList = new List<int>();
            Random rand = new Random((int)DateTime.Now.Ticks);

            int valueA = 0, valueB = 0, valueC = 0;

            int value = 0;

            while (true)
            {
                int j2 = 0;
                valueA = rand.Next(minValue, maxValue);
                valueB = rand.Next(minValue, maxValue);

                if (valueA > valueB)
                    value = valueA;
                else if (valueA < valueB)
                    value = valueB;
                else
                    continue;

                j2 = value;
                while (true)
                {
                    if (j2 % valueA == 0 && j2 % valueB == 0)
                    {
                        valueC = j2;
                        break;
                    }
                    j2++;
                }
                break;
            }

            string questionText = string.Format("写出{0}和{1}的最小公倍数", valueA, valueB);

            FIBQuestion fibQuestion = new FIBQuestion();
            fibQuestion.Content.Content = questionText;
            fibQuestion.Content.ContentType = ContentType.Text;
            section.QuestionCollection.Add(fibQuestion);

            QuestionBlank blank = new QuestionBlank();

            QuestionContent blankContent = new QuestionContent();
            blankContent.Content = valueC.ToString();
            blankContent.ContentType = ContentType.Text;
            blank.ReferenceAnswerList.Add(blankContent);

            fibQuestion.QuestionBlankCollection.Add(blank);

            fibQuestion.Content.Content += blank.PlaceHolder;
        }

        private string CreateSolution(int divValue)
        {
            if (divValue == 2)
            {
                return "";
            }

            return string.Empty;
        }

        public CommonMultipleDataCreator()
        {

        }
    }
}

