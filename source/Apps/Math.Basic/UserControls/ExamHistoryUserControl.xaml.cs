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
using SoonLearning.Math.Data;
using System.Collections.ObjectModel;

namespace Math.Basic.UserControls
{
    /// <summary>
    /// Interaction logic for ExamHistoryUserControl.xaml
    /// </summary>
    public partial class ExamHistoryUserControl : UserControl
    {
        private ObservableCollection<Exam> examCollection = new ObservableCollection<Exam>();

        public ExamHistoryUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
