using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Data;
using System.Reflection;

namespace SoonLearning.Assessment.Data.Converters
{
    public static class DataConverters
    {
        private static Dictionary<string, IValueConverter> valueConverterDictionary = new Dictionary<string, IValueConverter>();

        public static Index2CharConverter Index2CharConverter
        {
            get
            {
                return getConverter(typeof(Index2CharConverter)) as Index2CharConverter;
            }
        }

        public static QuestionType2StringConverter QuestionType2StringConverter
        {
            get
            {
                return getConverter(typeof(QuestionType2StringConverter)) as QuestionType2StringConverter;
            }
        }

        public static ListViewItem2IndexConverter ListViewItem2IndexConverter
        {
            get
            {
                return getConverter(typeof(ListViewItem2IndexConverter)) as ListViewItem2IndexConverter;
            }
        }

        private static IValueConverter getConverter(Type type)
        {
            if (!valueConverterDictionary.ContainsKey(type.FullName))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                object converter = assembly.CreateInstance(type.FullName);
                valueConverterDictionary.Add(type.FullName, converter as IValueConverter);
            }

            return valueConverterDictionary[type.FullName];
        }
    }
}
