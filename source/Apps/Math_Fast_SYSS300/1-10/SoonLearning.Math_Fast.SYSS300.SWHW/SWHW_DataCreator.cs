using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.SWHW
{
    public class SWHWDataCreator : DataCreator
    {
        private static SWHWDataCreator creator;

        public static SWHWDataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new SWHWDataCreator();

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
            public int a; //一位数a
            public int b; //一位数b
            public int c; //一位数c

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
                a = 1;
                b = 1;
                c = 1;
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
            this.exerciseTitle = "首尾换位法练习";
            this.examTitle = "首尾换位法测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.SWHW.SWHW_Document.xaml";
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

            if (maxValue < 500)
            {
                maxValue = 500;
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            valueABC.number = 2;// rand.Next(4, 8 + 1);
            int a, b, c;

            //取a和b
            while (true)
            {
                //取a, b
                a = rand.Next(1, 9 + 1);
                b = rand.Next(0, 9 + 1);
                c = rand.Next(1, 9 + 1);

                if (a != b && a != c && b != c)
                {
                    valueABC.a = a;
                    valueABC.b = b;
                    valueABC.c = c;
                    break;
                }
            }

            valueABC.values[0] = 100 * a + 10 * b + c;
            valueABC.values[1] = 100 * c + 10 * b + a;

            valueABC.answer = valueABC.values[0] + valueABC.values[1]; // +valueABC.values[2] + valueABC.values[3] + valueABC.values[4] + valueABC.values[5];
        }

        private String SolveSteps(valuesStruct valueMC)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            int a = valueMC.a;
            int b = valueMC.b;
            int c = valueMC.c;

            //题干
            string calSteps = valueMC.values[0].ToString();
            calSteps += "+";
            calSteps += valueMC.values[1].ToString();
            //calSteps += "+";
            //calSteps += valueMC.values[2].ToString();
            //calSteps += "+";
            //calSteps += valueMC.values[3].ToString();
            //calSteps += "+";
            //calSteps += valueMC.values[4].ToString();
            //calSteps += "+";
            //calSteps += valueMC.values[5].ToString();
      
            //解题步骤

            //第一步
          
            //int tmpA = decimal.ToInt32(valueMC.values[0]); //100部分
            //int tmpB = decimal.ToInt32(valueMC.values[1]); //10部分
            //int tmpC = decimal.ToInt32(valueMC.values[2]); //个位部分

            calSteps += "=101×(";
            calSteps += a.ToString();
            calSteps += "+";
            calSteps += c.ToString();
            calSteps += ")";
            calSteps += "+20×";
            calSteps += b.ToString();

            //第二步
            decimal a_c_100 = 101 * (a + c);
            decimal b_20 = 20 * b;
            calSteps += "=";
            calSteps += a_c_100.ToString();
            calSteps += "+";
            calSteps += b_20.ToString();
            
            //第三步
            decimal answer = a_c_100 + b_20;
            calSteps += "=";

            if (valueMC.answer != answer)
            {
                calSteps += "Error!";
            }
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
            valuesStruct valueMC = new valuesStruct(10); //题干展示的值

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
            valuesStruct valueOrg = new valuesStruct(10); //原始产生的值
            valuesStruct valueMC = new valuesStruct(10); //题干展示的值

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

        public SWHWDataCreator()
        {

        }
    }
}
