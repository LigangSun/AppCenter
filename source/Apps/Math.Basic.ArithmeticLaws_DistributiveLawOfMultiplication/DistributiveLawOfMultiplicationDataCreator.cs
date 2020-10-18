using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math.ArithmeticLaws_DistributiveLawOfMultiplication
{
    public class DistributiveLawOfMultiplicationDataCreator : DataCreator
    {
        private static DistributiveLawOfMultiplicationDataCreator creator;

        public static DistributiveLawOfMultiplicationDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new DistributiveLawOfMultiplicationDataCreator();

                return creator;
            }
        }

        //private List<int> questionValueList = new List<int>();

        private List<decimal> questionValueList = new List<decimal>();

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "乘法分配率练习";
            this.examTitle = "乘法分配率测验";
            this.flowDocumentFile = "SoonLearning.Math.ArithmeticLaws_DistributiveLawOfMultiplication.DistributiveLawOfMultiplicationFlowDocument.xaml";

            SectionValueRangeInfo info = new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                0,
                10);
            this.sectionInfoCollection.Add(info);

            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题：",
                "（按照乘法分配率填空）",
                5,
                0,
                10));

            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.Table,
                "表格题：",
                "（从表格中选择符合乘法分配率条件的等式）",
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
            int minValue = 10;
            int maxValue = 100;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            // 因为因数为0比较特殊，所以暂时先排除0
            if (minValue < 1)
                minValue++;
            decimal valueA = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueB = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueC = rand.Next(minValue, maxValue + 1);

            decimal result = (valueA + valueB) * valueC;

            string questionText = "从下面选项中选出符合乘法分配率的等式。";

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                //for (int i = 0; i < 3; i++)
                //{
                //    decimal a = rand.Next(minValue, maxValue + 1);
                //    decimal b = rand.Next(minValue, maxValue + 1);
                //    decimal c = rand.Next(minValue, maxValue + 1);
                //    decimal d = rand.Next(minValue, maxValue + 1);
                //    if (d == c)
                //    {
                //        d = d == 0 ? d + 1 : d - 1;
                //    }

                //    QuestionOption option = new QuestionOption();
                //    option.OptionContent.Content = string.Format("({0} × {1}) × {2} = {0} × ({1} × {3})", a, b, c, d);
                //    optionList.Add(option);
                //}

                QuestionOption option1 = new QuestionOption();
                option1.OptionContent.Content = string.Format("({0} + {1}) × {2} = {0} + {1} × {2}", valueA, valueB, valueC);
                optionList.Add(option1);
                
                QuestionOption option2 = new QuestionOption();
                option2.OptionContent.Content = string.Format("({0} + {1}) × {2} = {0} × {2} + {1}", valueA, valueB, valueC);
                optionList.Add(option2);

                QuestionOption option3 = new QuestionOption();
                option3.OptionContent.Content = string.Format("({0} + {1}) × {2} = ({0} + {2}) × ({1} + {2})", valueA, valueB, valueC);
                optionList.Add(option3);

                QuestionOption correctOption = new QuestionOption();
                correctOption.OptionContent.Content = string.Format("({0} + {1}) × {2} = {0} × {2} + {1} × {2}", valueA, valueB, valueC);
                correctOption.IsCorrect = true;
                int correctIndex = rand.Next(100) % 4;
                if (correctIndex == optionList.Count)
                    optionList.Add(correctOption);
                else
                    optionList.Insert(correctIndex, correctOption);

                return optionList;
            }
            );

            mcQuestion.Solution.Content = string.Format("({0} + {1}) × {2} = {0} × {2} + {1} × {2}，在这个等式中，两边结果不变，都是{3}。\n是符合乘法分配率的，是正确答案。", valueA, valueB, valueC, result);

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

            if (minValue < 1)
                minValue = 1;

            decimal valueA = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueB = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueC = rand.Next(minValue, maxValue + 1);

            decimal result = (valueA + valueB) * valueC;
            this.questionValueList.Add(result);

            string questionText = string.Format("({0} + {1}) × {2} = ", valueA, valueB, valueC);

            FIBQuestion fibQuestion = new FIBQuestion();
            fibQuestion.Content.Content = questionText;
            fibQuestion.Content.ContentType = ContentType.Text;
            fibQuestion.ShowBlankInContent = true;

            QuestionBlank blankA = new QuestionBlank();
            blankA.MatchOwnRefAnswer = true;
            QuestionContent blankContentA = new QuestionContent();
            blankContentA.Content = valueA.ToString();
            blankContentA.ContentType = ContentType.Text;
            blankA.ReferenceAnswerList.Add(blankContentA);
            fibQuestion.QuestionBlankCollection.Add(blankA);
            fibQuestion.Content.Content += blankA.PlaceHolder;

            fibQuestion.Content.Content += " × ";

            QuestionBlank blankB = new QuestionBlank();
            blankB.MatchOwnRefAnswer = true;
            QuestionContent blankContentB = new QuestionContent();
            blankContentB.Content = valueC.ToString();
            blankContentB.ContentType = ContentType.Text;
            blankB.ReferenceAnswerList.Add(blankContentB);
            fibQuestion.QuestionBlankCollection.Add(blankB);
            fibQuestion.Content.Content += blankB.PlaceHolder;

            fibQuestion.Content.Content += " + ";

            QuestionBlank blankC = new QuestionBlank();
            blankC.MatchOwnRefAnswer = true;
            QuestionContent blankContentC = new QuestionContent();
            blankContentC.Content = valueB.ToString();
            blankContentC.ContentType = ContentType.Text;
            blankC.ReferenceAnswerList.Add(blankContentC);
            fibQuestion.QuestionBlankCollection.Add(blankC);
            fibQuestion.Content.Content += blankC.PlaceHolder;

            fibQuestion.Content.Content += " × ";

            QuestionBlank blankD = new QuestionBlank();
            blankD.MatchOwnRefAnswer = true;
            QuestionContent blankContentD = new QuestionContent();
            blankContentD.Content = valueC.ToString();
            blankContentD.ContentType = ContentType.Text;
            blankD.ReferenceAnswerList.Add(blankContentD);
            fibQuestion.QuestionBlankCollection.Add(blankD);
            fibQuestion.Content.Content += blankD.PlaceHolder;

            fibQuestion.Solution.Content = string.Format("因数{0}乘以{2}，然后加上因数{1}乘以{2}的结果，最终结果{3}。", valueA, valueB, valueC, result);

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
                if (minValue < 1)
                    minValue = 1;
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            string questionText = "从表格中选择符合乘法分配率条件的等式";

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
                        decimal valueC = rand.Next(minValue, maxValue + 1);
                        QuestionOption option = new QuestionOption();
                        option.IsCorrect = true;
                        option.OptionContent.Content = string.Format("({0}+{1})×{2}={0}×{2}+{1}×{2}", valueA, valueB, valueC);
                        optionList.Add(option);
                    }
                    else
                    {
                        decimal valueA = rand.Next(minValue, maxValue + 1);
                        decimal valueB = rand.Next(minValue, maxValue + 1);
                        decimal valueC = rand.Next(minValue, maxValue + 1);
                        decimal valueD = rand.Next(minValue, maxValue + 1);
                        QuestionOption option = new QuestionOption();
                        option.IsCorrect = (valueC == valueD) ? true : false;
                        option.OptionContent.Content = string.Format("({0}+{1})×{2}={0}×{2}+{1}×{3}", valueA, valueB, valueC, valueD);
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
