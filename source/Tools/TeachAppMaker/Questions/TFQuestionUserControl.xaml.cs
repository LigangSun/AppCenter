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
using System.Collections.ObjectModel;
using SoonLearning.Assessment.Data;

namespace SoonLearning.TeachAppMaker.Questions
{
    /// <summary>
    /// Interaction logic for TFQuestionUserControl.xaml
    /// </summary>
    public partial class TFQuestionUserControl : UserControl, IEditQuestion
    {
        private TFQuestion tfQuestion = null;
        private ObservableCollection<QuestionOption> tempOptionList = new ObservableCollection<QuestionOption>();

        public TFQuestionUserControl(TFQuestion tfQuestion)
        {
            InitializeComponent();

            this.tfQuestion = tfQuestion;
            this.optionListView.ItemsSource = this.tempOptionList;

            foreach (QuestionOption option in this.tfQuestion.QuestionOptionCollection)
            {
                this.tempOptionList.Add(option.Clone() as QuestionOption);
            }

            this.richTextEditor.Text = this.tfQuestion.Content.Content;
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(this.richTextEditor.Text))
            {
                MessageBox.Show("试题内容不能为空！", "判断题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            foreach (QuestionOption option in this.tempOptionList)
            {
                if (string.IsNullOrEmpty(option.OptionContent.Content))
                {
                    MessageBox.Show("选项内容不能为空！", "判断题", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                MessageBox.Show("请为该判断题设置正确答案！", "判断题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            this.tfQuestion.Content.Content = this.richTextEditor.Text;
            this.tfQuestion.Content.ContentType = ContentType.FlowDocument;

            this.tfQuestion.QuestionOptionCollection.Clear();
            foreach (QuestionOption option in this.tempOptionList)
            {
                option.OptionContent.ContentType = ContentType.FlowDocument;
                this.tfQuestion.QuestionOptionCollection.Add(option);
            }

            return true;
        }
    }
}
