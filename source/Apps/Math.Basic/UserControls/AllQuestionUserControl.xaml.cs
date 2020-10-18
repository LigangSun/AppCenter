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
using SoonLearning.Math.Data;
using System.Windows.Markup;
using System.IO;
using Math.Basic.Data;

namespace Math.Basic.UserControls
{
    /// <summary>
    /// Interaction logic for AllQuestionUserControl.xaml
    /// </summary>
    public partial class AllQuestionUserControl : UserControl
    {
        private Exercise exercise;

        public AllQuestionUserControl()
        {
            InitializeComponent();
        }

        internal void ShowAllQuestion(Exercise exercise, bool canBackExercise)
        {
            //if (canBackExercise)
            //    this.continueButton.Visibility = System.Windows.Visibility.Visible;
            //else
            //    this.continueButton.Visibility = System.Windows.Visibility.Hidden;

            this.exercise = exercise;
            this.showExercise();
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            this.questionDocument.Print();
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.StartupUserControl.GoBack();
        }

        private void showAnswerCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.showExercise();
        }

        private void showResponseCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.showExercise();
        }

        private void showExercise()
        {
            this.questionDocument.Document = BaseObjectToFlowDocument.CreateExerciseFlowDocument(exercise,
                this.showAnswerCheckBox.IsChecked.Value,
                this.showResponseCheckBox.IsChecked.Value);
        }
    }
}
