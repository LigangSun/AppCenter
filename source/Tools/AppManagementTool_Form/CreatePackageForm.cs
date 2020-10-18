using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace AppManagementTool
{
    public partial class CreatePackageForm : Form
    {
        private string appId;

        public CreatePackageForm(string title, string id, string file)
        {
            InitializeComponent();

            this.titleLabel.Text = title;
            this.appId = id;
            this.mainDllTextBox.Text = Path.GetDirectoryName(file);
            this.BrowseFolder(this.mainDllTextBox.Text);
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDlg = new FolderBrowserDialog();
            if (folderBrowserDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.mainDllTextBox.Text = folderBrowserDlg.SelectedPath;
                this.BrowseFolder(folderBrowserDlg.SelectedPath);
            }
        }

        private void BrowseFolder(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            foreach (DirectoryInfo subDi in di.GetDirectories())
            {
                this.BrowseFolder(subDi.FullName);
            }

            foreach (FileInfo fi in di.GetFiles())
            {
                this.fileListBox.Items.Add(fi.FullName);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetDirectoryName(assembly.Location));

            string packFile = Path.Combine(di.FullName, "AppPackages");
            if (!Directory.Exists(packFile))
                Directory.CreateDirectory(packFile);

            packFile = Path.Combine(packFile, this.appId + ".zip");
            {
                FileStream fs = File.OpenWrite(packFile);

                // Header
                this.WriteString(fs, "{B5F6844E-984C-4129-8D19-79FDEFBDD5DC}");

                // Title
                this.WriteString(fs, this.titleLabel.Text);
                // AppId
                this.WriteString(fs, this.appId);

                this.WriteString(fs, this.fileListBox.Items.Count.ToString());
                foreach (string file in this.fileListBox.Items)
                {
                    string fileName = file.Remove(0, this.mainDllTextBox.Text.Length);
                    this.WriteString(fs, fileName);

                    this.WriteBytes(fs, File.ReadAllBytes(file));
                }

                fs.Close();
            }
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
    }
}
