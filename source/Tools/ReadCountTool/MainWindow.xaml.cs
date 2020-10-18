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
using Microsoft.Win32;
using SoonLearning.ReadCount.Data;
using System.Collections.ObjectModel;
using SoonLearning.AppCenter.Utility;

namespace ReadCountTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<ReadCountStage> stageCollection = new ObservableCollection<ReadCountStage>();
        private ObservableCollection<ImageItem> backgroundImageCollection = new ObservableCollection<ImageItem>();
        private ObservableCollection<ImageItem> goodsImageCollection = new ObservableCollection<ImageItem>();
        private ObservableCollection<ImageItem> musicCollection = new ObservableCollection<ImageItem>();

        public MainWindow()
        {
            InitializeComponent();

            this.stageListBox.ItemsSource = this.stageCollection;
            this.backgroundImageListBox.ItemsSource = this.backgroundImageCollection;
            this.goodsImageListBox.ItemsSource = this.goodsImageCollection;
            this.musicListBox.ItemsSource = this.musicCollection;
        }

        private void saveAllButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDlg = new SaveFileDialog();
            saveFileDlg.Filter = "*.xml|*.xml";
            if (saveFileDlg.ShowDialog().Value)
            {
                try
                {
                    string path = System.IO.Path.GetDirectoryName(saveFileDlg.FileName);
                    string backgroundPath = System.IO.Path.Combine(path, "Background");
                    string goodsPath = System.IO.Path.Combine(path, "Goods");
                    string backgroundMusic = System.IO.Path.Combine(path, "Music");

                    this.CreateDirectory(backgroundPath);
                    this.CreateDirectory(goodsPath);
                    this.CreateDirectory(backgroundMusic);

                    this.SaveMusics(backgroundMusic);

                    foreach (ReadCountStage stage in this.stageCollection)
                        this.SaveImages(backgroundPath, goodsPath, stage);

                    if (System.IO.File.Exists(saveFileDlg.FileName))
                        System.IO.File.Delete(saveFileDlg.FileName);

                    ReadCountStageCollection readCountStageCollection = new ReadCountStageCollection();
                    foreach (ImageItem musicItem in this.musicCollection)
                    {
                        readCountStageCollection.BackgroundMusicList.Add(System.IO.Path.GetFileName(musicItem.File));
                    }
                    foreach (ReadCountStage stage in Enumerable.ToArray<ReadCountStage>(this.stageCollection))
                        readCountStageCollection.StageCollection.Add(stage);

                    SerializerHelper<ReadCountStageCollection>.XmlSerialize(saveFileDlg.FileName, readCountStageCollection);

                    foreach (ReadCountStage stage in this.stageCollection)
                    {
                        stage.BackgroundImage = System.IO.Path.Combine(backgroundPath, stage.BackgroundImage);
                        foreach (ReadCountItem item in stage.Items)
                        {
                            item.GoodsImage = System.IO.Path.Combine(goodsPath, item.GoodsImage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }   
            }
        }

        private void CreateDirectory(string path)
        {
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
        }

        private void SaveImages(string backgroundPath, string goodsPath, ReadCountStage stage)
        {
            this.CopyFile(backgroundPath, stage.BackgroundImage);
            stage.BackgroundImage = System.IO.Path.GetFileName(stage.BackgroundImage);
            foreach (ReadCountItem item in stage.Items)
            {
                this.CopyFile(goodsPath, item.GoodsImage);
                item.GoodsImage = System.IO.Path.GetFileName(item.GoodsImage);
            }
        }

        private void SaveMusics(string musicPath)
        {
            foreach (ImageItem item in this.musicCollection)
            {
                this.CopyFile(musicPath, item.File);
            }
        }

        private void CopyFile(string targetFolder, string sourceFile)
        {
            try
            {
                string targetFile = System.IO.Path.Combine(targetFolder, System.IO.Path.GetFileName(sourceFile));
                System.IO.File.Copy(sourceFile, targetFile, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "*.xml|*.xml";
            if (openFileDlg.ShowDialog().Value)
            {
                try
                {
                    string path = System.IO.Path.GetDirectoryName(openFileDlg.FileName);
                    string backgroundPath = System.IO.Path.Combine(path, "Background");
                    string goodsPath = System.IO.Path.Combine(path, "Goods");
                    string musicPath = System.IO.Path.Combine(path, "Music");

                    ReadCountStageCollection stages = SerializerHelper<ReadCountStageCollection>.XmlDeserialize(openFileDlg.FileName);
                    foreach (ReadCountStage stage in stages.StageCollection)
                    {
                        this.stageCollection.Add(stage);

                        bool found = false;
                        stage.BackgroundImage = System.IO.Path.Combine(backgroundPath, stage.BackgroundImage);
                        foreach (ImageItem bk in this.backgroundImageCollection)
                        {
                            if (bk.File == stage.BackgroundImage)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            ImageItem bkItem = new ImageItem();
                            bkItem.File = stage.BackgroundImage;
                            bkItem.Title = System.IO.Path.GetFileNameWithoutExtension(bkItem.File);
                            this.backgroundImageCollection.Add(bkItem);
                        }

                        foreach (ReadCountItem item in stage.Items)
                        {
                            item.GoodsImage = System.IO.Path.Combine(goodsPath, item.GoodsImage);

                            found = false;
                            foreach (ImageItem temp in this.goodsImageCollection)
                            {
                                if (temp.File == item.GoodsImage)
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (found)
                                continue;

                            ImageItem imageItem = new ImageItem();
                            imageItem.File = item.GoodsImage;
                            imageItem.Title = System.IO.Path.GetFileNameWithoutExtension(imageItem.File);
                            this.goodsImageCollection.Add(imageItem);
                        }
                    }

                    for (int i = 0; i<stages.BackgroundMusicList.Count; i++)
                    {
                        stages.BackgroundMusicList[i] = System.IO.Path.Combine(musicPath, stages.BackgroundMusicList[i]);
                        ImageItem musicItem = new ImageItem();
                        musicItem.File = stages.BackgroundMusicList[i];
                        musicItem.Title = System.IO.Path.GetFileName(stages.BackgroundMusicList[i]);
                        this.musicCollection.Add(musicItem);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void addStageButton_Click(object sender, RoutedEventArgs e)
        {
            ReadCountStage stage = new ReadCountStage();
            stage.Title = string.Format("Stage {0}", this.stageCollection.Count);
            this.stageCollection.Add(stage);
        }

        private void stageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReadCountStage stage = this.stageListBox.SelectedItem as ReadCountStage;
            ReadCountItem goodsItem = this.goodsListBox.SelectedItem as ReadCountItem;

            //this.backgroundImageButton.IsEnabled = stage != null;
            //this.addGoodsButton.IsEnabled = stage != null;
            //this.removeGoodsButton.IsEnabled = stage != null && goodsItem != null;

            this.stageInfoPanel.DataContext = stage;
        }

        private void goodsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReadCountStage stage = this.stageListBox.SelectedItem as ReadCountStage;
            ReadCountItem goodsItem = this.goodsListBox.SelectedItem as ReadCountItem;

            this.removeGoodsButton.IsEnabled = stage != null && goodsItem != null;
        }

        private void removeStageButton_Click(object sender, RoutedEventArgs e)
        {
            ReadCountStage stage = this.stageListBox.SelectedItem as ReadCountStage;
            if (stage == null)
                return;

            this.stageCollection.Remove(stage);
        }

        private void addMusicButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Music Files (*.mp3, *.wma, *.wav)|*.mp3;*.wma;*.wav";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog().Value)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    ImageItem item = new ImageItem();
                    item.File = file;
                    item.Title = System.IO.Path.GetFileName(file);
                    this.musicCollection.Add(item);
                }
            }
        }

        private void backgroundImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.*|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog().Value)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    ImageItem item = new ImageItem();
                    item.File = file;
                    item.Title = System.IO.Path.GetFileNameWithoutExtension(file);
                    this.backgroundImageCollection.Add(item);
                }
            }
        }

        private void setAsBackgroundImageButton_Click(object sender, RoutedEventArgs e)
        {
            ReadCountStage stage = this.stageListBox.SelectedItem as ReadCountStage;
            if (stage == null)
                return;

            ImageItem imageItem = this.backgroundImageListBox.SelectedItem as ImageItem;
            if (imageItem == null)
                return;

            stage.BackgroundImage = imageItem.File;
        }

        private void backgroundImageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.setAsBackgroundImageButton.IsEnabled = this.backgroundImageListBox.SelectedItem != null;
            this.removeBackgroundImageButton.IsEnabled = this.setAsBackgroundImageButton.IsEnabled;
        }

        private void removeBackgroundImageButton_Click(object sender, RoutedEventArgs e)
        {
            ImageItem imageItem = this.backgroundImageListBox.SelectedItem as ImageItem;
            if (imageItem == null)
                return;

            this.backgroundImageCollection.Remove(imageItem);
        }

        private void goodsImageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.removeGoodsButton.IsEnabled = this.goodsImageListBox.SelectedItem != null;
            this.addToCurrentStageButton.IsEnabled = this.goodsImageListBox.SelectedItem != null;
        }

        private void addGoodsButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "*.*|*.*";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog().Value)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    ImageItem item = new ImageItem();
                    item.File = file;
                    item.Title = System.IO.Path.GetFileNameWithoutExtension(file);
                    this.goodsImageCollection.Add(item);
                }
            }
        }

        private void removeGoodsButton_Click(object sender, RoutedEventArgs e)
        {
            ImageItem item = this.goodsImageListBox.SelectedItem as ImageItem;
            if (item != null)
                this.goodsImageListBox.Items.Remove(item);
        }

        private void addToCurrentStageButton_Click(object sender, RoutedEventArgs e)
        {
            ReadCountStage stage = this.stageListBox.SelectedItem as ReadCountStage;
            if (stage == null)
                return;

            ImageItem imageItem = this.goodsImageListBox.SelectedItem as ImageItem;
            if (imageItem != null)
            {
                ReadCountItem item = new ReadCountItem(imageItem.File, 0, 9, true);
                stage.Items.Add(item);
            }
        }

        private void removeGoodsItemButton_Click(object sender, RoutedEventArgs e)
        {
            ReadCountStage stage = this.stageListBox.SelectedItem as ReadCountStage;
            if (stage == null)
                return;

            ReadCountItem item = this.goodsListBox.SelectedItem as ReadCountItem;
            if (item != null)
                stage.Items.Remove(item);
        }

        private void addMusicToCurrentStageButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void musicListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.removeMusicButton.IsEnabled = this.musicListBox.SelectedItem != null;
        }
    }
}
