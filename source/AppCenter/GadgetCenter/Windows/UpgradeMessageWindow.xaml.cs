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
    /// Interaction logic for UpgradeMessageWindow.xaml
    /// </summary>
    public partial class UpgradeMessageWindow : Window
    {
        public UpgradeMessageWindow(string message)
        {
            InitializeComponent();

            this.infoTextBlock.Text = message;

            this.changeListWebBrowser.Navigate(@"http://www.soonlearning.com/AppCenterChangeList.html");
        }

        private void upgradeButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
