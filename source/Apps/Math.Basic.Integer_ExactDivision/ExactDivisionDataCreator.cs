using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math.Integer_ExactDivision
{
    public class ExactDivisionDataCreator : DataCreator
    {
        private static ExactDivisionDataCreator creator;

        public static ExactDivisionDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new ExactDivisionDataCreator();

                return creator;
            }
        }

        private List<int> questionValueList = new List<int>();
        private int currentIndex = 2;
        
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "整除练习";
            this.examTitle = "整除测验";
            this.flowDocumentFile = "SoonLearning.Math.Integer_ExactDivision.ExactDivisionDocument.xaml";

            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                10,
                100));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.Table,
                "表格题：",
                "（从表格中选择符合条件的数）",
                8,
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
                case QuestionType.Table:
                    {
                        this.CreateTableQuestion(info, section);
                    }
                    break;
            }
        }

        private void CreateMCQuestion(SectionBaseInfo sectionInfo, Section secton)
        {
            Section sectionMC = ObjectCreator.CreateSection(sectionInfo.Name, sectionInfo.Description);

            Random rand = new Random((int)DateTime.Now.Ticks);

            int divValue = rand.Next(2, 13);
            string questionText = string.Format("请选出能被{0}整除的数。", divValue);

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
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
                            4, minValue, maxValue, false, (c => ((c % divValue) == 0))))
                    optionList.Add(option);

                return optionList;
            }
            );

            secton.QuestionCollection.Add(mcQuestion);

            StringBuilder strBuilder = new StringBuilder();
            foreach (QuestionOption option in mcQuestion.QuestionOptionCollection)
            {
                QuestionContent content = option.OptionContent;
                decimal value = System.Convert.ToDecimal(content.Content);
                if (value % divValue == 0)
                {
                    strBuilder.AppendLine(string.Format("{0}除以{1}等于{2}，没有余数，是正确答案。", value, divValue, value / divValue));
                }
                else
                {
                    strBuilder.AppendLine(string.Format("{0}除以{1}等于{2}，余数是{3}。", value, divValue, (int)(value) / divValue, value % divValue));
                }
            }
            mcQuestion.Solution.Content = strBuilder.ToString();
        }

        private void CreateTableQuestion(SectionBaseInfo sectionInfo, Section section)
        {
            // Table Question
            Random rand = new Random((int)DateTime.Now.Ticks);

            int divValue = rand.Next(2, 13);
            string questionText = string.Format("请下表中选出能被{0}整除的数。", divValue);

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

            section.QuestionCollection.Add(tableQuestion);
        }

        private string CreateSolution(int divValue)
        {
            if (divValue == 2)
            {
                return "个位数是0,2,4,6,8的数都可以被2整除。";
            }

            return string.Empty;
        }

        public ExactDivisionDataCreator()
        {

        }
    }
}
