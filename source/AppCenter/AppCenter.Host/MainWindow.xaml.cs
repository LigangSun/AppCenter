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
using System.Reflection;
using SoonLearning.AppCenter.Interfaces;
using System.Windows.Forms;
using SoonLearning.AppCenter.Controls;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using System.Windows.Interop;
using System.Collections.ObjectModel;

namespace SoonLearning.AppCenter.Host
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IMessageControl
    {
        private bool disableExit = false;
        private bool canExit = false;
        private const int containerWidth = 1000;

        private ExitUserControl exitUserControl = new ExitUserControl();

        private ObservableCollection<ShareItem> shareItemCollection = new ObservableCollection<ShareItem>();

        public MainWindow()
        {
            InitializeComponent();

            this.containerGrid.Width = MainWindow.containerWidth - (this.borderLeftRow.Width.Value + this.borderRightRow.Width.Value);
            this.containerGrid.Height = MainWindow.containerWidth / ((float)SystemInformation.WorkingArea.Width /
                ((float)SystemInformation.WorkingArea.Height - (this.titleRow.Height.Value + this.borderTopRow.Height.Value + this.borderBottomRow.Height.Value + this.toolBarRow.Height.Value)));

            this.MinWidth = this.Width = MainWindow.containerWidth;
            this.MinHeight = this.Height = this.containerGrid.Height + this.titleRow.Height.Value + this.borderTopRow.Height.Value + this.borderBottomRow.Height.Value + this.toolBarRow.Height.Value;

            this.TitleHeight = (int)this.titleRow.Height.Value;

            this.MaxWidth = SystemInformation.WorkingArea.Width;
            this.MaxHeight = SystemInformation.WorkingArea.Height;

            this.shareListBox.ItemsSource = this.shareItemCollection;
        }

        private void PrepareShareItems()
        {
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (this.disableExit)
            {
                e.Cancel = true;
                return;
            }

            if (!canExit)
            {
                this.ShowMessageWindow(this.exitUserControl);
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        private void AppWindow_Initialized(object sender, EventArgs e)
        {
            Assembly gadgetAssembly = Assembly.LoadFile(App.appDllFile);
            App.currentEntry = gadgetAssembly.CreateInstance(App.entryType) as IGadgetEntry;
            this.Title = App.currentEntry.Title;

            //string thumbnail = System.IO.Path.GetDirectoryName(App.appDllFile);
            //thumbnail = System.IO.Path.Combine(thumbnail, @"AppLogos\");
            //thumbnail += App.currentEntry.Id;
            //thumbnail += System.IO.Path.GetExtension(App.currentEntry.Thumbnail);

            this.iconImage.Source = new BitmapImage(new Uri(App.currentEntry.Thumbnail, UriKind.Absolute));
            this.Icon = this.iconImage.Source;
            this.titleTextBlock.Text = this.Title;
            this.containerGrid.Children.Add(App.currentEntry.GetStartupPage());

            if (App.styleSetting.FullScreen)
                this.SwitchWindowState();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (App.styleSetting.FullScreen)
            {
                this.SwitchWindowState();
                this.FullScreen();
            }
        }

        internal void Exit()
        {
            this.CloseMessageWindow();

            this.canExit = true;
            this.Close();
        }

        public void ShowMessageWindow(System.Windows.Controls.UserControl msgWnd)
        {
            this.disableExit = true;
            this.messagePanel.Children.Clear();
            this.messagePanel.Children.Add(msgWnd);
            this.messagePanel.Visibility = System.Windows.Visibility.Visible;
        }

        public void CloseMessageWindow()
        {
            this.disableExit = false;
            this.messagePanel.Children.Clear();
            this.messagePanel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void maxBtn_Click(object sender, RoutedEventArgs e)
        {
            this.SwitchWindowState();
            this.FullScreen();
        }

        private void minBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void FullScreen()
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                this.borderTopRow.Height = new GridLength(0);
                this.borderLeftRow.Width = new GridLength(0);
                this.borderRightRow.Width = new GridLength(0);
                this.borderBottomRow.Height = new GridLength(0);
            }
            else
            {
                this.borderTopRow.Height = new GridLength(6);
                this.borderLeftRow.Width = new GridLength(6);
                this.borderRightRow.Width = new GridLength(6);
                this.borderBottomRow.Height = new GridLength(6);
            }
        }
    }
}
