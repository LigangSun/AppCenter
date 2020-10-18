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

namespace SoonLearning.TeachAppMaker.Dialogs
{
    /// <summary>
    /// Interaction logic for SubQuestionTypeDialog.xaml
    /// </summary>
    public partial class SubQuestionTypeDialog : Window
    {
        private QuestionType selectedQuestionType = QuestionType.MultiChoice;

        internal QuestionType SelectedQuestionType
        {
            get
            {
                return this.selectedQuestionType;
            }
        }

        public SubQuestionTypeDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void StackPanel_Click(object sender, RoutedEventArgs e)
        {
            if (e.Source is RadioButton)
            {
                RadioButton radioBtn = e.Source as RadioButton;
                if (radioBtn.IsChecked.Value)
                {
                    this.selectedQuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), radioBtn.Tag as string);
                }
            }
        }
    }
}
