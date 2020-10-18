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
using SoonLearning.AppCenter.Data;

namespace SoonLearning.AppCenter.Windows
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : UserControl
    {
        private AppItem gadgetItem;

        public HelpWindow(AppItem item)
        {
            InitializeComponent();

            this.gadgetItem = item;
            this.DataContext = item;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.goBack();
        }
    }
}
