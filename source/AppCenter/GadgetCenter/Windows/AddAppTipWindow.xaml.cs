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

namespace SoonLearning.AppCenter.Windows
{
    /// <summary>
    /// Interaction logic for AddAppTipWindow.xaml
    /// </summary>
    public partial class AddAppTipWindow : Window
    {
        public AddAppTipWindow()
        {
            InitializeComponent();
        }

        private void managementBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void x_Name__cancelBtn__Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
