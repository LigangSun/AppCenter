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
using SoonLearning.BlockPuzzle.Data;
using System.IO;
using System.Reflection;
using SoonLearning.AppCenter.Controls;
using System.Windows.Media.Animation;
using SoonLearning.AppCenter;

namespace SoonLearning.BlockPuzzle.Puzzle
{
    /// <summary>
    /// Interaction logic for NewPuzzleWindow.xaml
    /// </summary>
    public partial class NewPuzzleWindow : UserControl
    {
        private string imageFile;
        private PuzzleItem item;

        internal PuzzleItem PuzzleItem
        {
            get { return this.item; }
        }

        public NewPuzzleWindow()
        {
            InitializeComponent();
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.titleTextBox.Text))
            {
                MessageWindow msgWnd = new MessageWindow();
                msgWnd.ShowMessage(SoonLearning.BlockPuzzle.Properties.Resources.TitleEmpty, MessageBoxButton.OK, null);
                this.titleTextBox.SelectAll();
                this.titleTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(this.imageFile))
            {
                MessageWindow msgWnd = new MessageWindow();
                msgWnd.ShowMessage(SoonLearning.BlockPuzzle.Properties.Resources.SelectImageForNewPuzzle, MessageBoxButton.OK, null);
                return;
            }

            item = PuzzleItem.CreatePuzzle(this.titleTextBox.Text, this.imageFile, "SoonLearning");

            ControlMgr.Instance.DataMgr.Add(item);

       //     File.Copy(this.imageFile, System.IO.Path.Combine(dataFolder, item.ImageFile));

            ControlMgr.Instance.PuzzleStartupPage.ShowPuzzleList();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.PuzzleStartupPage.ShowPuzzleList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = SoonLearning.BlockPuzzle.Properties.Resources.ImageFilter;
            if (openFileDlg.ShowDialog(App.Current.MainWindow).Value)
            {
                this.imageFile = openFileDlg.FileName;
                this.puzzleImage.Source = new BitmapImage(new Uri(openFileDlg.FileName));
            }
        }

        

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PuzzleControl.Instance.ControlPanlVisible(false);

            Storyboard storyboard = this.TryFindResource("loadStoryboard") as Storyboard;
            if (storyboard != null)
                storyboard.Begin();
        }
    }
}
