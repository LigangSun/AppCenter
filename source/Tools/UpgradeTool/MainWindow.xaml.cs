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
using System.Threading;
using System.Windows.Threading;
using System.Reflection;
using System.Management;
using SevenZip;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SoonLearning.UpgradeTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DownloadHelper helper;
        private bool downloading;
        private string localPackFile = string.Empty;
        private bool success;
        private bool done;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.titleTextBlock.Text = string.Format("{0}新版本已经发布。\n版本: {1} {2}\n自动升级程序正在为您升级到最新版本。", App.appCenterName, App.version, App.versionState);
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.Start();
            }), DispatcherPriority.Background, null);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!done)
            {
                e.Cancel = true;
            }
            else
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                Process.Start(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), "SoonLearning.AppCenter.exe"));
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.downloading)
            {
                if (this.helper != null)
                {
                    this.helper.Cancel();
                }
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
        //    if (this.success)
            {
                Assembly assembly = Assembly.GetEntryAssembly();
                Process.Start(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), "SoonLearning.AppCenter.exe"));
            }

            this.Dispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
        }

        private void Start()
        {
            this.DownloadPackageFile();
        }

        /// <summary>
        /// Step 1
        /// </summary>
        private void DownloadPackageFile()
        {
            if (string.IsNullOrEmpty(App.packFile))
            {
                this.infoTextBlock.Text = "升级失败，未找到升级包。";
                this.cancelBtn.Visibility = System.Windows.Visibility.Hidden;
                this.closeBtn.Visibility = System.Windows.Visibility.Visible;
                this.infoTextBlock.Foreground = Brushes.Red;
                return;
            }

            this.downloading = true;

            this.infoTextBlock.Text = "正在下载更新包... ...";
            helper = new DownloadHelper();
            helper.DownloadProgressChanged += ((sender, e) =>
            {
                this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                    {
                        this.infoProgressBar.Value = e.BytesReceived * 100 / e.TotalBytesToReceive;
                    }), 
                    DispatcherPriority.Background,
                    null);
            });
            helper.DownloadFileCompleted += ((sender, e) =>
            {
                this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    if (e.Cancelled)
                    {
                        this.infoProgressBar.Value = 0;
                        this.infoTextBlock.Text = "升级过程被取消。";
                        this.cancelBtn.Visibility = System.Windows.Visibility.Hidden;
                        this.closeBtn.Visibility = System.Windows.Visibility.Visible;
                        this.infoTextBlock.Foreground = Brushes.Red;
                    }
                    else if (e.Error != null)
                    {
                        this.infoProgressBar.Value = 0;
                        this.infoTextBlock.Text = "下载更新包失败，请稍后重试... ...";
                        this.cancelBtn.Visibility = System.Windows.Visibility.Hidden;
                        this.closeBtn.Visibility = System.Windows.Visibility.Visible;
                        this.infoTextBlock.Foreground = Brushes.Red;
                    }
                    else
                    {
                        this.Install();
                    }

                    this.helper.Dispose();
                    this.helper = null;
                }),
                DispatcherPriority.Background,
                null);
            });

            Assembly assembly = Assembly.GetEntryAssembly();
            string localFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), "Temp");
            if (!System.IO.Directory.Exists(localFile))
                System.IO.Directory.CreateDirectory(localFile);
            this.localPackFile = System.IO.Path.Combine(localFile, System.IO.Path.GetFileName(App.packFile));
            helper.DownloadFile(App.packFile, this.localPackFile);
        }

        /// <summary>
        /// Step 2
        /// </summary>
        private void Install()
        {
            this.cancelBtn.IsEnabled = false;
            if (!System.IO.File.Exists(this.localPackFile))
            {
                this.infoProgressBar.Value = 0;
                this.infoTextBlock.Text = "安装更新失败，未找到更新包。";
                this.cancelBtn.Visibility = System.Windows.Visibility.Hidden;
                this.closeBtn.Visibility = System.Windows.Visibility.Visible;
                this.infoTextBlock.Foreground = Brushes.Red;
                return;
            }

            this.Unpack();
        }

        /// <summary>
        /// Step 3
        /// End upgrade and launch AppCenter
        /// </summary>
        private void EndUpgrade()
        {
            this.success = true;
            this.cancelBtn.Visibility = System.Windows.Visibility.Hidden;
            this.closeBtn.Visibility = System.Windows.Visibility.Visible;
            this.infoTextBlock.Text = "升级成功完成。";
            this.infoTextBlock.Foreground = Brushes.Green;

            Clear();
        }

        private void Clear()
        {
            try
            {
                System.IO.File.Delete(this.localPackFile);
            }
            catch
            {
            }

            try
            {
                string packPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.localPackFile), "UpgradePack");
                System.IO.Directory.Delete(packPath, true);
            }
            catch
            {
            }
        }

        private void Unpack()
        {
            this.infoProgressBar.Value = 0;
            this.infoTextBlock.Text = "正在从更新包中提取文件... ...";

            string folder = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (this.Is64BitOS())
                SevenZipExtractor.SetLibraryPath(System.IO.Path.Combine(folder, "ThirdParty\\7z\\7z64.dll"));
            else
                SevenZipExtractor.SetLibraryPath(System.IO.Path.Combine(folder, "ThirdParty\\7z\\7z.dll"));

            SevenZipExtractor extractor = null;
            try
            {
                string packPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.localPackFile), "UpgradePack");

                extractor = new SevenZipExtractor(this.localPackFile, "1@3$5^7*");
                extractor.FileExtractionFinished += ((sender, e) =>
                    {
                        this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                        {
                            this.infoProgressBar.Value = e.PercentDone;
                        }), DispatcherPriority.Background, null);
                    });
                extractor.ExtractionFinished += ((sender, e) =>
                    {
                        this.Dispatcher.Invoke(new ThreadStart(() => 
                            {
                                this.CopyFiles(packPath);
                            }), DispatcherPriority.Send, null);
                    });
                
                extractor.ExtractArchive(packPath);
            }
            catch (Exception ex)
            {
                this.infoProgressBar.Value = 0;
                this.infoTextBlock.Text = "升级失败，未能从升级包中提取文件，升级包格式错误。";
                this.cancelBtn.Visibility = System.Windows.Visibility.Hidden;
                this.closeBtn.Visibility = System.Windows.Visibility.Visible;
                this.infoTextBlock.Foreground = Brushes.Red;
            }
        }

        private void CopyFiles(string unpackPath)
        {
            this.infoProgressBar.Value = 0;
            
            // 1, Check config file
            string configFile = System.IO.Path.Combine(unpackPath, "UpgradeConfig.xml");
            if (!System.IO.File.Exists(configFile))
            {
                this.infoTextBlock.Text = "升级失败，未能找到升级配置文件。";
                this.cancelBtn.Visibility = System.Windows.Visibility.Hidden;
                this.closeBtn.Visibility = System.Windows.Visibility.Visible;
                this.infoTextBlock.Foreground = Brushes.Red;
                this.Clear();
                return;
            }

            UpgradeItem[] items;
            try
            {
                items = SerializerHelper<UpgradeItem[]>.XmlDeserialize(configFile);
            }
            catch (Exception ex)
            {
                this.infoTextBlock.Text = "升级失败，解析升级配置文件出错。";
                this.cancelBtn.Visibility = System.Windows.Visibility.Hidden;
                this.closeBtn.Visibility = System.Windows.Visibility.Visible;
                this.infoTextBlock.Foreground = Brushes.Red;
                this.Clear();
                return;
            }

            // 2. Copy Files
            string targetPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string backupPath = System.IO.Path.Combine(targetPath, "Backup\\" + Guid.NewGuid().ToString("N"));
            List<UpgradeItem> doneList = new List<UpgradeItem>();
            try
            {
                foreach (UpgradeItem item in items)
                {
                    string targetFile = System.IO.Path.Combine(targetPath, item.PathName);
                    // 2.1 Backup old file
                    this.CopyFile(targetFile, System.IO.Path.Combine(backupPath, item.PathName));

                    // 2.2 Copy new file to target folder
                    this.CopyFile(System.IO.Path.Combine(unpackPath, item.PathName), targetFile);

                    doneList.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                // Revert.
                foreach (UpgradeItem item in doneList)
                {
                    this.CopyFile(System.IO.Path.Combine(targetPath, item.PathName), System.IO.Path.Combine(backupPath, item.PathName));
                }

                this.infoTextBlock.Text = "升级失败，更新文件出错。";
                this.cancelBtn.Visibility = System.Windows.Visibility.Hidden;
                this.closeBtn.Visibility = System.Windows.Visibility.Visible;
                this.infoTextBlock.Foreground = Brushes.Red;

                this.Clear();

                try
                {
                    System.IO.Directory.Delete(backupPath, true);
                }
                catch
                {
                }

                return;
            }

            try
            {
                System.IO.Directory.Delete(backupPath, true);
            }
            catch
            {
            }
            
            this.EndUpgrade();
        }

        private void CopyFile(string srcFile, string destFile)
        {
            string targetPath = System.IO.Path.GetDirectoryName(destFile);
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            if (!System.IO.File.Exists(srcFile))
                return;

            System.IO.File.Copy(srcFile, destFile, true);
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
    }
}
