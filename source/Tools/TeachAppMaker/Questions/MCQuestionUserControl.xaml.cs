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
using System.Collections.ObjectModel;
using SoonLearning.TeachAppMaker.Data;

namespace SoonLearning.TeachAppMaker.Questions
{
    /// <summary>
    /// Interaction logic for MCQuestionUserControl.xaml
    /// </summary>
    public partial class MCQuestionUserControl : UserControl, IEditQuestion
    {
        private MCQuestion mcQuestion;
        private ObservableCollection<QuestionOption> tempOptionList = new ObservableCollection<QuestionOption>();

        public MCQuestionUserControl(MCQuestion mcQuestion)
        {
            InitializeComponent();

            this.mcQuestion = mcQuestion;
            this.optionListView.ItemsSource = this.tempOptionList;

            foreach (QuestionOption option in this.mcQuestion.QuestionOptionCollection)
            {
                this.tempOptionList.Add(option.Clone() as QuestionOption);
            }

            if (this.mcQuestion.Content != null)
                this.richTextEditor.Text = this.mcQuestion.Content.Content;
        }

        bool IEditQuestion.Save()
        {
            if (string.IsNullOrEmpty(this.richTextEditor.Text))
            {
                MessageBox.Show("试题内容不能为空！", "单选题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (this.tempOptionList.Count < 2)
            {
                MessageBox.Show("单选题选项数量不能少于两个！", "单选题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false; 
            }

            foreach (QuestionOption option in this.tempOptionList)
            {
                if (string.IsNullOrEmpty(option.OptionContent.Content))
                {
                    MessageBox.Show("选项内容不能为空！", "单选题", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false; 
                }
            }

            bool ok = false;
            foreach (QuestionOption option in this.tempOptionList)
            {
                if (option.IsCorrect)
                {
                    ok = true;
                    break;
                }
            }

            if (!ok)
            {
                MessageBox.Show("请为该单选题设置正确答案！", "选择题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            this.mcQuestion.Content.Content = this.richTextEditor.Text;
            this.mcQuestion.Content.ContentType = ContentType.FlowDocument;

            this.mcQuestion.QuestionOptionCollection.Clear();
            foreach (QuestionOption option in this.tempOptionList)
            {
                option.OptionContent.ContentType = ContentType.FlowDocument;
                this.mcQuestion.QuestionOptionCollection.Add(option);
            }

            foreach (var part in this.richTextEditor.Parts)
            {
                if (this.mcQuestion.Content.Content.Contains(part.PlaceHolder))
                    this.mcQuestion.Content.QuestionPartCollection.Add(part);
            }

            return true;
        }

        private void addOptionButton_Click(object sender, RoutedEventArgs e)
        {
            this.tempOptionList.Add(ProjectMgr.Instance.CreateOption(this.tempOptionList.Count));
        }

        private void deleteOptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.optionListView.SelectedItem == null)
                return;

            this.tempOptionList.RemoveAt(this.optionListView.SelectedIndex);
            int i = 0;
            foreach (QuestionOption option in this.tempOptionList)
            {
                option.Index = i++;
            }
        }

        private void optionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.deleteOptionButton.IsEnabled = this.optionListView.SelectedItem != null;
        }
    }
}
