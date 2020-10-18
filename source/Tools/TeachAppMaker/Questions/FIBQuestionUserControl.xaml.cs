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
    /// Interaction logic for FIBQuestionControl.xaml
    /// </summary>
    public partial class FIBQuestionUserControl : UserControl, IEditQuestion
    {
        private FIBQuestion fibQuestion;
        private ObservableCollection<QuestionBlank> tempOptionList = new ObservableCollection<QuestionBlank>(); 

        private QuestionContentPartAddedDelegate questionContentPartAddedHandler;
        private QuestionContentPartRemovedDelegate questionContentPartRemovedHandler;

        public FIBQuestionUserControl(FIBQuestion fibQuestion)
        {
            InitializeComponent();

            this.fibQuestion = fibQuestion;
            this.richTextEditor.Text = this.fibQuestion.Content.Content;

            this.prepareEventHandler();

            int index = 0;
            foreach (var blank in this.fibQuestion.QuestionBlankCollection)
            {
                this.addReferenceAnswerTabItem(string.Format("空{0}", ++index), blank as QuestionBlank);
            }

            if (index > 0)
                this.referenceAnswerTabCtrl.SelectedIndex = 0;
        }

        private void prepareEventHandler()
        {
            this.questionContentPartAddedHandler = (part) =>
            {
                if (part is QuestionBlank)
                {
                    var query = from temp in this.richTextEditor.Parts
                                where temp is QuestionBlank
                                select temp;

                    this.addReferenceAnswerTabItem(string.Format("空{0}", query.Count() + 1), part as QuestionBlank);
                }
            };

            this.questionContentPartRemovedHandler = (part) =>
            {
                if (part is QuestionBlank)
                    this.removeReferenceAnswerTabItem(part as QuestionBlank);
            };
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.richTextEditor.Parts.QuestionContentPartAddedEvent += this.questionContentPartAddedHandler;
            this.richTextEditor.Parts.QuestionContentPartRemovedEvent += this.questionContentPartRemovedHandler;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            this.richTextEditor.Parts.QuestionContentPartAddedEvent -= this.questionContentPartAddedHandler;
            this.richTextEditor.Parts.QuestionContentPartRemovedEvent -= this.questionContentPartRemovedHandler;
        }

        private void addReferenceAnswerTabItem(string header, QuestionBlank blank)
        {
            TabItem tabItem = new TabItem();
            tabItem.Header = header;
            tabItem.Tag = blank;
            tabItem.Width = 60;
            FIBReferenceAnswerUserControl referenceAnswerControl = new FIBReferenceAnswerUserControl(blank);
            tabItem.Content = referenceAnswerControl;
            this.referenceAnswerTabCtrl.Items.Add(tabItem);
            tabItem.IsSelected = true;
        }

        private void removeReferenceAnswerTabItem(QuestionBlank blank)
        {
            foreach (TabItem tabItem in this.referenceAnswerTabCtrl.Items)
            {
                if (tabItem.Tag == blank)
                {
                    this.referenceAnswerTabCtrl.Items.Remove(tabItem);
                    break;
                }
            }
        }

        private void addReferenceAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            TabItem item = this.referenceAnswerTabCtrl.SelectedItem as TabItem;
            if (item == null)
                return;

            FIBReferenceAnswerUserControl fibReferenceAnswerUserControl = item.Content as FIBReferenceAnswerUserControl;
            if (fibReferenceAnswerUserControl == null)
                return;

            fibReferenceAnswerUserControl.addReferenceAnswer();
        }

        private void deleteReferenceAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            TabItem item = this.referenceAnswerTabCtrl.SelectedItem as TabItem;
            if (item == null)
                return;

            FIBReferenceAnswerUserControl fibReferenceAnswerUserControl = item.Content as FIBReferenceAnswerUserControl;
            if (fibReferenceAnswerUserControl == null)
                return;

            fibReferenceAnswerUserControl.deleteReferenceAnswer();
        }

        private void referenceAnswerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        public bool Save()
        {
            if (string.IsNullOrEmpty(this.richTextEditor.Text))
            {
                MessageBox.Show("试题内容不能为空！", "填空题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (this.referenceAnswerTabCtrl.Items.Count == 0)
            {
                MessageBox.Show("填空题至少要有一个空！", "填空题", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            this.fibQuestion.Content.Content = this.richTextEditor.Text;
            this.fibQuestion.Content.ContentType = ContentType.FlowDocument;

            this.fibQuestion.QuestionBlankCollection.Clear();

            foreach (var part in this.richTextEditor.Parts)
            {
                if (this.fibQuestion.Content.Content.Contains(part.PlaceHolder))
                    this.fibQuestion.Content.QuestionPartCollection.Add(part);
            }

            foreach (TabItem item in this.referenceAnswerTabCtrl.Items)
            {
                this.fibQuestion.QuestionBlankCollection.Add(item.Tag as QuestionBlank);

                FIBReferenceAnswerUserControl refAnswerCtrl = item.Content as FIBReferenceAnswerUserControl;
                if (refAnswerCtrl == null)
                    continue;

                refAnswerCtrl.Save();
            }

            return true;
        }
    }
}
