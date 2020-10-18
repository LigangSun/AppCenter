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
using System.Windows.Forms;
using SoonLearning.AppCenter.Upgrade;
using SoonLearning.AppCenter.Utility;
using System.Management;
using SevenZip;
using System.Reflection;
using SoonLearning.AppCenter.Data;

namespace UpgradePackCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.folderTextBox.Text = dlg.SelectedPath;
                this.fileListBox.Items.Clear();
                this.browseFolder(this.folderTextBox.Text);
            }
        }

        private void generateButton_Click(object sender, RoutedEventArgs e)
        {
            List<UpgradeItem> items = new List<UpgradeItem>();
            List<string> fileList = new List<string>();
            foreach (string file in this.fileListBox.Items)
            {
                UpgradeItem item = new UpgradeItem();
                item.PathName = file.Remove(0, this.folderTextBox.Text.Length + 1);
                items.Add(item);
                fileList.Add(file);
            }

            string configFile = System.IO.Path.Combine(this.folderTextBox.Text, "UpgradeConfig.xml");
            SerializerHelper<UpgradeItem[]>.XmlSerialize(configFile, items.ToArray());

            fileList.Add(configFile);

            string folder = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (this.Is64BitOS())
                SevenZipCompressor.SetLibraryPath(System.IO.Path.Combine(folder, "ThirdParty\\7z\\7z64.dll"));
            else
                SevenZipCompressor.SetLibraryPath(System.IO.Path.Combine(folder, "ThirdParty\\7z\\7z.dll"));

            SevenZipCompressor cmp;

            try
            {
                cmp = new SevenZipCompressor();
                cmp.ArchiveFormat = OutArchiveFormat.Zip;
                cmp.CompressionLevel = CompressionLevel.Normal;
                cmp.CompressionMode = CompressionMode.Create;
                cmp.FastCompression = false;

                string targetFolder = this.folderTextBox.Text;
                cmp.TempFolderPath = targetFolder;
                string archiveFile = System.IO.Path.Combine(targetFolder, "AppCenterUpgradePackage.zip");
                if (System.IO.File.Exists(archiveFile))
                    System.IO.File.Delete(archiveFile);

                cmp.CompressFilesEncrypted(archiveFile, "1@3$5^7*", fileList.ToArray());
            }
            catch
            {
            }
        }

        private void browseFolder(string path)
        {
            string[] files = System.IO.Directory.GetFiles(path, "*.*", System.IO.SearchOption.AllDirectories);
            foreach (string file in files)
            {
                this.fileListBox.Items.Add(file);
            }
        }

        public bool Is64BitOS()
        {
            try
            {
                string addressWidth = String.Empty;
                ConnectionOptions mConnOption = new ConnectionOptions();
                ManagementScope mMs = new ManagementScope("\\\\localhost", mConnOption);
                ObjectQuery mQuery = new ObjectQuery("select AddressWidth from Win32_Processor");
                ManagementObjectSearcher mSearcher = new ManagementObjectSearcher(mMs, mQuery);
                ManagementObjectCollection mObjectCollection = mSearcher.Get();
                foreach (ManagementObject mObject in mObjectCollection)
                {
                    addressWidth = mObject["AddressWidth"].ToString();
                }
                return (addressWidth == "64");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        private void selectFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.musicPathTextBox.Text = dlg.SelectedPath;
            }
        }

        private void generateMusicConfigButton_Click(object sender, RoutedEventArgs e)
        {
            string[] files = System.IO.Directory.GetFiles(this.musicPathTextBox.Text);
            BackgroundMusicCollection collection = new BackgroundMusicCollection();
            foreach (string file in files)
            {
                collection.Add(new BackgroundMusicItem() { FileName = System.IO.Path.GetFileName(file), Url = string.Format(@"http://www.soonlearning.com/AppBackgroundMusic/{0}", System.IO.Path.GetFileName(file)) });
            }

            SerializerHelper<BackgroundMusicCollection>.XmlSerialize(System.IO.Path.Combine(this.musicPathTextBox.Text, "BackgroundMusicConfig.xml"), collection);
        }
    }
}
