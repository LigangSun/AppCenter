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
using System.Windows.Shapes;
using SoonLearning.Assessment.Data;

namespace SoonLearning.Assessment.Player.Editor
{
    /// <summary>
    /// Interaction logic for FractionDialog.xaml
    /// </summary>
    public partial class FractionDialog : Window
    {
        private QuestionFractionPart fractionPart;

        internal QuestionFractionPart FractionPart
        {
            get { return this.fractionPart; }
            set
            {
                this.fractionPart = value;
                if (value == null)
                    return;

                this.withPartTextBox.Text = value.WithPart.Content;
                this.denominatorTextBox.Text = value.Denominator.Content;
                this.numeratorTextBox.Text = value.Numerator.Content;
            }
        }

        internal string FractionImage
        {
            get { return Helper.CreateImageFile(this.fractionPart, "宋体"); }
        }

        public FractionDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.fractionPart = new QuestionFractionPart();
            this.fractionPart.WithPart = new QuestionContent(this.withPartTextBox.Text, ContentType.Text);
            this.fractionPart.Denominator = new QuestionContent(this.denominatorTextBox.Text, ContentType.Text);
            this.fractionPart.Numerator = new QuestionContent(this.numeratorTextBox.Text, ContentType.Text);

            this.DialogResult = true;
            this.Close();
        }

        private void numeratorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.okButton.IsEnabled = !string.IsNullOrEmpty(this.denominatorTextBox.Text) &&
                !string.IsNullOrEmpty(this.numeratorTextBox.Text);
        }
    }
}
