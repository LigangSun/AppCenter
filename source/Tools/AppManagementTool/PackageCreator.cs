using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Memorize.Data;
using System.IO;

namespace SoonLearning.AppManagementTool
{
    public class PackageCreator
    {
        public static void CreateMrePackage(string mreFile, string slpFile)
        {
            loadMreItem(mreFile, slpFile);
        }

        public static void CreateTeachAppPackage(string id, 
            string title,
            string description,
            string thumbnailFile,
            int type,
            int subType,
            DateTime createDate,
            string creator,
            string creatorLogo,
            string creatorWebsite,
            string appFile, 
            string slpFile)
        {
            // Create package
            string packFile = slpFile;

            FileStream fs = File.OpenWrite(packFile);

            try
            {
                // Header
                Help.WriteString(fs, "{8D2C2705-B49D-4730-BF39-B0E7E0E09172}");

                // AppId
                Help.WriteString(fs, id);
                // Title
                Help.WriteString(fs, title);
                Help.WriteString(fs, description);

                // Thumbnail ext
                Help.WriteString(fs, System.IO.Path.GetExtension(thumbnailFile));

                byte[] emptyData = new byte[0];
                // Thumbnail data
                if (File.Exists(thumbnailFile))
                    Help.WriteBytes(fs, File.ReadAllBytes(thumbnailFile));
                else
                    Help.WriteBytes(fs, emptyData);

                // Write a empty entry as Memorize App no entry and dll file.
                Help.WriteString(fs, System.IO.Path.GetFileName(appFile));
                Help.WriteString(fs, "TeachAppEntry");

                // Type
                Help.WriteBytes(fs, BitConverter.GetBytes(type));
                Help.WriteBytes(fs, BitConverter.GetBytes(subType));

                // Create Time
                Help.WriteString(fs, createDate.ToString());

                // Creator Information
                Help.WriteString(fs, creator);
                Help.WriteString(fs, creatorLogo);
                Help.WriteString(fs, creatorWebsite);

                string rootDir = System.IO.Path.GetDirectoryName(appFile);
                // File count
                Help.WriteString(fs, (1).ToString());

                string teachAppFile = appFile.Remove(0, rootDir.Length);
                Help.WriteString(fs, teachAppFile);

                Help.WriteBytes(fs, File.ReadAllBytes(appFile));
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

        private static void loadMreItem(string fileName, string slpFile)
        {
            MemorizeEntry memorizeEntry = MemorizeEntry.Load(fileName);

            string title = memorizeEntry.Title;
            string description = memorizeEntry.Description;

            string thumbnailFile = memorizeEntry.Thumbnail;

            string id = memorizeEntry.Id;

            List<string> additionalFileList = new List<string>();
            foreach (string file in memorizeEntry.BackgroundImages)
            {
                additionalFileList.Add(file);
            }

            foreach (string file in memorizeEntry.BackgroundMusics)
            {
                additionalFileList.Add(file);
            }

            if (!string.IsNullOrEmpty(memorizeEntry.MemorizeItemBackground))
                additionalFileList.Add(memorizeEntry.MemorizeItemBackground);

            if (!string.IsNullOrEmpty(memorizeEntry.ItemClickMusic))
                additionalFileList.Add(memorizeEntry.ItemClickMusic);

            if (!string.IsNullOrEmpty(memorizeEntry.Thumbnail))
                additionalFileList.Add(memorizeEntry.Thumbnail);

            foreach (MemorizeItem item in memorizeEntry.Items)
            {
                getItemInfo(item.ObjectA, additionalFileList);
                getItemInfo(item.ObjectB, additionalFileList);
            }

            // Create package
            string packFile = slpFile;

            FileStream fs = File.OpenWrite(packFile);

            try
            {
                // Header
                Help.WriteString(fs, "{BE4A1507-5B37-42EA-9E08-43EF4F363C42}");

                // AppId
                Help.WriteString(fs, id);
                // Title
                Help.WriteString(fs, title);
                Help.WriteString(fs, description);

                // Thumbnail ext
                Help.WriteString(fs, System.IO.Path.GetExtension(thumbnailFile));

                byte[] emptyData = new byte[0];
                // Thumbnail data
                if (File.Exists(thumbnailFile))
                    Help.WriteBytes(fs, File.ReadAllBytes(thumbnailFile));
                else
                    Help.WriteBytes(fs, emptyData);

                // Write a empty entry as Memorize App no entry and dll file.
                Help.WriteString(fs, System.IO.Path.GetFileName(fileName));
                Help.WriteString(fs, string.Empty);

                // Type
                Help.WriteBytes(fs, BitConverter.GetBytes(102));
                Help.WriteBytes(fs, BitConverter.GetBytes(memorizeEntry.SubType));

                // Create Time
                Help.WriteString(fs, memorizeEntry.CreateDate.ToString());

                // Creator Information
                Help.WriteString(fs, memorizeEntry.Creator);
                Help.WriteString(fs, memorizeEntry.CreatorLogo);
                Help.WriteString(fs, memorizeEntry.CreatorWebsite);

                string rootDir = System.IO.Path.GetDirectoryName(fileName);
                // File count
                Help.WriteString(fs, (additionalFileList.Count + 1).ToString());

                string mreFile = fileName.Remove(0, rootDir.Length);
                Help.WriteString(fs, mreFile);

                Help.WriteBytes(fs, File.ReadAllBytes(fileName));

                foreach (string temp in additionalFileList)
                {
                    string name = temp.Remove(0, rootDir.Length);
                    Help.WriteString(fs, name);

                    if (File.Exists(temp))
                        Help.WriteBytes(fs, File.ReadAllBytes(temp));
                    else
                        Help.WriteBytes(fs, emptyData);
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

        private static void getItemInfo(MemorizeObject obj, List<string> fileList)
        {
            if (obj is MemorizeImage)
            {
                MemorizeImage image = obj as MemorizeImage;
                if (fileList.Contains(image.Url))
                    return;
                fileList.Add(image.Url);
            }
            else if (obj is MemorizeMusic)
            {
                MemorizeMusic music = obj as MemorizeMusic;
                if (fileList.Contains(music.Url))
                    return;
                fileList.Add(music.Url);
            }
        }

        private static void createMrePackFile(string file)
        {
            
        }
    }
}
