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

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        private int type = 0;

        public int Type
        {
            get { return this.type; }
        }

        public StartupWindow()
        {
            InitializeComponent();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            this.type = 1;
            this.DialogResult = true;
            this.Close();
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            this.type = 2;
            this.DialogResult = true;
            this.Close();
        }
    }
}
