using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Math.Data;
using System.Threading;
using System.Windows.Forms;

namespace Math.Basic.Data
{
    public delegate void CreateQuestionContentDelegate(QuestionContent content);
    public delegate IEnumerable<QuestionOption> CreateQuestionOptionDelegate();

    internal static class ObjectCreator
    {
        internal static Exercise CreateExercise(string title, string description)
        {
            Exercise exercise = new Exercise();
            exercise.Title = title;
            exercise.Description = description;
            exercise.MarkType = MarkType.AutoAfterEachQuestion;
            exercise.RetryTimes = -1;
            exercise.DisplayAnswerType = DisplayAnswerType.AutoAfterEachQuestion;
            exercise.DisplayFeedbackType = DisplayFeedbackType.AfterEachQuestion;
            exercise.DisplayScore = false;
            exercise.DisplaySolutionType = DisplaySolutionType.AutoAfterEachQuestion;
            exercise.QuestionCountPerPage = 2;
            exercise.QuestionDisplayType = QuestionDisplayType.Question;
            return exercise;
        }

        internal static Exam CreateExam(string title, string description, int duration)
        {
            Exam exam = new Exam();
            exam.Title = title;
            exam.Description = description;
            exam.Duration = duration;
            exam.MarkType = MarkType.AutoAfterExam;
            exam.RetryTimes = -1;
            exam.DisplayAnswerType = DisplayAnswerType.Manual;
            exam.DisplayFeedbackType = DisplayFeedbackType.AfterExam;
            exam.DisplayScore = false;
            exam.DisplaySolutionType = DisplaySolutionType.Manual;
            exam.QuestionCountPerPage = 2;
            exam.QuestionDisplayType = QuestionDisplayType.Question;
            return exam;
        }

        internal static Section CreateSection(string title, string description)
        {
            Section secton = new Section();
            secton.Title = title;
            secton.Description = description;
            return secton;
        }

        internal static MCQuestion CreateMCQuestion(CreateQuestionContentDelegate bodyCallback, CreateQuestionOptionDelegate optionCallback)
        {
            MCQuestion mcQuestion = new MCQuestion();
            bodyCallback(mcQuestion.Content);
            foreach (QuestionOption option in optionCallback())
            {
                mcQuestion.QuestionOptionCollection.Add(option);
            }

            mcQuestion.RandomOption = false;

            return mcQuestion;
        }

        internal static TableQuestion CreateTableQuestion(CreateQuestionContentDelegate bodyCallback, CreateQuestionOptionDelegate optionCallback)
        {
            TableQuestion tableQuestion = new TableQuestion();
            bodyCallback(tableQuestion.Content);
            foreach (QuestionOption option in optionCallback())
            {
                tableQuestion.QuestionOptionCollection.Add(option);
            }

            return tableQuestion;
        }

        /// <summary>
        /// Append Options to Selectable question
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="optionCount"></param>
        /// <param name="isMR">If the option is multi response</param>
        internal static IEnumerable<QuestionOption> CreateDecimalOptions(int optionCount, 
            int minValue,
            int maxValue, 
            bool isMR,
            Func<decimal, bool> validationFun,
            decimal? correctDecemal = null)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            List<QuestionOption> optionList = new List<QuestionOption>();
            List<int> optionValueList = new List<int>();
            decimal correctValue = 0;
            if (correctDecemal != null)
                correctValue = correctDecemal.Value;
            for (int j = 0; j < optionCount - 1; j++)
            {
                while (true)
                {
                    int optionValue = rand.Next(minValue, maxValue + 1);
                    if (!isMR && validationFun(optionValue))
                    {
                        correctValue = optionValue;
                        Thread.Sleep(10);
                        continue;
                    }

                    IEnumerable<int> queryResult = optionValueList.Where<int>(c => c == optionValue);
                    if (queryResult.Count<int>() > 0)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    QuestionOption option = new QuestionOption();
                    option.OptionContent.Content = optionValue.ToString();
                    option.OptionContent.ContentType = ContentType.Text;
                    if (isMR)
                        option.IsCorrect = validationFun(optionValue);

                    optionList.Add(option);

                    optionValueList.Add(optionValue);
                    break;
                }
            }

