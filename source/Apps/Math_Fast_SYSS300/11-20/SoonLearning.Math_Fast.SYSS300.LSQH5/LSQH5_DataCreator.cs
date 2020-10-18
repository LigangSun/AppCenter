using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ComponentModel;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.Assessment.Data;
using System.Reflection;

namespace SoonLearning.Math_Fast.SYSS300.LSQH5
{
    public class LSQH5DataCreator : DataCreator
    {
        private static LSQH5DataCreator creator;

        public static LSQH5DataCreator Instance
        {
            get
            {
                if (creator == null)
                    creator = new LSQH5DataCreator();

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
            this.exerciseTitle = "连数求和法(五)练习";
            this.examTitle = "连数求和法(五)测验";
            this.flowDocumentFile = "SoonLearning.Math_Fast.SYSS300.LSQH5.LSQH5_Document.xaml";
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
            int minValue = 1;
            int maxValue = 100;
            if (info is SectionValueRangeInfo)
            {
                SectionValueRangeInfo rangeInfo = info as SectionValueRangeInfo;
                minValue = decimal.ToInt32(rangeInfo.MinValue);
                maxValue = decimal.ToInt32(rangeInfo.MaxValue);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            valueABC.number = rand.Next(10, 100 + 1);

            ////只能偶数个加数
            //if (valueABC.number % 2 != 0)
            //{
            //    valueABC.number++;
            //}

            int i = 0;
            valueABC.answer = 0;

            valueABC.valueStart = rand.Next(minValue, maxValue + 1);
            if (valueABC.valueStart % 2 == 0)
            {
                valueABC.valueStart++;
            }
            valueABC.values[i] = valueABC.valueStart;
            valueABC.answer += valueABC.values[i];
            valueABC.valueOffset = 2;// rand.Next(1, 15 + 1); //等差值   
            decimal curValue = valueABC.values[i];
  
            //前3个数
            for (i = 1; i < 3; i++)
            {
                valueABC.values[i] = valueABC.values[i - 1] + valueABC.valueOffset;
                curValue += 2;
                valueABC.answer += valueABC.values[i];// valueABC.values[i];
            }
            
            decimal first = valueABC.values[i - 1], second = 0;
            for (i = 3; i < valueABC.number - 3; i++)
            {
                second = first + valueABC.valueOffset;
                first = second;
                curValue += 2;
                valueABC.answer += second;// valueABC.values[i];
            }
            
            //最后3个数
            valueABC.values[3] = second + valueABC.valueOffset;
            curValue += 2;
            valueABC.answer += valueABC.values[3];
            for (i = 4; i < 6; i++)
            {
                valueABC.values[i] = valueABC.values[i - 1] + valueABC.valueOffset;
                curValue += 2;
                valueABC.answer += valueABC.values[i];
            }

            valueABC.valueLast = curValue;
        }

        private String SolveSteps(valuesStruct valueMC)
        {        
            //题干
            string calSteps = "";
            int i;

            //显示前三个数
            for (i = 0; i < 3; i++)
            {
                calSteps += valueMC.values[i].ToString();
                calSteps += "+";
            }

            calSteps += "......+";

            //显示最后三个数
            for (i = 3; i < 6; i++)
            {
                calSteps += valueMC.values[i].ToString();

                if (i != 6 - 1)
                {
                    calSteps += "+";
                }
                else
                {
                    calSteps += "=";
                }
            }
      
            //解题步骤

            //第一步
            decimal a = valueMC.valueLast;
            decimal a_1 = a + 1;
            decimal a_1_2 = a_1 / 2;
            calSteps += "=((";
            calSteps += a.ToString();
            calSteps += "+1)";
            calSteps += "÷2)";
            calSteps += "×((";
            calSteps += a.ToString();
            calSteps += "+1)";
            calSteps += "÷2)";

            decimal b = valueMC.valueStart - 2;
            decimal b_1 = b + 1;
            decimal b_1_2 = b_1 / 2;
            calSteps += "=-((";
            calSteps += b.ToString();
            calSteps += "+1)";
            calSteps += "÷2)";
            calSteps += "×((";
            calSteps += b.ToString();
            calSteps += "+1)";
            calSteps += "÷2)";
            

             //第二步
            calSteps += "=";
            calSteps += a_1_2.ToString();
            calSteps += "×";
            calSteps += a_1_2.ToString();
            calSteps += "-";
            calSteps += b_1_2.ToString();
            calSteps += "×";
            calSteps += b_1_2.ToString();
          
            //第三步
            calSteps += "=";
            decimal first = a_1_2 * a_1_2;
            decimal second = b_1_2 * b_1_2;
            calSteps += first.ToString();
            calSteps += "-";
            calSteps += second.ToString();

            //第四步
            decimal answer = first - second;

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
            int i = 0;

            //显示前三个数
            for (i = 0; i < 3; i++)
            {
                questionText += valueMC.values[i].ToString();
                questionText += "+";
            }

            questionText += "......+";
           
            //显示最后三个数
            for (i = 3; i < 6; i++)
            {
                questionText += valueMC.values[i].ToString();

                if (i != 6 - 1)
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

        public LSQH5DataCreator()
        {

        }
    }
}
