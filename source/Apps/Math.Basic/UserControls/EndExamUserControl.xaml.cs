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
using Math.Basic.Data;

namespace Math.Basic.UserControls
{
    /// <summary>
    /// Interaction logic for EndExamUserControl.xaml
    /// </summary>
    public partial class EndExamUserControl : UserControl
    {
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
            this.questionDocument.Document = BaseObjectToFlowDocument.CreateExamFlowDocument(exam, true, true);
        }
    }
}
