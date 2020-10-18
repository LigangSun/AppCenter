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
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using SoonLearning.Memorize.Data;
using System.Security.Cryptography;

namespace SoonLearning.AppManagementTool
{
    /// <summary>
    /// Interaction logic for CreatePackWindow.xaml
    /// </summary>
    public partial class CreatePackWindow : Window
    {
        private GadgetItemOnline gadetItemOnline;
        private string thumbnailFile;
        private string id = string.Empty;

        private MemorizeEntry memorizeEntry;

        private string projectFile;

        public CreatePackWindow()
        {
            InitializeComponent();
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetDirectoryName(assembly.Location));

            try
            {
                //System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
                //dlg.SelectedPath = System.IO.Path.GetDirectoryName(assembly.Location);
                //if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string packFile = this.outputPathTextBox.Text;
                    if (!Directory.Exists(packFile))
                        Directory.CreateDirectory(packFile);

                    string fileExt = System.IO.Path.GetExtension(this.fileNameTextBox.Text);
                    this.projectFile = string.Empty;
                    if (fileExt == ".mre")
                    {
                        this.createMrePackFile(packFile);
                    }
                    else if (fileExt == ".dll")
                    {
                        this.createAppPackFile(packFile);
                    }
                }
            }
            catch (Exception ex)
            {
                StringBuilder message = new StringBuilder("生成安装包失败!");
                message.AppendLine(ex.Message);
                MessageBox.Show(message.ToString(), "速学应用平台", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void createMrePackFile(string file)
        {
            string packFile = System.IO.Path.Combine(file, this.id + ".slp");

            FileStream fs = File.OpenWrite(packFile);

            try
            {
                // Header
                this.WriteString(fs, "{BE4A1507-5B37-42EA-9E08-43EF4F363C42}");

                // AppId
                this.WriteString(fs, id);
                // Title
                this.WriteString(fs, this.titleTextBlock.Text);
                this.WriteString(fs, this.descriptionTextBlock.Text);

                // Thumbnail ext
                this.WriteString(fs, System.IO.Path.GetExtension(this.memorizeEntry.Thumbnail));

                byte[] emptyData = new byte[0];
                // Thumbnail data
                if (File.Exists(this.thumbnailFile))
                    this.WriteBytes(fs, File.ReadAllBytes(this.thumbnailFile));
                else
                    this.WriteBytes(fs, emptyData);

                // Write a empty entry as Memorize App no entry and dll file.
                this.WriteString(fs, System.IO.Path.GetFileName(this.fileNameTextBox.Text));
                this.WriteString(fs, string.Empty);

                // Type
                this.WriteBytes(fs, BitConverter.GetBytes(102));
                this.WriteBytes(fs, BitConverter.GetBytes(this.memorizeEntry.SubType));

                // Create Time
                this.WriteString(fs, this.memorizeEntry.CreateDate.ToString());

                // Creator Information
                this.WriteString(fs, this.memorizeEntry.Creator);
                this.WriteString(fs, this.memorizeEntry.CreatorLogo);
                this.WriteString(fs, this.memorizeEntry.CreatorWebsite);

                string rootDir = System.IO.Path.GetDirectoryName(this.fileNameTextBox.Text);
                // File count
                this.WriteString(fs, (this.additionalFileListBox.Items.Count + 1).ToString());

                string mreFile = this.fileNameTextBox.Text.Remove(0, rootDir.Length);
                this.WriteString(fs, mreFile);

                this.WriteBytes(fs, File.ReadAllBytes(this.fileNameTextBox.Text));

                foreach (string temp in this.additionalFileListBox.Items)
                {
                    string fileName = temp.Remove(0, rootDir.Length);
                    this.WriteString(fs, fileName);

                    if (File.Exists(temp))
                        this.WriteBytes(fs, File.ReadAllBytes(temp));
                    else
                        this.WriteBytes(fs, emptyData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        private void createAppPackFile(string file)
        {
            string packFile = System.IO.Path.Combine(file, gadetItemOnline.Id + ".slp");

            FileStream fs = File.OpenWrite(packFile);

            // Header
            this.WriteString(fs, "{B5F6844E-984C-4129-8D19-79FDEFBDD5DC}");

            // AppId
            this.WriteString(fs, id);
            // Title
            this.WriteString(fs, this.titleTextBlock.Text);
            this.WriteString(fs, this.descriptionTextBlock.Text);

            // Thumbnail ext
            this.WriteString(fs, System.IO.Path.GetExtension(gadetItemOnline.Thumbnail));

            // Thumbnail data
            this.WriteBytes(fs, File.ReadAllBytes(this.thumbnailFile));

            this.WriteString(fs, System.IO.Path.GetFileName(gadetItemOnline.DllFile));
            this.WriteString(fs, gadetItemOnline.Entry);

            // Type
            this.WriteBytes(fs, BitConverter.GetBytes((int)gadetItemOnline.AppType));
            this.WriteBytes(fs, BitConverter.GetBytes((int)gadetItemOnline.AppSubType));

            // Create Time
            this.WriteString(fs, gadetItemOnline.CreateDate.ToString());

            // Creator Information
            this.WriteString(fs, gadetItemOnline.Creator);
            this.WriteString(fs, gadetItemOnline.CreatorLogo);
            this.WriteString(fs, gadetItemOnline.CreatorWebSite);

            string rootDir = System.IO.Path.GetDirectoryName(this.fileNameTextBox.Text);
            // File count
            this.WriteString(fs, (this.additionalFileListBox.Items.Count+1).ToString());

            string mreFile = this.fileNameTextBox.Text.Remove(0, rootDir.Length);
            this.WriteString(fs, mreFile);

            this.WriteBytes(fs, File.ReadAllBytes(this.fileNameTextBox.Text));

            foreach (string temp in this.additionalFileListBox.Items)
            {
                string fileName = temp.Remove(0, rootDir.Length);
                this.WriteString(fs, fileName);

                if (File.Exists(temp))
                    this.WriteBytes(fs, File.ReadAllBytes(temp));
                else
                    this.WriteBytes(fs, new byte[0]);
            }

            fs.Close();
        }

        private void WriteString(Stream stream, string text)
        {
            byte[] textData = Encoding.UTF8.GetBytes(text);
            byte[] textDataLength = BitConverter.GetBytes(textData.Length);
            stream.Write(textDataLength, 0, 4);
            stream.Write(textData, 0, textData.Length);
        }

        private void WriteBytes(Stream stream, byte[] bytes)
        {
            byte[] byteLen = BitConverter.GetBytes(bytes.Length);
            stream.Write(byteLen, 0, 4);
            stream.Write(bytes, 0, bytes.Length);
        }

        private void BrowseFolder(string folder)
        {
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(folder);
            foreach (System.IO.DirectoryInfo subDi in di.GetDirectories())
            {
                this.BrowseFolder(subDi.FullName);
            }

            foreach (System.IO.FileInfo fi in di.GetFiles())
            {
                this.additionalFileListBox.Items.Add(fi.FullName);
            }
        }

        private void addFilesButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Multiselect = true;
            if (openFileDlg.ShowDialog().Value)
            {
                foreach (string file in openFileDlg.FileNames)
                {
                    if (this.additionalFileListBox.Items.Contains(file))
                        continue;
                    this.additionalFileListBox.Items.Add(file);
                }
            }
        }

        private void removeFilesButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.additionalFileListBox.SelectedIndex < 0)
                return;

            List<string> removeList = new List<string>();
            foreach (string item in this.additionalFileListBox.SelectedItems)
            {
                removeList.Add(item);
            }

            foreach (string item in removeList)
            {
                this.additionalFileListBox.Items.Remove(item);
            }
        }

        private void selectEntryFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "*.dll|*.dll|记忆类应用|*.mre";
            if (openFileDlg.ShowDialog().Value)
            {
                this.fileNameTextBox.Text = openFileDlg.FileName;
                string fileExt = System.IO.Path.GetExtension(this.fileNameTextBox.Text);
                this.additionalFileListBox.Items.Clear();
                if (fileExt == ".mre")
                {
                    this.loadMreItem(this.fileNameTextBox.Text);
                }
                else if (fileExt == ".dll")
                {
                    this.loadAppItem(this.fileNameTextBox.Text);
                }
            }
        }

        private void loadMreItem(string fileName)
        {
            this.memorizeEntry = MemorizeEntry.Load(fileName);

            this.titleTextBlock.Text = this.memorizeEntry.Title;
            this.descriptionTextBlock.Text = this.memorizeEntry.Description;

            this.thumbnailFile = this.memorizeEntry.Thumbnail;

            this.id = this.memorizeEntry.Id;
            foreach (string file in this.memorizeEntry.BackgroundImages)
            {
                this.additionalFileListBox.Items.Add(file);
            }

            foreach (string file in this.memorizeEntry.BackgroundMusics)
            {
                this.additionalFileListBox.Items.Add(file);
            }

            if (!string.IsNullOrEmpty(this.memorizeEntry.MemorizeItemBackground))
                this.additionalFileListBox.Items.Add(this.memorizeEntry.MemorizeItemBackground);

            if (!string.IsNullOrEmpty(this.memorizeEntry.ItemClickMusic))
                this.additionalFileListBox.Items.Add(this.memorizeEntry.ItemClickMusic);

            if (!string.IsNullOrEmpty(this.memorizeEntry.Thumbnail))
                this.additionalFileListBox.Items.Add(this.memorizeEntry.Thumbnail);

            foreach (MemorizeItem item in this.memorizeEntry.Items)
            {
                this.getItemInfo(item.ObjectA);
                this.getItemInfo(item.ObjectB);
            }
        }

        private void getItemInfo(MemorizeObject obj)
        {
            if (obj is MemorizeImage)
            {
                MemorizeImage image = obj as MemorizeImage;
                if (this.additionalFileListBox.Items.Contains(image.Url))
                    return;
                this.additionalFileListBox.Items.Add(image.Url);
            }
            else if (obj is MemorizeMusic)
            {
                MemorizeMusic music = obj as MemorizeMusic;
                if (this.additionalFileListBox.Items.Contains(music.Url))
                    return;
                this.additionalFileListBox.Items.Add(music.Url);
            }
        }

        private void loadAppItem(string fileName)
        {
            GadgetItemOnline item = Helper.LoadApp(fileName);

            this.gadetItemOnline = item;
            if (item == null)
            {
                MessageBox.Show("加载应用失败，请把打包工具与您的Dll文件放在同一文件夹下！", "打包工具", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.titleTextBlock.Text = item.Title;
            this.descriptionTextBlock.Text = item.Description;
       //     this.fileNameTextBox.Text = System.IO.Path.GetDirectoryName(item.DllFile);

            this.thumbnailFile = item.LocalThumbnailFile;

            this.id = this.gadetItemOnline.Id;

            string drive = System.IO.Path.GetPathRoot(item.DllFile);
            Assembly assembly = Assembly.LoadFile(item.DllFile);
            AssemblyName[] refAssemblies = assembly.GetReferencedAssemblies();
            foreach (AssemblyName name in refAssemblies)
            {
                Assembly refAssembly = Assembly.Load(name);
                if (drive == System.IO.Path.GetPathRoot(refAssembly.Location))
                {
                    if (refAssembly.ManifestModule.Name == "SoonLearning.AppCenter.exe" ||
                        refAssembly.ManifestModule.Name == "SoonLearning.AppCenter.Common.dll")
                        continue;
                    this.additionalFileListBox.Items.Add(refAssembly.Location);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void browseTargetButton_Click(object sender, RoutedEventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetDirectoryName(assembly.Location));

            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            dlg.SelectedPath = System.IO.Path.GetDirectoryName(assembly.Location);
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string packFile = dlg.SelectedPath;
                this.outputPathTextBox.Text = packFile;
                if (!Directory.Exists(packFile))
                    Directory.CreateDirectory(packFile);
            }
        }

        private void saveConfigButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.projectFile))
            {
                SaveFileDialog saveFileDlg = new SaveFileDialog();
                saveFileDlg.Filter = "*.xml|*.xml";
                if (saveFileDlg.ShowDialog().Value)
                {
                    this.projectFile = saveFileDlg.FileName;
                }
            }

            ProjectData data = new ProjectData()
            {
                MainFile = this.fileNameTextBox.Text
            };
            foreach (string file in this.additionalFileListBox.Items)
            {
                data.AdditionalFiles.Add(file);
            }

            SerializerHelper<ProjectData>.XmlSerialize(this.projectFile, data);
        }

        private void openConfigButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "*.xml|*.xml";
            if (openFileDlg.ShowDialog().Value)
            {
                try
                {
                    ProjectData projectData = SerializerHelper<ProjectData>.XmlDeserialize(openFileDlg.FileName);
                    this.projectFile = openFileDlg.FileName;

                    this.fileNameTextBox.Text = projectData.MainFile;
                    string fileExt = System.IO.Path.GetExtension(projectData.MainFile);
                    if (fileExt == ".mre")
                    {
                        this.loadMreItem(this.fileNameTextBox.Text);
                    }
                    else if (fileExt == ".dll")
                    {
                        this.loadAppItem(this.fileNameTextBox.Text);
                    }

                    this.additionalFileListBox.Items.Clear();

                    foreach (var item in projectData.AdditionalFiles)
                    {
                        bool exist = false;
                        foreach (string file in this.additionalFileListBox.Items)
                        {
                            if (file == item)
                            {
                                exist = true;
                                break;
                            }
                        }

                        if (exist)
                            continue;

                        this.additionalFileListBox.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("打开工程失败." + ex.Message, "创建工具", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
