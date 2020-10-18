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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.License;

namespace SoonLearning.ConnectNumber
{
    /// <summary>
    /// Interaction logic for DrawNumberList.xaml
    /// </summary>
    public partial class DrawNumberList : UserControl
    {
        public DrawNumberList()
        {
            InitializeComponent();

         //   this.stageListBox.Title = SoonLearning.ConnectNumber.Properties.Resources.DrawNumberTitle;
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            if (LicenseMgr.Instance.IsTrialVersion("A3031C587426429D877D68CF9BCC706C"))
            {
                this.newBtn.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                this.newBtn.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void newBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.DrawNumberStartupPage.ShowNewDrawNumberPage();
        }

        private void reloadBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.DrawNumberStartupPage.LoadDrawNumberItems(endLoadDrawNumberItems);
        }

        private void endLoadDrawNumberItems(object sender, EventArgs e)
        {
            ControlMgr.Instance.DrawNumberStartupPage.ShowListPage();
        }

        private void win_DataLoadCompletedEvent()
        {
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DrawNumberControl.Instance.ControlPanlVisible(false);

            this.stageListBox.ItemsSource = ControlMgr.Instance.DataMgr.DrawNumberItems;
            this.stageListBox.SelectedIndex = ControlMgr.Instance.DataMgr.CurrentIndex;

            Storyboard storyboard = this.TryFindResource("loadStoryboard") as Storyboard;
            if (storyboard != null)
                storyboard.Begin();
        }

        private void stageListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void stageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void doneButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.DataMgr.CurrentIndex = this.stageListBox.SelectedIndex;
            ControlMgr.Instance.DrawNumberStartupPage.ShowDrawNumberPage();
            ControlMgr.Instance.DrawNumberPage.ShowDrawNumberData();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.DrawNumberStartupPage.ShowDrawNumberPage();
        }

        private void stageListBox_ThumbnailItemSelectedEvent(int index)
        {
            if (index < 0)
                return;

            ControlMgr.Instance.DataMgr.CurrentIndex = index;

            ControlMgr.Instance.DrawNumberStartupPage.ShowDrawNumberPage();
            ControlMgr.Instance.DrawNumberPage.ShowDrawNumberData();
        }
    }
}
