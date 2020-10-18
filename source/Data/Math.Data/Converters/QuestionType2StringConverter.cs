using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SoonLearning.Assessment.Data.Converters
{
    [ValueConversion(typeof(QuestionType), typeof(string))]
    public class QuestionType2StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            QuestionType type = (QuestionType)value;
            switch (type)
            {
                case QuestionType.Composite:
                    return "复合题";
                case QuestionType.Essay:
                    return "简答题";
                case QuestionType.FillInBlank:
                    return "填空题";
                case QuestionType.Match:
                    return "配对题";
                case QuestionType.MultiChoice:
                    return "单选题";
                case QuestionType.MultiResponse:
                    return "多选题";
                case QuestionType.Table:
                    return "表格题";
                case QuestionType.TrueFalse:
                    return "判断题";
                case QuestionType.VerticalForm:
                    return "竖式题";
            }

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
