using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data;
using System.Threading;
using System.ComponentModel;

namespace SoonLearning.Assessment.Player.Data.Decimal
{
    class WithDecimalDataCreator : DataCreator
    {
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "带小数练习";
            this.examTitle = "带小数测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Player.Data.Decimal.WithDecimalDocument.xaml";
           //this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
           //     "填空题：",
           //     "（在空格中填入符合条件的数）",
           //     5,
           //     0,
           //     10));         
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                0,
                10));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.Table,
               "表格题：",
               "（从表格中选择符合条件的数）",
               5,
               0,
               10));
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

             int minValue = 0;
             int maxValue = 10 + 1;
             if (sectionInfo is SectionValueRangeInfo)
             {
                 SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                 minValue = decimal.ToInt32(rangeInfo.MinValue);
                 maxValue = decimal.ToInt32(rangeInfo.MaxValue);
             }

             string questionText = string.Format("选出是带小数的数。");

             MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
             {
                 content.Content = questionText;
                 content.ContentType = ContentType.Text;
                 return;
             },
             () =>
             {
                 List<QuestionOption> optionList = new List<QuestionOption>();

                 foreach (QuestionOption temp in ObjectCreator.CreateDoubleOption(0, 0, minValue, maxValue))
                     optionList.Add(temp);

                 foreach (QuestionOption temp in ObjectCreator.CreateIntegerOption(minValue, maxValue))
                     optionList.Add(temp);

                 foreach (QuestionOption temp in ObjectCreator.CreateDoubleOption(minValue + 1, maxValue + 1, minValue + 1, maxValue))
                 {
                     temp.IsCorrect = true;
                     optionList.Add(temp);
                 }

                 foreach (QuestionOption temp in ObjectCreator.CreateDoubleOption(0, 0, minValue + 1, maxValue))
                 {
                     optionList.Add(temp);
                 }

                 return optionList;
             }
             );

             mcQuestion.RandomOption = true;

             section.QuestionCollection.Add(mcQuestion);

             StringBuilder strBuilder = new StringBuilder();
             foreach (QuestionOption option in mcQuestion.QuestionOptionCollection)
             {
                 QuestionContent content = option.OptionContent;
                 if (content.QuestionPartCollection[0] is ArithmeticDecimalValuePart)
                 {
                     ArithmeticDecimalValuePart decimalPart = content.QuestionPartCollection[0] as ArithmeticDecimalValuePart;
                     decimal value = decimalPart.Value.Value;
                     if (value < 1 && value != 0)
                     {
                         strBuilder.AppendLine(string.Format("{0}是整数部分为零的小数，是纯小数。", value));
                     }
                     else if (value % 1 != 0)
                     {
                         strBuilder.AppendLine(string.Format("{0}整数部分不为零的小数，是带小数，是正确答案。", value));
                     }
                     else
                     {
                         strBuilder.AppendLine(string.Format("{0}是整数。", value));
                     }
                 }
             }

             mcQuestion.Solution.Content = strBuilder.ToString();
         }

         private void CreateTableQuestion(SectionBaseInfo sectionInfo, Section section)
         {
             // Table Question
             Random rand = new Random((int)DateTime.Now.Ticks);

             int minValue = 0;
             int maxValue = 10 + 1;
             if (sectionInfo is SectionValueRangeInfo)
             {
                 SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                 minValue = decimal.ToInt32(rangeInfo.MinValue);
                 maxValue = decimal.ToInt32(rangeInfo.MaxValue);
             }

             string questionText = string.Format("请下表中选出所有的带小数。");

             TableQuestion tableQuestion = ObjectCreator.CreateTableQuestion((content) =>
             {
                 content.Content = questionText;
                 content.ContentType = ContentType.Text;
                 return;
             },
             () =>
             {
                 List<QuestionOption> optionList = new List<QuestionOption>();

                 for (int k = 0; k < 36; k++)
                 {
                     int rand0 = rand.Next(0, 2);
                     if (rand0 != 0)
                     {
                         foreach (QuestionOption temp in ObjectCreator.CreateDoubleOption(0, 0, minValue + 1, maxValue))
                         {
                             optionList.Add(temp);
                         }
                     }
                     else
                     {
                         foreach (QuestionOption temp in ObjectCreator.CreateDoubleOption(minValue + 1, maxValue, minValue, maxValue))
                         {
                             temp.IsCorrect = true;
                             optionList.Add(temp);
                         }
                     }
                 }

                 return optionList;
             });

             tableQuestion.Solution.Content = this.CreateSolution(0);

             section.QuestionCollection.Add(tableQuestion);
         }

        private Section CreateFIBSection(SectionBaseInfo sectionInfo, BackgroundWorker worker)
        {
            Section fibSection = ObjectCreator.CreateSection(sectionInfo.Name, sectionInfo.Description);

            int minValue = 0;
            int maxValue = 10 + 1;
            if (sectionInfo is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = sectionInfo as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            List<int> questionValueList = new List<int>();
            Random rand = new Random((int)DateTime.Now.Ticks);

            int valueA = 0, valueB = 0;
 
            for (int i = 0; i < sectionInfo.QuestionCount; i++)
            {
                int[] value = new int[2];
                valueA = rand.Next(minValue, maxValue + 1);
                valueB = rand.Next(minValue + 1, maxValue + 1);
 
                string questionText = string.Format("写出小数{0}.{1}的整数部分和小数部分", valueA, valueB);

                FIBQuestion fibQuestion = new FIBQuestion();
                fibQuestion.Content.Content = questionText;
                fibQuestion.Content.ContentType = ContentType.Text;
                fibSection.QuestionCollection.Add(fibQuestion);

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

                worker.ReportProgress(0, fibQuestion);
            }

            return fibSection;
        }
        
        private string CreateSolution(int value)
        {
            if (value == 1)
            {
                return "";
            }

            return string.Empty;
        }

        public WithDecimalDataCreator()
        {

        }
    }
}
