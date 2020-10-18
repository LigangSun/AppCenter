using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.FZCZ_114
{
    public class FZCZ_114DataCreator : DataCreator
    {
        private static FZCZ_114DataCreator creator;

        public static FZCZ_114DataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new FZCZ_114DataCreator();

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
            this.exerciseTitle = "分组凑整法练习";
            this.examTitle = "分组凑整法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.FZCZ_114.FZCZ_114_Document.xaml";
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.MultiChoice,
                "单选题：",
                "（下面每道题都只有一个选项是正确的）",
                5,
                1,
                100));
            this.sectionInfoCollection.Add(new SectionValueRangeInfo(QuestionType.FillInBlank,
                "填空题：",
                "（在空格中填入符合条件的数）",
                5,
                1,
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

            if (minValue < 1)
            {
                minValue = 1;
            }

            if (maxValue > 1000)
            {
                maxValue = 1000;
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            valueABC.number = rand.Next(3, 6);
            valueABC.answer = 0;

            //decimal A1, A2 = 0;

            int times = rand.Next(1, 10);
            int a1 = 0;// rand.Next(1, 10); //百位数
            int b1 = rand.Next(1, 10); //十位数
            int c1 = rand.Next(1, 10); //个位数
            int a2 = 0;// rand.Next(1, 10); //百位数
            int b2 = 0;// rand.Next(1, 10); //十位数
            int c2 = rand.Next(1, 10); //个位数
            int d1 = 0;
            int d2 = 0;
            
            decimal[] As = new decimal[10];
            int posRand = rand.Next(0, 3);
            if (posRand == 0)
            {
                As[1] = rand.Next(1, 10) * 5;
                As[0] = rand.Next(minValue, maxValue + 1);
                As[2] = rand.Next(1, 10) * 2;
            }
            else if (posRand == 1)
            {
                As[0] = rand.Next(minValue, maxValue + 1);
                As[1] = rand.Next(1, 10) * 5;
                As[2] = rand.Next(1, 10) * 2;
            }
            else
            {
                As[0] = rand.Next(minValue, maxValue + 1);
                As[2] = rand.Next(1, 10) * 2;
                As[1] = rand.Next(1, 10) * 5;
            }

            for (int i = 3; i < valueABC.number; i++)
            {
                int rand52 = rand.Next(0, 3);//
                if (rand52 == 0)
                {
                    As[i] = rand.Next(1, 10) * 5;
                }
                else if (rand52 == 1)
                {
                    As[i] = rand.Next(minValue, maxValue + 1);
                }
                else
                {
                    As[i] = rand.Next(1, 10) * 2;
                }
            }

            valueABC.answer = 1;
            for (int j = 0; j < valueABC.number; j++)
            {
                valueABC.values[j] = As[j];
                valueABC.answer *= As[j];
            }
        }

        private String SolveSteps(valuesStruct valueMC)
        {
            string calSteps = "";
            //题干
            int num = valueMC.number;
            int i, j;

            for (i = 0; i < num - 1; i++)
            {
                calSteps += valueMC.values[i].ToString();
                calSteps += "×";
            }
            calSteps += valueMC.values[num - 1].ToString();

            //解题步骤
            //按5、2倍数规律交换、结合各个因数
            decimal[] As = new decimal[num];
            decimal[] bs = new decimal[num];
            for (i = 0; i < num; i++)
            {
                bs[i] = valueMC.values[0] % 10;
            }

            decimal tmp;

            if (bs[0] == 5 && bs[2] == 2)
            {
                As[0] = valueMC.values[0];
                As[1] = valueMC.values[2];
                As[2] = valueMC.values[1];
            }
            else if (bs[1] == 5 && bs[2] == 2)
            {
                As[0] = valueMC.values[1];
                As[1] = valueMC.values[2];
                As[2] = valueMC.values[0];
            }
            else// (bs[0] == 5 && bs[1] == 2)
            {
                As[0] = valueMC.values[2];
                As[1] = valueMC.values[1];
                As[2] = valueMC.values[0];
            } 
            
            if (num == 4)
            {
                As[3] = valueMC.values[3];
            }

            if (num == 5)
            {
                //if (bs[3] == 5 && bs[4] == 2
                As[3] = valueMC.values[3];
                As[4] = valueMC.values[4];
            }

            int A1 = decimal.ToInt32(valueMC.values[0]);
            int A2 = decimal.ToInt32(valueMC.values[1]);

            //第一步            
            calSteps += "=(";
            calSteps += As[0].ToString();
            calSteps += "×";
            calSteps += As[1].ToString();
            calSteps += ")×";
            calSteps += As[2].ToString();
            if (num == 4)
            {
                calSteps += "×";
                calSteps += As[3].ToString();
            }
            if (num == 5)
            {
                if ((bs[3] == 5 && bs[4] == 2) || (bs[3] == 2 && bs[4] == 5))
                {
                    calSteps += "×(";
                    calSteps += As[3].ToString();
                    calSteps += "×";
                    calSteps += As[4].ToString();
                    calSteps += ")";
                }
                else
                {
                    calSteps += "×";
                    calSteps += As[3].ToString();
                    calSteps += "×";
                    calSteps += As[4].ToString();
                }
            }
                
            //第二步 
            calSteps += "=";
            decimal As01 = As[0] * As[1];
            calSteps += As01.ToString();
            calSteps += "×";
            calSteps += As[2].ToString();
            if (num == 4)
            {
                calSteps += "×";
                calSteps += As[3].ToString();
            }

            decimal As34 = 1;

            if (num == 5)
            {
                As34 = As[3] * As[4];
                if ((bs[3] == 5 && bs[4] == 2) || (bs[3] == 2 && bs[4] == 5))
                {
                    calSteps += "×";
                    calSteps += As34.ToString();                   
                }
                else
                {
                    calSteps += "×";
                    calSteps += As[3].ToString();
                    calSteps += "×";
                    calSteps += As[4].ToString();
                }
            }
            
            //第三步 
            calSteps += "=";
            decimal As012 = As01 * As[2];
            if (num == 4)
            {
                calSteps += As012.ToString();
                calSteps += "×";
                calSteps += As[3].ToString();

                //
                calSteps += "=";
                As012 *= As[3];                
            }
            if (num == 5)
            {
                calSteps += As012.ToString();
                if ((bs[3] == 5 && bs[4] == 2) || (bs[3] == 2 && bs[4] == 5))
                {
                    calSteps += "×";
                    calSteps += As34.ToString();
 
                    //
                    calSteps += "=";
                    As012 *= As34;
                }
                else
                {
                    calSteps += "×";
                    calSteps += As[3].ToString();
                    calSteps += "×";
                    calSteps += As[4].ToString();

                    //
                    calSteps += "=";
                    As012 *= As[3];
                    calSteps += As012.ToString();
                    calSteps += "×";
                    calSteps += As[4].ToString();

                    //
                    calSteps += "=";
                    As012 *= As[4];                    
                }
            }
           
            //第四步
            int tmp0 = decimal.ToInt32(As012);
            int tmp1 = 0;
            int tmp2 = 0;
            //
            decimal answer = tmp0 + tmp1 + tmp2;
 
            if (answer != valueMC.answer)
            {
                calSteps += "Error!";
            }

            //calSteps += "=";
            calSteps += valueMC.answer.ToString();
            calSteps += ",是正确答案。";

            return calSteps;
        }

        private String QuestionText(valuesStruct valueMC)
        {
            string questionText = "";

            int num = valueMC.number;
            int i;

            for (i = 0; i < num - 1; i++)
            {
                questionText += valueMC.values[i].ToString();
                questionText += "×";
            }
            questionText += valueMC.values[num - 1].ToString();
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

        public FZCZ_114DataCreator()
        {

        }
    }
}
