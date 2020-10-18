using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.CTTJ
{
    public class CTTJDataCreator : DataCreator
    {
        private static CTTJDataCreator creator;

        public static CTTJDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new CTTJDataCreator();

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
            this.exerciseTitle = "凑同调加法练习";
            this.examTitle = "凑同调加法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.CTTJ.CTTJ_Document.xaml";
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
            int minValue = 100;
            int maxValue = 1000;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            if (minValue < 100)
            {
                minValue = 100;
            }

            if (maxValue < 1000)
            {
                maxValue = 1000;
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            valueABC.number = 2;// rand.Next(10, 100 + 1);
            valueABC.answer = 0;

            int bits = 0;
            int i = 0;
            int tmpMaxValue = decimal.ToInt32(maxValue);// / 10;

            //根据输入的最大值计算几位数运算
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

            //分配位数，存放位数值，从低到高依次存放个位、百位、千位......
            int[] numsB = new int[bits];
            decimal B1 = 0, B = 0;

            //随机得到减数B
            for (i = bits - 2; i >= 0; i--)
            {
                numsB[i] = rand.Next(1, 10);
                B1 = B1 * 10 + numsB[i];
            }
            
            numsB[bits - 1] = rand.Next(1, 10);
            decimal BH = numsB[bits - 1];
            for (i = bits - 1; i > 0; i--)
            {
                BH = BH * 10;
            } 

            B = BH + B1;
            int b1 = decimal.ToInt32(B1);

            //随机取被减数A
            //除了最高位的A’的值要大于B‘（除了最高位）
            decimal A1 = rand.Next(b1 + 2, b1 + 16);
            int ah = rand.Next(numsB[bits - 1], 10);
            decimal A = ah;// rand.Next(numsB[bits - 1], 10);

            for (i = 0; i < bits - 1; i++)
            {
                A *= 10;
            }
            A += A1;

            //把被减数A和减数B存入数组
            valueABC.values[0] = A;
            valueABC.values[1] = B;            
            valueABC.answer = A - B;

            //记录取值情况
            valueABC.A = A;
            valueABC.A1 = A1;
            valueABC.B = B;
            valueABC.B1 = B1;
        }

        private String SolveSteps(valuesStruct valueMC)
        {        
            //题干
            string calSteps = "";
            int i;

            calSteps += valueMC.values[0].ToString();
            calSteps += "-";
            calSteps += valueMC.values[1].ToString();
      
            //解题步骤

            //第一步
            decimal A = decimal.ToInt32(valueMC.values[0]);
            decimal B = decimal.ToInt32(valueMC.values[1]);
            decimal A1 = valueMC.A1;
            decimal B1 = valueMC.B1;            
            decimal sub_A1_B1 = A1 - B1;
            decimal B2 = B + sub_A1_B1;

            calSteps += "=";
            calSteps += A.ToString();
            calSteps += "-";
            calSteps += B2.ToString();
            calSteps += "+";
            calSteps += sub_A1_B1.ToString();  
        
            //第二步
            calSteps += "=";
            decimal sub_A_B2 = A - B2;
            calSteps += sub_A_B2.ToString();
            calSteps += "+";
            calSteps += sub_A1_B1.ToString();
          
            //第三步
            decimal answer = sub_A_B2 + sub_A1_B1;

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
            questionText += "-";
            questionText += valueMC.values[1].ToString();

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

        public CTTJDataCreator()
        {

        }
    }
}
