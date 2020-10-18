using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using GadgetCenter.Resources;

namespace GadgetCenter.CommonControl
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
                return new ValidationResult(false, string.Format(Strings.IllegalCharacters, e.Message));
            }

            if (intVal < Min || intVal > Max)
            {
                return new ValidationResult(false, Strings.EnterValueBetweenMinAndMax);
            }

            return new ValidationResult(true, null);
        }
    }
}
