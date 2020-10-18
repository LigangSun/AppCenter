using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.JYQB_32
{
    public class JYQB_32DataCreator : DataCreator
    {
        private static JYQB_32DataCreator creator;

        public static JYQB_32DataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new JYQB_32DataCreator();

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
            public decimal valueStart; //第一个加数
            public decimal valueOffset; //等差值
            public decimal valueMedian; //中间值
            public decimal valueLast; //最后一个加数
            public decimal A; //被减数A
            public decimal A1; //被减数A除了最高位的其余部分
            public decimal B; //减数B
            public decimal B1; //减数B除了最高位的其余部分
            public int nBitsA; //A的位数n


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
                valueStart = 1;
                valueOffset = 1;
                valueMedian = 5;
                valueLast = 10;
                A = A1 = B = B1 = 0;
                nBitsA = 2;
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
            this.exerciseTitle = "简易求补法练习";
            this.examTitle = "简易求补法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.JYQB_32.JYQB_32_Document.xaml";
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
            valueABC.number = 1;// rand.Next(10, 100 + 1);
            valueABC.answer = 0;

            int bits = 1;
            decimal A, B;

            while (true)
            {
                int tmpMaxValue1 = rand.Next(minValue, maxValue + 1);
                int tmpMaxValue = tmpMaxValue1;

                //计算几位数运算
                while (true)
                {
                    if (tmpMaxValue >= 10)
                    {
                        bits++;
                        tmpMaxValue /= 10;
                    }
                    else
                    {
                        break;
                    }
                }

                if (tmpMaxValue1 != 100 && tmpMaxValue1 != 1000 && tmpMaxValue1 != 10000 & tmpMaxValue1 != 100000)
                {
                    A = tmpMaxValue1;
                    break;
                }
            }

            double b = Math.Pow(10, bits);
            B = (int)b - A;
       
            //把被减数A和减数B存入数组
            valueABC.values[0] = A;
            valueABC.values[1] = B;            
            valueABC.answer = B;
            valueABC.nBitsA = bits;
        }

        private String SolveSteps(valuesStruct valueMC)
        {       
            //解题步骤
            string calSteps = "因为 ";
            int n = valueMC.nBitsA;
            int A = decimal.ToInt32(valueMC.values[0]);
            int B = decimal.ToInt32(valueMC.values[1]);
            int answer = 0;

            //第一步
            if (A < 100)
            {
                int A1 = A % 10;
                int A2 = A / 10;
                int B1 = 10 - A1;
                int B2 = 9 - A2;
                answer = B2 * 10 + B1;

                calSteps += "10-";
                calSteps += A1.ToString();
                calSteps += "=";
                calSteps += B1.ToString();

                calSteps += "  9-";
                calSteps += A2.ToString();
                calSteps += "=";
                calSteps += B2.ToString();
            }
            else if (A < 1000)
            {
                int A1 = A % 10;
                int A2 = A / 10;
                A2 = A2 % 10;
                int A3 = A / 100;
                int B1 = 10 - A1;
                int B2 = 9 - A2;
                int B3 = 9 - A3;
                answer = B3 * 100 + B2 * 10 + B1;

                calSteps += "10-";
                calSteps += A1.ToString();
                calSteps += "=";
                calSteps += B1.ToString();

                calSteps += "  9-";
                calSteps += A2.ToString();
                calSteps += "=";
                calSteps += B2.ToString();
                
                calSteps += "  9-";
                calSteps += A3.ToString();
                calSteps += "=";
                calSteps += B3.ToString();
            }
            else if (A < 10000)
            {
                int A1 = A % 10;
                int A2 = A / 10;
                A2 = A2 % 10;
                int A3 = A / 100;
                A3 = A3 % 10;
                int A4 = A / 1000;
                int B1 = 10 - A1;
                int B2 = 9 - A2;
                int B3 = 9 - A3;
                int B4 = 9 - A4;
                answer = B4 * 1000 + B3 * 100 + B2 * 10 + B1;

                calSteps += "10-";
                calSteps += A1.ToString();
                calSteps += "=";
                calSteps += B1.ToString();

                calSteps += "  9-";
                calSteps += A2.ToString();
                calSteps += "=";
                calSteps += B2.ToString();

                calSteps += "  9-";
                calSteps += A3.ToString();
                calSteps += "=";
                calSteps += B3.ToString();

                calSteps += "  9-";
                calSteps += A4.ToString();
                calSteps += "=";
                calSteps += B4.ToString();
            }
            else if (A < 100000)
            {
                int A1 = A % 10;
                int A2 = A / 10;
                A2 = A2 % 10;
                int A3 = A / 100;
                A3 = A3 % 10;
                int A4 = A / 1000;
                A4 = A4 % 10;
                int A5 = A / 10000;
                int B1 = 10 - A1;
                int B2 = 9 - A2;
                int B3 = 9 - A3;
                int B4 = 9 - A4;
                int B5 = 9 - A4;
                answer = B5 * 10000 + B4 * 1000 + B3 * 100 + B2 * 10 + B1;

                calSteps += "10-";
                calSteps += A1.ToString();
                calSteps += "=";
                calSteps += B1.ToString();

                calSteps += "  9-";
                calSteps += A2.ToString();
                calSteps += "=";
                calSteps += B2.ToString();

                calSteps += "  9-";
                calSteps += A3.ToString();
                calSteps += "=";
                calSteps += B3.ToString();

                calSteps += "  9-";
                calSteps += A4.ToString();
                calSteps += "=";
                calSteps += B4.ToString();

                calSteps += "  9-";
                calSteps += A5.ToString();
                calSteps += "=";
                calSteps += B5.ToString();
            }
            else
            {
                int A1 = A % 10;
                int A2 = A / 10;
                A2 = A2 % 10;
                int A3 = A / 100;
                A3 = A3 % 10;
                int A4 = A / 1000;
                int B1 = 10 - A1;
                int B2 = 9 - A2;
                int B3 = 9 - A3;
                int B4 = 9 - A4;
                answer = B4 * 1000 + B3 * 100 + B2 * 10 + B1;

                calSteps += "10-";
                calSteps += A1.ToString();
                calSteps += "=";
                calSteps += B1.ToString();

                calSteps += "  9-";
                calSteps += A2.ToString();
                calSteps += "=";
                calSteps += B2.ToString();

                calSteps += "  9-";
                calSteps += A3.ToString();
                calSteps += "=";
                calSteps += B3.ToString();

                calSteps += "  9-";
                calSteps += A4.ToString();
                calSteps += "=";
                calSteps += B4.ToString();
            }
      
            //第三步     
            if (answer != valueMC.answer)
            {
                calSteps += "Error!";
            }

            calSteps += " 所以 ";
            calSteps += A.ToString();
            calSteps += "的补数是";
            calSteps += valueMC.answer.ToString();

            calSteps += ",是正确答案。";

            return calSteps;
        }

        private String QuestionText(valuesStruct valueMC)
        {
            string questionText = "求";
        
            questionText += valueMC.values[0].ToString();
            questionText += "的补数";
      
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

        public JYQB_32DataCreator()
        {

        }
    }
}
