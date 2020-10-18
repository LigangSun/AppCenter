using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.JSHW
{
    public class JSHWDataCreator : DataCreator
    {
        private static JSHWDataCreator creator;

        public static JSHWDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new JSHWDataCreator();

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
            public decimal AL; //被减数的末n位
            public decimal AH; //被减数的A减去AL


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
                A = A1 = B = B1 = AH = AL = 0;
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
            this.exerciseTitle = "减数换位法练习";
            this.examTitle = "减数换位法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.JSHW.JSHW_Document.xaml";
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

            if (maxValue < 1000)
            {
                maxValue = 1000;
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            int number = 3; //rand.Next(3, 5 + 1);
           
            valueABC.number = number;
            valueABC.answer = 0;

            int i = 0;
            int tmp = 0;
            decimal sum = 0;

            int B = rand.Next(maxValue, maxValue + 1);
            int C = rand.Next(minValue, maxValue + 1);
            int maxHbit = 10;
            if (maxValue > 1000)
            {
                maxHbit = 100;
            }
     
            int baseNumB = rand.Next(11, 100);
            int baseNumC = rand.Next(11, 100);
            int hBitB = rand.Next(0, maxHbit);
            int hBitC = rand.Next(0, maxHbit);
            B = hBitB * 100 + baseNumB;
            C = hBitC * 100 + baseNumC;

            int hBitA = rand.Next(hBitB + hBitC + 1, maxHbit + hBitB + hBitC + 1);
            int A = hBitA * 100 + baseNumC;
            
            //
            valueABC.values[0] = A;
            valueABC.values[1] = B;
            valueABC.values[2] = C;

            valueABC.answer = A - B - C;
        }

        private String SolveSteps(valuesStruct valueMC)
        {
            //题干
            int i;
            string calSteps = "";     

            //解题步骤
            decimal A = valueMC.values[0];
            decimal B = valueMC.values[1];
            decimal C = valueMC.values[2];
            
            //第一步         
            calSteps += "=";
            calSteps += valueMC.values[0].ToString();
            calSteps += "-";
            calSteps += valueMC.values[2].ToString();
            calSteps += "-";
            calSteps += valueMC.values[1].ToString();
                    
            //第二步
            decimal A_C = A - C;
            calSteps += "=";
            calSteps += A_C.ToString();
            calSteps += "-";
            calSteps += B.ToString(); 
             
            decimal answer;// = add11 - add2;

            answer = A_C - B;
            if (answer != valueMC.answer)
            {
                calSteps += "Error!";
            }
            
            //最后一步

            calSteps += "=";
            calSteps += valueMC.answer.ToString();

            calSteps += ",是正确答案。";

            return calSteps;
        }

        private String QuestionText(valuesStruct valueMC)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            string questionText = "";
        
            //questionText += valueMC.values[0].ToString();

            //for (int i = 1; i < valueMC.number; i++)
            //{
            //    questionText += "-";
            //    questionText += valueMC.values[i].ToString();
            //}

            questionText += valueMC.values[0].ToString();
            int i = 0, j1 = 0, j2 = 0, iTmp = 0;
            bool flag = false;
            int[] iNums = new int[10];
            iTmp = rand.Next(1, valueMC.number);
            iNums[j2] = iTmp;
            questionText += "-";
            questionText += valueMC.values[iTmp].ToString();
            j2++;

            for (i = 2; i < valueMC.number; i++)
            {
                questionText += "-";
                while (true)
                {
                    flag = false;
                    iTmp = rand.Next(1, valueMC.number);
                    for (j1 = 0; j1 < j2; j1++)
                    {
                        if (iTmp == iNums[j1])
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag == false)
                        break;
                }
                iNums[j2] = iTmp;
                questionText += valueMC.values[iTmp].ToString();
                j2++;
            } 
            
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

        public JSHWDataCreator()
        {

        }
    }
}
