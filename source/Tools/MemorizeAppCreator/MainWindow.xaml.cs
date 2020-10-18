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
using SoonLearning.Memorize.Data;
using Microsoft.Win32;
using System.Reflection;
using SoonLearning.AppManagementTool;
using System.IO;

namespace MemorizeAppCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MemorizeEntry memorizeEntry;
        private string projectFile = string.Empty;
        private bool isChanged;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void newMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.newProject();
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.openProject();
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.saveProject();
        }

        private void saveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "记忆项目文件|*.mre";
            if (dlg.ShowDialog().Value)
            {
                this.projectFile = dlg.FileName;
                this.saveProject();
            }
        }

        private bool saveProject()
        {
            if (this.memorizeEntry == null)
                return false;

            if (string.IsNullOrEmpty(this.memorizeEntry.Title))
            {
                MessageBox.Show("应用标题不能为空!", "记忆工具", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(this.memorizeEntry.Description))
            {
                MessageBox.Show("应用描述不能为空!", "记忆工具", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (this.subTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("请选择应用类型!", "记忆工具", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrEmpty(this.projectFile))
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Filter = "记忆项目文件|*.mre";
                if (dlg.ShowDialog().Value)
                    this.projectFile = dlg.FileName;
            }

            if (string.IsNullOrEmpty(this.projectFile))
                return false;

            MemorizeTypeItem item = this.subTypeComboBox.SelectedItem as MemorizeTypeItem;
            this.memorizeEntry.SubType = item.Type;

            try
            {
                this.memorizeEntry.Save(this.projectFile);
            }
            catch (Exception ex)
            {
                StringBuilder strBuilder = new StringBuilder("保存工程失败!");
                strBuilder.AppendLine(ex.Message);
                MessageBox.Show(strBuilder.ToString(), "记忆工具", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            this.isChanged = false;

            return true;
        }

        private void newProject()
        {
            if (this.isChanged)
            {
                MessageBoxResult ret = MessageBox.Show("当前项目已经被修改，在新建项目前是否要保存当前修改?", "记忆工具", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    if (!this.saveProject())
                        return;
                }
                else if (ret == MessageBoxResult.No)
                {
                }
                else if (ret == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            NewWindow newWindow = new NewWindow();
            if (newWindow.ShowDialog().Value)
            {
                this.memorizeEntry = new MemorizeEntry();
                this.DataContext = this.memorizeEntry;
                this.projectFile = string.Empty;
                this.isChanged = false;

                if (newWindow.Type == 0)
                {
                    MemorizeCreatorUserControl generalCreator = new MemorizeCreatorUserControl(this.memorizeEntry);
                    this.creatorStackPanel.Children.Clear();
                    this.creatorStackPanel.Children.Add(generalCreator);
                }
                else if (newWindow.Type == 1)
                {
                    MemorizeMathCreatorUserControl mathCreator = new MemorizeMathCreatorUserControl(this.memorizeEntry);
                    this.creatorStackPanel.Children.Clear();
                    this.creatorStackPanel.Children.Add(mathCreator);
                }
            }
        }

        private void openProject()
        {
            if (this.isChanged)
            {
                MessageBoxResult ret = MessageBox.Show("当前项目已经被修改，在打开新项目前是否要保存当前修改?", "记忆工具", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    if (!this.saveProject())
                        return;
                }
                else if (ret == MessageBoxResult.No)
                {
                }
                else if (ret == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "记忆项目文件|*.mre";
            if (dlg.ShowDialog().Value)
            {
                this.memorizeEntry = MemorizeEntry.Load(dlg.FileName);
                this.DataContext = this.memorizeEntry;
                this.projectFile = dlg.FileName;

                if (this.memorizeEntry.Stages.Count > 0 &&
                    this.memorizeEntry.Stages[0] is MemorizeMathStage)
                {
                    MemorizeMathCreatorUserControl mathCreator = new MemorizeMathCreatorUserControl(this.memorizeEntry);
                    this.creatorStackPanel.Children.Clear();
                    this.creatorStackPanel.Children.Add(mathCreator);
                }
                else
                {
                    MemorizeCreatorUserControl generalCreator = new MemorizeCreatorUserControl(this.memorizeEntry);
                    this.creatorStackPanel.Children.Clear();
                    this.creatorStackPanel.Children.Add(generalCreator);
                }

                foreach (var item in UIHelper.MemorizeTypeItems)
                {
                    if (item.Type == this.memorizeEntry.SubType)
                    {
                        this.subTypeComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.isChanged)
            {
                MessageBoxResult ret = MessageBox.Show("当前项目已经被修改，在退出是否要保存当前修改?", "记忆工具", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (ret == MessageBoxResult.Yes)
                {
                    if (!this.saveProject())
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else if (ret == MessageBoxResult.No)
                {
                }
                else if (ret == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void itemBkImageBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.OpenFile(UIHelper.imageFilter, this.itemBkImageTextBox);
            this.isChanged = true;
        }

        private void itemClickMusicBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.OpenFile(UIHelper.musicFilter, this.itemClickMusicTextBox);
            this.isChanged = true;
        }

        #region Feedback

        private void itemFeedbackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void stageFeedbackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion


        #region Background Image

        private void backgroundImageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.removeBackgroundImageButton.IsEnabled = this.backgroundImageListBox.SelectedIndex >= 0;
            this.setAsBackgroundImageButton.IsEnabled = this.backgroundImageListBox.SelectedIndex >= 0 &&
                !this.memorizeEntry.RandomBackgroundImage;
        }

        private void backgroundImageButton_Click(object sender, RoutedEventArgs e)
        {
            string imageFile = UIHelper.OpenFile(UIHelper.imageFilter);
            string fileName = System.IO.Path.GetFileName(imageFile);
            var queryFile = from temp in this.memorizeEntry.BackgroundImages
                            where temp.Contains(fileName)
                            select temp;
            if (queryFile.Count() > 0)
                return;

            this.memorizeEntry.BackgroundImages.Add(imageFile);
            this.isChanged = true;
        }

        private void removeBackgroundImageButton_Click(object sender, RoutedEventArgs e)
        {
            this.memorizeEntry.BackgroundImages.RemoveAt(this.backgroundImageListBox.SelectedIndex);
            this.isChanged = true;
        }

        private void setAsBackgroundImageButton_Click(object sender, RoutedEventArgs e)
        {
            //string item = this.memorizeItemListBox.SelectedItem as string;
            //MemorizeStage stage = this.stageListBox.SelectedItem as MemorizeStage;
            //if (stage is MemorizeStableStage)
            //{
            //    ((MemorizeStableStage)stage).BackgroundImage = item;
            //    this.isChanged = true;
            //}
        }

        private void randomBackgroundImageCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.isChanged = true;
        }

        #endregion

        #region Background Music

        private void backgroundMusicListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.removeBackgroundMusiceButton.IsEnabled = this.backgroundImageListBox.SelectedIndex >= 0;
            this.setAsBackgroundImageButton.IsEnabled = this.backgroundImageListBox.SelectedIndex >= 0 &&
                !this.memorizeEntry.RandomBackgroundMusic;
        }

        private void backgroundMusicButton_Click(object sender, RoutedEventArgs e)
        {
            string musicFile = UIHelper.OpenFile(UIHelper.musicFilter);
            string fileName = System.IO.Path.GetFileName(musicFile);
            var queryFile = from temp in this.memorizeEntry.BackgroundMusics
                            where temp.Contains(fileName)
                            select temp;
            if (queryFile.Count() > 0)
                return;

            this.memorizeEntry.BackgroundMusics.Add(musicFile);
            this.isChanged = true;
        }

        private void removeBackgroundMusiceButton_Click(object sender, RoutedEventArgs e)
        {
            this.memorizeEntry.BackgroundMusics.RemoveAt(this.backgroundImageListBox.SelectedIndex);
            this.isChanged = true;
        }

        private void setAsBackgroundMusicButton_Click(object sender, RoutedEventArgs e)
        {
            this.isChanged = true;
        }

        private void randomBackgroundMusicCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.isChanged = true;
        }

        #endregion

        private void logoBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            UIHelper.OpenFile(UIHelper.imageFilter, this.logoTextBox);
            this.isChanged = true;
        }

        private void pkModeCheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.isChanged = true;
        }

        private void aboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWnd = new AboutWindow();
            aboutWnd.ShowDialog();
        }

        private void testMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.projectFile))
            {
                MessageBox.Show("请先保存工程，然后再进行测试!", "记忆工具", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            TestWindow testWnd = new TestWindow(this.projectFile);
            testWnd.ShowDialog();
        }

        private void publishMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.projectFile))
            {
                MessageBox.Show("请先保存工程，然后再发布该应用到速学应用平台!", "记忆工具", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                string tempPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                tempPath = System.IO.Path.Combine(tempPath, "SlpTemp");
                if (!Directory.Exists(tempPath))
                    Directory.CreateDirectory(tempPath);
                string slpFile = System.IO.Path.Combine(tempPath, this.memorizeEntry.Id + ".slp");
                PackageCreator.CreateMrePackage(this.projectFile, slpFile);

                installToLocal();

                UploadAppWindow uploadAppWindow = new UploadAppWindow(slpFile);
                uploadAppWindow.ShowDialog();

                // Install to local
            //    MessageBox.Show("发布成功后，打开速学应用平台在相应的分类学可以使用自己的应用.", "记忆工具", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("发布应用失败，请稍后再试!", "记忆工具", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void installToLocal()
        {
            RegistryKey subKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\上海速学信息科技有限公司(SoonLearning.com)\速学应用平台");
            if (subKey != null)
            {
                string appPath = subKey.GetValue("Path") as string;
                if (!string.IsNullOrEmpty(appPath))
                {
                    string dataPath = System.IO.Path.Combine(appPath, @"data\Memorize");
                    if (!Directory.Exists(dataPath))
                        Directory.CreateDirectory(dataPath);

                    string targetPath = System.IO.Path.Combine(dataPath, this.memorizeEntry.Id);
                    if (!Directory.Exists(targetPath))
                        Directory.CreateDirectory(targetPath);

                    try
                    {
                        string targetFile = System.IO.Path.Combine(targetPath, this.memorizeEntry.Title + ".mre");
                        this.memorizeEntry.Save(targetFile);

                        MessageBox.Show("你的应用已经安装到本地速学应用平台中，你可以打开速学应用平台对该应用进行测试。", "记忆工具", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        StringBuilder strBuilder = new StringBuilder("发布到本地失败!");
                        strBuilder.AppendLine(ex.Message);
                        MessageBox.Show(strBuilder.ToString(), "记忆工具", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("找不到本地安装的速学应用平台，请重新安装速学应用平台，然后再发布记忆应用。", "记忆工具", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartupWindow win = new StartupWindow();
            win.ShowDialog();
            if (win.Type == 1)
                this.newProject();
            else if (win.Type == 2)
                this.openProject();
        }
    }
}
