using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.IO;
using System.Windows.Markup;
using HTMLConverter;

namespace SoonLearning.TeachAppMaker.Data
{
    [ValueConversion(typeof(string), typeof(FlowDocument))]
    public class Html2FlowDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string html = value as string;
            if (string.IsNullOrEmpty(html))
                return null;

            return HtmlToXamlConverter.ConvertHtmlToXaml(html);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
