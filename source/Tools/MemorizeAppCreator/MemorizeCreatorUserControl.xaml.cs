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
using Microsoft.Win32;
using SoonLearning.Memorize.UI;

namespace MemorizeAppCreator
{
    /// <summary>
    /// Interaction logic for MemorizeCreatorUserControl.xaml
    /// </summary>
    public partial class MemorizeCreatorUserControl
    {
        private MemorizeEntry memorizeEntry;
        private string projectFile = string.Empty;
        private bool isChanged;

        private bool IsChanged
        {
            get { return this.isChanged; }
            set { this.isChanged = value; }
        }

        public MemorizeCreatorUserControl(MemorizeEntry entry)
        {
            InitializeComponent();

            this.memorizeEntry = entry;

            this.memorizeItemListBox.DataContext = entry;
        }

        private void addMemorizeItemButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeItem item = new MemorizeItem();
            MemorizeItemWindow wnd = new MemorizeItemWindow(item);
            if (wnd.ShowDialog().Value)
            {
                this.memorizeEntry.Items.Add(item);
                this.isChanged = true;
            }
        }

        private void editMemorizeItemButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeItem item = this.memorizeItemListBox.SelectedItem as MemorizeItem;
            MemorizeItemWindow wnd = new MemorizeItemWindow(item);
            if (wnd.ShowDialog().Value)
            {
                this.memorizeEntry.Items.Add(item);
                this.isChanged = true;
            }
        }

        private void removeMemorizeItemButton_Click(object sender, RoutedEventArgs e)
        {
            this.memorizeEntry.Items.Remove(this.memorizeItemListBox.SelectedItem as MemorizeItem);
            this.isChanged = true;
        }

        private void addToStageButton_Click(object sender, RoutedEventArgs e)
        {
        /*    MemorizeItem item = this.memorizeItemListBox.SelectedItem as MemorizeItem;
            MemorizeStage stage = this.stageListBox.SelectedItem as MemorizeStage;
            if (stage is MemorizeStableStage)
            {
                ((MemorizeStableStage)stage).MemorizeItemIds.Add(item.Id);
                this.isChanged = true;
            }*/
        }

        private void memorizeItemListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.editMemorizeItemButton.IsEnabled = 
            this.removeMemorizeItemButton.IsEnabled = this.memorizeItemListBox.SelectedItem != null;
            this.addToStageButton.IsEnabled = this.memorizeItemListBox.SelectedItem != null;
        }


        //////////////////////////////////////////////////////////////////////////////////////////////
        private void newMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.newProject();
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.openProject();
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.saveProject();
        }

        private void saveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "记忆项目文件|*.mre";
            if (dlg.ShowDialog().Value)
            {
                this.projectFile = dlg.FileName;
                this.saveProject();
            }
        }

        private bool saveProject()
        {
            return true;
        }

        private void newProject()
        {
        }

        private void openProject()
        {
            
        }

        private void createMathExpression_Click(object sender, RoutedEventArgs e)
        {
            MathExpressionGenerateWindow wnd = new MathExpressionGenerateWindow();
            if (wnd.ShowDialog().Value)
            {
                foreach (var item in wnd.Items)
                {
                    this.memorizeEntry.Items.Add(item);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }
    }
}
