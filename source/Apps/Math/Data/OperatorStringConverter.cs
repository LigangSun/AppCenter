using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace SoonLearning.Math.Data
{
    public class OperatorStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Operator op = (Operator)value;
            //switch (op)
            //{
            //    //case Operator.Plus:
            //    //    return @"pack:\\application:,,,/Math;Component/Resources/add.png";
            //    //case Operator.Minus:
            //    //    return @"pack:\\application:,,,/Math;Component/Resources/minus.png";
            //    //case Operator.Multiply:
            //    //    return @"pack:\\application:,,,/Math;Component/Resources/multiply.png";
            //    //case Operator.Division:
            //    //    return @"pack:\\application:,,,/Math;Component/Resources/divide.png";
            //}

            switch (op)
            {
                case Operator.Plus:
                    return @"+";
                case Operator.Minus:
                    return @"-";
                case Operator.Multiply:
                    return @"×";
                case Operator.Division:
                    return @"÷";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
