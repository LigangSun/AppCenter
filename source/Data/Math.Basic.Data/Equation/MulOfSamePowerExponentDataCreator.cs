using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;
using System.Threading;

namespace SoonLearning.Assessment.Data.Equation
{
    internal class MulOfSamePowerExponentDataCreator : DataCreator
    {
        private List<decimal> questionValueList = new List<decimal>();

        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "同底数幂的乘法练习";
            this.examTitle = "同底数幂的乘法测验";
            this.flowDocumentFile = "SoonLearning.Assessment.Data.Equation.MulOfSamePowerExponentFlowDocument.xaml";

            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题1-1（步骤）：",
                "（按照同底数幂的乘法填空）",
                5,
                1,
                10));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题1-2：",
                "（按照同底数幂的乘法填空）",
                5,
                1,
                10));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题2-1：",
                "（按照同底数幂的乘法填空）",
                5,
                1,
                10));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题3-1：",
                "（按照同底数幂的乘法填空）",
                5,
                1,
                5));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题3-2：",
                "（按照同底数幂的乘法填空）",
                5,
                1,
                5));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题4-1：",
                "（按照同底数幂的乘法填空）",
                5,
                1,
                5));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题5-1：",
                "（按照同底数幂的乘法填空）",
                5,
                1,
                10));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题6-1：",
                "（按照同底数幂的乘法填空）",
                5,
                1,
                10));
        }

        protected override void AppendQuestion(SectionBaseInfo info, SoonLearning.Math.Data.Section section)
        {
            int index = this.sectionInfoCollection.IndexOf(info);
            switch (info.QuestionType)
            {
                case QuestionType.MultiChoice:
                    this.CreateMCQuestion(info, section);
                    break;
                case QuestionType.FillInBlank:
                    switch (index)
                    {
                        case 0:
                            this.CreateFIBQuestion11(info, section);
                            break;
                        case 1:
                            this.CreateFIBQuestion12(info, section);
                            break;
                        case 2:
                            this.CreateFIBQuestion21(info, section);
                            break;
                        case 3:
                            this.CreateFIBQuestion31(info, section);
                            break;
                        case 4:
                            this.CreateFIBQuestion32(info, section);
                            break;
                        case 5:
                            this.CreateFIBQuestion41(info, section);
                            break;
                        case 6:
                            this.CreateFIBQuestion51(info, section);
                            break;
                        case 7:
                            this.CreateFIBQuestion61(info, section);
                            break;
                        default:
                            this.CreateFIBQuestion11(info, section);
                            break;
                    }
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

            mcQuestion.Solution.Content = string.Format("当x={2}，方程x + {0} = {1}等式左右两边相等，因此{2}是同底数幂的乘法，是正确答案。", valueB, result, valueA);

            section.QuestionCollection.Add(mcQuestion);
        }

        private void CreateFIBQuestion11(SectionBaseInfo info, Section section)
        {
            int minValue = 1;
            int maxValue = 10;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            int tmpValue = maxValue;

            decimal valueA = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueB = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueC = rand.Next(minValue, maxValue + 1);

            decimal coeff = rand.Next(2, tmpValue + 1);
            decimal symbol = rand.Next(0, 2);
            //    if (symbol == 0) // "+"
            {
                decimal result = valueA * valueB;
                this.questionValueList.Add(result);

                //string questionText = string.Format("已知");
                
                //QuestionContent amn = new QuestionContent();
                //QuestionPowerExponentPart an = new QuestionPowerExponentPart();
                //an.BaseNumber.Content = "a";
                //an.Power.Power.Content = "n";
                //amn.QuestionPartCollection.Add(an);

                //QuestionPowerPart mn = new QuestionPowerPart;
                //QuestionPowerExponentPart mnpart = new QuestionPowerExponentPart();
                //mnpart.BaseNumber.Content = "m";
                //mnpart.Power.Power.Content = "n";
                //mn.Power.QuestionPartCollection.Add(mnpart);

                //amn.QuestionPartCollection.Add(mn);

                //已知a^m=2,a^n=8,求：a^(m+n)；\n第一步, ", coeff, valueB, result);
                QuestionPowerExponentPart am = new QuestionPowerExponentPart();
                QuestionPowerExponentPart an = new QuestionPowerExponentPart();
                QuestionPowerExponentPart a_m_add_n = new QuestionPowerExponentPart();
                int rand_ab = rand.Next(0, 2);
                switch (rand_ab)
                {
                    case 0:
                        am.BaseNumber.Content = "a";
                        an.BaseNumber.Content = "a";
                        a_m_add_n.BaseNumber.Content = "a";
                        break;
                    case 1:
                        am.BaseNumber.Content = "b";
                        an.BaseNumber.Content = "b";
                        a_m_add_n.BaseNumber.Content = "b";
                        break;
                    default:
                        am.BaseNumber.Content = "a";
                        an.BaseNumber.Content = "a";
                        a_m_add_n.BaseNumber.Content = "a";
                        break;
                }
                am.Power.Content.Content = "m";
                an.Power.Content.Content = "n";
                a_m_add_n.Power.Content.Content = "m+n";

                //第一步，移项 {0}×x = {2} - {1}
                ESQuestion esQuestion = new ESQuestion();
                esQuestion.Content.QuestionPartCollection.Add(am);
                esQuestion.Content.QuestionPartCollection.Add(an);
                esQuestion.Content.QuestionPartCollection.Add(a_m_add_n);

                esQuestion.Content.Content = string.Format("已知{0}={1},{2}={3},求：{4}。\n",
                    am.PlaceHolder, valueA, an.PlaceHolder, valueB, a_m_add_n.PlaceHolder);

                /*
                esQuestion.Content.Content += "解：\n";
                esQuestion.Content.Content += "∵ ";

                QuestionBlank blankA = new QuestionBlank();
                blankA.MatchOwnRefAnswer = true;
                QuestionContent blankContentA = new QuestionContent();
                blankContentA.QuestionPartCollection.Add(am);
                blankContentA.Content = am.PlaceHolder;
                blankA.ReferenceAnswerList.Add(blankContentA);
                esQuestion.Content.QuestionPartCollection.Add(blankA);
                esQuestion.Content.Content += blankA.PlaceHolder;

                esQuestion.Content.Content += "=";

                QuestionBlank blankB = new QuestionBlank();
                blankB.MatchOwnRefAnswer = true;
                QuestionContent blankContentB = new QuestionContent();
                blankContentB.Content = valueA.ToString();
                blankContentB.ContentType = ContentType.Text;
                blankB.ReferenceAnswerList.Add(blankContentB);
                esQuestion.Content.QuestionPartCollection.Add(blankB);
                esQuestion.Content.Content += blankB.PlaceHolder;

                esQuestion.Content.Content += ",";

                QuestionBlank blankC = new QuestionBlank();
                blankC.MatchOwnRefAnswer = true;
                QuestionContent blankContentC = new QuestionContent();
                blankContentC.QuestionPartCollection.Add(an);
                blankContentC.Content = an.PlaceHolder;
                blankC.ReferenceAnswerList.Add(blankContentC);
                esQuestion.Content.QuestionPartCollection.Add(blankC);
                esQuestion.Content.Content += blankC.PlaceHolder;

                esQuestion.Content.Content += "=";
                QuestionBlank blankA1 = new QuestionBlank();
                blankA1.MatchOwnRefAnswer = true;
                QuestionContent blankContentA1 = new QuestionContent();
                blankContentA1.Content = valueB.ToString();
                blankContentA1.ContentType = ContentType.Text;
                blankA1.ReferenceAnswerList.Add(blankContentA1);
                esQuestion.Content.QuestionPartCollection.Add(blankA1);
                esQuestion.Content.Content += blankA1.PlaceHolder;

                esQuestion.Content.Content += "\n∴ ";

                QuestionBlank blankB1 = new QuestionBlank();
                blankB1.MatchOwnRefAnswer = true;
                QuestionContent blankContentB1 = new QuestionContent();
                blankContentB1.QuestionPartCollection.Add(a_m_add_n);
                blankContentB1.Content = a_m_add_n.PlaceHolder;
                blankB1.ReferenceAnswerList.Add(blankContentB1);
                esQuestion.Content.QuestionPartCollection.Add(blankB1);
                esQuestion.Content.Content += blankB1.PlaceHolder;

                esQuestion.Content.Content += "=";

                QuestionBlank blankA2 = new QuestionBlank();
                blankA2.MatchOwnRefAnswer = true;
                QuestionContent blankContentA2 = new QuestionContent();
                blankContentA2.QuestionPartCollection.Add(am);
                blankContentA2.Content = am.PlaceHolder;
                blankA2.ReferenceAnswerList.Add(blankContentA2);
                esQuestion.Content.QuestionPartCollection.Add(blankA2);
                esQuestion.Content.Content += blankA2.PlaceHolder;

                QuestionBlank blankB2 = new QuestionBlank();
                blankB2.MatchOwnRefAnswer = true;
                QuestionContent blankContentB2 = new QuestionContent();
                blankContentB2.QuestionPartCollection.Add(an);
                blankContentB2.Content = an.PlaceHolder;
                blankB2.ReferenceAnswerList.Add(blankContentB2);
                esQuestion.Content.QuestionPartCollection.Add(blankB2);
                esQuestion.Content.Content += blankB2.PlaceHolder;

                esQuestion.Content.Content += "=";

                QuestionBlank blankC2 = new QuestionBlank();
                blankC2.MatchOwnRefAnswer = true;
                QuestionContent blankContentC2 = new QuestionContent();
                blankContentC2.Content = valueA.ToString();
                blankContentC2.ContentType = ContentType.Text;
                blankC2.ReferenceAnswerList.Add(blankContentC2);
                esQuestion.Content.QuestionPartCollection.Add(blankC2);
                esQuestion.Content.Content += blankC2.PlaceHolder;

                esQuestion.Content.Content += "×";

                QuestionBlank blankD2 = new QuestionBlank();
                blankD2.MatchOwnRefAnswer = true;
                QuestionContent blankContentD2 = new QuestionContent();
                blankContentD2.Content = valueB.ToString();
                blankContentD2.ContentType = ContentType.Text;
                blankD2.ReferenceAnswerList.Add(blankContentD2);
                esQuestion.Content.QuestionPartCollection.Add(blankD2);
                esQuestion.Content.Content += blankD2.PlaceHolder;

                esQuestion.Content.Content += "=";

                //decimal valueA3 = valueA * valueB;
                QuestionBlank blankA3 = new QuestionBlank();
                blankA3.MatchOwnRefAnswer = true;
                QuestionContent blankContentA3 = new QuestionContent();
                blankContentA3.Content = result.ToString();
                blankContentA3.ContentType = ContentType.Text;
                blankA3.ReferenceAnswerList.Add(blankContentA3);
                esQuestion.Content.QuestionPartCollection.Add(blankA3);
                esQuestion.Content.Content += blankA3.PlaceHolder;
                 */

                // Solution
                esQuestion.Solution.QuestionPartCollection.Add(am);
                esQuestion.Solution.QuestionPartCollection.Add(an);
                esQuestion.Solution.QuestionPartCollection.Add(a_m_add_n);
                esQuestion.Solution.Content = string.Format(
                    "公式逆运用，{0}={1}{2}={3}×{4}={5}",
                    a_m_add_n.PlaceHolder, am.PlaceHolder, an.PlaceHolder, valueA, valueB, result);

                section.QuestionCollection.Add(esQuestion);
            }
            //   else // "-"
            {
            }
        }
 
        private void CreateFIBQuestion12(SectionBaseInfo info, Section section)
        {
            int minValue = 1;
            int maxValue = 10;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            int tmpValue = maxValue;

            decimal valueA = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueB = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal valueC = rand.Next(minValue, maxValue + 1);

            decimal coeff = rand.Next(2, tmpValue + 1);
            decimal symbol = rand.Next(0, 2);
 
            decimal result = valueA * valueB;
            this.questionValueList.Add(result);

                //已知a^m=2,a^n=8,求：a^(m+n)；\n第一步, ", coeff, valueB, result);
            QuestionPowerExponentPart am = new QuestionPowerExponentPart();
            QuestionPowerExponentPart an = new QuestionPowerExponentPart();
            QuestionPowerExponentPart a_m_add_n = new QuestionPowerExponentPart();
            int rand_ab = rand.Next(0, 2);
            switch (rand_ab)
            {
                case 0:
                    am.BaseNumber.Content = "a";
                    an.BaseNumber.Content = "a";
                    a_m_add_n.BaseNumber.Content = "a";
                    break;
                case 1:
                    am.BaseNumber.Content = "b";
                    an.BaseNumber.Content = "b";
                    a_m_add_n.BaseNumber.Content = "b";
                    break;
                default:
                    am.BaseNumber.Content = "a";
                    an.BaseNumber.Content = "a";
                    a_m_add_n.BaseNumber.Content = "a";
                    break;
            }
            am.Power.Content.Content = "m";
            an.Power.Content.Content = "n";
            a_m_add_n.Power.Content.Content = "m+n";

            ESQuestion esQuestion = new ESQuestion();
            esQuestion.Content.QuestionPartCollection.Add(am);
            esQuestion.Content.QuestionPartCollection.Add(an);
            esQuestion.Content.QuestionPartCollection.Add(a_m_add_n);

            esQuestion.Content.Content = string.Format("已知{0}={1},{2}={3}, 那么{4}",
            am.PlaceHolder, valueA, an.PlaceHolder, valueB, a_m_add_n.PlaceHolder);

            //QuestionBlank blankA3 = new QuestionBlank();
            //blankA3.MatchOwnRefAnswer = true;
            //QuestionContent blankContentA3 = new QuestionContent();
            //blankContentA3.Content = result.ToString();
            //blankContentA3.ContentType = ContentType.Text;
            //blankA3.ReferenceAnswerList.Add(blankContentA3);
            //esQuestion.Content.QuestionPartCollection.Add(blankA3);
            //esQuestion.Content.Content += blankA3.PlaceHolder;

            // Solution
            esQuestion.Solution.QuestionPartCollection.Add(am);
            esQuestion.Solution.QuestionPartCollection.Add(an);
            esQuestion.Solution.QuestionPartCollection.Add(a_m_add_n);
            esQuestion.Solution.Content = string.Format(
                "公式逆运用，{0}={1}{2}={3}×{4}={5}",
                a_m_add_n.PlaceHolder, am.PlaceHolder, an.PlaceHolder, valueA, valueB, result);

            section.QuestionCollection.Add(esQuestion);
        }

        private void CreateFIBQuestion21(SectionBaseInfo info, Section section)
        {
            int minValue = 1;
            int maxValue = 10;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            int tmpValue = maxValue;

            decimal value1 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value2 = rand.Next(minValue, maxValue / 2 + 1);
            Thread.Sleep(10);
            decimal value3 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value4 = rand.Next(minValue, maxValue + 1);

            decimal coeff = rand.Next(2, tmpValue + 1);
            decimal symbol = rand.Next(0, 2);
            //    if (symbol == 0) // "+"
            {
                decimal result = value3 + value4;
                this.questionValueList.Add(result);

                QuestionPowerExponentPart total = new QuestionPowerExponentPart();
                total.BaseNumber = new QuestionContent();

                int rand_ab = rand.Next(0, 1 + 1);

                QuestionPowerExponentPart baseNumber1 = new QuestionPowerExponentPart();
                if (rand_ab == 0)
                {
                    baseNumber1.BaseNumber.Content = "a";
                }
                else
                {
                    baseNumber1.BaseNumber.Content = "b";
                }
                baseNumber1.Power.Content.Content = value1.ToString();
                total.BaseNumber.QuestionPartCollection.Add(baseNumber1);

                total.BaseNumber.Content += "(";
                total.BaseNumber.Content += baseNumber1.PlaceHolder;
                if (symbol == 0)
                {
                    total.BaseNumber.Content += "-";
                }
                else
                {
                    total.BaseNumber.Content += "+";
                }

                QuestionPowerExponentPart baseNumber2 = new QuestionPowerExponentPart();
                int rand_ab2 = rand.Next(0, 1 + 1);
                if (rand_ab2 == 0)
                {
                    baseNumber2.BaseNumber.Content = "a";
                }
                else
                {
                    baseNumber2.BaseNumber.Content = "b";
                }
                baseNumber2.Power.Content.Content = value2.ToString();
                total.BaseNumber.QuestionPartCollection.Add(baseNumber2);

                total.BaseNumber.Content += baseNumber2.PlaceHolder;
                total.BaseNumber.Content += ")";

                //
                total.Power.Content.Content += value3.ToString();

                ESQuestion esQuestion = new ESQuestion();
                           
                esQuestion.Content.QuestionPartCollection.Add(total);
                esQuestion.Content.Content += total.PlaceHolder;

                // 题干的第二部分
                QuestionPowerExponentPart total2 = new QuestionPowerExponentPart();
                total2.BaseNumber = total.BaseNumber;
                total2.Power.Content.Content += value4.ToString();

                esQuestion.Content.QuestionPartCollection.Add(total2);
                esQuestion.Content.Content += total2.PlaceHolder;

                //fibQuestion.Content.Content = string.Format("({0},\n", domain4.PlaceHolder);
                
             //   esQuestion.Content.Content += "=";
     
                /*
                fibQuestion.Content.Content += "(";
     
                QuestionBlank blankA = new QuestionBlank();
                blankA.MatchOwnRefAnswer = true;
                QuestionContent blankContentA = new QuestionContent();
                blankContentA.QuestionPartCollection.Add(baseNumber1);
                blankContentA.Content = baseNumber1.PlaceHolder;
                blankA.ReferenceAnswerList.Add(blankContentA);
                fibQuestion.QuestionBlankCollection.Add(blankA);
                fibQuestion.Content.Content += blankA.PlaceHolder;

                if (symbol == 0)
                {
                    fibQuestion.Content.Content += "-";
                }
                else
                {
                    fibQuestion.Content.Content += "+";
                }
    
                QuestionBlank blankB = new QuestionBlank();
                blankB.MatchOwnRefAnswer = true;
                QuestionContent blankContentB = new QuestionContent();
                blankContentB.QuestionPartCollection.Add(baseNumber2);
                blankContentB.Content = baseNumber2.PlaceHolder;
                blankB.ReferenceAnswerList.Add(blankContentB);
                fibQuestion.QuestionBlankCollection.Add(blankB);
                fibQuestion.Content.Content += blankB.PlaceHolder;

                QuestionPowerExponentPart answer_pow = new QuestionPowerExponentPart();
                answer_pow.BaseNumber.Content += ")";
                answer_pow.Power.Power.Content += result.ToString();
                fibQuestion.QuestionBlankCollection.Add(answer_pow);
                fibQuestion.Content.Content += answer_pow.PlaceHolder;
                */
                /*
                QuestionBlank blankC = new QuestionBlank();
                blankC.MatchOwnRefAnswer = true;
                QuestionContent blankContentC = new QuestionContent();
                blankContentC.QuestionPartCollection.Add(an);
                blankContentC.Content = an.PlaceHolder;
                blankC.ReferenceAnswerList.Add(blankContentC);
                fibQuestion.QuestionBlankCollection.Add(blankC);
                fibQuestion.Content.Content += blankC.PlaceHolder;

                fibQuestion.Content.Content += "=";
                QuestionBlank blankA1 = new QuestionBlank();
                blankA1.MatchOwnRefAnswer = true;
                QuestionContent blankContentA1 = new QuestionContent();
                blankContentA1.Content = valueB.ToString();
                blankContentA1.ContentType = ContentType.Text;
                blankA1.ReferenceAnswerList.Add(blankContentA1);
                fibQuestion.QuestionBlankCollection.Add(blankA1);
                fibQuestion.Content.Content += blankA1.PlaceHolder;

                fibQuestion.Content.Content += "\n∴ ";

                QuestionBlank blankB1 = new QuestionBlank();
                blankB1.MatchOwnRefAnswer = true;
                QuestionContent blankContentB1 = new QuestionContent();
                blankContentB1.QuestionPartCollection.Add(a_m_add_n);
                blankContentB1.Content = a_m_add_n.PlaceHolder;
                blankB1.ReferenceAnswerList.Add(blankContentB1);
                fibQuestion.QuestionBlankCollection.Add(blankB1);
                fibQuestion.Content.Content += blankB1.PlaceHolder;

                fibQuestion.Content.Content += "=";

                QuestionBlank blankA2 = new QuestionBlank();
                blankA2.MatchOwnRefAnswer = true;
                QuestionContent blankContentA2 = new QuestionContent();
                blankContentA2.QuestionPartCollection.Add(am);
                blankContentA2.Content = am.PlaceHolder;
                blankA2.ReferenceAnswerList.Add(blankContentA2);
                fibQuestion.QuestionBlankCollection.Add(blankA2);
                fibQuestion.Content.Content += blankA2.PlaceHolder;

                QuestionBlank blankB2 = new QuestionBlank();
                blankB2.MatchOwnRefAnswer = true;
                QuestionContent blankContentB2 = new QuestionContent();
                blankContentB2.QuestionPartCollection.Add(an);
                blankContentB2.Content = an.PlaceHolder;
                blankB2.ReferenceAnswerList.Add(blankContentB2);
                fibQuestion.QuestionBlankCollection.Add(blankB2);
                fibQuestion.Content.Content += blankB2.PlaceHolder;

                fibQuestion.Content.Content += "=";

                QuestionBlank blankC2 = new QuestionBlank();
                blankC2.MatchOwnRefAnswer = true;
                QuestionContent blankContentC2 = new QuestionContent();
                blankContentC2.Content = valueA.ToString();
                blankContentC2.ContentType = ContentType.Text;
                blankC2.ReferenceAnswerList.Add(blankContentC2);
                fibQuestion.QuestionBlankCollection.Add(blankC2);
                fibQuestion.Content.Content += blankC2.PlaceHolder;

                fibQuestion.Content.Content += "×";

                QuestionBlank blankD2 = new QuestionBlank();
                blankD2.MatchOwnRefAnswer = true;
                QuestionContent blankContentD2 = new QuestionContent();
                blankContentD2.Content = valueB.ToString();
                blankContentD2.ContentType = ContentType.Text;
                blankD2.ReferenceAnswerList.Add(blankContentD2);
                fibQuestion.QuestionBlankCollection.Add(blankD2);
                fibQuestion.Content.Content += blankD2.PlaceHolder;

                fibQuestion.Content.Content += "=";

                decimal valueA3 = valueA * valueB;
                QuestionBlank blankA3 = new QuestionBlank();
                blankA3.MatchOwnRefAnswer = true;
                QuestionContent blankContentA3 = new QuestionContent();
                blankContentA3.Content = valueA3.ToString();
                blankContentA3.ContentType = ContentType.Text;
                blankA3.ReferenceAnswerList.Add(blankContentA3);
                fibQuestion.QuestionBlankCollection.Add(blankA3);
                fibQuestion.Content.Content += blankA3.PlaceHolder;

                // Solution
                fibQuestion.Solution.QuestionPartCollection.Add(am);
                fibQuestion.Solution.QuestionPartCollection.Add(an);
                fibQuestion.Solution.QuestionPartCollection.Add(a_m_add_n);
                fibQuestion.Solution.Content = string.Format(
                    "公式逆运用，{0}={1}{2}={3}×{4}={5}",
                    a_m_add_n.PlaceHolder, am.PlaceHolder, an.PlaceHolder, valueA, valueB, valueA3);
*/
                section.QuestionCollection.Add(esQuestion);
            }
            //   else // "-"
            {
            }
        }
        
        private void CreateFIBQuestion31(SectionBaseInfo info, Section section)
        {
            int minValue = 1;
            int maxValue = 10;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            int tmpValue = maxValue;

            string[] basenumber_serials = new string[] {"x", "y"};
            string[] powdigital_serials = new string[] {"1", "2", "3"};
            string[] powalpha_serials = new string[] {"m", "n"};

            decimal value1 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value2 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value3 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value4 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value5 = rand.Next(minValue, maxValue + 1);

            decimal coeff = rand.Next(2, tmpValue + 1);
            decimal symbol = rand.Next(0, 2);

            decimal rand_pow_alpha = rand.Next(0, powalpha_serials.Length);

            {
                decimal result = value1 + value2 + value3;
                this.questionValueList.Add(result);

                QuestionPowerExponentPart total = new QuestionPowerExponentPart();
                total.BaseNumber = new QuestionContent();

                int rand_ab = rand.Next(0, 1 + 1);

                QuestionPowerExponentPart baseNumber1 = new QuestionPowerExponentPart();
                QuestionPowerExponentPart baseNumber2 = new QuestionPowerExponentPart();
                if (rand_ab == 0)
                {
                    baseNumber1.BaseNumber.Content = "x";
                    baseNumber2.BaseNumber.Content = "y";
                }
                else
                {
                    baseNumber1.BaseNumber.Content = "y";
                    baseNumber2.BaseNumber.Content = "x";
                }
                baseNumber1.Power.Content.Content = value1.ToString();
                total.BaseNumber.QuestionPartCollection.Add(baseNumber1);

                total.BaseNumber.Content += "(";
                total.BaseNumber.Content += baseNumber1.PlaceHolder;
                total.BaseNumber.Content += "-";

                baseNumber2.Power.Content.Content = value1.ToString();
                total.BaseNumber.QuestionPartCollection.Add(baseNumber2);

                total.BaseNumber.Content += baseNumber2.PlaceHolder;
                total.BaseNumber.Content += ")";

                //
                total.Power.Content.Content += value3.ToString();

                ESQuestion esQuestion = new ESQuestion();

                esQuestion.Content.QuestionPartCollection.Add(total);
                esQuestion.Content.Content += total.PlaceHolder;

                // 题干的第二部分
                QuestionPowerExponentPart total2 = new QuestionPowerExponentPart();
                //total2.BaseNumber = new QuestionContent();

                //if (rand_ab == 0)
                //{
                //    baseNumber1.BaseNumber.Content = "x";
                //    baseNumber2.BaseNumber.Content = "y";
                //}
                //else
                //{
                //    baseNumber1.BaseNumber.Content = "y";
                //    baseNumber2.BaseNumber.Content = "x";
                //}
                //baseNumber2.Power.Power.Content = value2.ToString();
                
                total2.BaseNumber.Content += "(";
                total2.BaseNumber.QuestionPartCollection.Add(baseNumber2);
                total2.BaseNumber.Content += baseNumber2.PlaceHolder;
                
                total2.BaseNumber.Content += "-";
                total2.BaseNumber.QuestionPartCollection.Add(baseNumber1);
                total2.BaseNumber.Content += baseNumber1.PlaceHolder;
                total2.BaseNumber.Content += ")";
                 
                //
                total2.Power.Content.Content += "m" + "+" + value4.ToString();
                esQuestion.Content.QuestionPartCollection.Add(total2);
                esQuestion.Content.Content += total2.PlaceHolder;

                // 题干的第三部分
                QuestionPowerExponentPart total3 = new QuestionPowerExponentPart();
                total3.BaseNumber = total.BaseNumber;
                total3.Power.Content.Content += value5.ToString();

                esQuestion.Content.QuestionPartCollection.Add(total3);
                esQuestion.Content.Content += total3.PlaceHolder;

                // 题干的第四部分
                QuestionPowerExponentPart total4 = new QuestionPowerExponentPart();
                total4.BaseNumber = total2.BaseNumber;
                total4.Power.Content.Content += "m";

                esQuestion.Content.QuestionPartCollection.Add(total4);
                esQuestion.Content.Content += total4.PlaceHolder;

                /*
                esQuestion.Content.Content += "=(";

                QuestionBlank blankA = new QuestionBlank();
                blankA.MatchOwnRefAnswer = true;
                QuestionContent blankContentA = new QuestionContent();
                blankContentA.QuestionPartCollection.Add(baseNumber1);
                blankContentA.Content = baseNumber1.PlaceHolder;
                blankA.ReferenceAnswerList.Add(blankContentA);
                esQuestion.Content.QuestionPartCollection.Add(blankA);
                esQuestion.Content.Content += blankA.PlaceHolder;

                if (symbol == 0)
                {
                    esQuestion.Content.Content += "-";
                }
                else
                {
                    esQuestion.Content.Content += "+";
                }

                QuestionBlank blankB = new QuestionBlank();
                blankB.MatchOwnRefAnswer = true;
                QuestionContent blankContentB = new QuestionContent();
                blankContentB.QuestionPartCollection.Add(baseNumber2);
                blankContentB.Content = baseNumber2.PlaceHolder;
                blankB.ReferenceAnswerList.Add(blankContentB);
                esQuestion.Content.QuestionPartCollection.Add(blankB);
                esQuestion.Content.Content += blankB.PlaceHolder;

                QuestionPowerExponentPart answer_pow = new QuestionPowerExponentPart();
                answer_pow.BaseNumber.Content += ")";
                answer_pow.Power.Power.Content += result.ToString();
                esQuestion.Content.QuestionPartCollection.Add(answer_pow);
                esQuestion.Content.Content += answer_pow.PlaceHolder;
*/
                /*
                QuestionBlank blankC = new QuestionBlank();
                blankC.MatchOwnRefAnswer = true;
                QuestionContent blankContentC = new QuestionContent();
                blankContentC.QuestionPartCollection.Add(an);
                blankContentC.Content = an.PlaceHolder;
                blankC.ReferenceAnswerList.Add(blankContentC);
                fibQuestion.QuestionBlankCollection.Add(blankC);
                fibQuestion.Content.Content += blankC.PlaceHolder;

                fibQuestion.Content.Content += "=";
                QuestionBlank blankA1 = new QuestionBlank();
                blankA1.MatchOwnRefAnswer = true;
                QuestionContent blankContentA1 = new QuestionContent();
                blankContentA1.Content = valueB.ToString();
                blankContentA1.ContentType = ContentType.Text;
                blankA1.ReferenceAnswerList.Add(blankContentA1);
                fibQuestion.QuestionBlankCollection.Add(blankA1);
                fibQuestion.Content.Content += blankA1.PlaceHolder;

                fibQuestion.Content.Content += "\n∴ ";

                QuestionBlank blankB1 = new QuestionBlank();
                blankB1.MatchOwnRefAnswer = true;
                QuestionContent blankContentB1 = new QuestionContent();
                blankContentB1.QuestionPartCollection.Add(a_m_add_n);
                blankContentB1.Content = a_m_add_n.PlaceHolder;
                blankB1.ReferenceAnswerList.Add(blankContentB1);
                fibQuestion.QuestionBlankCollection.Add(blankB1);
                fibQuestion.Content.Content += blankB1.PlaceHolder;

                fibQuestion.Content.Content += "=";

                QuestionBlank blankA2 = new QuestionBlank();
                blankA2.MatchOwnRefAnswer = true;
                QuestionContent blankContentA2 = new QuestionContent();
                blankContentA2.QuestionPartCollection.Add(am);
                blankContentA2.Content = am.PlaceHolder;
                blankA2.ReferenceAnswerList.Add(blankContentA2);
                fibQuestion.QuestionBlankCollection.Add(blankA2);
                fibQuestion.Content.Content += blankA2.PlaceHolder;

                QuestionBlank blankB2 = new QuestionBlank();
                blankB2.MatchOwnRefAnswer = true;
                QuestionContent blankContentB2 = new QuestionContent();
                blankContentB2.QuestionPartCollection.Add(an);
                blankContentB2.Content = an.PlaceHolder;
                blankB2.ReferenceAnswerList.Add(blankContentB2);
                fibQuestion.QuestionBlankCollection.Add(blankB2);
                fibQuestion.Content.Content += blankB2.PlaceHolder;

                fibQuestion.Content.Content += "=";

                QuestionBlank blankC2 = new QuestionBlank();
                blankC2.MatchOwnRefAnswer = true;
                QuestionContent blankContentC2 = new QuestionContent();
                blankContentC2.Content = valueA.ToString();
                blankContentC2.ContentType = ContentType.Text;
                blankC2.ReferenceAnswerList.Add(blankContentC2);
                fibQuestion.QuestionBlankCollection.Add(blankC2);
                fibQuestion.Content.Content += blankC2.PlaceHolder;

                fibQuestion.Content.Content += "×";

                QuestionBlank blankD2 = new QuestionBlank();
                blankD2.MatchOwnRefAnswer = true;
                QuestionContent blankContentD2 = new QuestionContent();
                blankContentD2.Content = valueB.ToString();
                blankContentD2.ContentType = ContentType.Text;
                blankD2.ReferenceAnswerList.Add(blankContentD2);
                fibQuestion.QuestionBlankCollection.Add(blankD2);
                fibQuestion.Content.Content += blankD2.PlaceHolder;

                fibQuestion.Content.Content += "=";

                decimal valueA3 = valueA * valueB;
                QuestionBlank blankA3 = new QuestionBlank();
                blankA3.MatchOwnRefAnswer = true;
                QuestionContent blankContentA3 = new QuestionContent();
                blankContentA3.Content = valueA3.ToString();
                blankContentA3.ContentType = ContentType.Text;
                blankA3.ReferenceAnswerList.Add(blankContentA3);
                fibQuestion.QuestionBlankCollection.Add(blankA3);
                fibQuestion.Content.Content += blankA3.PlaceHolder;

                // Solution
                fibQuestion.Solution.QuestionPartCollection.Add(am);
                fibQuestion.Solution.QuestionPartCollection.Add(an);
                fibQuestion.Solution.QuestionPartCollection.Add(a_m_add_n);
                fibQuestion.Solution.Content = string.Format(
                    "公式逆运用，{0}={1}{2}={3}×{4}={5}",
                    a_m_add_n.PlaceHolder, am.PlaceHolder, an.PlaceHolder, valueA, valueB, valueA3);
*/
                section.QuestionCollection.Add(esQuestion);
            }
            //   else // "-"
            {
            }
        }

        private void CreateFIBQuestion32(SectionBaseInfo info, Section section)
        {
            int minValue = 1;
            int maxValue = 10;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            int tmpValue = maxValue;

            string[] basenumber_serials = new string[] { "x", "y" };
            string[] powdigital_serials = new string[] { "1", "2", "3" };
            string[] powalpha_serials = new string[] { "m", "n" };

            decimal value1 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value2 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value3 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value4 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value5 = rand.Next(minValue, maxValue + 1);

            decimal coeff = rand.Next(2, tmpValue + 1);
            decimal symbol = rand.Next(0, 2);

            decimal rand_pow_alpha = rand.Next(0, powalpha_serials.Length);

            //    if (symbol == 0) // "+"
            {
                decimal result = value1 + value2 + value3;
                this.questionValueList.Add(result);

                QuestionPowerExponentPart total = new QuestionPowerExponentPart();
                total.BaseNumber = new QuestionContent();

                int rand_ab = rand.Next(0, 1 + 1);

                QuestionPowerExponentPart baseNumber1 = new QuestionPowerExponentPart();
                QuestionPowerExponentPart baseNumber2 = new QuestionPowerExponentPart();
                if (rand_ab == 0)
                {
                    baseNumber1.BaseNumber.Content = "x";
                    baseNumber2.BaseNumber.Content = "y";
                }
                else
                {
                    baseNumber1.BaseNumber.Content = "y";
                    baseNumber2.BaseNumber.Content = "x";
                }
                baseNumber1.Power.Content.Content = value1.ToString();
                total.BaseNumber.QuestionPartCollection.Add(baseNumber1);

                total.BaseNumber.Content += "(";
                total.BaseNumber.Content += baseNumber1.PlaceHolder;
                total.BaseNumber.Content += "-";

                baseNumber2.Power.Content.Content = value2.ToString();
                total.BaseNumber.QuestionPartCollection.Add(baseNumber2);

                total.BaseNumber.Content += baseNumber2.PlaceHolder;
                total.BaseNumber.Content += ")";

                //
                total.Power.Content.Content += value3.ToString();

                ESQuestion esQuestion = new ESQuestion();

                esQuestion.Content.QuestionPartCollection.Add(total);
                esQuestion.Content.Content += total.PlaceHolder;

                // 题干的第二部分
                QuestionPowerExponentPart total2 = new QuestionPowerExponentPart();
                //total2.BaseNumber = new QuestionContent();

                //if (rand_ab == 0)
                //{
                //    baseNumber1.BaseNumber.Content = "x";
                //    baseNumber2.BaseNumber.Content = "y";
                //}
                //else
                //{
                //    baseNumber1.BaseNumber.Content = "y";
                //    baseNumber2.BaseNumber.Content = "x";
                //}
                //baseNumber2.Power.Power.Content = value2.ToString();

                total2.BaseNumber.Content += "(";
                total2.BaseNumber.QuestionPartCollection.Add(baseNumber2);
                total2.BaseNumber.Content += baseNumber2.PlaceHolder;

                total2.BaseNumber.Content += "-";
                total2.BaseNumber.QuestionPartCollection.Add(baseNumber1);
                total2.BaseNumber.Content += baseNumber1.PlaceHolder;
                total2.BaseNumber.Content += ")";

                //
                total2.Power.Content.Content += "m" + "+" + value4.ToString();
                esQuestion.Content.QuestionPartCollection.Add(total2);
                esQuestion.Content.Content += total2.PlaceHolder;

                // 题干的第三部分
                QuestionPowerExponentPart total3 = new QuestionPowerExponentPart();
                total3.BaseNumber = total.BaseNumber;
                total3.Power.Content.Content += value5.ToString();

                esQuestion.Content.QuestionPartCollection.Add(total3);
                esQuestion.Content.Content += total3.PlaceHolder;

                // 题干的第四部分
                QuestionPowerExponentPart total4 = new QuestionPowerExponentPart();
                total4.BaseNumber = total2.BaseNumber;
                total4.Power.Content.Content += "m";

                esQuestion.Content.QuestionPartCollection.Add(total4);
                esQuestion.Content.Content += total4.PlaceHolder;

                /*
                esQuestion.Content.Content += "=(";

                QuestionBlank blankA = new QuestionBlank();
                blankA.MatchOwnRefAnswer = true;
                QuestionContent blankContentA = new QuestionContent();
                blankContentA.QuestionPartCollection.Add(baseNumber1);
                blankContentA.Content = baseNumber1.PlaceHolder;
                blankA.ReferenceAnswerList.Add(blankContentA);
                esQuestion.Content.QuestionPartCollection.Add(blankA);
                esQuestion.Content.Content += blankA.PlaceHolder;

                if (symbol == 0)
                {
                    esQuestion.Content.Content += "-";
                }
                else
                {
                    esQuestion.Content.Content += "+";
                }

                QuestionBlank blankB = new QuestionBlank();
                blankB.MatchOwnRefAnswer = true;
                QuestionContent blankContentB = new QuestionContent();
                blankContentB.QuestionPartCollection.Add(baseNumber2);
                blankContentB.Content = baseNumber2.PlaceHolder;
                blankB.ReferenceAnswerList.Add(blankContentB);
                esQuestion.Content.QuestionPartCollection.Add(blankB);
                esQuestion.Content.Content += blankB.PlaceHolder;

                QuestionPowerExponentPart answer_pow = new QuestionPowerExponentPart();
                answer_pow.BaseNumber.Content += ")";
                answer_pow.Power.Power.Content += result.ToString();
                esQuestion.Content.QuestionPartCollection.Add(answer_pow);
                esQuestion.Content.Content += answer_pow.PlaceHolder;
*/
                /*
                QuestionBlank blankC = new QuestionBlank();
                blankC.MatchOwnRefAnswer = true;
                QuestionContent blankContentC = new QuestionContent();
                blankContentC.QuestionPartCollection.Add(an);
                blankContentC.Content = an.PlaceHolder;
                blankC.ReferenceAnswerList.Add(blankContentC);
                fibQuestion.QuestionBlankCollection.Add(blankC);
                fibQuestion.Content.Content += blankC.PlaceHolder;

                fibQuestion.Content.Content += "=";
                QuestionBlank blankA1 = new QuestionBlank();
                blankA1.MatchOwnRefAnswer = true;
                QuestionContent blankContentA1 = new QuestionContent();
                blankContentA1.Content = valueB.ToString();
                blankContentA1.ContentType = ContentType.Text;
                blankA1.ReferenceAnswerList.Add(blankContentA1);
                fibQuestion.QuestionBlankCollection.Add(blankA1);
                fibQuestion.Content.Content += blankA1.PlaceHolder;

                fibQuestion.Content.Content += "\n∴ ";

                QuestionBlank blankB1 = new QuestionBlank();
                blankB1.MatchOwnRefAnswer = true;
                QuestionContent blankContentB1 = new QuestionContent();
                blankContentB1.QuestionPartCollection.Add(a_m_add_n);
                blankContentB1.Content = a_m_add_n.PlaceHolder;
                blankB1.ReferenceAnswerList.Add(blankContentB1);
                fibQuestion.QuestionBlankCollection.Add(blankB1);
                fibQuestion.Content.Content += blankB1.PlaceHolder;

                fibQuestion.Content.Content += "=";

                QuestionBlank blankA2 = new QuestionBlank();
                blankA2.MatchOwnRefAnswer = true;
                QuestionContent blankContentA2 = new QuestionContent();
                blankContentA2.QuestionPartCollection.Add(am);
                blankContentA2.Content = am.PlaceHolder;
                blankA2.ReferenceAnswerList.Add(blankContentA2);
                fibQuestion.QuestionBlankCollection.Add(blankA2);
                fibQuestion.Content.Content += blankA2.PlaceHolder;

                QuestionBlank blankB2 = new QuestionBlank();
                blankB2.MatchOwnRefAnswer = true;
                QuestionContent blankContentB2 = new QuestionContent();
                blankContentB2.QuestionPartCollection.Add(an);
                blankContentB2.Content = an.PlaceHolder;
                blankB2.ReferenceAnswerList.Add(blankContentB2);
                fibQuestion.QuestionBlankCollection.Add(blankB2);
                fibQuestion.Content.Content += blankB2.PlaceHolder;

                fibQuestion.Content.Content += "=";

                QuestionBlank blankC2 = new QuestionBlank();
                blankC2.MatchOwnRefAnswer = true;
                QuestionContent blankContentC2 = new QuestionContent();
                blankContentC2.Content = valueA.ToString();
                blankContentC2.ContentType = ContentType.Text;
                blankC2.ReferenceAnswerList.Add(blankContentC2);
                fibQuestion.QuestionBlankCollection.Add(blankC2);
                fibQuestion.Content.Content += blankC2.PlaceHolder;

                fibQuestion.Content.Content += "×";

                QuestionBlank blankD2 = new QuestionBlank();
                blankD2.MatchOwnRefAnswer = true;
                QuestionContent blankContentD2 = new QuestionContent();
                blankContentD2.Content = valueB.ToString();
                blankContentD2.ContentType = ContentType.Text;
                blankD2.ReferenceAnswerList.Add(blankContentD2);
                fibQuestion.QuestionBlankCollection.Add(blankD2);
                fibQuestion.Content.Content += blankD2.PlaceHolder;

                fibQuestion.Content.Content += "=";

                decimal valueA3 = valueA * valueB;
                QuestionBlank blankA3 = new QuestionBlank();
                blankA3.MatchOwnRefAnswer = true;
                QuestionContent blankContentA3 = new QuestionContent();
                blankContentA3.Content = valueA3.ToString();
                blankContentA3.ContentType = ContentType.Text;
                blankA3.ReferenceAnswerList.Add(blankContentA3);
                fibQuestion.QuestionBlankCollection.Add(blankA3);
                fibQuestion.Content.Content += blankA3.PlaceHolder;

                // Solution
                fibQuestion.Solution.QuestionPartCollection.Add(am);
                fibQuestion.Solution.QuestionPartCollection.Add(an);
                fibQuestion.Solution.QuestionPartCollection.Add(a_m_add_n);
                fibQuestion.Solution.Content = string.Format(
                    "公式逆运用，{0}={1}{2}={3}×{4}={5}",
                    a_m_add_n.PlaceHolder, am.PlaceHolder, an.PlaceHolder, valueA, valueB, valueA3);
*/
                section.QuestionCollection.Add(esQuestion);
            }
            //   else // "-"
            {
            }
        }

        private void CreateFIBQuestion41(SectionBaseInfo info, Section section)
        {
            int minValue = 1;
            int maxValue = 10;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            string[] basenumber_serials = new string[] { "x", "y" };
            string[] powdigital_serials = new string[] { "1", "2", "3" };
            string[] powalpha_serials = new string[] { "m", "n" };

            decimal valueBase = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value1 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value2 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value3 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value4 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value5 = rand.Next(minValue, maxValue + 1);

            decimal rand_pow_alpha = rand.Next(0, powalpha_serials.Length);

            ESQuestion esQuestion = new ESQuestion();
                
            decimal result = value1 + value2 + value3;
            this.questionValueList.Add(result);

            // 题干的第一部分
            int pow_value = (int)System.Math.Pow(decimal.ToDouble(valueBase), decimal.ToDouble(value1));
            esQuestion.Content.Content += pow_value.ToString();
            esQuestion.Content.Content += "×";

            // 题干的第二部分
            QuestionPowerExponentPart PEP1 = new QuestionPowerExponentPart();
            decimal sign1 = rand.Next(0, 1 + 1);
            if (sign1 == 1) // "-"
            {
                PEP1.BaseNumber.Content += "(" + "-" + valueBase.ToString() + ")";
            }
            else
            {
                PEP1.BaseNumber.Content += valueBase.ToString();
            }

            PEP1.Power.Content.Content += value2.ToString();
            esQuestion.Content.QuestionPartCollection.Add(PEP1);
            esQuestion.Content.Content += PEP1.PlaceHolder;
            esQuestion.Content.Content += "×";

            // 题干的第三部分
            QuestionPowerExponentPart PEP2 = new QuestionPowerExponentPart();
            decimal sign2 = rand.Next(0, 1 + 1);
            if (sign2 == 1) // "-"
            {
                PEP2.BaseNumber.Content += "(" + "-" + valueBase.ToString() + ")";
            }
            else
            {
                PEP2.BaseNumber.Content += valueBase.ToString();
            }

            PEP2.Power.Content.Content += value3.ToString();
            esQuestion.Content.QuestionPartCollection.Add(PEP2);
            esQuestion.Content.Content += PEP2.PlaceHolder;
          
            decimal sign3 = rand.Next(0, 1 + 1);
            if (sign3 == 1) // "-"
            {
                esQuestion.Content.Content += "-";
            }
            else
            {
                esQuestion.Content.Content += "+";
            }

            // 题干的第4部分
            QuestionPowerExponentPart PEP3 = new QuestionPowerExponentPart();
            decimal sign4 = rand.Next(0, 1 + 1);
            if (sign4 == 1) // "-"
            {
                PEP3.BaseNumber.Content += "(" + "-" + valueBase.ToString() + ")";
            }
            else
            {
                PEP3.BaseNumber.Content += valueBase.ToString();
            }

            PEP3.Power.Content.Content += value4.ToString();
            esQuestion.Content.QuestionPartCollection.Add(PEP3);
            esQuestion.Content.Content += PEP3.PlaceHolder;
            esQuestion.Content.Content += "×";

            // 题干的第5部分
            QuestionPowerExponentPart PEP4 = new QuestionPowerExponentPart();
            decimal sign5 = rand.Next(0, 1 + 1);
            if (sign5 == 1) // "-"
            {
                PEP4.BaseNumber.Content += "(" + "-" + valueBase.ToString() + ")";
            }
            else
            {
                PEP4.BaseNumber.Content += valueBase.ToString();
            }

            PEP4.Power.Content.Content += value5.ToString();
            esQuestion.Content.QuestionPartCollection.Add(PEP4);
            esQuestion.Content.Content += PEP4.PlaceHolder;

            // 参考解题步骤及答案
            // Solution
            esQuestion.Solution.Content += "可化为同底的计算。\n";
            esQuestion.Solution.Content += esQuestion.Content.Content;
            esQuestion.Solution.QuestionPartCollection.AddRange(esQuestion.Content.QuestionPartCollection);

         //   esQuestion.Content.Content += "=";
            esQuestion.Solution.Content += "\n=";
            // First step
            QuestionPowerExponentPart PEP_Solu1 = new QuestionPowerExponentPart();
            PEP_Solu1.BaseNumber.Content = valueBase.ToString();
            PEP_Solu1.Power.Content.Content = value1.ToString();
            esQuestion.Solution.QuestionPartCollection.Add(PEP_Solu1);
            esQuestion.Solution.Content += PEP_Solu1.PlaceHolder;
            esQuestion.Solution.Content += "×";

            QuestionPowerExponentPart PEP_Solu2 = new QuestionPowerExponentPart();
            if (value2 % 2 == 0)
            {
                PEP_Solu2.BaseNumber.Content = valueBase.ToString();
            }
            else
            {
                PEP_Solu2.BaseNumber = PEP1.BaseNumber;
            }
            PEP_Solu2.Power = PEP1.Power;
            esQuestion.Solution.QuestionPartCollection.Add(PEP_Solu2);
            esQuestion.Solution.Content += PEP_Solu2.PlaceHolder;
            esQuestion.Solution.Content += "×";

            QuestionPowerExponentPart PEP_Solu3 = new QuestionPowerExponentPart();
            if (value3 % 2 == 0)
            {
                PEP_Solu3.BaseNumber.Content = valueBase.ToString();
            }
            else
            {
                PEP_Solu3.BaseNumber = PEP2.BaseNumber;
            }
            PEP_Solu3.Power = PEP2.Power;
            esQuestion.Solution.QuestionPartCollection.Add(PEP_Solu3);
            esQuestion.Solution.Content += PEP_Solu3.PlaceHolder;
           
            if (sign3 == 1)
            {
                esQuestion.Solution.Content += "-";
            }
            else
            {
                esQuestion.Solution.Content += "+";
            }

            QuestionPowerExponentPart PEP_Solu4 = new QuestionPowerExponentPart();
            if (value4 % 2 == 0)
            {
                PEP_Solu4.BaseNumber.Content = valueBase.ToString();
            }
            else
            {
                PEP_Solu4.BaseNumber = PEP3.BaseNumber;
            }
            PEP_Solu4.Power = PEP3.Power;
            esQuestion.Solution.QuestionPartCollection.Add(PEP_Solu4);
            esQuestion.Solution.Content += PEP_Solu4.PlaceHolder;
            esQuestion.Solution.Content += "×";

            QuestionPowerExponentPart PEP_Solu5 = new QuestionPowerExponentPart();
            if (value5 % 2 == 0)
            {
                PEP_Solu5.BaseNumber.Content = valueBase.ToString();
            }
            else
            {
                PEP_Solu5.BaseNumber = PEP4.BaseNumber;
            }
            PEP_Solu5.Power = PEP4.Power;
            esQuestion.Solution.QuestionPartCollection.Add(PEP_Solu5);
            esQuestion.Solution.Content += PEP_Solu5.PlaceHolder;
            
            /*

                          fibQuestion.Solution.Content += sum_coeff.ToString();
                          fibQuestion.Solution.Content += "×";
                          fibQuestion.Solution.QuestionPartCollection.Add(PEP1);
                          fibQuestion.Solution.Content += PEP1.PlaceHolder;
                          fibQuestion.Solution.Content += "=";

                          QuestionPowerExponentPart PEP2 = new QuestionPowerExponentPart();
                          PEP2.BaseNumber = PEP1.BaseNumber;
                          decimal pow2 = value1 + 1;
                          PEP2.Power.Power.Content = pow2.ToString();

                          fibQuestion.Solution.QuestionPartCollection.Add(PEP2);
                          fibQuestion.Solution.Content += PEP2.PlaceHolder;

                          // 填空最后答案
                          QuestionBlank blankA = new QuestionBlank();
                          blankA.MatchOwnRefAnswer = true;
                          QuestionContent blankContentA = new QuestionContent();

                          blankContentA.QuestionPartCollection.Add(PEP2);
                          blankContentA.Content = PEP2.PlaceHolder;
                          blankA.ReferenceAnswerList.Add(blankContentA);
                          fibQuestion.QuestionBlankCollection.Add(blankA);
                          fibQuestion.Content.Content += blankA.PlaceHolder;
                          */
            section.QuestionCollection.Add(esQuestion);
        }
        
        private void CreateFIBQuestion51(SectionBaseInfo info, Section section)
        {
            int minValue = 1;
            int maxValue = 10;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            // 随机出题干的项目数，暂时随机3 - 6
            Thread.Sleep(10);
            decimal sum_part = rand.Next(3, 6 + 1);
            Thread.Sleep(10);
            decimal valueBase = sum_part;//rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value1 = rand.Next(minValue, maxValue + 1);
 
            ESQuestion esQuestion = new ESQuestion();

            //decimal result = value1 + value2 + value3;
            //this.questionValueList.Add(result);
        
            // 题干的第一部分
            QuestionPowerExponentPart PEP1 = new QuestionPowerExponentPart();
            decimal sign1 = rand.Next(0, 1 + 1);
            if (sign1 == 1) // "-"
            {
                PEP1.BaseNumber.Content += "(" + "-" + valueBase.ToString() + ")";
            }
            else
            {
                PEP1.BaseNumber.Content += valueBase.ToString();
            }

            PEP1.Power.Content.Content += value1.ToString();
            esQuestion.Content.QuestionPartCollection.Add(PEP1);
            esQuestion.Content.Content += PEP1.PlaceHolder;

            // 题干的其余部分
            decimal sign2 = 0;// rand.Next(0, 1 + 1);
            decimal sum_coeff = 1;
            for (int i = 1; i < sum_part; i++)
            {
                if (sign2 == 1) // "-"
                {
                    esQuestion.Content.Content += "-";
                    sum_coeff--;
                }
                else
                {
                    esQuestion.Content.Content += "+";
                    sum_coeff++;
                }

                esQuestion.Content.QuestionPartCollection.Add(PEP1);
                esQuestion.Content.Content += PEP1.PlaceHolder;
            }

        //    esQuestion.Content.Content += "=";

            // 参考解题步骤及答案
            // Solution
            esQuestion.Solution.Content += "合并同类项法则。\n";
            esQuestion.Solution.Content += esQuestion.Content.Content;
            esQuestion.Solution.Content += sum_coeff.ToString();
            esQuestion.Solution.Content += "×";
            esQuestion.Solution.QuestionPartCollection.Add(PEP1);
            esQuestion.Solution.Content += PEP1.PlaceHolder;
         //   esQuestion.Solution.Content += "=";

            QuestionPowerExponentPart PEP2 = new QuestionPowerExponentPart();
            PEP2.BaseNumber = PEP1.BaseNumber;
            decimal pow2 = value1 + 1;
            PEP2.Power.Content.Content = pow2.ToString();

            esQuestion.Solution.QuestionPartCollection.Add(PEP2);
            esQuestion.Solution.Content += PEP2.PlaceHolder;

            // 填空最后答案
            //QuestionBlank blankA = new QuestionBlank();
            //blankA.MatchOwnRefAnswer = true;
            //QuestionContent blankContentA = new QuestionContent();

            //blankContentA.QuestionPartCollection.Add(PEP2);
            //blankContentA.Content = PEP2.PlaceHolder;
            //blankA.ReferenceAnswerList.Add(blankContentA);
            //esQuestion.Content.QuestionPartCollection.Add(blankA);
            //esQuestion.Content.Content += blankA.PlaceHolder;

            section.QuestionCollection.Add(esQuestion);
        }

        private void CreateFIBQuestion61(SectionBaseInfo info, Section section)
        {
            int minValue = 1;
            int maxValue = 10;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);

            int tmpValue = maxValue;

            string[] basenumber_serials = new string[] { "x", "y", "a", "b" };
            //string[] powdigital_serials = new string[] { "1", "2", "3" };
            string[] powalpha_serials = new string[] { "m", "n" };

            Thread.Sleep(10);
            decimal valueBase = rand.Next(minValue, maxValue + 1);
            decimal value1 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value2 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value3 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value4 = rand.Next(minValue, maxValue + 1);
            Thread.Sleep(10);
            decimal value5 = rand.Next(minValue, maxValue + 1);

            decimal coeff = rand.Next(2, tmpValue + 1);

            decimal rand_pow_alpha = rand.Next(0, powalpha_serials.Length);

            FIBQuestion fibQuestion = new FIBQuestion();

            //this.questionValueList.Add(result);

            // 随机出题干的项目数，暂时随机3 - 5
            decimal sum_part = rand.Next(3, 5 + 1);

            // 选字母
            decimal rand_alpha = rand.Next(0, 4);

            // 题干的第一部分
            fibQuestion.Content.Content += "已知";
            QuestionPowerExponentPart PEP1 = new QuestionPowerExponentPart();
            decimal sign1 = rand.Next(0, 1 + 1);
            if (sign1 == 1) // "-"
            {
                PEP1.BaseNumber.Content += "(" + "-" + basenumber_serials[decimal.ToInt32(rand_alpha)] + ")";
            }
            else
            {
                PEP1.BaseNumber.Content += basenumber_serials[decimal.ToInt32(rand_alpha)];
            }

            PEP1.Power.Content.Content += value1.ToString();
            fibQuestion.Content.QuestionPartCollection.Add(PEP1);
            fibQuestion.Content.Content += PEP1.PlaceHolder;

            // 题干的第二部分
            QuestionPowerExponentPart PEP2 = new QuestionPowerExponentPart();
            PEP2.BaseNumber = PEP1.BaseNumber;
            PEP2.Power.Content.Content += value2.ToString();
            fibQuestion.Content.QuestionPartCollection.Add(PEP2);
            fibQuestion.Content.Content += PEP2.PlaceHolder;

            // 题干的第3部分
            QuestionPowerExponentPart PEP3 = new QuestionPowerExponentPart();
            PEP3.BaseNumber = PEP1.BaseNumber;
            PEP3.Power.Content.Content += "n";
            fibQuestion.Content.QuestionPartCollection.Add(PEP3);
            fibQuestion.Content.Content += PEP3.PlaceHolder;

            fibQuestion.Content.Content += "=";
            // 题干的第4部分
            decimal valueFour = value1 + value2 + value3;
            QuestionPowerExponentPart PEP4 = new QuestionPowerExponentPart();
            PEP4.BaseNumber = PEP1.BaseNumber;
            PEP4.Power.Content.Content += valueFour.ToString();
            fibQuestion.Content.QuestionPartCollection.Add(PEP4);
            fibQuestion.Content.Content += PEP4.PlaceHolder;

            fibQuestion.Content.Content += "，求n=";
            
            // 填空最后答案
            QuestionBlank blankA = new QuestionBlank();
            blankA.MatchOwnRefAnswer = true;
            QuestionContent blankContentA = new QuestionContent();
            blankContentA.Content = value3.ToString();
            blankContentA.ContentType = ContentType.Text;
            blankA.ReferenceAnswerList.Add(blankContentA);
            fibQuestion.Content.QuestionPartCollection.Add(blankA);
            fibQuestion.Content.Content += blankA.PlaceHolder;

            // 参考解题步骤及答案
            // Solution
            fibQuestion.Solution.Content += "底相等，指数也相等。";
            
            // 第一步
            fibQuestion.Solution.Content += "\n解：\n";
            fibQuestion.Solution.Content += "∵ ";
            fibQuestion.Solution.QuestionPartCollection.Add(PEP1);
            fibQuestion.Solution.Content += PEP1.PlaceHolder;
            fibQuestion.Solution.QuestionPartCollection.Add(PEP2);
            fibQuestion.Solution.Content += PEP2.PlaceHolder;
            fibQuestion.Solution.QuestionPartCollection.Add(PEP3);
            fibQuestion.Solution.Content += PEP3.PlaceHolder;
            fibQuestion.Solution.Content += "=";
            fibQuestion.Solution.QuestionPartCollection.Add(PEP4);
            fibQuestion.Solution.Content += PEP4.PlaceHolder;

            // 第二步
            fibQuestion.Solution.Content += "\n∴ ";
            QuestionPowerExponentPart PEP_Solu1 = new QuestionPowerExponentPart();
            PEP_Solu1.BaseNumber = PEP1.BaseNumber;
            decimal valueAnswer1 = value1 + value2;
            PEP_Solu1.Power.Content.Content = valueAnswer1.ToString() + "+" + "n";
            fibQuestion.Solution.QuestionPartCollection.Add(PEP_Solu1);
            fibQuestion.Solution.Content += PEP_Solu1.PlaceHolder;
            fibQuestion.Solution.Content += "=";
            fibQuestion.Solution.QuestionPartCollection.Add(PEP4);
            fibQuestion.Solution.Content += PEP4.PlaceHolder;

            // 第三步
            fibQuestion.Solution.Content += "\n∴ ";
            fibQuestion.Solution.Content += valueAnswer1.ToString() + "+" + "n";
            fibQuestion.Solution.Content += "=";
            fibQuestion.Solution.Content += valueFour.ToString();

            // 第四步
            fibQuestion.Solution.Content += "\n∴ ";
            fibQuestion.Solution.Content += "n=";
            fibQuestion.Solution.Content += value3.ToString();

            section.QuestionCollection.Add(fibQuestion);
        }
    }
}
