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
using SoonLearning.AppCenter.Controls;
using SoonLearning.Assessment.Player.CommonControl;
using HTMLConverter;

namespace SoonLearning.Assessment.Player.UserControls
{
    /// <summary>
    /// Interaction logic for FIBQuestionUserControl.xaml
    /// </summary>
    public partial class FIBQuestionUserControl : UserControl
    {
        private FIBQuestion fibQuestion;
        private FIBQuestionResponse fibResponse;
        private int index;

        public bool IsCorrect
        {
            get
            {
                int blankCount = 0;
                foreach (QuestionContentPart part in fibQuestion.QuestionBlankCollection)
                {
                    if (part is QuestionBlank)
                        blankCount++;
                }

                if (blankCount != fibResponse.BlankResponseList.Count)
                    return false;

                foreach (QuestionBlankResponse response in fibResponse.BlankResponseList)
                {
                    if (!fibQuestion.IsContentCorrect(response.ObjectId, response.BlankContent))
                        return false;
                }

                return true;
            }
        }

        public FIBQuestionUserControl(FIBQuestion fibQuestion, FIBQuestionResponse fibResponse, int index)
        {
            this.fibQuestion = fibQuestion;
            this.fibResponse = fibResponse;
            this.index = index;

            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            this.ShowQuestion();
        }

        private void ShowQuestion()
        {
            if (this.fibQuestion.ShowBlankInContent)
            {
                this.ShowQuestionWithBlankInContent();
            }
            else
            {
                this.ShowQuestionWithBlankDividual();
            }
        }

        private void ShowQuestionWithBlankDividual()
        {
            string textPart = this.fibQuestion.ClearContent;
            Label questionBodyLabel = new Label();
            questionBodyLabel.Content = string.Format("{0}. {1}", this.index + 1, textPart);
            questionBodyLabel.FontSize = 20;
            questionBodyLabel.FontWeight = FontWeights.Medium;
            this.questionBodyPanel.Children.Add(questionBodyLabel);

            int index = 0;
            foreach (QuestionBlank blank in this.fibQuestion.QuestionBlankCollection)
            {
                this.CreateBlank(blank, index++);
            }
        }

        private void ShowQuestionWithBlankInContent()
        {
            Label indexLabel = new Label();
            indexLabel.FontSize = 20;
            indexLabel.Margin = new Thickness(0);
            indexLabel.Content = string.Format("{0}. ", this.index + 1);
            indexLabel.FontWeight = FontWeights.Medium;
            indexLabel.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.questionBodyPanel.Children.Add(indexLabel);

            Panel panel = CommonControlCreator.CreateContentControl(this.fibQuestion.Content, null, this.Foreground, OnContentPartCreated);
            this.questionBodyPanel.Children.Add(panel);
        }

        private void OnContentPartCreated(UIElement element)
        {
            if (element is TextBox)
            {
                TextBox tb = element as TextBox;
                tb.LostFocus += new RoutedEventHandler(blankTextBox_LostFocus);
                tb.TextChanged += new TextChangedEventHandler(blankTextBox_TextChanged);
            }
            else if (element is RichTextBox)
            {
                RichTextBox rtb = element as RichTextBox;
                rtb.LostFocus += new RoutedEventHandler(blankTextBox_LostFocus);
                rtb.TextChanged += new TextChangedEventHandler(blankTextBox_TextChanged);
            }
        }

        private void CreateBlank(QuestionBlank blank, int index)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Margin = new Thickness(2);
            this.blankPanel.Children.Add(panel);

            // Index
            Label indexLabel = new Label();
            indexLabel.Content = string.Format("{0}).", index + 1);
            indexLabel.FontSize = 20;
            indexLabel.Width = 60;
            indexLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            indexLabel.Margin = new Thickness(5);
            panel.Children.Add(indexLabel);

            // Blank
            NumberOnlyTextBox blankTextBox = new NumberOnlyTextBox();
            blankTextBox.Tag = blank;
            blankTextBox.Width = 80;
            blankTextBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            blankTextBox.FontSize = 20;
            blankTextBox.Margin = new Thickness(5);
            blankTextBox.LostFocus += new RoutedEventHandler(blankTextBox_LostFocus);
            blankTextBox.TextChanged += new TextChangedEventHandler(blankTextBox_TextChanged);
            panel.Children.Add(blankTextBox);
        }

        private void CreateBlank(QuestionBlank blank)
        {
            // Blank
            NumberOnlyTextBox blankTextBox = new NumberOnlyTextBox();
            blankTextBox.Tag = blank;
            blankTextBox.Width = 80;
            blankTextBox.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            blankTextBox.FontSize = 20;
            blankTextBox.Margin = new Thickness(5);
            blankTextBox.LostFocus += new RoutedEventHandler(blankTextBox_LostFocus);
            blankTextBox.TextChanged += new TextChangedEventHandler(blankTextBox_TextChanged);
            this.questionBodyPanel.Children.Add(blankTextBox);
        }

        private void blankTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            NumberOnlyTextBox textBox = sender as NumberOnlyTextBox;
            if (textBox == null)
                return;

            if (string.IsNullOrEmpty(textBox.Text))
                return;

            QuestionBlank blank = textBox.Tag as QuestionBlank;
            if (this.fibQuestion.IsContentCorrect(blank.Id, new QuestionContent(textBox.Text, ContentType.Text)))
            {
                textBox.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                textBox.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void blankTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is RichTextBox)
            {
                RichTextBox rtb = sender as RichTextBox;
                if (rtb == null)
                    return;

                QuestionBlank blank = getQuestionBlank(rtb.Tag);
                QuestionBlankResponse blankResponse = this.fibResponse.GetBlankResponse(blank.Id, true);
                blankResponse.BlankContent.Content = HtmlFromXamlConverter.GetFlowDocumentText(rtb.Document);
            }
            else
            {
                TextBox textBox = sender as TextBox;
                if (textBox == null)
                    return;

                QuestionBlank blank = getQuestionBlank(textBox.Tag);
                QuestionBlankResponse blankResponse = this.fibResponse.GetBlankResponse(blank.Id, true);
                blankResponse.BlankContent.Content = textBox.Text;
            }
        }

        private QuestionBlank getQuestionBlank(object tag)
        {
            if (tag is QuestionBlank)
                return tag as QuestionBlank;

            if (tag is string)
            {
                foreach (var blank in this.fibQuestion.QuestionBlankCollection)
                {
                    if (blank.PlaceHolder == tag as string)
                        return blank as QuestionBlank;
                }
            }

            return null;
        }
    }
}
