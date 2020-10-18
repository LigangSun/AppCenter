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
using SoonLearning.TeachAppMaker.Data;
using SoonLearning.TeachAppMaker.Commands;
using SoonLearning.Assessment.Data;

namespace SoonLearning.TeachAppMaker.Questions
{
    /// <summary>
    /// Interaction logic for QuestionListUserControl.xaml
    /// </summary>
    public partial class QuestionListUserControl : UserControl
    {
        private ListCollectionView aquestionListCollectionView;
        private QuestionType type;

        public QuestionListUserControl()
        {
            InitializeComponent();

            this.aquestionListCollectionView = (ListCollectionView)CollectionViewSource.GetDefaultView(ProjectMgr.Instance.App.Items);
        }

        internal void ShowQuestion(QuestionType type)
        {
            this.questionListView.ItemsSource = ProjectMgr.Instance.App.Items;
            this.type = type;
            this.filterQuestions();
        }

        private void questionListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Question question = this.questionListView.SelectedItem as Question;
            if (question == null)
                return;

            EditCommands.EditQuestionCommand.Execute(question);
        }

        private void questionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProjectMgr.Instance.SelectedQuestion = this.questionListView.SelectedItem as Question;
        }

        private void filterQuestions()
        {
            if (this.aquestionListCollectionView == null)
                return;

            this.aquestionListCollectionView.Filter -= q_Filter;
            this.aquestionListCollectionView.Filter += new Predicate<object>(q_Filter);
        }

        private bool q_Filter(object item)
        {
            Question question = item as Question;
            if (question == null)
                return false;

            if (type == QuestionType.None)
                return true;

            return question.Type == type;
        }
    }
}
