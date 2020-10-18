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
    /// Interaction logic for MRQuestionUserControl.xaml
    /// </summary>
    public partial class MRQuestionUserControl : UserControl, IEditQuestion
    {
        private MRQuestion mrQuestion;
        private ObservableCollection<QuestionOption> tempOptionList = new ObservableCollection<QuestionOption>();

        public MRQuestionUserControl(MRQuestion mrQuestion)
        {
            InitializeComponent();

            this.mrQuestion = mrQuestion;
            this.optionListView.ItemsSource = this.tempOptionList;

            foreach (QuestionOption option in this.mrQuestion.QuestionOptionCollection)
            {
                this.tempOptionList.Add(option.Clone() as QuestionOption);
            }

            if (this.mrQuestion.Content != null)
                this.richTextEditor.Text = this.mrQuestion.Content.Content;
        }

        bool IEditQuestion.Save()
        {
            if (string.IsNullOrEmpty(this.richTextEditor.Text))
            {
                MessageBox.Show("试题内容不能为空！", "多选题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (this.tempOptionList.Count < 2)
            {
                MessageBox.Show("多选题的配对项不能少于两个！", "多选题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            foreach (QuestionOption option in this.tempOptionList)
            {
                if (string.IsNullOrEmpty(option.OptionContent.Content))
                {
                    MessageBox.Show("选项内容不能为空！", "多选题", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                MessageBox.Show("请为该多选题设置正确答案！", "多选题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            this.mrQuestion.Content.Content = this.richTextEditor.Text;
            this.mrQuestion.Content.ContentType = ContentType.FlowDocument;

            this.mrQuestion.QuestionOptionCollection.Clear();
            foreach (QuestionOption option in this.tempOptionList)
            {
                option.OptionContent.ContentType = ContentType.FlowDocument;
                this.mrQuestion.QuestionOptionCollection.Add(option);
            }

            foreach (var part in this.richTextEditor.Parts)
            {
                if (this.mrQuestion.Content.Content.Contains(part.PlaceHolder))
                    this.mrQuestion.Content.QuestionPartCollection.Add(part);
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
