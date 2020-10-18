using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace GadgetCenter.CommonControl
{
    public class IntegerOnlyTextBox : TextBox
    {
        public IntegerOnlyTextBox()
        {
        }

        protected override void OnPreviewTextInput(System.Windows.Input.TextCompositionEventArgs e)
        {
            int value;
            if (!int.TryParse(e.Text, out value))
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }
    }
}
