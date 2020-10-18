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

namespace SoonLearning.Assessment.Player.Editor
{
    /// <summary>
    /// Interaction logic for RichEditorToolBarUserControl.xaml
    /// </summary>
    public partial class RichEditorToolBarUserControl : UserControl
    {
        public bool InsertBlankButtonVisible
        {
            set
            {
                if (value)
                    this.insertBlankButton.Visibility = System.Windows.Visibility.Visible;
                else
                    this.insertBlankButton.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public RichEditorToolBarUserControl()
        {
            InitializeComponent();
        }
    }
}
