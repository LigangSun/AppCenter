using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoonLearning.TeachAppMaker.Controls
{
    /// <summary>
    /// Interaction logic for RichEditorEduToolBarUserControl.xaml
    /// </summary>
    public partial class RichEditorEduToolBarUserControl : UserControl
    {
        private RichTextBox richTextBox;

        internal RichTextBox RichTextBox
        {
            set
            {
                this.richTextBox = value;
            }
        }

        public RichEditorEduToolBarUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var currentAlignment = richTextBox.Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);
            BaselineAlignment newAlignment = ((BaselineAlignment)currentAlignment == BaselineAlignment.Superscript) ? BaselineAlignment.Baseline : BaselineAlignment.Superscript;

            richTextBox.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment); 
        }
    }
}
