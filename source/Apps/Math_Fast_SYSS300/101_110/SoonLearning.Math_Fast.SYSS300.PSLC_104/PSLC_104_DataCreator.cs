﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.PSLC_104
{
    public class PSLC_104DataCreator : DataCreator
    {
        private static PSLC_104DataCreator creator;

        public static PSLC_104DataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new PSLC_104DataCreator();

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
            public int posMul; //两个数相乘时，posMul=0表示values[0]为被乘数，values[1]为乘数；否则，反之。
            public int a; //乘数的分解因子1
            public int b; //乘数的分解因子2

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
                posMul = 0;
                a = b = 1;
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
            this.exerciseTitle = "破数连乘法练习";
            this.examTitle = "破数连乘法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.PSLC_104.PSLC_104_Document.xaml";
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

            //if (minValue < 1000)
            {
                minValue = 100;
            }

            //if (maxValue < 10000)
            {
                maxValue = 1000;
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            valueABC.number = 2;
            valueABC.answer = 0;

            decimal A1, A2;

            int bitNum = 2;// rand.Next(3, 5);
            int a1 = 0;// rand.Next(1, 10); //百位数
            int b1 = rand.Next(1, 10); //十位数
            int c1 = rand.Next(0, 10); //个位数
            int a2 = 0;// rand.Next(1, 10); //百位数
            int b2 = rand.Next(1, 10); //十位数
            int c2 = rand.Next(1, 10); //个位数
            int d1 = 0;
            int d2 = 0;

            int posRand = rand.Next(0, 2);
            valueABC.posMul = posRand;
                        
            if (posRand == 0)
            {
                A1 = 100 * a1 + 10 * b1 + c1;
                A2 = b2 * c2;// 100 * a2 + 10 * b2 + c2;
            }
            else
            {
                A2 = 100 * a1 + 10 * b1 + c1;
                A1 = b2 * c2;// 100 * a2 + 10 * b2 + c2;
            }

            valueABC.a = b2;
            valueABC.b = c2;

            //if (b1 >= b2)
            //{
            //    b1 = b2 + 1;
            //}
            //else
            //{
            //    b2 = b1 + 1;
            //}

            //if (bitNum == 2) //
            //{
            //    A1 = 100 * a1 + 10 * b1 + c1;
            //    A2 = 100 * a2 + 10 * b2 + c2;
            //}
            //else if (bitNum == 3) //
            //{
            //    A1 = 100 * a1 + 10 * b1 + c1;
            //    A2 = 100 * a2 + 10 * b2 + c2;
            //}
            //else
            //{
            //    A1 = 1000 * a1 + 100 * b1 + 10 * c1 + d1;
            //    A2 = 1000 * a2 + 100 * b2 + 10 * c2 + d2;
            //}

            //int posRand = rand.Next(0, 2);
            //if (posRand == 0)
            //{
            //    A = 10 * a + b;
            //    B = 10 * c + d;
            //}
            //else
            //{
            //    B = 10 * a + b;
            //    A = 10 * c + d;
            //}

            valueABC.values[0] = A1;
            valueABC.values[1] = A2;
            valueABC.answer = A1 * A2;
        }

        private String SolveSteps(valuesStruct valueMC)
        {
            string calSteps = "";
            //题干
            calSteps += valueMC.values[0].ToString();
            calSteps += "×";
            calSteps += valueMC.values[1].ToString();

            //解题步骤
            int A1 = decimal.ToInt32(valueMC.values[0]);
            int A2 = decimal.ToInt32(valueMC.values[1]);

            if (valueMC.posMul == 1)
            {
                A1 = decimal.ToInt32(valueMC.values[1]);
                A2 = decimal.ToInt32(valueMC.values[0]);
            }

            int a = valueMC.a;
            int b = valueMC.b;

            int baseNum1 = 100;
            int baseNum2 = 10;

            int a1 = A1 / baseNum1;
            int b1 = (A1 / baseNum2) % 10;
            int c1 = A1 % 10;
            int a2 = A2 / baseNum1;
            int b2 = (A2 / baseNum2) % 10;
            int c2 = A2 % 10;
            decimal answer = 0;

            int c1_10 = 10 - c1;
            int c2_10 = 10 - c2;

            //if (A1 >= 1000 && A2 >= 1000)
            //{
            //    baseNum = 1000;
            //}

            //第一步            
            calSteps += "=";
            calSteps += A1.ToString();
            calSteps += "×";
            calSteps += a.ToString();
            calSteps += "×";
            //calSteps += A1.ToString();
            //calSteps += "+1)-(";
            calSteps += b.ToString();
            //calSteps += "+1)+1";
           
            //第二步
            int A11 = A1 * a;
            calSteps += "=";
            calSteps += A11.ToString();
            calSteps += "×";
            calSteps += b.ToString();
         
            //第三步        
          

            //第四步
            int tmp0 = A11 * b;
            int tmp1 = 0;
            int tmp2 = 0;
            //
            answer = tmp0 + tmp1 + tmp2;
 
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
            questionText += "×";
            questionText += valueMC.values[1].ToString();
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

                int valueLst = (decimal.ToInt32(valueMC.answer)) - 50, valueMst = (decimal.ToInt32(valueMC.answer)) + 50;
                if (valueLst - 50 <= 1)
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

        public PSLC_104DataCreator()
        {

        }
    }
}
