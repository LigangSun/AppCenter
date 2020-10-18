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
using System.Threading;
using SoonLearning.Assessment.Player.CommonControl;
using System.Windows.Controls.Primitives;

namespace SoonLearning.Assessment.Player.UserControls
{
    /// <summary>
    /// Interaction logic for MCQuestionUserControl.xaml
    /// </summary>
    public partial class MCQuestionUserControl : UserControl
    {
        private SelectableQuestion selectableQuestion;
        private int index;
        private bool isCorrect = false;
        private SelectableQuestionResponse response;

        private RoutedEventHandler toggleButtonCheckedHandler;
        private RoutedEventHandler toggleButtonUnCheckedHandler;

        private List<ToggleButton> toggleButtonList = new List<ToggleButton>();

        internal bool IsCorrect
        {
            get
            {
                if (this.response == null)
                    return false;

                foreach (QuestionOption option in this.selectableQuestion.QuestionOptionCollection)
                {
                    if (option.IsCorrect)
                    {
                        if (!this.response.OptionIdList.Contains(option.Id))
                            return false;
                    }
                    else
                    {
                        if (this.response.OptionIdList.Contains(option.Id))
                            return false;
                    }
                }

                return true;
            }
        }

        public MCQuestionUserControl(SelectableQuestion selectableQuestion, SelectableQuestionResponse response, int index)
        {
            this.selectableQuestion = selectableQuestion;
            this.index = index;
            this.response = response;

            InitializeComponent();

            this.toggleButtonCheckedHandler = (s, e) =>
            {
                this.radioBtn_Checked(s, e);
            };

            this.toggleButtonUnCheckedHandler = (s, e) =>
            {
                this.radioBtn_UnChecked(s, e);
            };
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
        }

        private void radioBtn_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton radioBtn = sender as ToggleButton;
            if (radioBtn == null)
                return;

            QuestionOption option = radioBtn.Tag as QuestionOption;
            if (option == null)
                return;

            if (this.selectableQuestion is MCQuestion ||
                this.selectableQuestion is TFQuestion)
            {
                if (this.response.OptionIdList.Count == 0)
                    this.response.OptionIdList.Add(option.Id);
                else
                    this.response.OptionIdList[0] = option.Id;
            }
            else if (this.selectableQuestion is MRQuestion)
            {
                if (!this.response.OptionIdList.Contains(option.Id))
                {
                    this.response.OptionIdList.Add(option.Id);
                }
            }
        }

        private void radioBtn_UnChecked(object sender, RoutedEventArgs e)
        {
            ToggleButton radioBtn = sender as ToggleButton;
            if (radioBtn == null)
                return;

            QuestionOption option = radioBtn.Tag as QuestionOption;
            if (option == null)
                return;

            if (this.selectableQuestion is MCQuestion ||
                this.selectableQuestion is TFQuestion)
            {
            }
            else if (this.selectableQuestion is MRQuestion)
            {
                if (this.response.OptionIdList.Contains(option.Id))
                {
                    this.response.OptionIdList.Remove(option.Id);
                }
            }
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

            ToggleButton toggleButton = null;
            if (this.selectableQuestion is MCQuestion)
            {
                toggleButton = new RadioButton();  
                ((RadioButton)toggleButton).GroupName = this.selectableQuestion.Id;
            }
            else if (this.selectableQuestion is MRQuestion)
            {
                toggleButton = new CheckBox();
            }
            else if (this.selectableQuestion is TFQuestion)
            {
                toggleButton = new RadioButton();
                ((RadioButton)toggleButton).GroupName = this.selectableQuestion.Id;
            }
            toggleButton.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            toggleButton.Tag = option;
            if (this.response != null)
            {
                if (this.response.OptionIdList.Count > 0)
                {
                    if (option.Id == this.response.OptionIdList[0])
                    {
                        toggleButton.IsChecked = true;
                        this.isCorrect = true;
                    }
                }
            }
            toggleButton.Checked += toggleButtonCheckedHandler;
            toggleButton.Unchecked += this.toggleButtonUnCheckedHandler;
            this.toggleButtonList.Add(toggleButton);
            stackPanel.Children.Add(toggleButton);

            Panel contentControl = CommonControlCreator.CreateContentControl(option.OptionContent, null, this.Foreground, null);
            if (contentControl != null)
            {
                contentControl.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                contentControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                toggleButton.Content = contentControl;
            }

            this.optionPanel.Children.Add(stackPanel);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.questionBodyPanel.Children.Clear();
            this.optionPanel.Children.Clear();

            UIElement indexPart = CommonControlCreator.CreateUIPart(new QuestionTextPart(string.Format("{0}. ", this.index + 1)), this.Foreground);
            if (indexPart is Control)
            {
                ((Control)indexPart).VerticalAlignment = System.Windows.VerticalAlignment.Top;
            }
            this.questionBodyPanel.Children.Add(indexPart);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Orientation = Orientation.Vertical;
            this.questionBodyPanel.Children.Add(stackPanel);
            CommonControlCreator.CreateContentControl(selectableQuestion.Content, stackPanel, this.Foreground, null);

            List<int> optionIndexList = new List<int>();
            if (this.selectableQuestion.RandomOption)
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                while (true)
                {
                    int index = rand.Next(selectableQuestion.QuestionOptionCollection.Count * 10) % selectableQuestion.QuestionOptionCollection.Count;
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

                    if (optionIndexList.Count == selectableQuestion.QuestionOptionCollection.Count)
                        break;

                    Thread.Sleep(10);
                }
            }
            else
            {
                for (int i = 0; i < selectableQuestion.QuestionOptionCollection.Count; i++)
                {
                    optionIndexList.Add(i);
                }
            }

            this.toggleButtonList.Clear();
            for (int i = 0; i < selectableQuestion.QuestionOptionCollection.Count; i++)
            {
                this.AppendOption(selectableQuestion.QuestionOptionCollection[optionIndexList[i]], i);
            }
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            foreach (var btn in this.toggleButtonList)
            {
                btn.Unchecked -= this.toggleButtonUnCheckedHandler;
                btn.Checked -= this.toggleButtonCheckedHandler;
            }

            this.toggleButtonList.Clear();
        }
    }
}
