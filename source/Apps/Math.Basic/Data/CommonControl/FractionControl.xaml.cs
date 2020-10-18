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

namespace Math.Basic.Data.CommonControl
{
    /// <summary>
    /// Interaction logic for FractionControl.xaml
    /// </summary>
    public partial class FractionControl : UserControl
    {
        public decimal Denominator
        {
            set { this.denominatorLabel.Content = value; }
        }

        public decimal Numerator
        {
            set { this.umeratorLabel.Content = value; }
        }

        public FractionControl()
        {
            InitializeComponent();
        }
    }
}
