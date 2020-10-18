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
using System.Collections.ObjectModel;
using System.Net;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using System.Security.Cryptography;

namespace SoonLearning.AppManagementTool
{
    /// <summary>
    /// Interaction logic for UploadAppWindow.xaml
    /// </summary>
    public partial class UploadAppWindow : Window
    {
        private GadgetItemOnline appItem;
        private int snapshotUploadingIndex = 0;
        private string appUrl = string.Empty;
        private string iconUrl = string.Empty;

        private WebClient client;

        public UploadAppWindow(string slpFile)
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(slpFile))
            {
                this.appFileTextBox.Text = slpFile;
            }
        }

        private void browseAppFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "速学应用安装包|*.slp";
            if (openFileDialog.ShowDialog().Value)
            {
                this.appFileTextBox.Text = openFileDialog.FileName;
            }
        }

        private void addThumbnailButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog().Value)
            {
                foreach (string file in openFileDialog.FileNames)
                {
                    if (this.thumbnailListBox.Items.Count >= 5)
                    {
                        MessageBox.Show("应用缩略图最多只能有5张。", "上传应用", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                    }

                    bool exist = false;
                    foreach (ThumbnailInfo info in this.thumbnailListBox.Items)
                    {
                        if (file.Equals(info.File, StringComparison.OrdinalIgnoreCase))
                        {
                            exist = true;
                            break;
                        }
                    }

                    if (exist)
                        continue;

                    this.thumbnailListBox.Items.Add(new ThumbnailInfo(file));
                }
            }
        }

        private void removeThumbnailButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.thumbnailListBox.SelectedIndex < 0)
                return;

            this.thumbnailListBox.Items.RemoveAt(this.thumbnailListBox.SelectedIndex);
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.appFileTextBox.Text))
            {
                MessageBox.Show("请选择速学应用安装包文件。", "上传应用", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            this.enableCtrls(false);

            if (!ExtractPackFile())
            {
                MessageBox.Show("安装包格式不正确", "上传应用", MessageBoxButton.OK, MessageBoxImage.Error);
                this.enableCtrls(true);
                return;
            }

            string userId = this.userIdTextBox.Text;
            string password = this.passwordTextBox.Password;
            if (string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("请输入用户ID", "上传应用", MessageBoxButton.OK, MessageBoxImage.Error);
                this.enableCtrls(true);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入用户密码", "上传应用", MessageBoxButton.OK, MessageBoxImage.Error);
                this.enableCtrls(true);
                return;
            }

            try
            {
                AppServerSoapClient service = new AppServerSoapClient();
                service.Endpoint.Address = new System.ServiceModel.EndpointAddress(@"http://www.soonlearning.com/WebServers/AppServer.asmx");
                int ret = service.CheckApp(userId, Helper.GetMD5Hash(password), appItem.Id, Helper.GetMD5Hash("$df9!d^_"));
                if (ret == 1)
                {
                    string message = "该应用已经上传到服务器上，你确定要更新该应用吗？";
                    if (MessageBox.Show(message, "记忆应用", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        this.enableCtrls(false);
                        return;
                    }
                }
                else if (ret == 2)
                {
                    string message = "该应用已经由其他用户上传到速学应用平台，你不能修改并上传该应用！";
                    MessageBox.Show(message, "记忆应用", MessageBoxButton.OK, MessageBoxImage.Information);
                    {
                        this.enableCtrls(false);
                        return;
                    }
                }
                else if (ret == 0)
                {
                }
                else
                {
                    string message = "检查应用时发生了未知错误！";
                    MessageBox.Show(message, "记忆应用", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.enableCtrls(false);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "记忆工具", MessageBoxButton.OK, MessageBoxImage.Error);
                this.enableCtrls(true);
                return;
            }

            this.uploadStatusTextBox.Text = "正在上传应用安装包...";
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
            client = new WebClient();
            client.Headers.Add("AddApp", "{A298CC02-CE4B-4B69-A1B5-94FF344C6B95}");
            client.UploadProgressChanged += ((s, e1) =>
                {
                    this.uploadProgressBar.Value = e1.BytesSent * 100 / e1.TotalBytesToSend;
                });
            client.UploadFileCompleted += ((s, e1) =>
                {
                    if (e1.Error != null)
                    {
                        this.uploadProgressBar.Value = 0;
                        this.uploadStatusTextBox.Text = "上传应用失败：" + e1.Error.Message;
                        this.enableCtrls(true);
                    }
                    else if (e1.Cancelled)
                    {
                        this.uploadProgressBar.Value = 0;
                        this.uploadStatusTextBox.Text = "上传应用过程被取消";
                        this.enableCtrls(true);
                    }
                    else
                    {
                        this.uploadProgressBar.Value = 100;

                      //  if (this.thumbnailListBox.Items.Count > 0)
                        {
                            this.snapshotUploadingIndex = 0;
                            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                                {
                                    if (this.thumbnailListBox.Items.Count == 0)
                                    {
                                        this.uploadIcon();
                                    }
                                    else
                                    {
                                        this.uploadSnapshot(this.thumbnailListBox.Items[this.snapshotUploadingIndex] as ThumbnailInfo);
                                    }
                                }), DispatcherPriority.Background, null);
                        }
                    }
                });
            this.appUrl = this.CreateAppUri("Apps/Packages", this.appFileTextBox.Text);
            client.UploadFileAsync(new Uri(appUrl), "POST", this.appFileTextBox.Text);
        }

        private void uploadSnapshot(ThumbnailInfo snapshotFile)
        {
            if (this.thumbnailListBox.Items.Count == 0)
            {
                this.uploadIcon();
                return;
            }

            this.uploadStatusTextBox.Text = "正在上传应用缩略图...";
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
            client = new WebClient();
            client.Headers.Add("AddApp", "{A298CC02-CE4B-4B69-A1B5-94FF344C6B95}");
            client.UploadProgressChanged += ((s, e1) =>
            {
                this.uploadProgressBar.Value = e1.BytesSent * 100 / e1.TotalBytesToSend;
            });
            client.UploadFileCompleted += ((s, e1) =>
            {
                if (e1.Error != null)
                {
                    this.uploadProgressBar.Value = 0;
                    this.uploadStatusTextBox.Text = "上传应用失败：" + e1.Error.Message;
                    this.enableCtrls(true);
                }
                else if (e1.Cancelled)
                {
                    this.uploadProgressBar.Value = 0;
                    this.uploadStatusTextBox.Text = "上传应用过程被取消";
                    this.enableCtrls(true);
                }
                else
                {
                    this.uploadProgressBar.Value = 100;

                    this.snapshotUploadingIndex++;
                    this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                    {
                        if (this.snapshotUploadingIndex < this.thumbnailListBox.Items.Count)
                            this.uploadSnapshot(this.thumbnailListBox.Items[this.snapshotUploadingIndex] as ThumbnailInfo);
                        else
                            this.uploadIcon();
                    }), DispatcherPriority.Background, null);
                }
            });
            snapshotFile.RemoteUrl = this.CreateAppUri("Apps/snapshots", snapshotFile.File);
            client.UploadFileAsync(new Uri(snapshotFile.RemoteUrl), snapshotFile.File);
        }

        private void uploadIcon()
        {
            if (!File.Exists(this.appItem.Thumbnail))
            {
                this.addApp();
                return;
            }

            this.uploadStatusTextBox.Text = "正在上传应用图标...";
            if (client != null)
            {
                client.Dispose();
                client = null;
            }
            client = new WebClient();
            client.Headers.Add("AddApp", "{A298CC02-CE4B-4B69-A1B5-94FF344C6B95}");
            client.UploadProgressChanged += ((s, e1) =>
            {
                this.uploadProgressBar.Value = e1.BytesSent * 100 / e1.TotalBytesToSend;
            });
            client.UploadFileCompleted += ((s, e1) =>
            {
                if (e1.Error != null)
                {
                    this.uploadProgressBar.Value = 0;
                    this.uploadStatusTextBox.Text = "上传应用失败：" + e1.Error.Message;
                    this.enableCtrls(true);
                }
                else if (e1.Cancelled)
                {
                    this.uploadProgressBar.Value = 0;
                    this.uploadStatusTextBox.Text = "上传应用过程被取消";
                    this.enableCtrls(true);
                }
                else
                {
                    this.uploadProgressBar.Value = 100;
                    this.Dispatcher.BeginInvoke(new ThreadStart(() => { this.addApp(); }), DispatcherPriority.Background, null);
                }
            });
            this.iconUrl = this.CreateAppUri("Apps/Icons", this.appItem.Thumbnail);
            client.UploadFileAsync(new Uri(iconUrl), this.appItem.Thumbnail);
        }

        private void addApp()
        {
            if (client != null)
            {
                client.Dispose();
                client = null;
            }

            double price = 0.0f;
            if (!double.TryParse(this.priceTextBox.Text, out price))
            {
                
            }

            string userId = this.userIdTextBox.Text;
            string password = this.passwordTextBox.Password;
            if (string.IsNullOrEmpty(userId))
            {
                MessageBox.Show("请输入用户ID", "上传应用", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入用户密码", "上传应用", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            APKSoftModel module = new APKSoftModel();
            module.AddDate = DateTime.Now.ToUniversalTime();
            module.Attach = new APKAttachModel();
            module.Attach.Image1 = this.getSnapshot(0);
            module.Attach.Image2 = this.getSnapshot(1);
            module.Attach.Image3 = this.getSnapshot(2);
            module.Attach.Image4 = this.getSnapshot(3);
            module.Attach.Image5 = this.getSnapshot(4);
            module.ClassID = this.appItem.AppType;
            module.SubClassID = this.appItem.AppSubType;
            module.TopicNum = 0;
            module.UniqueId = this.appItem.Id;
            module.UserID = 0; // TODO
            module.Version = this.versionTextBox.Text;
            module.Detail = this.descriptionTextBox.Text;
            module.DownNum = 0;

            FileStream fs = File.OpenRead(this.appFileTextBox.Text);
            module.Filesize = new decimal(fs.Length);
            fs.Close();

            module.FileUrl = this.appUrl;
            module.ICON = this.iconUrl;
            module.IsIndex = 0; // TODO;
            module.IsNew = 1;
            module.IsUse = 0;
            module.Md5Check = GetMD5HashFromFile(this.appFileTextBox.Text); // TODO
            module.Name = this.appItem.Title;
            module.Price = new decimal(price);
            module.Sketch = this.appItem.Description;
            module.SubClassID = this.appItem.AppSubType;

            try
            {
                AppServerSoapClient service = new AppServerSoapClient();
                service.Endpoint.Address = new System.ServiceModel.EndpointAddress(@"http://www.soonlearning.com/WebServers/AppServer.asmx");
                service.AddApp(module, userId, Helper.GetMD5Hash(password), Helper.GetMD5Hash("$df9!d^_"));
                foreach (TypeItem item in this.categoryListBox.Items)
                {
                    module.ClassID = item.ParentType;
                    module.SubClassID = item.Type;
                    service.AddApp(module, userId, Helper.GetMD5Hash(password), Helper.GetMD5Hash("$df9!d^_"));
                }
                this.uploadStatusTextBox.Text = "应用上传成功.";
                MessageBox.Show("应用上传成功", "应用上传", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                this.uploadStatusTextBox.Text = string.Format("应用上传失败:{0}", ex.Message);
                MessageBox.Show(this.uploadStatusTextBox.Text, "应用上传", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.enableCtrls(true);
        }

        private string getSnapshot(int index)
        {
            if (index >= this.thumbnailListBox.Items.Count)
                return string.Empty;

            ThumbnailInfo info = this.thumbnailListBox.Items[index] as ThumbnailInfo;
            return info.RemoteUrl;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.client != null)
            {
                this.client.CancelAsync();
                this.client = null;
            }
        }

        private string CreateAppUri(string type, string file)
        {
            string year = DateTime.Now.Year.ToString();
            string months = DateTime.Now.Month.ToString();
            string days = DateTime.Now.Day.ToString();

            string fileTpye = type + string.Format(@"/{0}/{1}", this.appItem.Id, System.IO.Path.GetFileName(file));

            return @"http://www.soonlearning.com/" + fileTpye;
        }

        private bool ExtractPackFile()
        {
            try
            {
                double price = 0.0f;
                if (!double.TryParse(this.priceTextBox.Text, out price))
                {
                    MessageBox.Show("请为应用输入正确的价格。", "上传应用", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                string userId = this.userIdTextBox.Text;
                string password = this.passwordTextBox.Password;
                if (string.IsNullOrEmpty(userId))
                {
                    MessageBox.Show("请输入注册用的邮箱或者登陆ID。", "上传应用", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("请输入密码。", "上传应用", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }


                FileStream fs = File.OpenRead(this.appFileTextBox.Text);

                // Check File Header
                string fileHeader = this.ReadString(fs);
                if (fileHeader != "{B5F6844E-984C-4129-8D19-79FDEFBDD5DC}" &&
                    fileHeader != "{BE4A1507-5B37-42EA-9E08-43EF4F363C42}")
                {
                    MessageBox.Show("安装包格式不正确。", "上传应用", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                // Check AppTitle and AppId
                string appId = this.ReadString(fs);
                string appTitle = this.ReadString(fs);

                this.appItem = new GadgetItemOnline();

                appItem.Id = appId;
                appItem.Title = appTitle;
                // Description
                appItem.Description = this.ReadString(fs);

                // Logos
                appItem.Thumbnail = this.ExtractLogo(fs, appId);

                // Entry File;
                string temp = this.ReadString(fs);
                temp = this.ReadString(fs);

                appItem.AppType = BitConverter.ToInt32(this.ReadBytes(fs), 0);
                appItem.AppSubType = BitConverter.ToInt32(this.ReadBytes(fs), 0);
                appItem.CreateDate = DateTime.Parse(this.ReadString(fs));

                appItem.Creator = this.ReadString(fs);
                appItem.CreatorLogo = this.ReadString(fs);
                appItem.CreatorWebSite = this.ReadString(fs);

                fs.Close();
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
                StringBuilder strBuilder = new StringBuilder("安装包格式错误.");
                strBuilder.AppendLine(ex.Message);
                MessageBox.Show(strBuilder.ToString(), "上传应用", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private string ReadString(Stream stream)
        {
            byte[] textDataLength = new byte[4];
            stream.Read(textDataLength, 0, 4);

            byte[] textData = new byte[BitConverter.ToInt32(textDataLength, 0)];
            stream.Read(textData, 0, textData.Length);
            return Encoding.UTF8.GetString(textData);
        }

        private byte[] ReadBytes(Stream stream)
        {
            byte[] dataLength = new byte[4];
            stream.Read(dataLength, 0, 4);

            byte[] data = new byte[BitConverter.ToInt32(dataLength, 0)];
            stream.Read(data, 0, data.Length);
            return data;
        }

        private string ExtractLogo(Stream stream, string id)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string logoExt = this.ReadString(stream);

            string thumbnail = System.IO.Path.GetDirectoryName(assembly.Location);
            thumbnail = System.IO.Path.Combine(thumbnail, @"AppLogos\");
            if (!Directory.Exists(thumbnail))
                Directory.CreateDirectory(thumbnail);
            thumbnail += id;
            thumbnail += logoExt;

            if (!string.IsNullOrEmpty(logoExt))
            {
                try
                {
                    File.WriteAllBytes(thumbnail, this.ReadBytes(stream));
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                this.ReadBytes(stream);
                return string.Empty;
            }

            return thumbnail;
        }

        private void enableCtrls(bool enable)
        {
            for (int i = 0; i < this.rootGrid.RowDefinitions.Count-1; i++)
            {
                RowDefinition row = this.rootGrid.RowDefinitions[i];
                row.IsEnabled = enable;
            }

            this.startButton.IsEnabled = enable;
            this.cancelButton.IsEnabled = !enable;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.client != null)
            {
                this.client.CancelAsync();
                this.client.Dispose();
                this.client = null;
            }
        }

        private string GetMD5HashFromFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }
            return sb.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void GetMainTypes(string[] typeList)
        {
            foreach (string value in typeList)
            {
                string[] temp = value.Split(new char[] { '$' });
                int parentId = Convert.ToInt32(temp[1]);
                if (parentId == TypeItem.Root)
                {
                    TypeItem typeItem = new TypeItem(temp[0] as string,
                        string.Empty,
                        string.Empty,
                        Convert.ToInt32(temp[2]),
                        TypeItem.Root);

                    TypeCollection.Instance.Add(typeItem);
                }
            }

            foreach (string value in typeList)
            {
                string[] temp = value.Split(new char[] { '$' });
                int parentId = Convert.ToInt32(temp[1]);
                if (parentId == 0)
                    continue;

                foreach (TypeItem item in TypeCollection.Instance)
                {
                    if (parentId == item.Type)
                    {
                        item.SubTypeItems.Add(new TypeItem(temp[0] as string,
                            string.Empty,
                            string.Empty,
                            Convert.ToInt32(temp[2]),
                            parentId));
                    }
                }
            }

            if (TypeCollection.Instance.Count > 0)
                this.categoryComboBox.SelectedIndex = 0;
        }

        private void removeFromList_Click(object sender, RoutedEventArgs e)
        {
            if (this.categoryListBox.SelectedIndex < 0)
                return;

            this.categoryListBox.Items.Remove(this.categoryListBox.SelectedIndex);
        }

        private void addToList_Click(object sender, RoutedEventArgs e)
        {
            this.categoryListBox.Items.Add(this.subCategoryComboBox.SelectedItem);
        }

        private void getCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor cursor = this.Cursor;
            try
            {
                this.Cursor = Cursors.Wait;
                AppServerSoapClient service = new AppServerSoapClient();
                service.Endpoint.Address = new System.ServiceModel.EndpointAddress(@"http://www.soonlearning.com/WebServers/AppServer.asmx");
                string[] result = service.GetAppClass(Helper.GetMD5Hash("$af3#d_&"));
                this.GetMainTypes(result);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "上传应用", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                this.Cursor = cursor;
            }
        }

        private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.subCategoryComboBox.Items.Count > 0)
                this.subCategoryComboBox.SelectedIndex = 0;
        }
    }

    public class ThumbnailInfo
    {
        public string File
        {
            get;
            set;
        }

        public string Title
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(this.File); }
        }

        public string RemoteUrl
        {
            get;
            set;
        }

        public ThumbnailInfo(string file)
        {
            this.File = file;
        }
    }
}
