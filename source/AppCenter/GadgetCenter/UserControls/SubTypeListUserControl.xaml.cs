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
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Controls;

namespace SoonLearning.AppCenter.UserControls
{
    /// <summary>
    /// Interaction logic for SubTypeListUserControl.xaml
    /// </summary>
    public partial class SubTypeListUserControl
    {
        public SubTypeListUserControl()
        {
            InitializeComponent();
        }

        internal void LoadGadgetList(SubTypeItem item)
        {
            //base.Title = item.Title;

            //SortedDictionary<DateTime, AppItem> itemDictionary = new SortedDictionary<DateTime, AppItem>();

            //this.subTypeList.Children.Clear();
            //this.subTypeList.SelectedIndex = -1;
            //foreach (AppItem gi in DataMgr.Instance.GadgetItems)
            //{
            //    if (gi.SubType == (int)item.Type)
            //    {
            //        if (itemDictionary.ContainsKey(gi.CreateDate))
            //        {
            //            DateTime createDate = gi.CreateDate;
            //            while (itemDictionary.ContainsKey(gi.CreateDate))
            //            {
            //                createDate = createDate.AddDays(1);
            //            }

            //            itemDictionary.Add(createDate, gi);
            //        }
            //        else
            //        {
            //            itemDictionary.Add(gi.CreateDate, gi);
            //        }
            //    }
            //}

            //foreach (DateTime dt in itemDictionary.Keys)
            //{
            //    ListBoxItem li = new ListBoxItem();
            //    li.DataContext = itemDictionary[dt];
            //    li.Style = App.Current.FindResource("thumbnailListBoxItemStyle") as Style;
            //    subTypeList.Children.Add(li);
            //}

            //if (this.subTypeList.Children.Count > 0)
            //    this.subTypeList.SelectedIndex = 0;

            //Binding binding = new Binding();
            //binding.Source = this.subTypeList;
            //binding.Path = new PropertyPath("SelectedIndex");
            //this.SetBinding(GadgetUserControl.IndexProperty, binding);

            //this.Total = this.subTypeList.Children.Count;
        }

        private void subTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.LoadGadget();
        }

        private void LoadGadget()
        {
            if (this.subTypeList.SelectedItem == null)
                return;

            ListBoxItem lbi = this.subTypeList.SelectedItem as ListBoxItem;
            MainWindow mainWnd = App.Current.MainWindow as MainWindow;
            mainWnd.LoadGadget(lbi.DataContext as AppItem);

        //    this.subTypeList.SelectedItem = null;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            this.subTypeList.Focus();
        }
    }
}
