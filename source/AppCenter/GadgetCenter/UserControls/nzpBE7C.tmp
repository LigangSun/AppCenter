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
using System.Windows.Media.Animation;
using SoonLearning.AppCenter.Data;
using System.Collections.ObjectModel;
using System.Collections;
using SoonLearning.AppCenter.CommonControl;
using SoonLearning.AppCenter.Controls;

namespace SoonLearning.AppCenter.UserControls
{
    public delegate void ThumbnailItemSelectedDelegate(int index);

    /// <summary>
    /// Interaction logic for ThumbnailListControl.xaml
    /// </summary>
    public partial class ThumbnailListControl
    {
        public event ThumbnailItemSelectedDelegate ThumbnailItemSelectedEvent;

        private ObservableCollection<object> itemsSource = new ObservableCollection<object>();

        public IEnumerable ItemsSource
        {
            set 
            {
                this.itemsSource.Clear();
                foreach (object o in value)
                {
                    this.itemsSource.Add(o);
                }
                this.BuildThumbnailList();
            }
            get { return this.itemsSource; }
        }

        public int SelectedIndex
        {
            set
            {
                this.thumbnailList.SelectedIndex = value;
            }
            get 
            { 
                return this.thumbnailList.SelectedIndex; 
            }
        }

        public ThumbnailListControl()
        {
            InitializeComponent();

            Binding binding = new Binding();
            binding.Source = this.thumbnailList;
            binding.Path = new PropertyPath("SelectedIndex");
            this.SetBinding(GadgetUserControl.IndexProperty, binding);
        }

        private void BuildThumbnailList()
        {
            Style style = App.Current.FindResource("thumbnailListBoxItemStyle") as Style;
            this.thumbnailList.Children.Clear();
            foreach (object o in this.itemsSource)
            {
                ListBoxItem lbi = new ListBoxItem();
                lbi.DataContext = o;
                lbi.Style = style;
                this.thumbnailList.Children.Add(lbi);
            }

            base.Total = this.itemsSource.Count;
        }

        private void thumbnailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.thumbnailList.SelectedItem == null)
                return;

            if (this.ThumbnailItemSelectedEvent != null)
            {
                this.ThumbnailItemSelectedEvent(this.thumbnailList.SelectedIndex);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
            this.thumbnailList.Focus();
        }

        private void thumbnailList_MouseEnter(object sender, MouseEventArgs e)
        {

        }
    }
}
