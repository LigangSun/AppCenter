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
using SoonLearning.TeachAppMaker.Modules;
using Microsoft.Win32;

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for NewWindow.xaml
    /// </summary>
    public partial class NewWindow : Window
    {
        internal const string imageFilter = "所以图片|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff|Jpeg文件|*.jpg;*.jpeg|位图文件|*.bmp|png文件|*.png|Gif文件|*.gif|tiff文件|*.tiff;*.tif";

        internal string AssessmentName
        {
            get
            {
                return this.titleTextBox.Text;
            }
            set { this.titleTextBox.Text = value; }
        }

        internal string Description
        {
            get { return this.descriptionTextBox.Text; }
            set { this.descriptionTextBox.Text = value; }
        }

        internal string Thumbnail
        {
            get { return this.thumbnailTextBox.Text; }
            set { this.thumbnailTextBox.Text = value; }
        }

        public NewWindow()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = imageFilter;
            if (openFileDlg.ShowDialog().Value)
            {
                //using (System.Drawing.Image image = System.Drawing.Image.FromFile(openFileDlg.FileName))
                //{
                //    if (image.Width > 256 || image.Height > 256)
                //    {
                //        MessageBox.Show("请选择宽和高小于256像素的图片。", "图片超出范围", MessageBoxButton.OK, MessageBoxImage.Warning);
                //        return;
                //    }
                //}
                this.thumbnailTextBox.Text = openFileDlg.FileName;
            }
        }
    }
}
