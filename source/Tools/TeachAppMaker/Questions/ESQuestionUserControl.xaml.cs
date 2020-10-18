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

namespace SoonLearning.TeachAppMaker.Questions
{
    /// <summary>
    /// Interaction logic for ESQuestionControl.xaml
    /// </summary>
    public partial class ESQuestionUserControl : UserControl, IEditQuestion
    {
        private ESQuestion esQuestion;

        public ESQuestionUserControl(ESQuestion esQuestion)
        {
            InitializeComponent();

            this.esQuestion = esQuestion;

            this.richTextEditor.Text = esQuestion.Content.Content;
            this.refAnswerRichTextEditor.Text = esQuestion.ReferenceAnswer.Content;
        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(this.richTextEditor.Text))
            {
                MessageBox.Show("试题内容不能为空！", "简答题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            this.esQuestion.Content.Content = this.richTextEditor.Text;
            this.esQuestion.Content.ContentType = ContentType.FlowDocument;

            this.esQuestion.ReferenceAnswer.Content = this.refAnswerRichTextEditor.Text;
            this.esQuestion.ReferenceAnswer.ContentType = ContentType.FlowDocument;

            return true;
        }
    }
}
