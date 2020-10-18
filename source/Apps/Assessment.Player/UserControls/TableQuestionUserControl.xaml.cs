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
using System.Windows.Media.Animation;
using System.Windows.Threading;
using SoonLearning.AppCenter.Controls;
using System.Threading;
using SoonLearning.Assessment.Player.CommonControl;

namespace SoonLearning.Assessment.Player.UserControls
{
    /// <summary>
    /// Interaction logic for TableQuestionUserControl.xaml
    /// </summary>
    public partial class TableQuestionUserControl : UserControl
    {
        private TableQuestion mrQuestion;
        private List<QuestionOption> correctOptionList = new List<QuestionOption>();
        private SelectableQuestionResponse response;
        private int resetFailCount = 3;
        private int failedCount = 0;

        internal bool IsCorrect
        {
            get { return this.correctOptionList.Count == 0; }
        }

        public TableQuestionUserControl(TableQuestion mrQuestion, SelectableQuestionResponse response)
        {
            this.mrQuestion = mrQuestion;
            this.response = response;

            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            this.ShowQuestion();
        }

        private void ShowQuestion()
        {
            this.correctOptionList.Clear();

            this.optionGrid.RowDefinitions.Clear();
            this.optionGrid.ColumnDefinitions.Clear();
            this.optionGrid.Children.Clear();

            QuestionContent contentContent = this.mrQuestion.Content;
            this.questionContentLabel.Content = contentContent.Content;

       //     this.solutionTextBlock.Text = this.mrQuestion.Tip;

            int rowCount = (int)System.Math.Sqrt(this.mrQuestion.QuestionOptionCollection.Count);
            System.Drawing.Size monitorSize = System.Windows.Forms.SystemInformation.PrimaryMonitorSize;
            double rowWidth = 135f;
            double rowHeight = 110f / ((float)monitorSize.Width / (float)monitorSize.Height);
            for (int i = 0; i<rowCount; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new GridLength(1, GridUnitType.Star);
                this.optionGrid.RowDefinitions.Add(rowDef);

                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new GridLength(rowWidth, GridUnitType.Pixel);
                this.optionGrid.ColumnDefinitions.Add(colDef);
            }

            Random rand = new Random((int)DateTime.Now.Ticks);
            int index = 0;
            for (int i = 0; i<rowCount; i++)
            {
                for (int j = 0; j<rowCount; j++)
                {
                    QuestionOption option = this.mrQuestion.QuestionOptionCollection[index++];
                    bool found = false;
                    foreach (string optionId in this.response.OptionIdList)
                    {
                        if (option.Id == optionId)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        continue;

                    Button btn = new Button();
                    btn.Content = CommonControlCreator.CreateContentControl(option.OptionContent, null, this.Foreground, null);
                    btn.Style = this.FindResource("ButtonStyle_Sub2") as Style;
                    btn.Tag = option;
                    btn.Click += new RoutedEventHandler(btn_Click);

                    if (option.IsCorrect)
                    {
                        this.correctOptionList.Add(option);
                    }

                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);

                    this.optionGrid.Children.Add(btn);

                    btn.BeginAnimation(Button.OpacityProperty, new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(rand.Next(200, 800)))));
                }
            }
        }

        private void ResetQuestion()
        {
            if (this.response != null)
                this.response.OptionIdList.Clear();

            this.resetFailCount = 3;
            this.failedCount = 0;

            this.ShowQuestion();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            QuestionOption option = btn.Tag as QuestionOption;
            if (option == null)
                return;

            if (option.IsCorrect)
            {
                this.correctOptionList.Remove(option);
                this.hideButton(btn);

                bool found = false;
                foreach (string id in this.response.OptionIdList)
                {
                    if (id == option.Id)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    this.response.OptionIdList.Add(option.Id);
            }
            else
            {
                this.failedCount++;
                if (this.resetFailCount == this.failedCount)
                {
                    Panel contentPanel = CommonControlCreator.CreateContentControl(option.OptionContent, null, Brushes.White, null);

                    StackPanel line1Panel = new StackPanel();
                    line1Panel.Orientation = Orientation.Horizontal;
                    line1Panel.Children.Add(contentPanel);

                    Label label1 = new Label();
                    label1.Content = "不是正确答案!";
                    label1.FontSize = 24;
                    label1.Foreground = Brushes.White;
                    label1.FontWeight = FontWeights.Medium;
                    line1Panel.Children.Add(label1);

                    this.infoPanel.Children.Clear();
                    this.infoPanel.Children.Add(line1Panel);

                    Label label = new Label();
                    label.FontSize = 24;
                    label.FontWeight = FontWeights.Medium;
                    label.Foreground = Brushes.White;
                    label.Content = string.Format("你已经答错{0}次, 本题将重新开始!", this.resetFailCount);
                    this.infoPanel.Children.Add(label);

                    this.infoPopup.IsOpen = true;
                    Thread.Sleep(500);
                    this.ResetQuestion();
                }
                else
                {
                    Panel contentPanel = CommonControlCreator.CreateContentControl(option.OptionContent, null, Brushes.White, null);

                    StackPanel line1Panel = new StackPanel();
                    line1Panel.Orientation = Orientation.Horizontal;
                    line1Panel.Children.Add(contentPanel);

                    Label label = new Label();
                    label.FontSize = 24;
                    label.FontWeight = FontWeights.Medium;
                    label.Content = "不是正确答案!";
                    label.Foreground = Brushes.White;
                    line1Panel.Children.Add(label);

                    this.infoPanel.Children.Clear();
                    this.infoPanel.Children.Add(line1Panel);
                    
                    this.infoPopup.IsOpen = true;
                }
                Thread.Sleep(200);
            }
        }

        private void hideButton(Button btn)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation(0.0f, new Duration(TimeSpan.FromMilliseconds(200)));
            doubleAnimation.Completed += new EventHandler(doubleAnimation_Completed);
            btn.BeginAnimation(Button.OpacityProperty, doubleAnimation);
        }

        private void doubleAnimation_Completed(object sender, EventArgs e)
        {
            if (this.correctOptionList.Count != 0)
                return;

            this.Dispatcher.BeginInvoke(new EventHandler<EventArgs>(safeDone), DispatcherPriority.Background,
                new object[] {sender, e});
        }

        private void safeDone(object sender, EventArgs e)
        {
            MessageWindow win = new MessageWindow();
            win.ShowMessage("你已经找到所有正确选项，可以进入下一题！", MessageBoxButton.OK, MessageWindowCallback);
        }

        private void MessageWindowCallback(bool ok)
        {
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
