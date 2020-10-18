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

namespace SoonLearning.Assessment.Player.CommonControl
{
    /// <summary>
    /// Interaction logic for RichEditUserControl.xaml
    /// </summary>
    public partial class RichEditUserControl : UserControl
    {
        public TextChangedEventHandler TextChangedEvent;

        public string Document
        {
            get { return this.richTextBox.Text; }
            set { this.richTextBox.Text = value; }
        }

        public RichEditUserControl()
        {
            InitializeComponent();

            this.richTextBox.RichTextBox.TextChanged += new TextChangedEventHandler(richTextBox_TextChanged);
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
