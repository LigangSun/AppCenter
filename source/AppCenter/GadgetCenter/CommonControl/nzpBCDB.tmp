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
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.Controls
{
    public delegate void MessageWindowCallback(bool ok);
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow
    {
        private MessageWindowCallback callback;

        public MessageWindow()
        {
            InitializeComponent();
        }

        public void ShowMessage(string message, MessageBoxButton msgBtn, MessageWindowCallback callback)
        {
            if (msgBtn == MessageBoxButton.OK)
            {
                this.btnPanel.Children.Remove(this.cancelBtn);
            }

            this.callback = callback;
       //     this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            this.infoTextBlock.Text = message;
       //     this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

            GC_UIHelper.ShowMessageWindow(this);

        //    return this.ShowDialog().Value;
        }

        private void okBtn_Click(object sender, RoutedEventArgs e)
        {
            GC_UIHelper.CloseMessageWindow();
            if (this.callback != null)
                this.callback(true);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            GC_UIHelper.CloseMessageWindow();
            if (this.callback != null)
                this.callback(false);
        }
    }
}
