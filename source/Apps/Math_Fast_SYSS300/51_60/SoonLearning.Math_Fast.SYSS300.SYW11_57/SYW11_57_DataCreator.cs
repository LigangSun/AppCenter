using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.SYW11_57
{
    public class SYW11_57DataCreator : DataCreator
    {
        private static SYW11_57DataCreator creator;

        public static SYW11_57DataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new SYW11_57DataCreator();

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
            this.exerciseTitle = "首异尾1法（一）练习";
            this.examTitle = "首异尾1法（一）测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.SYW11_57.SYW11_57_Document.xaml";
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

            //if (minValue < 1000)
            {
                minValue = 10;
            }

            //if (maxValue < 10000)
            {
                maxValue = 100;
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            valueABC.number = 2;
            valueABC.answer = 0;

            decimal A, B;
            int a = rand.Next(1, 10);
            int b = rand.Next(1, 10);
            
            A = 10 * a + 1;
            B = 10 * b + 1;
  
            valueABC.values[0] = A;
            valueABC.values[1] = B;
            valueABC.answer = A * B;      
        }

        private String SolveSteps(valuesStruct valueMC)
        {
            string calSteps = "";
            //题干
            calSteps += valueMC.values[0].ToString();
            calSteps += "×";
            calSteps += valueMC.values[1].ToString();

            //解题步骤
            int A = decimal.ToInt32(valueMC.values[0]);
            int B = decimal.ToInt32(valueMC.values[1]);
            int a = A / 10;
            int b = B / 10;
            decimal answer = 0;

            //第一步
            calSteps += "=100×";
            calSteps += a.ToString();
            calSteps += "×";
            calSteps += b.ToString();
            calSteps += "+10×(";
            calSteps += a.ToString();
            calSteps += "+";
            calSteps += b.ToString();
            calSteps += ")+1";

            //第二步
            int a_b_100 = 100 * a * b;
            calSteps += "=";
            calSteps += a_b_100.ToString();
            calSteps += "+";
            int a_b = 10 * (a + b);
            calSteps += a_b.ToString();
            calSteps += "+1";

            //第三步
            int a_b_2 = a_b_100 + a_b;
            calSteps += "=";
            calSteps += a_b_2.ToString();
            calSteps += "+1";

            //第四步
            int tmp1 = a_b_2;
            int tmp2 = 1;
            int tmp0 = 0;
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

        public SYW11_57DataCreator()
        {

        }
    }
}
