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
using SoonLearning.Assessment.Data;
using SoonLearning.TeachAppMaker.Data;

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for QuestionEditWindow.xaml
    /// </summary>
    public partial class QuestionEditWindow : Window
    {
        private Question editingQuestion;
        private bool save;
        private bool subQuestion;

        public QuestionEditWindow(Question question, bool subQuestion, bool copy)
        {
            InitializeComponent();

            this.subQuestion = subQuestion;
            this.editingQuestion = question;
            this.questionUserControl.Init(question);

            string typeName = string.Empty;
            switch (question.Type)
            {
                case QuestionType.Composite:
                    typeName = "复合题";
                    break;
                case QuestionType.Essay:
                    typeName = "简答题";
                    break;
                case QuestionType.FillInBlank:
                    typeName = "填空题";
                    break;
                case QuestionType.Match:
                    typeName = "配对题";
                    break;
                case QuestionType.MultiChoice:
                    typeName = "单选题";
                    break;
                case QuestionType.MultiResponse:
                    typeName = "多选题";
                    break;
                case QuestionType.Table:
                    typeName = "表格题";
                    break;
                case QuestionType.TrueFalse:
                    typeName = "判断题";
                    break;
                case QuestionType.VerticalForm:
                    typeName = "竖式题";
                    break;
            }

            this.Title = string.Format("{0} - {1}", "编辑试题", typeName);
            if (copy)
                this.Title = this.Title += "(编辑副本)";
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.questionUserControl.Save())
                return;

            this.save = true;

            if (!this.subQuestion)
            {
                if (!ProjectMgr.Instance.App.Items.Contains(this.editingQuestion))
                {
                    ProjectMgr.Instance.App.Items.Add(this.editingQuestion);
                    ProjectMgr.Instance.Changed = true;
                }
            }

            this.DialogResult = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.save)
                return;

            MessageBoxResult ret = MessageBox.Show("你确定要退出编辑吗？ 取消后已修改的内容会被放弃！", "编辑试题", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ret == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }

        private void saveAndCreateNextButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.questionUserControl.Save())
                return;

            this.save = false;

            if (!this.subQuestion)
            {
                if (!ProjectMgr.Instance.App.Items.Contains(this.editingQuestion))
                {
                    ProjectMgr.Instance.App.Items.Add(this.editingQuestion);
                    ProjectMgr.Instance.Changed = true;
                }
            }

            this.editingQuestion = ProjectMgr.Instance.CreateQuestion(this.editingQuestion.Type);
            this.questionUserControl.Init(this.editingQuestion);
        }
    }
}
