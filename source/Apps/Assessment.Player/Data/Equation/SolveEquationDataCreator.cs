using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data;
using System.Threading;

namespace SoonLearning.Assessment.Player.Data.Equation
{
    internal class SolveEquationDataCreator : DataCreator
    {
        private List<decimal> questionValueList = new List<decimal>();

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "解方程练习";
            this.examTitle = "解方程测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Player.Data.Equation.SolveEquationFlowDocument.xaml";

            //SectionValueRangeInfo info = new SectionValueRangeInfo(QuestionType.MultiChoice,
            //    "单选题：",
            //    "（下面每道题都只有一个选项是正确的）",
            //    5,
            //    1,
            //    10);
            //this.sectionInfoCollection.Add(info);

            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题：",
                "（按照解方程填空）",
                5,
                1,
                50));

            //this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.Table,
            //    "表格题：",
            //    "（从表格中选择符合解方程条件的等式）",
            //    5,
            //    0,
            //    10));
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

            decimal valueA = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueB = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueC = rand.Next(minValue, maxValue + 1);

            decimal result = valueA + valueB;

            string questionText = string.Format("从下面选项中选出方程x + {0} = {1} 的解。", valueB, result);

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
                decimal d = rand.Next(minValue, maxValue + 1);
                if (d == valueA)
                {
                    d = d == 0 ? d + 1 : d - 1;
                }
                QuestionOption option = new QuestionOption();
                option.OptionContent.Content = string.Format("{0}", d);
                optionList.Add(option);
                //}

                //int tmpminValue, tmpmaxValue;
                //if (d <= 2)
                //{
                //    tmpminValue = decimal.ToInt32(d) + 1;
                //    tmpmaxValue = maxValue;
                //}
                //else
                //{
                //    tmpminValue = decimal.ToInt32(d) + 1;
                //    tmpmaxValue = maxValue;
                //}
                decimal d1;
                while (true)
                {
                    d1 = rand.Next(minValue, maxValue + 1);
                    if (d1 != valueA && d1 != d)
                    {
                        break;
                    }
                }
                QuestionOption option1 = new QuestionOption();
                option1.OptionContent.Content = string.Format("{0}", d1);
                optionList.Add(option1);

                decimal d2;
                while (true)
                {
                    d2 = rand.Next(minValue, maxValue + 1);
                    if (d2 != valueA && d2 != d && d2 != d1)
                    {
                        break;
                    }
                }
                QuestionOption option2 = new QuestionOption();
                option2.OptionContent.Content = string.Format("{0}", d2);
                optionList.Add(option2);

                QuestionOption correctOption = new QuestionOption();
                correctOption.OptionContent.Content = string.Format("{0}", valueA);
                correctOption.IsCorrect = true;
                int correctIndex = rand.Next(100) % 4;
                if (correctIndex == optionList.Count)
                    optionList.Add(correctOption);
                else
                    optionList.Insert(correctIndex, correctOption);

                return optionList;
            }
            );

            mcQuestion.Solution.Content = string.Format("当x={2}，方程x + {0} = {1}等式左右两边相等，因此{2}是解方程，是正确答案。", valueB, result, valueA);

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

            int tmpValue = maxValue / 10;

            decimal valueA = rand.Next(minValue, maxValue / 5 + 1);
            Thread.Sleep(10);
            decimal valueB = rand.Next(minValue, maxValue / 5 + 1);
            Thread.Sleep(10);
            decimal valueC = rand.Next(minValue, maxValue + 1);

            decimal coeff = rand.Next(2, tmpValue + 1);
            decimal symbol = rand.Next(0, 2);
        //    if (symbol == 0) // "+"
            {
                decimal result = coeff*valueA + valueB;
                this.questionValueList.Add(result);

                string questionText = string.Format("{0}×x+{1}={2}；\n第一步, ", coeff, valueB, result);

                //第一步，移项 {0}×x = {2} - {1}
                FIBQuestion fibQuestion = new FIBQuestion();
                fibQuestion.Content.Content = questionText;
                fibQuestion.Content.ContentType = ContentType.Text;
                fibQuestion.ShowBlankInContent = true;

                QuestionBlank blankA = new QuestionBlank();
                blankA.MatchOwnRefAnswer = true;
                QuestionContent blankContentA = new QuestionContent();
                blankContentA.Content = coeff.ToString();
                blankContentA.ContentType = ContentType.Text;
                blankA.ReferenceAnswerList.Add(blankContentA);
                fibQuestion.QuestionBlankCollection.Add(blankA);
                fibQuestion.Content.Content += blankA.PlaceHolder;

                fibQuestion.Content.Content += " ×x= ";

                QuestionBlank blankB = new QuestionBlank();
                blankB.MatchOwnRefAnswer = true;
                QuestionContent blankContentB = new QuestionContent();
                blankContentB.Content = result.ToString();
                blankContentB.ContentType = ContentType.Text;
                blankB.ReferenceAnswerList.Add(blankContentB);
                fibQuestion.QuestionBlankCollection.Add(blankB);
                fibQuestion.Content.Content += blankB.PlaceHolder;

                fibQuestion.Content.Content += " - ";

                QuestionBlank blankC = new QuestionBlank();
                blankC.MatchOwnRefAnswer = true;
                QuestionContent blankContentC = new QuestionContent();
                blankContentC.Content = valueB.ToString();
                blankContentC.ContentType = ContentType.Text;
                blankC.ReferenceAnswerList.Add(blankContentC);
                fibQuestion.QuestionBlankCollection.Add(blankC);
                fibQuestion.Content.Content += blankC.PlaceHolder;

                //第二步{0}×x = {3}
                fibQuestion.Content.Content += " \n第二步, ";
                QuestionBlank blankA1 = new QuestionBlank();
                blankA1.MatchOwnRefAnswer = true;
                QuestionContent blankContentA1 = new QuestionContent();
                blankContentA1.Content = coeff.ToString();
                blankContentA1.ContentType = ContentType.Text;
                blankA1.ReferenceAnswerList.Add(blankContentA1);
                fibQuestion.QuestionBlankCollection.Add(blankA1);
                fibQuestion.Content.Content += blankA1.PlaceHolder;

                fibQuestion.Content.Content += " ×x= ";

                decimal valueB1 = result - valueB;
                QuestionBlank blankB1 = new QuestionBlank();
                blankB1.MatchOwnRefAnswer = true;
                QuestionContent blankContentB1 = new QuestionContent();
                blankContentB1.Content = valueB1.ToString();
                blankContentB1.ContentType = ContentType.Text;
                blankB1.ReferenceAnswerList.Add(blankContentB1);
                fibQuestion.QuestionBlankCollection.Add(blankB1);
                fibQuestion.Content.Content += blankB1.PlaceHolder;

                //第三步，两边除以系数{0}，{0}÷{0}×x = {3}÷{0}
                fibQuestion.Content.Content += " \n第三步, ";
                QuestionBlank blankA2 = new QuestionBlank();
                blankA2.MatchOwnRefAnswer = true;
                QuestionContent blankContentA2 = new QuestionContent();
                blankContentA2.Content = coeff.ToString();
                blankContentA2.ContentType = ContentType.Text;
                blankA2.ReferenceAnswerList.Add(blankContentA2);
                fibQuestion.QuestionBlankCollection.Add(blankA2);
                fibQuestion.Content.Content += blankA2.PlaceHolder;

                fibQuestion.Content.Content += " ÷ ";

                QuestionBlank blankB2 = new QuestionBlank();
                blankB2.MatchOwnRefAnswer = true;
                QuestionContent blankContentB2 = new QuestionContent();
                blankContentB2.Content = coeff.ToString();
                blankContentB2.ContentType = ContentType.Text;
                blankB2.ReferenceAnswerList.Add(blankContentB2);
                fibQuestion.QuestionBlankCollection.Add(blankB2);
                fibQuestion.Content.Content += blankB2.PlaceHolder;

                fibQuestion.Content.Content += " ×x= ";

                QuestionBlank blankC2 = new QuestionBlank();
                blankC2.MatchOwnRefAnswer = true;
                QuestionContent blankContentC2 = new QuestionContent();
                blankContentC2.Content = valueB1.ToString();
                blankContentC2.ContentType = ContentType.Text;
                blankC2.ReferenceAnswerList.Add(blankContentC2);
                fibQuestion.QuestionBlankCollection.Add(blankC2);
                fibQuestion.Content.Content += blankC2.PlaceHolder;

                fibQuestion.Content.Content += " ÷ ";

                QuestionBlank blankD2 = new QuestionBlank();
                blankD2.MatchOwnRefAnswer = true;
                QuestionContent blankContentD2 = new QuestionContent();
                blankContentD2.Content = coeff.ToString();
                blankContentD2.ContentType = ContentType.Text;
                blankD2.ReferenceAnswerList.Add(blankContentD2);
                fibQuestion.QuestionBlankCollection.Add(blankD2);
                fibQuestion.Content.Content += blankD2.PlaceHolder;

                //第四步，x={4}
                fibQuestion.Content.Content += " \n第四步, ";
                fibQuestion.Content.Content += " x= ";

                decimal valueA3 = valueB1 / coeff;
                QuestionBlank blankA3 = new QuestionBlank();
                blankA3.MatchOwnRefAnswer = true;
                QuestionContent blankContentA3 = new QuestionContent();
                blankContentA3.Content = valueA3.ToString();
                blankContentA3.ContentType = ContentType.Text;
                blankA3.ReferenceAnswerList.Add(blankContentA3);
                fibQuestion.QuestionBlankCollection.Add(blankA3);
                fibQuestion.Content.Content += blankA3.PlaceHolder;
                //∴x={4}是方程的解
                fibQuestion.Content.Content += string.Format(" \n∴ x=");
                QuestionBlank blankA4 = new QuestionBlank();
                blankA4.MatchOwnRefAnswer = true;
                QuestionContent blankContentA4 = new QuestionContent();
                blankContentA4.Content = valueA3.ToString();
                blankContentA4.ContentType = ContentType.Text;
                blankA4.ReferenceAnswerList.Add(blankContentA4);
                fibQuestion.QuestionBlankCollection.Add(blankA4);
                fibQuestion.Content.Content += blankA4.PlaceHolder;
                fibQuestion.Content.Content += "是方程的解。";
               
                fibQuestion.Solution.Content = string.Format(
                    "第一步，移项 {0}×x={2}-{1}；第二步{0}×x={3}；第三步，两边除以系数{0}，{0}÷{0}×x={3}÷{0}；第四步，x={4}；答案，∴x={4}是方程的解。",
                    coeff, valueB, result, result-valueB, (result-valueB)/coeff);
              
                section.QuestionCollection.Add(fibQuestion);
            }
         //   else // "-"
            {
            }
        }

        private void CreateTableQuestion(SectionBaseInfo info, Section section)
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

            string questionText = "从表格中选择符合解方程条件的等式";

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
                        QuestionOption option = new QuestionOption();
                        option.IsCorrect = true;
                        option.OptionContent.Content = string.Format("{0}×{1}={1}×{0}", valueA, valueB);
                        optionList.Add(option);
                    }
                    else
                    {
                        decimal valueA = rand.Next(minValue, maxValue + 1);
                        decimal valueB = rand.Next(minValue, maxValue + 1);
                        decimal valueC = rand.Next(minValue, decimal.ToInt32(valueA + valueB + 1));
                        decimal valueD = valueA + valueB - valueC;
                        QuestionOption option = new QuestionOption();
                        option.IsCorrect = (valueC == valueB) ? true : false;
                        option.OptionContent.Content = string.Format("{0}×{1}={2}×{3}", valueA, valueB, valueC, valueD);
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
