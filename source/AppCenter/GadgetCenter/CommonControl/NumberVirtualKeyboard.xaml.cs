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

namespace SoonLearning.AppCenter.Controls
{
    public delegate void NumberVirtualKeyboadInputDelegate(Key key);

    /// <summary>
    /// Interaction logic for NumberVirtualKeyboard.xaml
    /// </summary>
    public partial class NumberVirtualKeyboard : UserControl
    {
        public event NumberVirtualKeyboadInputDelegate NumberVirtualKeyboardInputEvent;

        public NumberVirtualKeyboard()
        {
            InitializeComponent();
        }

        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            if (btn == null)
                return;

            string tag = btn.Tag as string;
            if (string.IsNullOrEmpty(tag))
                return;

            Key key = (Key)Enum.Parse(typeof(Key), tag);
            if (this.NumberVirtualKeyboardInputEvent != null)
                this.NumberVirtualKeyboardInputEvent(key);
        }
    }
}
