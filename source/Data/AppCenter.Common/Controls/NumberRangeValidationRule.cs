using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using SoonLearning.AppCenter.Properties;

namespace SoonLearning.AppCenter.Controls
{
    public class NumberRangeValidationRule : ValidationRule
    {
        public int Min
        {
            get;
            set;
        }

        public int Max
        {
            get;
            set;
        }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            int intVal = 0;
            try
            {
                intVal = Convert.ToInt32(value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, string.Format(Resources.IllegalCharacters, e.Message));
            }

            if (intVal < Min || intVal > Max)
            {
                return new ValidationResult(false, Resources.EnterValueBetweenMinAndMax);
            }

            return new ValidationResult(true, null);
        }
    }
}
