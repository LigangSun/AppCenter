using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using SoonLearning.TeachAppMaker.Data;
using HTMLConverter;

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for KnowledgeUserControl.xaml
    /// </summary>
    public partial class KnowledgeUserControl : UserControl
    {
        public KnowledgeUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        internal void ShowKnowledge()
        {
            if (ProjectMgr.Instance.App.Knowledge.Content == null)
                return;

#if _SAVE_CONTENT_AS_XAML_
            this.knowledgeDocument.Document = HtmlToXamlConverter.DeserializeFlowDocument(ProjectMgr.Instance.App.Knowledge.Content.Content);
#else
            this.knowledgeDocument.Document = HtmlToXamlConverter.ConvertHtmlToXaml(ProjectMgr.Instance.App.Knowledge.Content.Content);
#endif
        }
    }
}
