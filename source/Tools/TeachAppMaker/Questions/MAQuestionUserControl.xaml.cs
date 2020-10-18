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
    /// Interaction logic for MAQuestionUserControl.xaml
    /// </summary>
    public partial class MAQuestionUserControl : UserControl, IEditQuestion
    {
        private MAQuestion maQuestion = null;
        private ObservableCollection<MAQuestionOption> tempOptionList = new ObservableCollection<MAQuestionOption>();

        public MAQuestionUserControl(MAQuestion maQuestion)
        {
            InitializeComponent();

            this.maQuestion = maQuestion;
            this.optionListView.ItemsSource = this.tempOptionList;

            foreach (var option in this.maQuestion.OptionList)
            {
                this.tempOptionList.Add(option.Clone() as MAQuestionOption);
            }

            this.richTextEditor.Text = this.maQuestion.Content.Content;
        }

        private void addOptionButton_Click(object sender, RoutedEventArgs e)
        {
            this.tempOptionList.Add(ProjectMgr.Instance.CreateMAOption(this.tempOptionList.Count));
        }

        private void deleteOptionButton_Click(object sender, RoutedEventArgs e)
        {
            this.tempOptionList.RemoveAt(this.optionListView.SelectedIndex);

            int i = 0;
            foreach (MAQuestionOption option in this.tempOptionList)
            {
                option.Index = i++;
            }
        }

        private void optionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.deleteOptionButton.IsEnabled = this.optionListView.SelectedItem != null;
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(this.richTextEditor.Text))
            {
                MessageBox.Show("试题内容不能为空！", "配对题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (this.tempOptionList.Count < 2)
            {
                MessageBox.Show("配对题的配对项不能少于两个！", "配对题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            foreach (var option in this.tempOptionList)
            {
                if (string.IsNullOrEmpty(option.LeftContent.Content) ||
                    string.IsNullOrEmpty(option.RightContent.Content))
                {
                    MessageBox.Show("配对项内容不能为空！", "配对题", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }

            this.maQuestion.Content.Content = this.richTextEditor.Text;
            this.maQuestion.Content.ContentType = ContentType.FlowDocument;

            this.maQuestion.OptionList.Clear();
            foreach (var option in this.tempOptionList)
            {
                this.maQuestion.OptionList.Add(option);
            }

            foreach (var part in this.richTextEditor.Parts)
            {
                if (this.maQuestion.Content.Content.Contains(part.PlaceHolder))
                    this.maQuestion.Content.QuestionPartCollection.Add(part);
            }

            return true;
        }
    }
}
