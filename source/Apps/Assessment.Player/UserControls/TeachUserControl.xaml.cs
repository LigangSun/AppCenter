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
using System.IO;
using System.Reflection;
using System.Windows.Markup;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.AppCenter.Interfaces;
using System.Threading;
using System.Windows.Threading;

namespace SoonLearning.Assessment.Player.UserControls
{
    /// <summary>
    /// Interaction logic for TeachUserControl.xaml
    /// </summary>
    public partial class TeachUserControl
    {
        public TeachUserControl()
        {
            InitializeComponent();
        }

        private void GadgetUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                FlowDocument doc = DataMgr.Instance.DataCreator.GetKnowledgeDefinition();
                List<Block> blockCollection = new List<Block>();
                blockCollection.AddRange(doc.Blocks);
                this.definitionDocument.Document = new FlowDocument();
                this.definitionDocument.Document.Blocks.AddRange(blockCollection);
            }),
            DispatcherPriority.Background,
            null);
            
            this.exerciseButton.Focus();
        }

        private void exerciseButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.StartupUserControl.ShowExerciseSettingPage(false);
        }

        private void examButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.StartupUserControl.ShowExamSettingPage();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.StartupUserControl.GoBack();
        }
    }
}
