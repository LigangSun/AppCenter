using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Windows.Media.Imaging;

namespace AppManagementTool
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            List<GadgetItemOnline> appList = new List<GadgetItemOnline>();
            foreach (GadgetItemOnline item in this.appListBox.Items)
            {
                appList.Add(item);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            string appConfigFile = Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), "AppConfig\\AppConfig.xml");
            if (!Directory.Exists(Path.GetDirectoryName(appConfigFile)))
                Directory.CreateDirectory(Path.GetDirectoryName(appConfigFile));
            SerializerHelper<List<GadgetItemOnline>>.XmlSerialize(appConfigFile, appList);
        }

        private void loadConfigFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = "*.dll|*.dll";
            if (openFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.LoadApp(openFileDlg.FileName);
            }
        }

        private void loadAppsButton_Click(object sender, EventArgs e)
        {
            this.appListBox.Items.Clear();
            this.FindGadgets();
        }

        private void FindGadgets()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetDirectoryName(assembly.Location));

            this.FindApp(di.FullName);
        }

        private void FindApp(string folder)
        {
            DirectoryInfo di = new DirectoryInfo(folder);
            foreach (FileInfo fi in di.GetFiles("*.dll"))
            {
                this.LoadApp(fi.FullName);
            }

            foreach (DirectoryInfo subDi in di.GetDirectories())
            {
                this.FindApp(subDi.FullName);
            }
        }

        private void LoadApp(string fileName)
        {
            try
            {
                Assembly gadgetAssembly = Assembly.LoadFile(fileName);
                Module[] modules = gadgetAssembly.GetModules();
                foreach (Module module in modules)
                {
                    Type[] types = module.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsClass && !type.IsAbstract)
                        {
                            Type entryInterface = type.GetInterface("IGadgetEntry");
                            Type authorInterface = type.GetInterface("IAuthorInfo");
                            if (entryInterface != null)
                            {
                                object instance = gadgetAssembly.CreateInstance(type.FullName);
                                GadgetItemOnline item = new GadgetItemOnline(fileName);
                                PropertyInfo piId = entryInterface.GetProperty("Id");
                                PropertyInfo piTitle = entryInterface.GetProperty("Title");
                                PropertyInfo piDescription = entryInterface.GetProperty("Description");
                                PropertyInfo piCreateDate = entryInterface.GetProperty("CreateDate");
                                PropertyInfo piAppType = entryInterface.GetProperty("Tag");
                                PropertyInfo piAppSubType = entryInterface.GetProperty("SubTag");
                                PropertyInfo piThumbnail = entryInterface.GetProperty("Thumbnail");

                                object id = piId.GetValue(instance, null);
                                object title = piTitle.GetValue(instance, null);
                                item.Id = (string)id;
                                item.Title = (string)title;
                                item.Description = (string)piDescription.GetValue(instance, null);
                                item.CreateDate = (DateTime)piCreateDate.GetValue(instance, null);
                                item.AppType = (GadgetType)piAppType.GetValue(instance, null);
                                item.AppSubType = (GadgetSubType)piAppSubType.GetValue(instance, null);
                                item.Thumbnail = @"http://www.soonlearning.com/AppThumbnails/" + item.Id + ".png";
                                item.PackageUrl = @"http://www.soonlearning.com/AppPackages/" + item.Id + ".zip";
                                item.Version = gadgetAssembly.GetName().Version.ToString();

                                if (authorInterface != null)
                                {
                                    PropertyInfo piCreator = authorInterface.GetProperty("Name");
                                    PropertyInfo piWebSite = authorInterface.GetProperty("WebSite");

                                    item.Creator = (string)piCreator.GetValue(instance, null);
                                    item.CreatorWebSite = (string)piWebSite.GetValue(instance, null);
                                    item.CreatorLogo = @"http://www.soonlearning.com/AppPackages/" + item.Creator + ".png";
                                }

                                int index = this.appListBox.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            GadgetItemOnline item = this.appListBox.SelectedItem as GadgetItemOnline;
            if (item == null)
                return;

            CreatePackageForm form = new CreatePackageForm(item.Title, item.Id, item.DllFile);
            form.ShowDialog();
        }
    }
}
