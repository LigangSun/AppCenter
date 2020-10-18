using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Collections;
using System.Diagnostics;
using SoonLearning.AppCenter.Interfaces;
using SoonLearning.Memorize.UI;
using SoonLearning.Assessment.Player.Entry;

namespace SoonLearning.AppCenter.Data
{
    public class AppItemCollection : ObservableCollection<AppItem>
    {
        private XmlDocument document = new XmlDocument();
        private List<AppItem> uninstallAppList = new List<AppItem>();
        private string tempConfigFile;

        private string TempConfigFile
        {
            get
            {
                if (string.IsNullOrEmpty(this.tempConfigFile))
                {
                    this.tempConfigFile = Path.Combine(DataMgr.Instance.BaseFolder, @"TempConfig\AppConfig.xml");
                    string tempFolder = Path.GetDirectoryName(this.tempConfigFile);
                    if (!Directory.Exists(tempFolder))
                        Directory.CreateDirectory(tempFolder);
                }

                return this.tempConfigFile;
            }
        }

        public AppItemCollection()
        {
            
        }

        internal void Load()
        {
            string configFile = Path.Combine(DataMgr.Instance.BaseFolder, "AppsConfig.xml");

            XmlReader reader = XmlReader.Create(configFile);

            this.uninstallAppList.Clear();

            List<XmlNode> uninstallNodeList = new List<XmlNode>();
            
            this.document.Load(reader);
            XmlNode rootElement = this.document.SelectSingleNode("AppItems");
            XmlNodeList appNodeList = rootElement.SelectNodes(typeof(AppItem).Name);
            foreach (XmlNode node in appNodeList)
            {
                int state = Convert.ToInt32(GetAttributeValue(node, "State"));

                AppItem item = new AppItem(GetAttributeValue(node, "Id") as string,
                    GetAttributeValue(node, "Title") as string,
                    GetAttributeValue(node, "Description") as string,
                    DateTime.Parse(GetAttributeValue(node, "CreateDate") as string),
                    GetAttributeValue(node, "Thumbnail") as string,
                    Convert.ToInt32(GetAttributeValue(node, "Type")),
                    Convert.ToInt32(GetAttributeValue(node, "SubType")),
                    GetAttributeValue(node, "FullName") as string,
                    GetAttributeValue(node, "CreatorName") as string,
                    GetAttributeValue(node, "CreatorWebSite") as string,
                    GetAttributeValue(node, "CreatorLogo") as string);

                item.AddedTime = DateTime.Parse(GetAttributeValue(node, "AddedTime") as string);
                item.LastUsedTime = DateTime.Parse(GetAttributeValue(node, "LastUsedTime") as string);
                item.UsedCount = Convert.ToInt32(GetAttributeValue(node, "UsedCount"));
                item.State = state;
                item.AppEntryFile = GetAttributeValue(node, "AppEntryFile") as string;

                if (node.ChildNodes.Count > 0)
                {
                    XmlElement refFileElement = node.ChildNodes[0] as XmlElement;
                    foreach (XmlNode fileNode in refFileElement.ChildNodes)
                    {
                        if (fileNode.Name == "Item")
                            item.RelatedFiles.Add(GetAttributeValue(fileNode, "File") as string);
                    }
                }

                if (item.State == 1)
                {
                    this.uninstallAppList.Add(item);
                    uninstallNodeList.Add(node);
                }
            }

            foreach (XmlNode node in uninstallNodeList)
                rootElement.RemoveChild(node);
                        
            reader.Close();

            this.ClearUninstalledApps();

            this.Save();
        }

        internal void ClearUninstalledApps()
        {
            foreach (AppItem item in this.uninstallAppList)
            {
                if (!IsFileInUse(item.AppEntryFile))
                    this.DeleteFile(item.AppEntryFile);

                foreach (string file in item.RelatedFiles)
                {
                    bool fileInUsed = IsFileInUse(file);
                    if (!fileInUsed)
                    {
                        this.DeleteFile(file);
                    }
                }
            }

            this.uninstallAppList.Clear();
        }

