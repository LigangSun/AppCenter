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
using System.Reflection;
using System.IO;
using System.Windows.Resources;

namespace SoonLearning.AppManagementTool
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

        private void generateConfigFileButton_Click(object sender, RoutedEventArgs e)
        {
            List<GadgetItemOnline> appList = new List<GadgetItemOnline>();
            foreach (GadgetItemOnline item in this.appListBox.Items)
            {
                appList.Add(item);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            string appConfigFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assembly.Location), "AppConfig\\AppConfig.xml");
            if (!Directory.Exists(System.IO.Path.GetDirectoryName(appConfigFile)))
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(appConfigFile));
            SerializerHelper<List<GadgetItemOnline>>.XmlSerialize(appConfigFile, appList);
        }

        private void generatePackButton_Click(object sender, RoutedEventArgs e)
        {
            GadgetItemOnline item = this.appListBox.SelectedItem as GadgetItemOnline;
            if (item == null)
                return;

            string thumbnail = System.IO.Path.GetDirectoryName(item.DllFile);
            thumbnail = System.IO.Path.Combine(thumbnail, @"AppLogos\");
            if (!Directory.Exists(thumbnail))
                Directory.CreateDirectory(thumbnail);
            thumbnail += item.Id;
            thumbnail += System.IO.Path.GetExtension(item.Thumbnail);

            CreatePackWindow form = new CreatePackWindow();
            form.ShowDialog();
        }

        private void loadAppsButton_Click(object sender, RoutedEventArgs e)
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
                AssemblyName[] refAssembly = gadgetAssembly.GetReferencedAssemblies();
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
                                object thumbnail = piThumbnail.GetValue(instance, null);

                                string thumbnailFile = ExtractLogo(thumbnail as string, id as string, gadgetAssembly);

                                item.Id = (string)id;
                                item.Title = (string)title;
                                item.Entry = type.FullName;
                                item.Description = (string)piDescription.GetValue(instance, null);
                                item.CreateDate = (DateTime)piCreateDate.GetValue(instance, null);
                                item.AppType = Convert.ToInt32(piAppType.GetValue(instance, null));
                                item.AppSubType = Convert.ToInt32(piAppSubType.GetValue(instance, null));
                                item.Thumbnail = @"http://www.soonlearning.com/AppThumbnails/" + System.IO.Path.GetFileName(thumbnailFile);
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

        private string ExtractLogo(string logo, string id, Assembly assembly)
        {
            string thumbnail = System.IO.Path.GetDirectoryName(assembly.Location);
            thumbnail = System.IO.Path.Combine(thumbnail, @"AppLogos\");
            if (!Directory.Exists(thumbnail))
                Directory.CreateDirectory(thumbnail);
            thumbnail += id;
            thumbnail += System.IO.Path.GetExtension(logo);

            int index = logo.LastIndexOf(';');
            string logoFile = System.IO.Path.GetFileNameWithoutExtension(assembly.Location) + logo.Substring(index + ";component".Length, logo.Length - index - ";component".Length);
            logoFile = logoFile.Replace('/', '.');
            // /Gadget.Math.Basic;component/Resources/decimal.png
            string logoName = System.IO.Path.GetFileName(logo);

            StreamResourceInfo sri = Application.GetResourceStream(new Uri(logo));
            Stream stream = sri.Stream;

            if (stream != null)
            {
                FileStream fs = File.OpenWrite(thumbnail);
                while (true)
                {
                    byte[] data = new byte[1024];
                    int len = stream.Read(data, 0, 1024);
                    fs.Write(data, 0, len);
                    if (len < 1024)
                        break;
                }
                fs.Close();
            }

            if (stream != null)
                stream.Close();

            return thumbnail;
        }
    }
}
