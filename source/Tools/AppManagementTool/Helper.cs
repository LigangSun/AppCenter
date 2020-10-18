using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Resources;
using System.Reflection;
using System.Windows;
using System.Diagnostics;

namespace SoonLearning.AppManagementTool
{
    internal class Helper
    {
        private static string ExtractLogo(string logo, string id, Assembly assembly)
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
                FileStream fs = null;
                try
                {
                    fs = File.OpenWrite(thumbnail);
                    while (true)
                    {
                        byte[] data = new byte[1024];
                        int len = stream.Read(data, 0, 1024);
                        fs.Write(data, 0, len);
                        if (len < 1024)
                            break;
                    }
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                        fs = null;
                    }
                }
            }

            if (stream != null)
                stream.Close();

            return thumbnail;
        }

        internal static GadgetItemOnline LoadApp(string fileName)
        {
            Assembly gadgetAssembly = Assembly.LoadFile(fileName);
            AssemblyName[] refAssembly = gadgetAssembly.GetReferencedAssemblies();
            Module[] modules = gadgetAssembly.GetModules();
            foreach (Module module in modules)
            {
                Type[] types = null;

                try
                {
                    types = module.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    Trace.Assert(false, ex.Message);
                }

                try
                {
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
                                item.LocalThumbnailFile = thumbnailFile;

                                if (authorInterface != null)
                                {
                                    PropertyInfo piCreator = authorInterface.GetProperty("Name");
                                    PropertyInfo piWebSite = authorInterface.GetProperty("WebSite");

                                    item.Creator = (string)piCreator.GetValue(instance, null);
                                    item.CreatorWebSite = (string)piWebSite.GetValue(instance, null);
                                    item.CreatorLogo = @"http://www.soonlearning.com/AppPackages/" + item.Creator + ".png";
                                }

                                return item;
                            }
                        }
                    }
                }
                catch
                {
                }
            }

            return null;
        }

        internal static string GetMD5Hash(string input)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }
    }
}
