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

namespace Math.Basic.UserControls
{
    /// <summary>
    /// Interaction logic for ExerciseSettingUserControl.xaml
    /// </summary>
    public partial class ExerciseSettingUserControl : UserControl
    {
        public ExerciseSettingUserControl()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.StartupUserControl.ShowExercisePage();
        }

        private void historyButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.StartupUserControl.ShowExerciseHistoryPage();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.startButton.Focus();
        }
    }
}
