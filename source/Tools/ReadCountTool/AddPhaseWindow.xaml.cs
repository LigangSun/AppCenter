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

namespace ReadCountTool
{
    /// <summary>
    /// Interaction logic for AddPhaseWindow.xaml
    /// </summary>
    public partial class AddPhaseWindow : Window
    {
        public string Phase
        {
            get { return this.phaseTextBox.Text; }
        }

        public AddPhaseWindow(string phase)
        {
            InitializeComponent();

            this.phaseTextBox.Text = phase;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
