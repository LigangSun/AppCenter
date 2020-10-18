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
using SoonLearning.Math.Data;
using SoonLearning.AppCenter;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    /// <summary>
    /// Interaction logic for PrintWindow.xaml
    /// </summary>
    public partial class PrintWindow
    {
        private QuestionData questionData;
        public PrintWindow()
        {
            InitializeComponent();
        }

        internal void Load(QuestionData questionData)
        {
            this.questionData = questionData;
            this.LoadQuestion();
        }

        private void LoadQuestion()
        {
            if (this.questionData == null)
                return;

            string title = string.Empty;
            //switch (MathSetting.Instance.Type)
            //{
            //    case MathType.Add_Minus:
            //        title = SoonLearning.Math.Fast.Properties.Resources.mathAddMinusTitle;
            //        break;
            //    case MathType.Add_Minus_Multiply_Division:
            //        title = SoonLearning.Math.Fast.Properties.Resources.mathAddMinusMultiplyDivisionTitle;
            //        break;
            //    case MathType.Multiply_Division:
            //        title = SoonLearning.Math.Fast.Properties.Resources.mathMultiplyDivisionTitle;
            //        break;
            //    case MathType.Nine_Nine_Table:
            //        title = SoonLearning.Math.Fast.Properties.Resources.NineXNineTableTitle;
            //        break;
            //}

            PrintInfo info = new PrintInfo();
            info.Title = title;
            info.ShowAnswer = this.printAnswer.IsChecked.Value;
            info.ShowResult = this.printResult.IsChecked.Value;
            info.CountPerLine = this.questionCountPerLineCbx.SelectedIndex + 1;
            info.ShowDateTime = true;
            this.questionDocumentViewer.Document = this.questionData.ToFlowDocument(info);
        }

        private void printBtn_Click(object sender, RoutedEventArgs e)
        {
            this.questionDocumentViewer.Print();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void printResult_Click(object sender, RoutedEventArgs e)
        {
            this.LoadQuestion();
        }

        private void printAnswer_Click(object sender, RoutedEventArgs e)
        {
            this.LoadQuestion();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.MathStartupControl.GoBack();
        }

        private void printResult_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadQuestion();
        }

        private void printPreviewBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
