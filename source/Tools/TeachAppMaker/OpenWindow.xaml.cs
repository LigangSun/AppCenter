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

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for OpenWindow.xaml
    /// </summary>
    public partial class OpenWindow : Window
    {
        internal string File
        {
            get
            {
                AppDescription appDescription = this.appListBox.SelectedItem as AppDescription;
                if (appDescription != null)
                    return appDescription.File;

                return string.Empty;
            }
        }

        public OpenWindow()
        {
            InitializeComponent();
        }

        private void appListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.okButton.IsEnabled = this.appListBox.SelectedItem != null;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            
            this.DialogResult = true;
            this.Close();
        }
    }
}
