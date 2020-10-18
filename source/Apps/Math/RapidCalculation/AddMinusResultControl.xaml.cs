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
using System.Windows.Media.Animation;
using Microsoft.Win32;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    /// <summary>
    /// Interaction logic for AddMinusResultControl.xaml
    /// </summary>
    public partial class AddMinusResultControl
    {
        private QuestionData questionData;
        private int totalPage = 1;
        private int currentPage = 0;
        private float opacityTime = 0.1f;

        private const int questonCountPerPage = 10;

        public AddMinusResultControl()
        {
            InitializeComponent();

        //    this.Title = ControlMgr.Instance.Entry.Title;
        }

        internal void ShowResult(string file)
        {
            this.ShowResult(QuestionData.Load(file));
        }

        internal void ShowResult(QuestionData questionData)
        {
            this.questionData = questionData;

            this.ShowResult();
        }

        private void ShowResult()
        {
            this.currentPage = 0;
            this.totalPage = this.questionData.Items.Count / questonCountPerPage;
            if (this.questionData.Items.Count % questonCountPerPage != 0)
                this.totalPage++;

            //   this.countPerPageLabel.Content = string.Format(SoonLearning.Math.Fast.Properties.Resources.questionCountPerPage, questonCountPerPage);
            this.UpdatePageInfo();

            int i = 0;
            foreach (Question_a_b_c_ResultControl ctrl in this.questionGrid.Children)
            {
                if (i >= this.questionData.Items.Count)
                    break;

                ctrl.DataContext = this.questionData.Items[i++];
                if (i == questonCountPerPage)
                    break;
            }

            int correct = 0;
            int incorrect = 0;
            int noAnswer = 0;
            foreach (Question_a_b_c q in questionData.Items)
            {
                if (q.IsCorrect == null)
                    noAnswer++;
                else if (!q.IsCorrect.Value)
                    incorrect++;
                else
                    correct++;
            }

            this.correctResultLabel.Content = string.Format(SoonLearning.Math.Fast.Properties.Resources.Corrrent, correct);
            this.incorrectResultLabel.Content = string.Format(SoonLearning.Math.Fast.Properties.Resources.InCorrect, incorrect);
            this.noAnswerResultLabel.Content = string.Format(SoonLearning.Math.Fast.Properties.Resources.NoAnswer, noAnswer);

            this.scoreLabel.Content = string.Format(SoonLearning.Math.Fast.Properties.Resources.Score, this.questionData.Score);

            ResultInfoUserControl resultControl = new ResultInfoUserControl();
            resultControl.Visibility = System.Windows.Visibility.Visible;
            if (this.questionData.MaxNumber == correct)
                resultControl.State = ResultState.Perfect;
            else if (this.questionData.PassQuizCorrectCount <= correct)
                resultControl.State = ResultState.Pass;
            else
                resultControl.State = ResultState.NotPass;
            resultControl.Show = true;
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.MathStartupControl.ShowSettingControl();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Filter = SoonLearning.Math.Fast.Properties.Resources.QuestionDataFilter;
            bool? ret = saveFileDlg.ShowDialog(App.Current.MainWindow);
            if (ret != null && ret.Value)
            {
                try
                {
                    this.questionData.Save(saveFileDlg.FileName);
                }
                catch (Exception ex)
                {
                    MessageWindow msgWnd = new MessageWindow();
                    msgWnd.ShowMessage(string.Format(SoonLearning.Math.Fast.Properties.Resources.SaveFail, ex.Message), MessageBoxButton.OK, null);
                }
            }
        }

        private void prePage_Click(object sender, RoutedEventArgs e)
        {
            if (this.prePage.IsVisible && this.prePage.IsEnabled)
                this.OpacityShowQuestionPage(0.0f, preOpacityAnimate_Completed);
        }

        private void preOpacityAnimate_Completed(object sender, EventArgs e)
        {
            this.questionGrid.Opacity = 0.0f;

            this.currentPage--;

            int count = 0;
            int index = this.currentPage * questonCountPerPage;
            foreach (Question_a_b_c_ResultControl ctrl in this.questionGrid.Children)
            {
                ctrl.Visibility = System.Windows.Visibility.Visible;
                ctrl.DataContext = this.questionData.Items[index++];
                count++;
                if (count == questonCountPerPage ||
                    index == this.questionData.Items.Count)
                    break;
            }

            this.OpacityShowQuestionPage(1.0f, EndSwtichPage);

            this.UpdatePageInfo();
        }

        private void nextPage_Click(object sender, RoutedEventArgs e)
        {
            if (this.nextPage.IsVisible && this.nextPage.IsEnabled)
                this.OpacityShowQuestionPage(0.0f, nextOpacityAnimate_Completed);
        }

        private void nextOpacityAnimate_Completed(object sender, EventArgs e)
        {
            this.questionGrid.Opacity = 0.0f;

            this.currentPage++;

            int count = 0;
            int index = this.currentPage * questonCountPerPage;
            foreach (Question_a_b_c_ResultControl ctrl in this.questionGrid.Children)
            {
                if (index == this.questionData.Items.Count)
                {
                    ctrl.Visibility = System.Windows.Visibility.Hidden;
                    continue;
                }
                else
                {
                    ctrl.Visibility = System.Windows.Visibility.Visible;
                }

                ctrl.DataContext = this.questionData.Items[index++];
                count++;
                if (count == questonCountPerPage)
                    break;
            }

            this.OpacityShowQuestionPage(1.0f, EndSwtichPage);

            this.UpdatePageInfo();
        }

        private void EndSwtichPage(object sender, EventArgs e)
        {
            this.prePage.IsEnabled = true;
            this.nextPage.IsEnabled = true;
        }

        private void OpacityShowQuestionPage(float to, EventHandler handler)
        {
            DoubleAnimation opacityAnim = new DoubleAnimation(to, TimeSpan.FromSeconds(opacityTime), FillBehavior.HoldEnd);
            if (handler != null)
                opacityAnim.Completed += handler;

            this.prePage.IsEnabled = false;
            this.nextPage.IsEnabled = false;

            this.questionGrid.BeginAnimation(Grid.OpacityProperty, opacityAnim);
        }

        private void UpdatePageInfo()
        {
        //    this.Index = this.currentPage;
        //    this.Total = this.totalPage;
         //   this.pageIndexLabel.Content = string.Format(SoonLearning.Math.Fast.Properties.Resources.PageInfo, this.currentPage + 1, this.totalPage);

            if (this.totalPage == 1)
            {
                this.prePage.Visibility = System.Windows.Visibility.Hidden;
                this.nextPage.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                if (this.currentPage == 0)
                {
                    this.prePage.Visibility = System.Windows.Visibility.Hidden;
                    this.nextPage.Visibility = System.Windows.Visibility.Visible;
                }
                else if (this.currentPage == this.totalPage - 1)
                {
                    this.prePage.Visibility = System.Windows.Visibility.Visible;
                    this.nextPage.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    this.prePage.Visibility = System.Windows.Visibility.Visible;
                    this.nextPage.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        private void printBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.MathStartupControl.ShowPrintControl(this.questionData);
        }

        private void GadgetUserControl_Initialized(object sender, EventArgs e)
        {
            
        }

        private void GadgetUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.questionGrid.Children.Clear();
            this.questionGrid.RowDefinitions.Clear();
            this.questionGrid.ColumnDefinitions.Clear();

            string maxNumber = MathSetting.Instance.CurrentQuestionData.MaxResultNumber.ToString();
           
            TextBlock testTextWidthLabel = new TextBlock();
            testTextWidthLabel.Text = maxNumber;
            testTextWidthLabel.FontSize = 18f;
            testTextWidthLabel.FontWeight = FontWeights.Medium;
            testTextWidthLabel.HorizontalAlignment = HorizontalAlignment.Center;
            this.questionGrid.Children.Add(testTextWidthLabel);
            this.questionGrid.UpdateLayout();
            double abColumeWidth = testTextWidthLabel.ActualWidth + 10;
            this.questionGrid.Children.Remove(testTextWidthLabel);

            int row = questonCountPerPage;
            for (int i = 0; i < row; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new System.Windows.GridLength(1.0f, System.Windows.GridUnitType.Star);
                this.questionGrid.RowDefinitions.Add(rowDef);
            }

            for (int i = 0; i < 10; i++)
            {
                Control ctrl = new Question_a_b_c_ResultControl(24f, FontWeights.Medium, abColumeWidth);
                ctrl.Margin = new Thickness(3);
                ctrl.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                Grid.SetRow(ctrl, i);
                Grid.SetColumn(ctrl, 0);
                this.questionGrid.Children.Add(ctrl);
            }

            this.ShowResult();
        }
    }
}
