using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.JCJC
{
    public class JCJCDataCreator : DataCreator
    {
        private static JCJCDataCreator creator;

        public static JCJCDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new JCJCDataCreator();

                return creator;
            }
        }

        private List<int> questionValueList = new List<int>();
        private int currentIndex = 2;

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "加超减凑法练习";
            this.examTitle = "加超减凑法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.JCJC.JCJC_Document.xaml";
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                50,
                500));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题：",
                "（在空格中填入符合条件的数）",
                5,
                50,
                500));
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

        private void GetRandomValues(SectionBaseInfo info, 
            ref int valueA, ref int valueB, ref int valueC, ref int valueM, ref int valueN, ref int sign)
        {
            int minValue = 50;
            int maxValue = 500;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            if (maxValue < 100)
            {
                maxValue = 100;
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            int valueTmp = 0;
           
            while (true)
            {
                valueA = rand.Next(minValue, maxValue + 1);
                valueTmp = rand.Next(100, maxValue + 1);
                valueM = valueTmp / 10;
                valueN = rand.Next(1, 9 + 1);

                if (valueN > 5)
                {
                    sign = 1; //负号
                }
                else
                {
                    sign = 0; //正号
                }

                if (sign == 0) //正号
                {
                    valueB = valueM * 10 + valueN;
                }
                else //负号
                {
                    valueB = valueM * 10 + valueN;
                    valueM += 1;
                    valueN = 10 - valueN;
                }

                valueC = valueA + valueB;

                if (valueB > 0)
                {
                    break;
                }
            }
        }

        private MCQuestion CreateMCQuestion(SectionBaseInfo info, Section section)
        {
            //int minValue = 50;
            //int maxValue = 500;
            //if (info is SectionValueRangeInfo)
            //{
            //    SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
            //    minValue = decimal.ToInt32(rangeInfo.MinValue);
            //    maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            //}

            //if (maxValue < 100)
            //{
            //    maxValue = 100;
            //}

            //if (section.QuestionCollection.Count == 0)
            //    this.questionValueList.Clear();
            
            //Random rand = new Random((int)DateTime.Now.Ticks);
            //int valueA = 0, valueB = 0, valueC = 0, valueM = 0, valueN = 0, valueTmp = 0;
            //int sign = 0;

            //int j = 0;
            
            //while (true)
            //{
            //    valueA = rand.Next(minValue, maxValue + 1);
            //    valueTmp = rand.Next(100, maxValue + 1);
            //    valueM = valueTmp / 100;
            //    valueN = rand.Next(1, 9 + 1);
            //    sign = rand.Next(0, 2);

            //    if (sign == 0) //正号
            //    {
            //        valueB = valueM * 100 + valueN;
            //    }
            //    else //负号
            //    {
            //        valueB = valueM * 100 - valueN;
            //    }

            //    valueC = valueA + valueB;

            //    if (valueB > 0)
            //    {
            //        break;
            //    }
            //}
            int valueA = 0, valueB = 0, valueC = 0, valueM = 0, valueN = 0, sign = 0;

            //this.GetRandomValues(info, &valueA, &valueB, &valueC, &valueM, &valueN, &sign);
            this.GetRandomValues(info, ref valueA, ref valueB, ref valueC, ref valueM, ref valueN, ref sign);

            questionValueList.Add((valueA));

            string questionText = string.Format("{0}+{1}=", valueA, valueB);

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                int valueLst = (valueC) - 30, valueMst = (valueC) + 30;
                if (valueLst - 30 <= 1)
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

                    if (sign == 0) //正号
                    {
                        strBuilder.AppendLine(string.Format(
                        "{0}+{1}={0}+({2}×10+{3})={0}+{2}×10+{3}= {0}+{4}+{3}={5}+{3}={6}，是正确答案。",
                        valueA, valueB, valueM, valueN, valueM * 10, valueA + valueM * 10, value));
                    }   
                    else //负号
                    {
                        strBuilder.AppendLine(string.Format(
                        "{0}+{1}={0}+({2}×10-{3})={0}+{2}×10-{3}= {0}+{4}-{3}={5}-{3}={6}，是正确答案。",
                        valueA, valueB, valueM, valueN, valueM * 10, valueA + valueM * 10, value));
                    }                    
                }
                else
                {
                    strBuilder.AppendLine(string.Format("{0}+{1}不等于{2}。", valueA, valueB, value));
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
            string questionText = string.Format("请下表中选出能被{0}加超减凑法的数。", divValue);

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
            int valueA = 0, valueB = 0, valueC = 0, valueM = 0, valueN = 0, sign = 0;

            this.GetRandomValues(info, ref valueA, ref valueB, ref valueC, ref valueM, ref valueN, ref sign);

            string questionText = string.Format("{0}+{1}=", valueA, valueB);

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
            
            StringBuilder strBuilder = new StringBuilder();
          
            if (sign == 0) //正号
            {
                strBuilder.AppendLine(string.Format(
                "{0}+{1}={0}+({2}×10+{3})={0}+{2}×10+{3}= {0}+{4}+{3}={5}+{3}={6}。",
                valueA, valueB, valueM, valueN, valueM * 10, valueA + valueM * 10, valueC));
            }
            else //负号
            {
                strBuilder.AppendLine(string.Format(
                "{0}+{1}={0}+({2}×10-{3})={0}+{2}×10-{3}= {0}+{4}-{3}={5}-{3}={6}。",
                valueA, valueB, valueM, valueN, valueM * 10, valueA + valueM * 10, valueC));
            }
               
            fibQuestion.Solution.Content = strBuilder.ToString();
         }

        private string CreateSolution(int divValue)
        {
            if (divValue == 2)
            {
                return "";
            }

            return string.Empty;
        }

        public JCJCDataCreator()
        {

        }
    }
}
