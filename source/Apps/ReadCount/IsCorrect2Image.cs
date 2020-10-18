using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SoonLearning.ReadCount
{
    public class IsCorrect2Image : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool? isCorrect = value as bool?;
            if (isCorrect == null)
                return string.Empty;

            if (isCorrect.Value)
            {
                return @"pack://application:,,,/SoonLearning.ReadCount;component/Resources/pig_yes.png";
            }

            return @"pack://application:,,,/SoonLearning.ReadCount;component/Resources/pig_no.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
