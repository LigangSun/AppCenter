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

namespace SoonLearning.AppManagementTool
{
    /// <summary>
    /// Interaction logic for StarterWindow.xaml
    /// </summary>
    public partial class StarterWindow : Window
    {
        public StarterWindow()
        {
            InitializeComponent();
        }

        private void createPackButton_Click(object sender, RoutedEventArgs e)
        {
            CreatePackWindow wnd = new CreatePackWindow();
            wnd.Show();
        }

        private void uploadPackButton_Click(object sender, RoutedEventArgs e)
        {
            UploadAppWindow wnd = new UploadAppWindow(string.Empty);
            wnd.Show();
        }
    }
}
