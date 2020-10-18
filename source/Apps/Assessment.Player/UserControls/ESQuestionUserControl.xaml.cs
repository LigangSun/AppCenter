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
using SoonLearning.Assessment.Player.CommonControl;
using SoonLearning.Assessment.Data;
using System.Windows.Markup;
using System.Threading;
using System.Windows.Threading;
using System.IO;

namespace SoonLearning.Assessment.Player.UserControls
{
    /// <summary>
    /// Interaction logic for ESQuestionUserControl.xaml
    /// </summary>
    public partial class ESQuestionUserControl : UserControl
    {
        private ESQuestion esQuestion;
        private int index;
        private bool isCorrect = false;
        private ESQuestionResponse response;
        private object saveResponseLocker = new object();

        public ESQuestionUserControl(ESQuestion esQuestion, ESQuestionResponse response, int index)
        {
            this.esQuestion = esQuestion;
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
            CommonControlCreator.CreateContentControl(this.esQuestion.Content, stackPanel, this.Foreground, null);

            if (!string.IsNullOrEmpty(this.response.Answer.FlowDocument))
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(this.response.Answer.FlowDocument));
                this.answerEditControl.Document = this.response.Answer.FlowDocument;
            }

            this.answerEditControl.TextChangedEvent += answerEditorControl_TextChanged;
        }

        private void answerEditorControl_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.response != null)
            {
                this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                    {
                        lock (this.saveResponseLocker)
                        {
                            this.response.Answer.FlowDocument = this.answerEditControl.Document;
                        }
                    }),
                    DispatcherPriority.Background,
                    null);
            }
        }
    }
}
