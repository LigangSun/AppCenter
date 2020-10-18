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
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;
using SoonLearning.AppCenter.Controls;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    /// <summary>
    /// Interaction logic for RapidHistoryUserControl.xaml
    /// </summary>
    public partial class RapidHistoryUserControl : UserControl
    {
        public RapidHistoryUserControl()
        {
            InitializeComponent();

            this.historyListView.ItemsSource = MathSetting.Instance.QuestionDataCollection;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void viewDetailButton_Click(object sender, RoutedEventArgs e)
        {
            QuestionData questionData = this.historyListView.SelectedItem as QuestionData;
            if (questionData == null)
                return;

            ControlMgr.Instance.MathStartupControl.ShowDetailControl(questionData);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            QuestionData questionData = this.historyListView.SelectedItem as QuestionData;
            if (questionData == null)
                return;

            MessageWindow msgWnd = new MessageWindow();
            msgWnd.ShowMessage("你确定要删除所选的测验吗？", MessageBoxButton.OKCancel, MessageWindowCallback);
        }

        private void retryButton_Click(object sender, RoutedEventArgs e)
        {
            QuestionData questionData = this.historyListView.SelectedItem as QuestionData;
            if (questionData == null)
                return;

            MathSetting.Instance.CurrentQuestionData = questionData;
            ControlMgr.Instance.MathStartupControl.RetryQuiz();
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            QuestionData questionData = this.historyListView.SelectedItem as QuestionData;
            if (questionData == null)
                return;

            MathSetting.Instance.CurrentQuestionData = questionData;
            ControlMgr.Instance.MathStartupControl.ContinueQuiz();
        }

        private void historyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            QuestionData questionData = this.historyListView.SelectedItem as QuestionData;
            this.viewDetailButton.IsEnabled = 
                this.retryButton.IsEnabled = 
                this.continueButton.IsEnabled = 
                this.deleteButton.IsEnabled = questionData != null;

            if (questionData == null)
                return;

            if (this.continueButton.IsEnabled)
            {
                this.continueButton.IsEnabled = (questionData.LeftTime > 0);
            }
        }

        private void MessageWindowCallback(bool ok)
        {
            if (ok)
            {
                QuestionData questionData = this.historyListView.SelectedItem as QuestionData;
                if (questionData == null)
                    return;

                MathSetting.Instance.QuestionDataCollection.Remove(questionData);

                Assembly assembly = Assembly.GetExecutingAssembly();
                string dataFolder = System.IO.Path.GetDirectoryName(assembly.Location);
                dataFolder = System.IO.Path.Combine(dataFolder, "Data\\Rapid");
                if (!System.IO.Directory.Exists(dataFolder))
                    System.IO.Directory.CreateDirectory(dataFolder);

                try
                {
                    File.Delete(System.IO.Path.Combine(dataFolder, questionData.Id + "." + QuestionData.ext));
                }
                catch
                {
                }
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.MathStartupControl.ShowSettingControl();
        }
    }
}
