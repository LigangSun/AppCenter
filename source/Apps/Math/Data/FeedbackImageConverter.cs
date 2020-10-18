using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SoonLearning.Math.Data
{
    public class FeedbackImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;

            bool? correct = (bool?)value;
            if (correct.Value)
                return new Uri(@"pack:\\application:,,,/Math;component/Resources/correct.png");

            return new Uri(@"pack:\\application:,,,/Math;component/Resources/incorrect.png");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