            if (correctValue == 0)
            {
                while (true)
                {
                    int optionValue = rand.Next(minValue, maxValue);
                    if (validationFun(optionValue))
                    {
                        correctValue = optionValue;
                        break;
                    }
                    Thread.Sleep(10);
                }
            }

            QuestionOption correctOption = new QuestionOption();
            correctOption.OptionContent.Content = correctValue.ToString();
            correctOption.IsCorrect = true;
            int correctOptionIndex = rand.Next(0, optionCount * 8) % optionCount;
            if (correctOptionIndex == optionCount - 1)
                optionList.Add(correctOption);
            else
                optionList.Insert(correctOptionIndex, correctOption);

            Thread.Sleep(50);

            return optionList;
        }

        internal static IEnumerable<QuestionOption> CreateFractionOptions(int optionCount,
            int minValue,
            int maxValue,
            bool isMR,
            Func<ArithmeticFractionValuePart, bool> validationFun,
            ArithmeticFractionValuePart correctFraction = null)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            List<QuestionOption> optionList = new List<QuestionOption>();
            List<ArithmeticFractionValuePart> optionValueList = new List<ArithmeticFractionValuePart>();
            ArithmeticFractionValuePart correctValue = null;
            if (correctFraction != null)
                correctValue = correctFraction;
            for (int j = 0; j < optionCount - 1; j++)
            {
                while (true)
                {
                    ArithmeticFractionValuePart fractionValue = null;
                    foreach (ArithmeticFractionValuePart value in CreateFractionValue(minValue, maxValue, minValue == 0 ? 1: minValue, maxValue))
                        fractionValue = value;

                    if (!isMR && validationFun(fractionValue))
                    {
                        correctValue = fractionValue;
                        Thread.Sleep(10);
                        continue;
                    }

                    IEnumerable<ArithmeticFractionValuePart> queryResult = optionValueList.Where<ArithmeticFractionValuePart>(c => c.Equals(fractionValue));
                    if (queryResult.Count<ArithmeticFractionValuePart>() > 0)
                    {
                        Thread.Sleep(10);
                        continue;
                    }

                    QuestionOption option = new QuestionOption();
                    option.OptionContent.Content = fractionValue.PlaceHolder;
                    option.OptionContent.ContentType = ContentType.Text;
                    option.OptionContent.QuestionPartCollection.Add(fractionValue);
                    if (isMR)
                        option.IsCorrect = validationFun(fractionValue);

                    optionList.Add(option);

                    optionValueList.Add(fractionValue);
                    break;
                }
            }

            if (correctValue == null)
            {
                while (true)
                {
                    ArithmeticFractionValuePart fractionValue = null;
                    foreach (ArithmeticFractionValuePart value in CreateFractionValue(minValue, maxValue, minValue == 0 ? 1 : minValue, maxValue))
                        fractionValue = value;

                    if (validationFun(fractionValue))
                    {
                        correctValue = fractionValue;
                        break;
                    }
                    Thread.Sleep(10);
                }
            }

            QuestionOption correctOption = new QuestionOption();
            correctOption.OptionContent.Content = correctValue.PlaceHolder;
            correctOption.OptionContent.QuestionPartCollection.Add(correctValue);
            correctOption.IsCorrect = true;
            int correctOptionIndex = rand.Next(0, optionCount * 8) % optionCount;
            if (correctOptionIndex == optionCount - 1)
                optionList.Add(correctOption);
            else
                optionList.Insert(correctOptionIndex, correctOption);

            Thread.Sleep(50);