        private bool IsFileInUse(string file)
        {
            foreach (AppItem temp in this)
            {
                if (file.Equals(temp.AppEntryFile, StringComparison.InvariantCultureIgnoreCase))
                    return true;

                foreach (string tempFile in temp.RelatedFiles)
                {
                    if (file.Equals(tempFile, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        private void DeleteFile(string file)
        {
            try
            {
                File.Delete(Path.Combine(DataMgr.Instance.BaseFolder, file));
            }
            catch
            {
            }
        }

        internal void Save()
        {
            try
            {
                this.document.Save(this.TempConfigFile);
                string configFile = Path.Combine(DataMgr.Instance.BaseFolder, "AppsConfig.xml");
                File.Copy(this.TempConfigFile, configFile, true);
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }
        }

        internal void AddApp(AppItem item)
        {
            try
            {
                if (this.document == null)
                    this.document = new XmlDocument();

                if (this.document.ChildNodes.Count == 0)
                {
                    XmlElement el = this.document.CreateElement("AppItems");
                    this.document.AppendChild(el);
                }

                XmlNode rootElement = this.document.SelectSingleNode("AppItems");

                foreach (XmlNode node in rootElement.ChildNodes)
                {
                    string id = this.GetAttributeValue(node, "Id") as string;
                    if (item.Id == id)
                    {
                        rootElement.RemoveChild(node);
                        break;
                    }
                }

                bool exist = false;
                foreach (AppItem temp in this)
                {
                    if (temp.Id == item.Id)
                    {
                        exist = true;
                        break;
                    }
                }

                if (!exist && item.State != 1)
                    base.Add(item);

                XmlElement element = this.document.CreateElement("AppItem");
                rootElement.AppendChild(element);
                Type itemType = item.GetType();
                PropertyInfo[] pis = itemType.GetProperties();
                foreach (PropertyInfo pi in pis)
                {
                    object value = pi.GetValue(item, null);
                    if (value is IList)
                    {
                        IList enumerable = value as IList;
                        XmlElement subElement = this.document.CreateElement(pi.Name);
                        element.AppendChild(subElement);
                        foreach (object obj in enumerable)
                        {
                            if (obj != null)
                            {
                                XmlElement itemElement = this.document.CreateElement("Item");
                                itemElement.Attributes.Append(this.CreateAttribute("File", obj));
                                subElement.AppendChild(itemElement);
                            }
                        }
                    }
                    else
                    {
                        element.Attributes.Append(this.CreateAttribute(pi.Name, pi.GetValue(item, null)));
                    }
                }
            }
            finally
            {
                this.Save();
            }
        }

        internal void UninstallApp(AppItem item)
        {
            item.State = 1;

            if (item is MemorizeAppItem)
            {
                MemorizeAppItem memorizeItem = item as MemorizeAppItem;
                MemorizeControl.Instance.Uninstall(memorizeItem.MemorizeEntry, memorizeItem.AppEntryFile);
            }
            else if (item is AssessmentAppItem)
            {
                AssessmentAppItem assessmentItem = item as AssessmentAppItem;
                AssessmentAppControl.Instance.Uninstall(assessmentItem.AppEntryFile);
            }
            else
            {
                this.AddApp(item);
                if (item.Entry == null)
                {
                    try
                    {
                        Assembly gadgetAssembly = Assembly.LoadFile(item.AppEntryFile);
                        item.Entry = gadgetAssembly.CreateInstance(item.FullName) as IGadgetEntry;
                    }
                    catch (Exception ex)
                    {
                        Debug.Assert(false, ex.Message);
                    }
                }

                if (item.Entry != null)
                {
                    try
                    {
                        item.Entry.Uninstall();
                    }
                    catch
                    {
                    }
                }
            }

            this.Save();

            this.Remove(item);

            DataMgr.Instance.MruItems.UninstallApp(item);
        }

        private XmlAttribute CreateAttribute(string name, object value)
        {
            XmlAttribute attribute = this.document.CreateAttribute(name);
            attribute.Value = value.ToString();
            return attribute;
        }

        private object GetAttributeValue(XmlNode node, string name)
        {
            try
            {
                return node.Attributes[name].Value;
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            return null;
        }

        internal bool IsAppExist(string id)
        {
            foreach (AppItem item in this)
            {
                if (item.Id == id)
                    return true;
            }

            return false;
        }
    }
}
