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
using SoonLearning.TeachAppMaker.Commands;

namespace SoonLearning.TeachAppMaker.Questions
{
    /// <summary>
    /// Interaction logic for CPQuestionUserControl.xaml
    /// </summary>
    public partial class CPQuestionUserControl : UserControl, IEditQuestion
    {
        private CPQuestion cpQuestion;
        private ObservableCollection<Question> subQuestionTempCollection = new ObservableCollection<Question>();

        public CPQuestionUserControl(CPQuestion cpQuestion)
        {
            InitializeComponent();

            this.cpQuestion = cpQuestion;
            this.richTextEditor.Text = cpQuestion.Content.Content;

            foreach (Question q in this.cpQuestion.SubQuestions)
            {
                this.subQuestionTempCollection.Add(q.Clone() as Question);
            }

            this.questionListView.ItemsSource = this.subQuestionTempCollection;
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(this.richTextEditor.Text))
            {
                MessageBox.Show("试题内容不能为空！", "复合题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            this.cpQuestion.Content.Content = this.richTextEditor.Text;
            this.cpQuestion.Content.ContentType = ContentType.FlowDocument;

            this.cpQuestion.SubQuestions.Clear();
            foreach (var q in this.subQuestionTempCollection)
                this.cpQuestion.SubQuestions.Add(q);

            return true;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            EditCommands.AddSubQuestionCommand.Execute(this.subQuestionTempCollection);
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.questionListView.SelectedIndex < 0)
                return;

            if (MessageBox.Show("你确定要删除选中的子题吗?", "删除子题", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.subQuestionTempCollection.RemoveAt(this.questionListView.SelectedIndex);
            }
        }

        private void questionListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            EditCommands.EditQuestionCommand.Execute(this.questionListView.SelectedItem as Question);
        }

        private void questionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.addButton.IsEnabled = this.removeButton.IsEnabled = this.questionListView.SelectedItem != null;
        }
    }
}
