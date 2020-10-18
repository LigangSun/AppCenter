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
using SoonLearning.Math.Data;
using System.Threading;
using SoonLearning.Math.UI;

namespace Math.Basic.UserControls
{
    /// <summary>
    /// Interaction logic for MCQuestionUserControl.xaml
    /// </summary>
    public partial class MCQuestionUserControl : UserControl
    {
        private MCQuestion mcQuestion;
        private int index;
        private bool isCorrect = false;
        private SelectableQuestionResponse response;

        internal bool IsCorrect
        {
            get { return this.isCorrect; }
        }

        public MCQuestionUserControl(MCQuestion mcQuestion, SelectableQuestionResponse response, int index)
        {
            this.mcQuestion = mcQuestion;
            this.index = index;
            this.response = response;

            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            UIElement indexPart = CommonControlCreator.CreateUIPart(new QuestionTextPart(string.Format("{0}. ", this.index + 1)), this.Foreground);
            if (indexPart is Control)
            {
                ((Control)indexPart).VerticalAlignment = System.Windows.VerticalAlignment.Top;
            }
            this.questionBodyPanel.Children.Add(indexPart);
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            this.questionBodyPanel.Children.Add(stackPanel);
            CommonControlCreator.CreateContentControl(mcQuestion.Content, stackPanel, this.Foreground, null);

            List<int> optionIndexList = new List<int>();
            if (this.mcQuestion.RandomOption)
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                while (true)
                {
                    int index = rand.Next(mcQuestion.QuestionOptionCollection.Count * 10) % mcQuestion.QuestionOptionCollection.Count;
                    bool found = false;
                    foreach (int temp in optionIndexList)
                    {
                        if (temp == index)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        optionIndexList.Add(index);

                    if (optionIndexList.Count == mcQuestion.QuestionOptionCollection.Count)
                        break;

                    Thread.Sleep(10);
                }
            }
            else
            {
                for (int i = 0; i < mcQuestion.QuestionOptionCollection.Count; i++)
                {
                    optionIndexList.Add(i);
                }
            }

            for (int i = 0; i < mcQuestion.QuestionOptionCollection.Count; i++)
            {
                this.AppendOption(mcQuestion.QuestionOptionCollection[optionIndexList[i]], i);
            }
        }

        private void radioBtn_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioBtn = sender as RadioButton;
            if (radioBtn == null)
                return;

            QuestionOption option = radioBtn.Tag as QuestionOption;
            if (option == null)
                return;

            this.isCorrect = option.IsCorrect;
            if (this.response.OptionIdList.Count == 0)
                this.response.OptionIdList.Add(option.Id);
            else
                this.response.OptionIdList[0] = option.Id;
        }

        private void AppendOption(QuestionOption option, int index)
        {
            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Horizontal;
            stackPanel.Margin = new Thickness(20, 3, 0, 3);

            Label indexLabel = new Label();
            indexLabel.Content = string.Format("{0}. ", (char)('A' + index));
            indexLabel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            indexLabel.FontSize = 20;
            stackPanel.Children.Add(indexLabel);

            RadioButton radioBtn = new RadioButton();
            radioBtn.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            radioBtn.GroupName = this.mcQuestion.Id;
            radioBtn.Tag = option;
            if (this.response != null)
            {
                if (this.response.OptionIdList.Count > 0)
                {
                    if (option.Id == this.response.OptionIdList[0])
                    {
                        radioBtn.IsChecked = true;
                        this.isCorrect = true;
                    }
                }
            }
            radioBtn.Checked += new RoutedEventHandler(radioBtn_Checked);
            stackPanel.Children.Add(radioBtn);

            Panel contentControl = CommonControlCreator.CreateContentControl(option.OptionContent, null, this.Foreground, null);
            if (contentControl != null)
            {
                contentControl.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                contentControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                radioBtn.Content = contentControl;
            }

            this.optionPanel.Children.Add(stackPanel);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
