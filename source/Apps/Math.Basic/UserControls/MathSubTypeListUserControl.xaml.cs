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
using Math.Basic.Data;
using SoonLearning.AppCenter.Interfaces;

namespace Math.Basic.UserControls
{
    /// <summary>
    /// Interaction logic for MathSubTypeListUserControl.xaml
    /// </summary>
    public partial class MathSubTypeListUserControl
    {
        public MathSubTypeListUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.mathSubTypeListBox.Focus();

            this.mathSubTypeListBox.ItemsSource = DataMgr.Instance.MathSubItemCollection;
            if (this.mathSubTypeListBox.SelectedIndex < 0)
            {
                this.mathSubTypeListBox.SelectedIndex = 0;
            //    ((ListBoxItem)this.mathSubTypeListBox.SelectedItem).Focus();
            }
        }

        private void mathSubTypeListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.startToLearn();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            this.startToLearn();
        }

        private void startToLearn()
        {
            if (this.mathSubTypeListBox.SelectedItem == null)
                return;

            MathSubTypeItem item = this.mathSubTypeListBox.SelectedItem as MathSubTypeItem;
            DataMgr.Instance.ActiveMathSubTypeItem = item;

            ControlMgr.Instance.StartupUserControl.ShowTeachPage();
        }

        private void mathSubTypeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.startButton.IsEnabled = (this.mathSubTypeListBox.SelectedItem != null);
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.startToLearn();
            }
        }
    }
}
