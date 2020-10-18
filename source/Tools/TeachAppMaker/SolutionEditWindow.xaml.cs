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

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for SolutionEditWindow.xaml
    /// </summary>
    public partial class SolutionEditWindow : Window
    {
        private QuestionContent solution;
        public SolutionEditWindow(QuestionContent solution)
        {
            InitializeComponent();

            this.solution = solution;
            this.richTextEditor.Text = this.solution.Content;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.solution.Content = this.richTextEditor.Text;
            this.solution.ContentType = ContentType.FlowDocument;
            this.DialogResult = true;
            this.Close();
        }
    }
}
