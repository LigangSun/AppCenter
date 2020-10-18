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
using SoonLearning.Memorize.Data;

namespace MemorizeAppCreator
{
    /// <summary>
    /// Interaction logic for MemorizeMathCreatorUserControl.xaml
    /// </summary>
    public partial class MemorizeMathCreatorUserControl : UserControl
    {
        private MemorizeEntry memorizeEntry;
        private bool isChanged;

        private bool IsChanged
        {
            get { return this.isChanged; }
            set { this.isChanged = value; }
        }

        public MemorizeMathCreatorUserControl(MemorizeEntry entry)
        {
            InitializeComponent();

            this.memorizeEntry = entry;
            this.stageListBox.DataContext = entry;
        }

        private void addStageButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeStageWindow wnd = new MemorizeStageWindow(null);
            if (wnd.ShowDialog().Value)
            {
                this.memorizeEntry.Stages.Add(wnd.Stage);
                this.isChanged = true;
            }
        }

        private void removeStageButton_Click(object sender, RoutedEventArgs e)
        {
            this.memorizeEntry.Stages.RemoveAt(this.stageListBox.SelectedIndex);
            this.isChanged = true;
        }

        private void editStageButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeStage stage = this.stageListBox.SelectedItem as MemorizeStage;
            MemorizeStageWindow wnd = new MemorizeStageWindow(stage);
            if (wnd.ShowDialog().Value)
            {
                this.memorizeEntry.Stages.Insert(this.stageListBox.SelectedIndex, wnd.Stage);
                this.memorizeEntry.Stages.Remove(stage);
                this.isChanged = true;
            }
        }

        private void stageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.removeStageButton.IsEnabled =
                this.editStageButton.IsEnabled = this.stageListBox.SelectedItem != null;
        }
    }
}
