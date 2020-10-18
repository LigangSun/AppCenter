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
using SoonLearning.AppCenter.Resources;
using System.Reflection;
using System.IO;
using System.Windows.Markup;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.Windows
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : UserControl
    {
        public AboutWindow()
        {
            InitializeComponent();

            this.infoTextBlock.Text = string.Format(Strings.AboutMessage,
                Assembly.GetExecutingAssembly().GetName().Version.ToString());

            this.infoDoumentReader.Document = GetFlowDocument("SoonLearning.AppCenter.AboutFlowDocument.xaml");
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            GC_UIHelper.CloseMessageWindow();
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GC_UIHelper.CloseMessageWindow();
        }

        protected virtual FlowDocument GetFlowDocument(string file)
        {
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            Stream stream = executingAssembly.GetManifestResourceStream(file);
            FlowDocument doc = (FlowDocument)XamlReader.Load(stream);
            return doc;
        }
    }
}
