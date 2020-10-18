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
using SoonLearning.TeachAppMaker.Data;
using SoonLearning.Assessment.Data;

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for KnowledgeWindow.xaml
    /// </summary>
    public partial class KnowledgeWindow : Window
    {
        public KnowledgeWindow()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProjectMgr.Instance.App.Knowledge.Content == null)
                ProjectMgr.Instance.App.Knowledge.Content = new Assessment.Data.QuestionContent();

            ProjectMgr.Instance.App.Knowledge.Content.Content = this.richTextEditor.Text;
            ProjectMgr.Instance.App.Knowledge.Content.ContentType = Assessment.Data.ContentType.FlowDocument;

            foreach (var part in this.richTextEditor.Parts)
            {
                if (ProjectMgr.Instance.App.Knowledge.Content.Content.Contains(part.PlaceHolder))
                    ProjectMgr.Instance.App.Knowledge.Content.QuestionPartCollection.Add(part);
            }

            this.DialogResult = true;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ProjectMgr.Instance.App.Knowledge.Content != null)
            {
                this.richTextEditor.Text = ProjectMgr.Instance.App.Knowledge.Content.Content;
                foreach (QuestionContentPart item in ProjectMgr.Instance.App.Knowledge.Content.QuestionPartCollection)
                    this.richTextEditor.Parts.Add(item);
            }
        }
    }
}