            return optionList;
        }

        internal static IEnumerable<QuestionOption> CreateFractionOption(int minNumeratorValue, int maxNumeratorValue, int minDenominatorValue, int maxDenominatorValue)
        {
            ArithmeticFractionValuePart fractionValue = null;
            foreach (ArithmeticFractionValuePart value in CreateFractionValue(minNumeratorValue, maxNumeratorValue, minDenominatorValue, maxDenominatorValue))
                fractionValue = value;

            QuestionOption option = new QuestionOption();
            option.OptionContent.Content = fractionValue.PlaceHolder;
            option.OptionContent.ContentType = ContentType.Text;
            option.OptionContent.QuestionPartCollection.Add(fractionValue);

            yield return option;
        }

        internal static IEnumerable<QuestionOption> CreateIntegerOption(int minValue, int maxValue)
        {
            ArithmeticDecimalValuePart decimalValue = null;
            foreach (ArithmeticDecimalValuePart value in CreateIntergetValue(minValue, maxValue))
                decimalValue = value;

            QuestionOption option = new QuestionOption();
            option.OptionContent.Content = decimalValue.PlaceHolder;
            option.OptionContent.ContentType = ContentType.Text;
            option.OptionContent.QuestionPartCollection.Add(decimalValue);

            yield return option;
        }

        internal static IEnumerable<QuestionOption> CreateDoubleOption(int leftMinValue, int leftMaxValue, int rightMinValue, int rightMaxValue)
        {
            ArithmeticDecimalValuePart decimalValue = null;
            foreach (ArithmeticDecimalValuePart value in CreateDoubleValue(leftMinValue, leftMaxValue, rightMinValue, rightMaxValue))
                decimalValue = value;

            QuestionOption option = new QuestionOption();
            option.OptionContent.Content = decimalValue.PlaceHolder;
            option.OptionContent.ContentType = ContentType.Text;
            option.OptionContent.QuestionPartCollection.Add(decimalValue);

            yield return option;
        }

        internal static IEnumerable<QuestionOption> CreateDoubleTextOption(int leftMinValue, int leftMaxValue, int rightMinValue, int rightMaxValue, string suffix)
        {
            QuestionTextPart textValue = null;
            foreach (QuestionTextPart value in CreateDoubleTextValue(leftMinValue, leftMaxValue, rightMinValue, rightMaxValue))
                textValue = value;

            textValue.Text += suffix;

            QuestionOption option = new QuestionOption();
            option.OptionContent.Content = textValue.PlaceHolder;
            option.OptionContent.ContentType = ContentType.Text;
            option.OptionContent.QuestionPartCollection.Add(textValue);

            yield return option;
        }

        private static IEnumerable<ArithmeticFractionValuePart> CreateFractionValue(int minNumeratorValue, int maxNumeratorValue, int minDenominatorValue, int maxDenominatorValue)
        {
            if (minDenominatorValue == 0)
                throw new ArgumentOutOfRangeException("Denominator cannot be zero!");

            Random rand = new Random((int)DateTime.Now.Ticks);

            decimal numerator;
            decimal denominator;

            numerator = rand.Next(minNumeratorValue, maxNumeratorValue + 1);
            Thread.Sleep(50);
            denominator = rand.Next(minDenominatorValue, maxDenominatorValue + 1);

            ArithmeticFractionValuePart fractionValue = new ArithmeticFractionValuePart();
            fractionValue.Numerator = numerator;
            fractionValue.Denominator = denominator;

            yield return fractionValue;
        }

        internal static IEnumerable<ArithmeticDecimalValuePart> CreateIntergetValue(int minValue, int maxValue)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int value = rand.Next(minValue, maxValue + 1);
            ArithmeticDecimalValuePart valuePart = new ArithmeticDecimalValuePart(value);
            yield return valuePart;
        }

        private static IEnumerable<ArithmeticDecimalValuePart> CreateDoubleValue(int leftMinValue, int leftMaxValue, int rightMinValue, int rightMaxValue)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int partLeft = rand.Next(leftMinValue, leftMaxValue + 1);
            Thread.Sleep(50);
            int partRight = rand.Next(rightMinValue, rightMaxValue + 1);
            double value = Convert.ToDouble(string.Format("{0}.{1}", partLeft, partRight));
            ArithmeticDecimalValuePart valuePart = new ArithmeticDecimalValuePart(new decimal(value));
            yield return valuePart;
        }

        private static IEnumerable<QuestionTextPart> CreateDoubleTextValue(int leftMinValue, int leftMaxValue, int rightMinValue, int rightMaxValue)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int partLeft = rand.Next(leftMinValue, leftMaxValue + 1);
            Thread.Sleep(50);
            int partRight = rand.Next(rightMinValue, rightMaxValue + 1);
            double value = Convert.ToDouble(string.Format("{0}.{1}", partLeft, partRight));
            QuestionTextPart valuePart = new QuestionTextPart(value.ToString());
            yield return valuePart;
        }
    }
}
