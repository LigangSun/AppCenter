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
using SoonLearning.Assessment.Data;
using SoonLearning.Assessment.Player.Data;
using System.Threading;
using System.Windows.Threading;

namespace SoonLearning.Assessment.Player.UserControls
{
    /// <summary>
    /// Interaction logic for EndExamUserControl.xaml
    /// </summary>
    public partial class EndExamUserControl : UserControl
    {
        private Exam exam;

        public EndExamUserControl()
        {
            InitializeComponent();
        }

        private void viewAnswerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.StartupUserControl.EndExam();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        internal void ShowExam(Exam exam)
        {
            this.exam = exam;
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.questionDocument.Document = BaseObjectToFlowDocument.CreateExamFlowDocument(exam, this.showAnswerCheckBox.IsChecked.Value, this.showResponseCheckBox.IsChecked.Value);
            }),
            DispatcherPriority.Background,
            null);
        }

        private void showAnswerCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.ShowExam(this.exam);
        }

        private void showResponseCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.ShowExam(this.exam);
        }

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            this.questionDocument.Print();
        }
    }
}
