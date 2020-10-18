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

namespace SoonLearning.TeachAppMaker.Questions
{
    /// <summary>
    /// Interaction logic for FIBReferenceAnswerUserControl.xaml
    /// </summary>
    public partial class FIBReferenceAnswerUserControl : UserControl, IEditQuestion
    {
        private QuestionBlank questionBlank;
        private ObservableCollection<QuestionContent> referenceAnswerList = new ObservableCollection<QuestionContent>();

        public FIBReferenceAnswerUserControl(QuestionBlank blank)
        {
            InitializeComponent();

            this.questionBlank = blank;

            foreach (var refAnswer in this.questionBlank.ReferenceAnswerList)
            {
                this.referenceAnswerList.Add(refAnswer.Clone() as QuestionContent);
            }

            this.referenceAnswerListView.ItemsSource = referenceAnswerList;
        }

        internal void addReferenceAnswer()
        {
            QuestionContent content = new QuestionContent();
            this.referenceAnswerList.Add(content);
        }

        internal void deleteReferenceAnswer()
        {
            if (this.referenceAnswerListView.SelectedItem == null)
                return;

            this.referenceAnswerList.Remove(this.referenceAnswerListView.SelectedItem as QuestionContent);
        }

        public bool Save()
        {
            if (this.questionBlank == null)
                return false;

            this.questionBlank.ReferenceAnswerList.Clear();
            foreach (var refAnswer in this.referenceAnswerList)
            {
                this.questionBlank.ReferenceAnswerList.Add(refAnswer);
            }

            return true;
        }
    }
}
