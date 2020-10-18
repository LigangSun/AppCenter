using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.ZLFS_39
{
    public class ZLFS_39DataCreator : DataCreator
    {
        private static ZLFS_39DataCreator creator;

        public static ZLFS_39DataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new ZLFS_39DataCreator();

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
            public int[] signs; //正负号标志，0为减号，1为加号，2为×号，3为÷号
            public int number; //加数的个数
            public decimal answer; //所有加数的和，即答案
            public int[] complementPos; //互为补数对的原下标
            public int[] complementList1Pos; //互为补数对的原下标
            public int[] complementList2Pos; //互为补数对的原下标
            public decimal valueStart; //第一个加数
            public decimal valueOffset; //等差值
            public decimal valueMedian; //中间值
            public decimal valueLast; //最后一个加数
            public decimal A; //被减数A
            public decimal A1; //被减数A除了最高位的其余部分
            public decimal B; //减数B
            public decimal B1; //减数B除了最高位的其余部分
            public int nBitsA; //A的位数n
            public int mode; //三项加减混合式，0是两个加，1是一加一减，2是一减一加，3是两个减


            public valuesStruct(int valueSize)
            {
                valuesRef = new decimal[valueSize];
                values = new decimal[valueSize];
                valueList1 = new decimal[valueSize];
                valueList2 = new decimal[valueSize];
                errors = new decimal[valueSize];
                signs = new int[valueSize];
                complementPos = new int[valueSize];
                complementList1Pos = new int[valueSize];
                complementList2Pos = new int[valueSize];
                number = 5;
                answer = 0;
                valueStart = 1;
                valueOffset = 1;
                valueMedian = 5;
                valueLast = 10;
                A = A1 = B = B1 = 0;
                nBitsA = 2;
                mode = 1;
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
            this.exerciseTitle = "整零分算法练习";
            this.examTitle = "整零分算法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.ZLFS_39.ZLFS_39_Document.xaml";
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                10,
                1000));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题：",
                "（在空格中填入符合条件的数）",
                5,
                10,
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

            if (minValue < 10)
            {
                minValue = 10;
            }

            if (maxValue < 100)
            {
                maxValue = 100;
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            valueABC.number = 3;// rand.Next(10, 100 + 1);
            valueABC.answer = 0;
            int mode = rand.Next(1, 4); //三项加减混合式，0是两个加，1是一加一减，2是一减一加，3是两个减
            valueABC.mode = mode;

            decimal A, B, C;
            int errorA, errorB, errorC;
            errorA = rand.Next(1, 10);
            errorB = rand.Next(1, 10);
            errorC = rand.Next(1, 10);

            if (mode == 0) //两个加号
            {
            }
            else if (mode == 1) //一加一减
            {
                int CH = rand.Next(1, 100);
                int BH = rand.Next(CH + 1, 100 + CH + 1);
                int AH = rand.Next(1, 100);
                A = AH * 10 + errorA;
                B = BH * 10 + errorB;
                C = CH * 10 + errorC;
                valueABC.values[0] = A;
                valueABC.values[1] = B;
                valueABC.values[2] = C;
                valueABC.answer = A + B - C;
            }
            else if (mode == 2) //一减一加
            {
                int CH = rand.Next(1, 100);
                int BH = rand.Next(1, 100);
                int AH = rand.Next(BH + 1, 100 + BH + 1);
                A = AH * 10 + errorA;
                B = BH * 10 + errorB;
                C = CH * 10 + errorC;
                valueABC.values[0] = A;
                valueABC.values[1] = B;
                valueABC.values[2] = C;
                valueABC.answer = A - B + C;                
            }
            else if (mode == 3) //两个减
            {
                int CH = rand.Next(1, 100);
                int BH = rand.Next(1, 100);
                int AH = rand.Next(BH + CH + 2, 100 + BH + CH + 2);
                A = AH * 10 + errorA;
                B = BH * 10 + errorB;
                C = CH * 10 + errorC;
                valueABC.values[0] = A;
                valueABC.values[1] = B;
                valueABC.values[2] = C;
                valueABC.answer = A - B - C;
            }
        }

        private String SolveSteps(valuesStruct valueMC)
        {
            string calSteps = "";
            int A = decimal.ToInt32(valueMC.values[0]);
            int B = decimal.ToInt32(valueMC.values[1]);
            int C = decimal.ToInt32(valueMC.values[2]);
            decimal answer = 0;
            int A_B_C;
            int errorA = A % 10;
            int errorB = B % 10;
            int errorC = C % 10;
            int errorSum = 0;
            int A1, B1, C1;

            if (errorA > 5)
            {
                A1 = A + 10 - errorA;
            }
            else
            {
                A1 = A - errorA;
            }

            if (errorB > 5)
            {
                B1 = B + 10 - errorB;
            }
            else
            {
                B1 = B - errorB;
            }

            if (errorC > 5)
            {
                C1 = C + 10 - errorC;
            }
            else
            {
                C1 = C - errorC;
            }

            calSteps += A.ToString();
            switch (valueMC.mode)
            {
                case 0: //两个加
                    break;
                case 1: //一加一减
                    //题干
                    errorSum = 0;
                    calSteps += "+";
                    calSteps += B.ToString();
                    calSteps += "-";
                    calSteps += C.ToString();
          
                    //第一步:
                    calSteps += "=";
                    calSteps += A1.ToString();
                    calSteps += "+";
                    calSteps += B1.ToString();
                    calSteps += "-";
                    calSteps += C1.ToString();
                    if (errorA > 5)
                    {
                        calSteps += "-";
                        int tmp = 10 - errorA;
                        calSteps += tmp.ToString();
                        errorSum += -(10 - errorA);
                    }
                    else
                    {
                        calSteps += "+";
                        calSteps += errorA.ToString();
                        errorSum += errorA;
                    }

                    if (errorB > 5)
                    {
                        calSteps += "-";
                        int tmp = 10 - errorB;
                        calSteps += tmp.ToString();
                        errorSum += -(10 - errorB);
                    }
                    else
                    {
                        calSteps += "+";
                        calSteps += errorB.ToString();
                        errorSum += errorB;
                    }

                    if (errorC > 5)
                    {
                        calSteps += "+";
                        int tmp = 10 - errorC;
                        calSteps += tmp.ToString();
                        errorSum += (10 - errorC);
                    }
                    else
                    {
                        calSteps += "-";
                        calSteps += errorC.ToString();
                        errorSum -= errorC;
                    }

                    //第二步：
                    A_B_C = A1 + B1 - C1;
                    calSteps += "=";
                    calSteps += A_B_C.ToString();
                    if (errorSum > 0)
                    {
                        calSteps += "+";
                        calSteps += errorSum.ToString();
                    }
                    else if (errorSum < 0)
                    {
                        //calSteps += "-";
                        calSteps += errorSum.ToString();
                    }

                    answer = A_B_C + errorSum;
                    break;
                case 2: //一减一加
                    //题干
                    errorSum = 0;
                    calSteps += "-";
                    calSteps += B.ToString();
                    calSteps += "+";
                    calSteps += C.ToString();
          
                    //第一步:
                    calSteps += "=";
                    calSteps += A1.ToString();
                    calSteps += "-";
                    calSteps += B1.ToString();
                    calSteps += "+";
                    calSteps += C1.ToString();

                    if (errorA > 5)
                    {
                        calSteps += "-";
                        int tmp = 10 - errorA;
                        calSteps += tmp.ToString();
                        errorSum += -(10 - errorA);
                    }
                    else
                    {
                        calSteps += "+";
                        calSteps += errorA.ToString();
                        errorSum += errorA;
                    }

                    if (errorB > 5)
                    {
                        calSteps += "+";
                        int tmp = 10 - errorB;
                        calSteps += tmp.ToString();
                        errorSum += (10 - errorB);
                    }
                    else
                    {
                        calSteps += "-";
                        calSteps += errorB.ToString();
                        errorSum -= errorB;
                    }

                    if (errorC > 5)
                    {
                        calSteps += "-";
                        int tmp = 10 - errorC;
                        calSteps += tmp.ToString();
                        errorSum += -(10 - errorC);
                    }
                    else
                    {
                        calSteps += "+";
                        calSteps += errorC.ToString();
                        errorSum += errorC;
                    }

                    //第二步：
                    A_B_C = A1 - B1 + C1;
                    calSteps += "=";
                    calSteps += A_B_C.ToString();
                    if (errorSum > 0)
                    {
                        calSteps += "+";
                        calSteps += errorSum.ToString();
                    }
                    else if (errorSum < 0)
                    {
                        //calSteps += "-";
                        calSteps += errorSum.ToString();
                    }

                    answer = A_B_C + errorSum;
                    break;
                case 3: //两个减
                    errorSum = 0;
                    calSteps += "-";
                    calSteps += B.ToString();
                    calSteps += "-";
                    calSteps += C.ToString();
          
                    //第一步:
                    calSteps += "=";
                    calSteps += A1.ToString();
                    calSteps += "-";
                    calSteps += B1.ToString();
                    calSteps += "-";
                    calSteps += C1.ToString();
                    if (errorA > 5)
                    {
                        calSteps += "-";
                        int tmp = 10 - errorA;
                        calSteps += tmp.ToString();
                        errorSum += -(10 - errorA);
                    }
                    else
                    {
                        calSteps += "+";
                        calSteps += errorA.ToString();
                        errorSum += errorA;
                    }

                    if (errorB > 5)
                    {
                        calSteps += "+";
                        int tmp = 10 - errorB;
                        calSteps += tmp.ToString();
                        errorSum += (10 - errorB);
                    }
                    else
                    {
                        calSteps += "-";
                        calSteps += errorB.ToString();
                        errorSum = errorB;
                    }

                    if (errorC > 5)
                    {
                        calSteps += "+";
                        int tmp = 10 - errorC;
                        calSteps += tmp.ToString();
                        errorSum += (10 - errorC);
                    }
                    else
                    {
                        calSteps += "-";
                        calSteps += errorC.ToString();
                        errorSum -= errorC;
                    }

                    //第二步：
                    A_B_C = A1 - B1 - C1;
                    calSteps += "=";
                    calSteps += A_B_C.ToString();
                    if (errorSum > 0)
                    {
                        calSteps += "+";
                        calSteps += errorSum.ToString();
                    }
                    else if (errorSum < 0)
                    {
                        //calSteps += "-";
                        calSteps += errorSum.ToString();
                    }

                    answer = A_B_C + errorSum;     
                    break;
                default:
                     break;
            }
            
             //第四步     
            if (answer != valueMC.answer)
            {
                calSteps += "Error!";
            }

            calSteps += "=";
            calSteps += valueMC.answer.ToString();
            calSteps += ",是正确答案。";

            return calSteps;
        }

        private String QuestionText(valuesStruct valueMC)
        {
            string questionText = "";
        
            questionText += valueMC.values[0].ToString();
            switch (valueMC.mode)
            {
                case 0: //两个加
                    questionText += "+";
                    questionText += valueMC.values[1].ToString();
                    questionText += "+";
                    questionText += valueMC.values[2].ToString();
                    break;
                case 1: //一加一减
                    questionText += "+";
                    questionText += valueMC.values[1].ToString();
                    questionText += "-";
                    questionText += valueMC.values[2].ToString();
                    break;
                case 2: //一减一加
                    questionText += "-";
                    questionText += valueMC.values[1].ToString();
                    questionText += "+";
                    questionText += valueMC.values[2].ToString();
                    break;
                case 3: //两个减
                    questionText += "-";
                    questionText += valueMC.values[1].ToString();
                    questionText += "-";
                    questionText += valueMC.values[2].ToString();
                    break;
                default:
                    questionText += "+";
                    questionText += valueMC.values[1].ToString();
                    questionText += "+";
                    questionText += valueMC.values[2].ToString();
                    break;
            }
            questionText += "=";
      
            return questionText;
        }

        private MCQuestion CreateMCQuestion(SectionBaseInfo info, Section section)
        {
            valuesStruct valueMC = new valuesStruct(20); //题干展示的值

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
            valuesStruct valueMC = new valuesStruct(20); //题干展示的值

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

        private string CreateSolution(int divValue)
        {
            if (divValue == 2)
            {
                return "";
            }

            return string.Empty;
        }

        public ZLFS_39DataCreator()
        {

        }
    }
}
