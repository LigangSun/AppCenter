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
using System.Collections;
using SoonLearning.AppCenter.Data;
using System.Windows.Media.Animation;

namespace SoonLearning.AppCenter.UserControls
{
    /// <summary>
    /// Interaction logic for StageListUserControl.xaml
    /// </summary>
    public partial class StageListUserControl : UserControl
    {
        public EventHandler<EventArgs> BackEvent;
        public EventHandler<EventArgs> DoneEvent;

        public IEnumerable ItemsSource
        {
            get { return this.stageListBox.ItemsSource; }
            set { this.stageListBox.ItemsSource = value; }
        }

        public StageItem SelectedItem
        {
            get { return this.stageListBox.SelectedItem as StageItem; }
        }

        public int SelectedIndex
        {
            get { return this.stageListBox.SelectedIndex; }
        }

        public StageListUserControl()
        {
            InitializeComponent();
        }

        private void stageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.BackEvent != null)
                this.BackEvent(sender, e);
        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DoneEvent != null)
                this.DoneEvent(sender, e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = this.TryFindResource("loadStoryboard") as Storyboard;
            if (storyboard != null)
                storyboard.Begin();
        }
    }
}
