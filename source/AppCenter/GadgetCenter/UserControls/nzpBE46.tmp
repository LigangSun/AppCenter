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
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.UserControls
{
    /// <summary>
    /// Interaction logic for ExitUserControl.xaml
    /// </summary>
    public partial class ExitUserControl : UserControl
    {
        public ExitUserControl()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.Exit();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            GC_UIHelper.CloseMessageWindow();
        }
    }
}
