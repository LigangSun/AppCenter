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
using System.Reflection;
using System.IO;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter;

namespace SoonLearning.ConnectNumber
{
    /// <summary>
    /// Interaction logic for NewDrawNumberWindow.xaml
    /// </summary>
    public partial class NewDrawNumberWindow : UserControl
    {
        private double canvasWidth;
        private double canvasHeight;

        public NewDrawNumberWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.canvasWidth = this.drawNumberEditingCanvas.ActualWidth;
            this.canvasHeight = this.drawNumberEditingCanvas.ActualHeight;
            this.drawNumberEditingCanvas.NewDrawNumber();
        }

        private void imageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "*.png|*.png";
            if (openFileDlg.ShowDialog(App.Current.MainWindow).Value)
            {
                this.drawNumberEditingCanvas.ImageFile = openFileDlg.FileName;
                this.zoomSlider.Value = this.drawNumberEditingCanvas.NumberImageScale;
            }
        }

        private void addNumberBtn_Click(object sender, RoutedEventArgs e)
        {
            this.drawNumberEditingCanvas.AddCircle();
        }

        private void bkImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = SoonLearning.ConnectNumber.Properties.Resources.ImageFilter;
            if (openFileDlg.ShowDialog(App.Current.MainWindow).Value)
            {
                System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(openFileDlg.FileName);

                Vector vec = GC_UIHelper.MatchImageToWnd(new Size(bmp.Width, bmp.Height), new Size(this.canvasWidth, this.canvasHeight));
                this.drawNumberEditingCanvas.Width = vec.X;
                this.drawNumberEditingCanvas.Height = vec.Y;

                bmp.Dispose();

                this.drawNumberEditingCanvas.BkImageFile = openFileDlg.FileName;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.titleTextBox.Text))
            {
                this.titleTextBox.SelectAll();
                this.titleTextBox.Focus();
                return;
            }

            Assembly assembly = Assembly.GetEntryAssembly();
            string dataFolder = System.IO.Path.GetDirectoryName(assembly.Location);
            dataFolder = System.IO.Path.Combine(dataFolder, @"Data\DrawNumber");
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            this.drawNumberEditingCanvas.Save();
            this.drawNumberEditingCanvas.DrawNumberData.DrawNumberItem.TitleText = this.titleTextBox.Text;
            this.drawNumberEditingCanvas.DrawNumberData.DrawNumberItem.DescriptionText = this.descriptionTextBox.Text;
            this.drawNumberEditingCanvas.DrawNumberData.DrawNumberItem.Creator = this.creatorTextBox.Text;
            this.drawNumberEditingCanvas.DrawNumberData.DrawNumberItem.CreateDate = DateTime.Now;

            this.drawNumberEditingCanvas.DrawNumberData.Save(System.IO.Path.Combine(dataFolder, this.titleTextBox.Text + ".dd"));

            ControlMgr.Instance.DataMgr.Add(this.drawNumberEditingCanvas.DrawNumberData.DrawNumberItem);

            ControlMgr.Instance.DrawNumberStartupPage.ShowListPage();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.DrawNumberStartupPage.ShowListPage();
        }

        private void zoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.drawNumberEditingCanvas.NumberImageScale = this.zoomSlider.Value;
        }
    }
}
