using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Win32;
using SoonLearning.Memorize.Data;
using System.Collections.ObjectModel;

namespace MemorizeAppCreator
{
    internal class UIHelper
    {
        internal const string imageFilter = "所以图片|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tif;*.tiff|Jpeg文件|*.jpg;*.jpeg|位图文件|*.bmp|png文件|*.png|Gif文件|*.gif|tiff文件|*.tiff;*.tif";
        internal const string musicFilter = "mp3文件|*.mp3|wma文件|*.wma|wav文件|*.wav";

        private static ObservableCollection<MemorizeTypeItem> memorizeTypeCollection = new ObservableCollection<MemorizeTypeItem>();

        public static ObservableCollection<MemorizeTypeItem> MemorizeTypeItems
        {
            get
            {
                if (memorizeTypeCollection.Count == 0)
                {
                    memorizeTypeCollection.Add(new MemorizeTypeItem() { Name = "数学", Type = 10201 });
                    memorizeTypeCollection.Add(new MemorizeTypeItem() { Name = "英语", Type = 10202 });
                    memorizeTypeCollection.Add(new MemorizeTypeItem() { Name = "汉字", Type = 10203 });
                    memorizeTypeCollection.Add(new MemorizeTypeItem() { Name = "图形组合", Type = 10204 });
                    memorizeTypeCollection.Add(new MemorizeTypeItem() { Name = "其他记忆", Type = 10299 });
                }

                return memorizeTypeCollection;
            }
        }

        public static void OpenFile(string filter, TextBox textBox)
        {
            try
            {
                string file = OpenFile(filter);
                if (!string.IsNullOrEmpty(file))
                    textBox.Text = file;
            }
            catch
            {
            }
        }

        public static string OpenFile(string filter)
        {
            OpenFileDialog openFileDlg = new OpenFileDialog();
            openFileDlg.Filter = filter;
            if (openFileDlg.ShowDialog().Value)
            {
                return openFileDlg.FileName;
            }

            return string.Empty;
        }
    }
}
