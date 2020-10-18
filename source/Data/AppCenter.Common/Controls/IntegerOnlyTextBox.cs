using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace SoonLearning.AppCenter.Controls
{
    public class NumberOnlyTextBox : TextBox
    {
        public NumberOnlyTextBox()
        {
        }

        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            decimal value;
            if (!decimal.TryParse(e.Text, out value))
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }
    }
}
