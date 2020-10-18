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
using SoonLearning.Memorize.Data;

namespace MemorizeAppCreator
{
    /// <summary>
    /// Interaction logic for MemorizeItemWindow.xaml
    /// </summary>
    public partial class MemorizeItemWindow : Window
    {
        private MemorizeItem memorizeItem;

        public MemorizeItemWindow(MemorizeItem item)
        {
            InitializeComponent();

            if (item == null)
            {
                this.memorizeItem = new MemorizeItem();
            }
            else
            {
                this.memorizeItem = item;
                this.setContent();
            }
        }

        private void aImageBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.OpenFile(UIHelper.imageFilter, this.aImageTextBox);
        }

        private void bImageBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.OpenFile(UIHelper.imageFilter, this.bImageTextBox);
        }

        private void aMusicBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.OpenFile(UIHelper.musicFilter, this.aMusicTextBox);
        }

        private void bMusicBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.OpenFile(UIHelper.musicFilter, this.bMusicTextBox);
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.getContent())
                return;

            this.DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private bool getContent()
        {
            if (this.aTextRadioButton.IsChecked.Value)
            {
                this.memorizeItem.ObjectA = new MemorizeText(this.aTextBox.Text);
            }
            else if (this.aImageRadioButton.IsChecked.Value)
            {
                int count = -1;
                if (this.aShowMoreImageCheckBox.IsChecked.Value)
                {
                    count = 0;
                    if (!int.TryParse(this.aImageCountTextBox.Text, out count))
                    {
                        //    MessageBox.Show("请输入数字!", "物品图片", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    this.aImageCountTextBox.SelectAll();
                        //    this.aImageCountTextBox.Focus();
                        //    return false;
                    }
                }
                this.memorizeItem.ObjectA = new MemorizeImage(this.aImageTextBox.Text, count, ImageLayout.Random);
            }
            else if (this.aMusicRadioButton.IsChecked.Value)
            {
                this.memorizeItem.ObjectA = new MemorizeMusic(this.aMusicTextBox.Text);
            }
            else if (this.aReadTextRadioButton.IsChecked.Value)
            {
                this.memorizeItem.ObjectA = new MemorizeReadText(this.aReadTextBox.Text);
            }
            else
            {
                MessageBox.Show("请为物品A设置内容!", "物品图片", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (this.bTextRadioButton.IsChecked.Value)
            {
                this.memorizeItem.ObjectB = new MemorizeText(this.bTextBox.Text);
            }
            else if (this.bImageRadioButton.IsChecked.Value)
            {
                int count = -1;
                if (this.bShowMoreImageCheckBox.IsChecked.Value)
                {
                    count = 0;
                    if (!int.TryParse(this.bImageCountTextBox.Text, out count))
                    {
                        //    MessageBox.Show("请输入数字!", "物品图片", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    this.bImageCountTextBox.SelectAll();
                        //    this.bImageCountTextBox.Focus();
                        //    return false;
                    }
                }
                this.memorizeItem.ObjectB = new MemorizeImage(this.bImageTextBox.Text, count, ImageLayout.Random);
            }
            else if (this.bMusicRadioButton.IsChecked.Value)
            {
                this.memorizeItem.ObjectB = new MemorizeMusic(this.bMusicTextBox.Text);
            }
            else if (this.bReadTextRadioButton.IsChecked.Value)
            {
                this.memorizeItem.ObjectB = new MemorizeReadText(this.bReadTextBox.Text);
            }
            else
            {
                MessageBox.Show("请为物品B设置内容!", "物品图片", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void setContent()
        {
            if (this.memorizeItem.ObjectA is MemorizeReadText)
            {
                this.aReadTextBox.Text = ((MemorizeReadText)this.memorizeItem.ObjectA).Text;
                this.aReadTextRadioButton.IsChecked = true;
            }
            else if (this.memorizeItem.ObjectA is MemorizeText)
            {
                this.aTextBox.Text = ((MemorizeText)this.memorizeItem.ObjectA).Text;
                this.aTextRadioButton.IsChecked = true;
            }
            else if (this.memorizeItem.ObjectA is MemorizeImage)
            {
                this.aImageTextBox.Text = ((MemorizeImage)this.memorizeItem.ObjectA).Url;
                if (((MemorizeImage)this.memorizeItem.ObjectA).Count >= 0)
                {
                    this.aShowMoreImageCheckBox.IsChecked = true;
                    this.aImageCountTextBox.Text = ((MemorizeImage)this.memorizeItem.ObjectA).Count.ToString();
                }
                this.aImageRadioButton.IsChecked = true;
            }
            else if (this.memorizeItem.ObjectA is MemorizeMusic)
            {
                this.aMusicTextBox.Text = ((MemorizeMusic)this.memorizeItem.ObjectA).Url;
                this.aMusicRadioButton.IsChecked = true;
            }

            if (this.memorizeItem.ObjectB is MemorizeReadText)
            {
                this.bReadTextBox.Text = ((MemorizeReadText)this.memorizeItem.ObjectB).Text;
                this.bReadTextRadioButton.IsChecked = true;
            }
            else if (this.memorizeItem.ObjectB is MemorizeText)
            {
                this.bTextBox.Text = ((MemorizeText)this.memorizeItem.ObjectB).Text;
                this.bTextRadioButton.IsChecked = true;
            }
            else if (this.memorizeItem.ObjectB is MemorizeImage)
            {
                this.bImageTextBox.Text = ((MemorizeImage)this.memorizeItem.ObjectB).Url;
                if (((MemorizeImage)this.memorizeItem.ObjectB).Count >= 0)
                {
                    this.bShowMoreImageCheckBox.IsChecked = true;
                    this.bImageCountTextBox.Text = ((MemorizeImage)this.memorizeItem.ObjectB).Count.ToString();
                }
                this.bImageRadioButton.IsChecked = true;
            }
            else if (this.memorizeItem.ObjectB is MemorizeMusic)
            {
                this.bMusicTextBox.Text = ((MemorizeMusic)this.memorizeItem.ObjectB).Url;
                this.bMusicRadioButton.IsChecked = true;
            }
        }
    }
}
