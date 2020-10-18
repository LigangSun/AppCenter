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
using System.Windows.Shapes;
using System.Diagnostics;

namespace SoonLearning.Memorize.UI
{
    /// <summary>
    /// Interaction logic for MemorizeResultWindow.xaml
    /// </summary>
    public partial class MemorizeResultWindow : Window
    {
        public MemorizeResultWindow()
        {
            InitializeComponent();
        }

        private void shareButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(string.Format(@"http://www.soonlearning.com/MemorizeAppSharedPage.aspx?AppUniqueId={0}&SharedUID={1}&PKMode={2}&TimingMode={3}&CurrentStage={4}&TotalStage={6}&UsedTime={5}", 
                MemorizeDataMgr.Instance.Entry.Id, 
                MemorizeDataMgr.Instance.UserId,
                MemorizeDataMgr.Instance.Entry.IsPkMode,
                (int)MemorizeDataMgr.Instance.CurrentTimingMode,
                MemorizeDataMgr.Instance.CurrentStage,
                MemorizeDataMgr.Instance.UsedTime,
                MemorizeDataMgr.Instance.Entry.Stages.Count));
        }

        private void retryButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
