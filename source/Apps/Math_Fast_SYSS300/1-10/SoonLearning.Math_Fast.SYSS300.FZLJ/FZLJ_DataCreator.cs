using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.FZLJ
{
    public class FZLJDataCreator : DataCreator
    {
        private static FZLJDataCreator creator;

        public static FZLJDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new FZLJDataCreator();

                return creator;
            }
        }

        private List<int> questionValueList = new List<int>();
        private int currentIndex = 2;

        public struct valuesStruct
        {
            public decimal[] valuesRef; //基准值
            public decimal[] values; //所有加数的值
            public decimal[] valueList1; //各个加数的值
            public decimal[] valueList2; //对应的互补数
            public decimal[] errors; //各个加数跟基准数的差
            public bool[] signs; //正负号标志
            public int number; //加数的个数
            public decimal answer; //所有加数的和，即答案
            public int[] complementPos; //互为补数对的原下标
            public int[] complementList1Pos; //互为补数对的原下标
            public int[] complementList2Pos; //互为补数对的原下标

            public valuesStruct(int valueSize)
            {
                valuesRef = new decimal[valueSize];
                values = new decimal[valueSize];
                valueList1 = new decimal[valueSize];
                valueList2 = new decimal[valueSize];
                errors = new decimal[valueSize];
                signs = new bool[valueSize];
                complementPos = new int[valueSize];
                complementList1Pos = new int[valueSize];
                complementList2Pos = new int[valueSize];
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
            this.exerciseTitle = "分组连加法练习";
            this.examTitle = "分组连加法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.FZLJ.FZLJ_Document.xaml";
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                100,
                1000));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题：",
                "（在空格中填入符合条件的数）",
                5,
                100,
                1000));
        }

        protected override void AppendQuestion(SectionBaseInfo info, Section section)
        {
            switch (info.QuestionType)
            {
                case QuestionType.MultiChoice:
                    this.CreateMCQuestion(info, section);
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

        private void GetRandomValues(SectionBaseInfo info, ref valuesStruct valueOrg, ref valuesStruct valueABC)
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
            valueABC.number = rand.Next(4, 8 + 1);
            valueOrg.number = valueABC.number;
            ////题干数为偶数个
            //valueABC.number >>= 1;
            //valueABC.number <<= 1;

            int j = 0, i = 0;
            valueABC.answer = 0;
            int tmp;
            int iCom = 1;
            int iNum = 0, iRef = 0, iNumList1 = 0, iNumList2 = 0; ;

            decimal[] tmpValues = new decimal[valueABC.number];
            int[] tmpComplements = new int[valueABC.number];
            int tmpNumber = valueABC.number;

            //取两两互补数
            while (true)
            {
                valueOrg.values[iNum] = rand.Next(minValue, maxValue + 1);
                tmp = decimal.ToInt32(valueOrg.values[iNum]);
                tmpValues[iNum] = valueOrg.values[iNum];
                valueOrg.complementPos[iNum] = iNum;
                tmpComplements[iNum] = iNum;
                iNum++;
           
                if (iNum >= valueABC.number)
                    break;

                int complment = 0;

                iCom = 1;
                while (true)
                {
                    tmp /= 10;
                    if (tmp > 0 && tmp < 10)
                    {
                        break;
                    }
                    else
                    {
                        iCom++;
                    }
                }

                tmp = rand.Next(tmp + 1, 9 + 1);
                for (i = 0; i < iCom; i++)
                {
                    tmp *= 10;
                }

                //mplment = rand.Next(tmp, maxValue + 1);
                valueOrg.valuesRef[iRef++] = tmp;
                complment = tmp - decimal.ToInt32(valueOrg.values[iNum - 1]);
                valueOrg.values[iNum] = complment;
                tmpValues[iNum] = complment;
                valueOrg.complementPos[iNum] = iNum;
                tmpComplements[iNum] = iNum;
                iNum++;
                
                if (iNum >= valueABC.number)
                    break;
            }

            //随机存放取到的数
            //for (i = 0; i < valueABC.number / 2 + valueABC.number % 2; i++)
            //{
            //    valueABC.complementList1Pos[i] = rand.Next(0
            //   }
            
            //for (i = 0; i < valueABC.number / 2; i++)
            //{
            //}

            //complementList2Pos
            for (i = 0; i < valueABC.number; i++)
            {
                int tmpRand = rand.Next(0, tmpNumber--);
                valueABC.values[i] = tmpValues[tmpRand];
                valueABC.complementPos[i] = tmpComplements[tmpRand];           
                //valueABC.complementMatchPos[i] = tmpRand;//存下互补数对的位置(下坐标)
                for (j = tmpRand; j < valueABC.number - 1; j++)
                {
                    tmpValues[j] = tmpValues[j + 1];
                    tmpComplements[j] = tmpComplements[j + 1];
                }
                valueABC.answer += valueABC.values[i];
            }

            valueOrg.answer = valueABC.answer;
        }

        private String SolveSteps(valuesStruct valueOrg, valuesStruct valueMC)
        {
            valuesCompare valuesRank = new valuesCompare(10, 10, 10);
            decimal[] valuesTmp = new decimal[10];
            int[] iComplements = new int[10];
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

            //第一步
            decimal[] valuesPeek = new decimal[10];
            int iPeek = 0;
            decimal lastValue;
            int iNum = 0, iNum2 = 0, iTmp = 0;
            for (j = 0; j < valueMC.number; j++)
            {
                bool flagPeek = false;
                for (i = 0; i < iPeek; i++)
                {
                    if (valueMC.values[j] == valuesPeek[i])
                    {
                        flagPeek = true;
                        break;
                    }
                }

                if (flagPeek == true)
                {
                    continue;
                }

                //找到相对应的互补数
                iNum = j;
                iComplements[2 * iNum] = valueMC.complementPos[j];
                if (iComplements[2 * iNum] == valueMC.number - 1 && valueMC.number % 2 != 0) //最后一个数，且总共的数为奇数个
                {
                    lastValue = valueOrg.values[iComplements[2 * iNum]];
                    continue;
                }
                else
                {
                    if (iComplements[2 * iNum] % 2 == 0)
                    {
                        iComplements[2 * iNum + 1] = iComplements[2 * iNum] + 1;
                    }
                    else
                    {
                        iComplements[2 * iNum + 1] = iComplements[2 * iNum] - 1;
                    }
                }

                calSteps += "(";
                calSteps += valueMC.values[j].ToString(); //互补数A
                //valuesPeek[iPeek++] = valueMC.values[j];
                calSteps += "+";
                valuesTmp[iTmp++] = valueMC.values[j];

                calSteps += valueOrg.values[iComplements[2 * iNum + 1]].ToString(); //互补数A'
                valuesPeek[iPeek++] = valueOrg.values[iComplements[2 * iNum + 1]];
                calSteps += ")";
                valuesTmp[iTmp++] = valueOrg.values[iComplements[2 * iNum + 1]];

                iNum2 += 2;
                if (valueMC.number % 2 == 0)
                {
                    if (iNum2 < valueMC.number)
                    {
                        calSteps += "+";
                    }
                }
                else
                {
                    if (iNum2 < valueMC.number - 1)
                    {
                        calSteps += "+";
                    }
                }            
            }

            if (valueMC.number % 2 != 0)
            {
                calSteps += "+";
                calSteps += valueOrg.values[valueMC.number - 1];
                valuesTmp[iTmp++] = valueOrg.values[valueMC.number - 1];
            }

            //第二步
            calSteps += "=";
            for (j = 0; j < valueMC.number / 2; j++)
            {
                decimal _tmp = valuesTmp[2 * j] + valuesTmp[2 * j + 1];
                calSteps += _tmp.ToString();

                if (j != valueMC.number / 2 - 1)
                {
                    calSteps += "+";
                }
            }

            if (valueMC.number % 2 != 0)
            {
                calSteps += "+";
                calSteps += valuesTmp[valueMC.number - 1];
            }
            
            //第三步
            calSteps += "=";
            calSteps += valueMC.answer.ToString();

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

            return questionText;
        }

        private MCQuestion CreateMCQuestion(SectionBaseInfo info, Section section)
        {
            valuesStruct valueOrg = new valuesStruct(10); //原始产生的值
            valuesStruct valueMC = new valuesStruct(10); //题干展示的值

            //随即获得数值
            this.GetRandomValues(info, ref valueOrg, ref valueMC);

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
            string Steps = SolveSteps(valueOrg, valueMC);

            strBuilder.AppendLine(Steps);

            mcQuestion.Solution.Content = strBuilder.ToString();

            section.QuestionCollection.Add(mcQuestion);

            return mcQuestion;
        }

        private void CreateFIBQuestion(SectionBaseInfo info, Section section)
        {
            valuesStruct valueOrg = new valuesStruct(10); //原始产生的值
            valuesStruct valueMC = new valuesStruct(10); //题干展示的值

            //随即获得数值
            this.GetRandomValues(info, ref valueOrg, ref valueMC);

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
            string Steps = SolveSteps(valueOrg, valueMC);

            strBuilder.AppendLine(Steps);

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

        public FZLJDataCreator()
        {

        }
    }
}
