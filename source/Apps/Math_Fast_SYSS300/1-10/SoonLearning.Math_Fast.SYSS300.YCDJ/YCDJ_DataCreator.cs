using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.YCDJ
{
    public class YCDJDataCreator : DataCreator
    {
        private static YCDJDataCreator creator;

        public static YCDJDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new YCDJDataCreator();

                return creator;
            }
        }

        private List<int> questionValueList = new List<int>();
        private int currentIndex = 2;

        public struct valuesStruct
        {
            public decimal valueRef; //基准值
            public decimal[] values; //各个加数的值
            public decimal[] errors; //各个加数跟基准数的差
            public bool[] signs; //正负号标志
            public int number; //加数的个数
            public decimal answer; //所有加数的和，即答案

            public valuesStruct(int valueSize, int errorSize, int signSize)
            {
                valueRef = 50;
                values = new decimal[valueSize];
                errors = new decimal[errorSize];
                signs = new bool[signSize];
                number = 5;
                answer = 0;
            }            
        }

        public struct valuesCompare
        {
            public decimal valueRef; //基准值
            public decimal[] valuesGT; //大于基准值
            public decimal numGT; //大于基准值的个数
            public decimal[] valuesLT; //小于基准值
            public decimal numLT; //小于基准值的个数
            public decimal[] valuesEQ; //等于基准值
            public decimal numEQ; //等于基准值的个数
 
            public valuesCompare(int GTSize, int LTSize, int EQSize)
            {
                valueRef = 50;
                valuesGT = new decimal[GTSize];
                valuesLT = new decimal[LTSize];
                valuesEQ = new decimal[EQSize];
                numGT = 0;
                numLT = 0;
                numEQ = 0;
            }            
        }
        
        protected override void PrepareSectionInfoCollection()
        {
            this.exerciseTitle = "以乘代加法练习";
            this.examTitle = "以乘代加法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.YCDJ.YCDJ_Document.xaml";
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                10,
                100));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题：",
                "（在空格中填入符合条件的数）",
                5,
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
                case QuestionType.Table:
                    this.CreateTableQuestion(info, section);
                    break;
                case QuestionType.FillInBlank:
                    this.CreateFIBQuestion(info, section);
                    break;
            }
        }

        private void GetRandomValues1(SectionBaseInfo info, 
            ref int value1, ref int value2, ref int value3, ref int value4, ref int value5, ref int value6, ref int value7, ref int num, ref int valueAnswer)
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
            num = rand.Next(3, 7 + 1);
            int error;
            int j = 0;
            int sign;
            valueAnswer = 0;

            while (true)
            {
                if (j >= num)
                    break;
                value1 = rand.Next(minValue, maxValue + 1);
                error = rand.Next(0, 3);
                sign = rand.Next(0, 2);
                if (sign == 0)
                {
                    value1 += error;
                }
                else
                {
                    value1 -= error;
                }
                valueAnswer += value1;
               j++;

               if (j >= num)
                   break;
               value2 = value1;
               error = rand.Next(0, 3);
               sign = rand.Next(0, 2);
               if (sign == 0)
               {
                   value2 += error;
               }
               else
               {
                   value2 -= error;
               }
               valueAnswer += value2;
               j++;


               if (j >= num)
                   break;
               value3 = value1;
               error = rand.Next(0, 3);
               sign = rand.Next(0, 2);
               if (sign == 0)
               {
                   value3 += error;
               }
               else
               {
                   value3 -= error;
               }
               valueAnswer += value3;
               j++;


               if (j >= num)
                   break;
               value4 = value1;
               error = rand.Next(0, 3);
               sign = rand.Next(0, 2);
               if (sign == 0)
               {
                   value4 += error;
               }
               else
               {
                   value4 -= error;
               }
               valueAnswer += value4;
               j++;


               if (j >= num)
                   break;
               value5 = value1;
               error = rand.Next(0, 3);
               sign = rand.Next(0, 2);
               if (sign == 0)
               {
                   value5 += error;
               }
               else
               {
                   value5 -= error;
               }
               valueAnswer += value5;
               j++;


               if (j >= num)
                   break;
               value6 = value1;
               error = rand.Next(0, 3);
               sign = rand.Next(0, 2);
               if (sign == 0)
               {
                   value6 += error;
               }
               else
               {
                   value6 -= error;
               }
               valueAnswer += value6;
               j++;


               if (j >= num)
                   break;
               value7 = value1;
               error = rand.Next(0, 3);
               sign = rand.Next(0, 2);
               if (sign == 0)
               {
                   value7 += error;
               }
               else
               {
                   value7 -= error;
               }
               valueAnswer += value7;
               j++;
            }
        }

        private void GetRandomValues(SectionBaseInfo info, ref valuesStruct valueABC)
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
            valueABC.number = rand.Next(3, 7 + 1);
            
            int j = 0;
            int sign = 0;
            valueABC.answer = 0;
            valueABC.valueRef = rand.Next(minValue, maxValue + 1);

            while (true)
            {
                if (j >= valueABC.number)
                    break;
                valueABC.values[j] = valueABC.valueRef;
                valueABC.errors[j] = rand.Next(0, 4);
                sign = rand.Next(0, 2);
                if (sign == 0) //负号
                {
                    valueABC.signs[j] = false;
                    valueABC.values[j] -= valueABC.errors[j];
                }
                else //正号
                {
                    valueABC.signs[j] = true;
                    valueABC.values[j] += valueABC.errors[j];
                }
                valueABC.answer += valueABC.values[j];
                j++;
            }
        }

        private String SolveSteps(valuesStruct valueMC)
        {
            valuesCompare valuesRank = new valuesCompare(10, 10, 10);
            decimal[] valuesTmp = new decimal[10];
            int i, j;

            //题干
            valuesTmp[0] = valueMC.values[0];
            string calSteps = valueMC.values[0].ToString();
            for (i = 1; i < valueMC.number; i++)
            {
                calSteps += "+";
                calSteps += valueMC.values[i].ToString();
                valuesTmp[i] = valueMC.values[i];
            }
            calSteps += "=";

            //解题步骤
            decimal sumValues = 0;
            for (i = 0; i < valueMC.number; i++)
            {
                sumValues += valueMC.values[i];
            }
            int valueRefInt = (decimal.ToInt32(sumValues) + decimal.ToInt32(valueMC.number) / 2)
                    / decimal.ToInt32(valueMC.number);
            valuesRank.valueRef = valueRefInt;

            //valuesRank.valueRef = valueMC.valueRef;
            //int iGT = 0, iEQ = 0, iLT = 0;
            for (i = 0; i < valueMC.number; i++)
            {
                if (valueMC.values[i] > valuesRank.valueRef)
                {
                    valuesRank.valuesGT[decimal.ToInt32(valuesRank.numGT++)] = valueMC.values[i];
                }
                else if (valueMC.values[i] == valuesRank.valueRef)
                {
                    valuesRank.valuesEQ[decimal.ToInt32(valuesRank.numEQ++)] = valueMC.values[i];
                }
                else
                {
                    valuesRank.valuesLT[decimal.ToInt32(valuesRank.numLT++)] = valueMC.values[i];
                }
            }

            //第一步
            if (valueMC.number != valuesRank.numEQ)
            {
                for (j = 0; j < valueMC.number; j++)
                {
                    calSteps += valuesRank.valueRef.ToString();
                    if (valueMC.values[j] < valuesRank.valueRef)
                    {
                        calSteps += "-";
                        decimal error = valuesRank.valueRef - valueMC.values[j];
                        calSteps += error.ToString();
                    }
                    else if (valueMC.values[j] > valuesRank.valueRef)
                    {
                        calSteps += "+";
                        decimal error = valueMC.values[j] - valuesRank.valueRef;
                        calSteps += error.ToString();
                    }
                    if (j != valueMC.number - 1)
                    {
                        calSteps += "+";
                    }
                }
            }

            //第二步
            calSteps += "=";
            calSteps += valuesRank.valueRef.ToString();
            calSteps += "×";
            calSteps += valueMC.number.ToString();

            if (valuesRank.numGT != 0)
            {
                calSteps += "+(";
            }

            decimal tmp;
            for (j = 0; j < valuesRank.numGT; j++)
            {
                tmp = (valuesRank.valuesGT[j] - valuesRank.valueRef);
                calSteps += tmp.ToString();
                if (j != valuesRank.numGT - 1)
                {
                    calSteps += "+";
                }
            }

            for (j = 0; j < valuesRank.numLT; j++)
            {
                calSteps += "-";
                tmp = (valuesRank.valueRef - valuesRank.valuesLT[j]);
                calSteps += tmp.ToString();
            }

            if (valuesRank.numGT != 0)
            {
                calSteps += ")";
            }

            //第三步
            calSteps += "=";
            tmp = valuesRank.valueRef * valueMC.number;
            calSteps += tmp.ToString();

            if (tmp > valueMC.answer) //还有最后一步
            {
                calSteps += "-";
                tmp = tmp - valueMC.answer;
                calSteps += tmp.ToString();

                //第四步
                calSteps += "=";
                calSteps += valueMC.answer.ToString();
            }
            else if (tmp < valueMC.answer)
            {
                calSteps += "+";
                tmp = valueMC.answer - tmp;
                calSteps += tmp.ToString();

                //第四步
                calSteps += "=";
                calSteps += valueMC.answer.ToString();
            }

            calSteps += ",是正确答案。";

            return calSteps;
        }

        private String QuestionText(valuesStruct valueMC)
        {
            string questionText = "";
            for (int i = 0; i < valueMC.number; i++)
            {
                questionText += valueMC.values[i].ToString();

                if (i != valueMC.number - 1)
                {
                    questionText += "+";
                }
                else
                {
                    questionText += "=";
                }
            }
            //switch (valueMC.number)
            //{
            //    case 3:
            //        questionText = string.Format("{0}+{1}+{2}=", valueMC.values[0], valueMC.values[1], valueMC.values[2]);
            //        break;
            //    case 4:
            //        questionText = string.Format("{0}+{1}+{2}+{3}=", valueMC.values[0], valueMC.values[1], valueMC.values[2], valueMC.values[3]);
            //        break;
            //    case 5:
            //        questionText = string.Format("{0}+{1}+{2}+{3}+{4}=", valueMC.values[0], valueMC.values[1], valueMC.values[2], valueMC.values[3], valueMC.values[4]);
            //        break;
            //    case 6:
            //        questionText = string.Format("{0}+{1}+{2}+{3}+{4}+{5}=", valueMC.values[0], valueMC.values[1], valueMC.values[2], valueMC.values[3], valueMC.values[4], valueMC.values[5]);
            //        break;
            //    case 7:
            //        questionText = string.Format("{0}+{1}+{2}+{3}+{4}+{5}+{6}=", valueMC.values[0], valueMC.values[1], valueMC.values[2], valueMC.values[3], valueMC.values[4], valueMC.values[5], valueMC.values[6]);
            //        break;
            //    default:
            //        break;
            // }

            return questionText;
        }

        private MCQuestion CreateMCQuestion(SectionBaseInfo info, Section section)
        {
            valuesStruct valueMC = new valuesStruct(10, 10, 10);
                        
            //随即获得数值
            this.GetRandomValues(info, ref valueMC);

            questionValueList.Add((decimal.ToInt32(valueMC.values[0])));

            //生产题干文本
            string questionText = QuestionText(valueMC);

            MCQuestion mcQuestion = ObjectCreator.CreateMCQuestion((content) =>
            {
                content.Content = questionText;
                content.ContentType = ContentType.Text;
                return;
            },
            () =>
            {
                List<QuestionOption> optionList = new List<QuestionOption>();

                int valueLst = (decimal.ToInt32(valueMC.answer)) - 30, valueMst = (decimal.ToInt32(valueMC.answer)) + 30;
                if (valueLst - 30 <= 1)
                {
                    valueLst = 1;
                }

                foreach (QuestionOption option in ObjectCreator.CreateDecimalOptions(
                            4, valueLst, valueMst, false, (c => (c == decimal.ToInt32(valueMC.answer))), decimal.ToInt32(valueMC.answer)))
                    optionList.Add(option);

                return optionList;
            }
            );

            StringBuilder strBuilder = new StringBuilder();

            //解题步骤
            string Steps = SolveSteps(valueMC);
 
            strBuilder.AppendLine(Steps);                

            mcQuestion.Solution.Content = strBuilder.ToString();

            section.QuestionCollection.Add(mcQuestion);

            return mcQuestion;
        }

        private void CreateFIBQuestion(SectionBaseInfo info, Section section)
        {
            valuesStruct valueMC = new valuesStruct(10, 10, 10);

            //随即获得数值
            this.GetRandomValues(info, ref valueMC);

            //生产题干文本
            string questionText = QuestionText(valueMC);

            FIBQuestion fibQuestion = new FIBQuestion();
            fibQuestion.Content.Content = questionText;
            fibQuestion.Content.ContentType = ContentType.Text;
            section.QuestionCollection.Add(fibQuestion);

            QuestionBlank blank = new QuestionBlank();

            QuestionContent blankContent = new QuestionContent();
            blankContent.Content = valueMC.answer.ToString();
            blankContent.ContentType = ContentType.Text;
            blank.ReferenceAnswerList.Add(blankContent);

            fibQuestion.QuestionBlankCollection.Add(blank);

            fibQuestion.Content.Content += blank.PlaceHolder;

            StringBuilder strBuilder = new StringBuilder();

            //解题步骤
            string Steps = SolveSteps(valueMC);

            strBuilder.AppendLine(Steps);  

            fibQuestion.Solution.Content = strBuilder.ToString();
        }

        private void CreateTableQuestion(SectionBaseInfo info, Section section)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            if (section.QuestionCollection.Count == 0)
                this.currentIndex = 2;

            int divValue = this.currentIndex;
            string questionText = string.Format("请下表中选出能被{0}以乘代加法的数。", divValue);

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

        private string CreateSolution(int divValue)
        {
            if (divValue == 2)
            {
                return "";
            }

            return string.Empty;
        }

        public YCDJDataCreator()
        {

        }
    }
}
