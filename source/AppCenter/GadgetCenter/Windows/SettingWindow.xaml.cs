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
using Microsoft.Win32;
using SoonLearning.AppCenter.Resources;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.AppCenter.Windows
{
    /// <summary>
    /// Interaction logic for SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow
    {
        private string imageFile;

        public SettingWindow()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
        //    UIStyleSetting.Instance.StartColor = this.startColor.Color;
        //    UIStyleSetting.Instance.EndColor = this.endColor.Color;
       //     UIStyleSetting.Instance.ForegroundColor = ((SolidColorBrush)(this.foregroundColor.Foreground)).Color;
            UIStyleSetting.Instance.OpenSound = this.voiceCheckBox.IsChecked.Value;
            UIStyleSetting.Instance.FullScreen = this.fullScreenCheckBox.IsChecked.Value;
        //    UIStyleSetting.Instance.UIStyle = this.uiStyleCbx.SelectedIndex;
        //    UIStyleSetting.Instance.BackgroundImageIndex = this.thumbnailBackgroundCbx.SelectedIndex;
            UIStyleSetting.Instance.SpeechRecognizer = this.speechRecognizerCheckBox.IsChecked.Value;
            UIStyleSetting.Instance.Save();

            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.CloseMessageWindow();

            mainWnd.SwitchToFullScreen(UIStyleSetting.Instance.FullScreen);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.CloseMessageWindow();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            if (openFileDlg.ShowDialog().Value)
            {
                this.imageFile = openFileDlg.FileName;
            //    this.bkImage.Source = new BitmapImage(new Uri(this.imageFile));
            }
        }

        private void foregroundBtn_Click(object sender, RoutedEventArgs e)
        {
        }

        private void thumbnailBackgroundCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
         //   this.bkImage.Source = new BitmapImage(new Uri(UIStyleSetting.Instance.BackgroundImageList[this.thumbnailBackgroundCbx.SelectedIndex], UriKind.Relative));
        }

        private void GadgetUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //    this.startColor.Color = UIStyleSetting.Instance.StartColor;
            //    this.endColor.Color = UIStyleSetting.Instance.EndColor;
            //    this.bkImage.Source = new BitmapImage(new Uri(UIStyleSetting.Instance.BackgroundImage, UriKind.Relative));
            //    this.foregroundColor.Foreground = new SolidColorBrush(UIStyleSetting.Instance.ForegroundColor);
            this.voiceCheckBox.IsChecked = UIStyleSetting.Instance.OpenSound;
            this.fullScreenCheckBox.IsChecked = UIStyleSetting.Instance.FullScreen;
            //    this.uiStyleCbx.SelectedIndex = UIStyleSetting.Instance.UIStyle;
            //     this.thumbnailBackgroundCbx.SelectedIndex = UIStyleSetting.Instance.BackgroundImageIndex;
            this.speechRecognizerCheckBox.IsChecked = UIStyleSetting.Instance.SpeechRecognizer;
        }
    }
}
