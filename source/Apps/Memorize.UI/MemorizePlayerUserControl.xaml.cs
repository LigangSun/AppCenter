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
    /// MemorizePlayerUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class MemorizePlayerUserControl : UserControl
    {
        private static MemorizePlayerUserControl instance;

        internal static MemorizePlayerUserControl Instance
        {
            get
            {
                if (instance == null)
                    instance = new MemorizePlayerUserControl();
                return instance;
            }
        }

        public MemorizePlayerUserControl()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeUIContainerUserControl.Instance.SwitchToSettingPage();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeDataMgr.Instance.PlayerAName = this.playerANameTextBox.Text;
            MemorizeDataMgr.Instance.PlayerBName = this.playerBNameTextBox.Text;
            if (string.IsNullOrEmpty(MemorizeDataMgr.Instance.PlayerAName))
                MemorizeDataMgr.Instance.PlayerAName = "玩家A";
            if (string.IsNullOrEmpty(MemorizeDataMgr.Instance.PlayerBName))
                MemorizeDataMgr.Instance.PlayerBName = "玩家B";
            MemorizeUIContainerUserControl.Instance.SwitchToStartupPage();
        }
    }
}
