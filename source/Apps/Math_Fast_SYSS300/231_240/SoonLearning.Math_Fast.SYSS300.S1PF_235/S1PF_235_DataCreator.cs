using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.S1PF_235
{
    public class S1PF_235DataCreator : DataCreator
    {
        private static S1PF_235DataCreator creator;

        public static S1PF_235DataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new S1PF_235DataCreator();

                return creator;
            }
        }

        private List<int> questionValueList = new List<int>();
        private int currentIndex = 2;

        public struct valuesStruct
        {
            public decimal[] valuesRef; //基准值
            public decimal[] values; //所有乘数的值
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
            public int baseNum;

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
                baseNum = 0;
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
            this.exerciseTitle = "首1平方法练习";
            this.examTitle = "首1平方法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.S1PF_235.S1PF_235_Document.xaml";
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

            if (maxValue > 100)
            {
                maxValue = 100;
            }

            if (maxValue < 100)
            {
                maxValue = 100;
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            valueABC.number = 2;// rand.Next(3, 6);
            valueABC.answer = 0;

            //decimal A1, A2 = 0;

            //int times = rand.Next(1, 10);
            int d1 = rand.Next(1, 9); //千位数;
            int a1 = rand.Next(d1, 10); //百位数
            int b1 = a1;// 9;// rand.Next(0, 3); //十位数
            int c1 = a1 - d1;// rand.Next(5, 10); //个位数
            int d2 = 0;
            int a2 = rand.Next(4, 10); //百位数
            int b2 = rand.Next(7, 10); //十位数
            int c2 = rand.Next(1, 10); //个位数

            decimal A1 = 0, A2 = 0, A3 = 0, A4 = 0;
            int posRand = 0;// rand.Next(0, 2);
            valueABC.posMul = posRand;
            int n = rand.Next(2, 11);
            int baseNum = rand.Next(1, maxValue / 10);// a1 * 100 + b1 * 10 + c1;
            valueABC.baseNum = n;// a1 * 100 + b1 * 10 + c1;
            
            if (posRand == 0)
            {
                A1 = rand.Next(11, 20);
                A2 = A1;// rand.Next(10, 100); ;// *(rand.Next(minValue + 1, maxValue + 1) / 5);// (decimal)System.Math.Pow(5, n);// 1000 * d2 + 100 * a2 + 10 * b2 + c2;
                valueABC.values[0] = A1;
                valueABC.values[1] = A2;
                valueABC.answer = A1 * (A2);
            }
            else
            {
                A2 = rand.Next(10, 100); ;// *(rand.Next(minValue + 1, maxValue + 1) / 5);// (decimal)System.Math.Pow(5, n);// 1000 * d2 + 100 * a2 + 10 * b2 + c2;
                A1 = A2 * (100 - rand.Next(1, 3));
                valueABC.values[0] = A1;
                valueABC.values[1] = A2;
                valueABC.answer = A1 * (A2);
            }
        }

        private String SolveSteps(valuesStruct valueMC)
        {
            string calSteps = "";
            //题干
            int num = valueMC.number;
            int i, j;
            int sign = valueMC.posMul;

            //for (i = 0; i < num - 1; i++)
            {
                calSteps += valueMC.values[0].ToString();
                calSteps += "×";
                calSteps += valueMC.values[1].ToString();
            }
      
            //解题步骤
            int A1, A2, A3, A4;
            int n = valueMC.baseNum;
            if (valueMC.posMul == 0)
            {
                A1 = decimal.ToInt32(valueMC.values[0]);
                A2 = decimal.ToInt32(valueMC.values[1]);
                A3 = decimal.ToInt32(valueMC.values[2]);
                A4 = decimal.ToInt32(valueMC.values[3]);
            }
            else
            {
                A1 = decimal.ToInt32(valueMC.values[0]);
                A2 = decimal.ToInt32(valueMC.values[1]);
                A3 = decimal.ToInt32(valueMC.values[2]);
                A4 = decimal.ToInt32(valueMC.values[3]);
            }

            int baseNum = 1000;
            int a1 = (A1 % baseNum) / 100;
            int b1 = ((A1 % baseNum) % 100) / 10;
            int c1 = ((A1 % baseNum) % 100) % 10;
            int a2 = (A2 % baseNum) / 100;
            int b2 = ((A2 % baseNum) % 100) / 10;
            int c2 = ((A2 % baseNum) % 100) % 10;
            int d1 = A1 / 1000;
            int d2 = A2 / 1000;

            int baseNum2 = 100;
            int a = A1 % 10;
            
            //第一步            
            calSteps += "=100+";
            calSteps += a.ToString();
            calSteps += "×";       
            calSteps += a.ToString();
            calSteps += "+20×";
            calSteps += a.ToString();
           
            //第二步 
            int aa = a * a;
            int a20 = 20 * a;
            calSteps += "=100+";
            calSteps += aa.ToString();
            calSteps += "+";
            calSteps += a20.ToString();           
         
            decimal answer = 0;
            
            int x1 = 100 + aa;
            calSteps += "=";
            calSteps += x1.ToString();
            calSteps += "+";
            calSteps += a20.ToString();

            answer = x1 + a20;
           
            ////第三步 
            //int x23 = x1 + b1;
            //int x4 = x2 / A2;  
            //calSteps += "=";
            //calSteps += x23.ToString();
            //calSteps += "+";
            //calSteps += x4.ToString();
     
            //int x5 = (x23 + x4 * a) / 100;
            //calSteps += "=";
            //calSteps += x1.ToString();
            //calSteps += "+";
            //calSteps += x5.ToString();
               
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

            int num = valueMC.number;
            int i;
            int sign = valueMC.posMul;

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

        public S1PF_235DataCreator()
        {

        }
    }
}
