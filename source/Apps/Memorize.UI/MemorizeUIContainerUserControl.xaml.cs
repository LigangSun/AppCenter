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

namespace SoonLearning.Memorize.UI
{
    /// <summary>
    /// MemorizeUIContainerUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class MemorizeUIContainerUserControl : UserControl
    {
        private static MemorizeUIContainerUserControl instance;

        internal static MemorizeUIContainerUserControl Instance
        {
            get
            {
                if (instance == null)
                    instance = new MemorizeUIContainerUserControl();
                return instance;
            }
        }

        public MemorizeUIContainerUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.SwitchToSettingPage();
        }

        internal void SwitchToSettingPage()
        {
            this.rootGrid.Children.Clear();
            this.rootGrid.Children.Add(MemorizeSettingUserControl.Instance);
        }

        internal void SwitchToPlayerNamePage()
        {
            this.rootGrid.Children.Clear();
            this.rootGrid.Children.Add(MemorizePlayerUserControl.Instance);
        }

        internal void SwitchToStartupPage()
        {
            this.rootGrid.Children.Clear();
            this.rootGrid.Children.Add(MemorizeStartupUserControl.Instance);
        }
    }
}
