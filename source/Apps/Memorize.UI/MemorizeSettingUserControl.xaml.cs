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
    /// MemorizeSettingUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class MemorizeSettingUserControl : UserControl
    {
        private static MemorizeSettingUserControl instance;

        internal static MemorizeSettingUserControl Instance
        {
            get
            {
                if (instance == null)
                    instance = new MemorizeSettingUserControl();
                return instance;
            }
        }

        public MemorizeSettingUserControl()
        {
            InitializeComponent();
        }

        private void singleButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeDataMgr.Instance.CurrentChanllengeMode = ChanllengeMode.SinglePlayer;
            MemorizeUIContainerUserControl.Instance.SwitchToStartupPage();
        }

        private void multiButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeDataMgr.Instance.CurrentChanllengeMode = ChanllengeMode.TwoPlayer;
            MemorizeUIContainerUserControl.Instance.SwitchToPlayerNamePage();
        }

        private void pcButton_Click(object sender, RoutedEventArgs e)
        {
            MemorizeDataMgr.Instance.PlayerAName = "你：";
            MemorizeDataMgr.Instance.PlayerBName = "电脑：";

            MemorizeDataMgr.Instance.CurrentChanllengeMode = ChanllengeMode.VsPC;
            MemorizeUIContainerUserControl.Instance.SwitchToStartupPage();
        }
    }
}
