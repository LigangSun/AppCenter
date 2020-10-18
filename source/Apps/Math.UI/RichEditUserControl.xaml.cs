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

namespace SoonLearning.Math.UI
{
    /// <summary>
    /// Interaction logic for RichEditUserControl.xaml
    /// </summary>
    public partial class RichEditUserControl : UserControl
    {
        public TextChangedEventHandler TextChangedEvent;

        public FlowDocument Document
        {
            get { return this.richTextBox.Document; }
            set { this.richTextBox.Document = value; }
        }

        public RichEditUserControl()
        {
            InitializeComponent();
        }

        private void supperScriptButton_Click(object sender, RoutedEventArgs e)
        {
            var currentAlignment = richTextBox.Selection.GetPropertyValue(Inline.BaselineAlignmentProperty); 
            BaselineAlignment newAlignment = ((BaselineAlignment)currentAlignment == BaselineAlignment.Superscript) ? BaselineAlignment.Baseline : BaselineAlignment.Superscript; 

            richTextBox.Selection.ApplyPropertyValue(Inline.BaselineAlignmentProperty, newAlignment); 
        }

        private void richTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var currentAlignment = richTextBox.Selection.GetPropertyValue(Inline.BaselineAlignmentProperty);
            if (currentAlignment is BaselineAlignment)
                this.supperScriptButton.IsChecked = ((BaselineAlignment)currentAlignment == BaselineAlignment.Superscript);
        }

        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.TextChangedEvent != null)
            {
                this.TextChangedEvent(sender, e);
            }
        }
    }
}
