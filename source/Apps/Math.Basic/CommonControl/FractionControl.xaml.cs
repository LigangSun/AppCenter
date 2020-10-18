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

namespace Math.Basic.CommonControl
{
    /// <summary>
    /// Interaction logic for FractionControl.xaml
    /// </summary>
    public partial class FractionControl : UserControl
    {
        private decimal? denominator;
        private decimal? numerator;
        private bool withFraction;

        public decimal Denominator
        {
            set 
            {
                this.denominator = value;
                this.denominatorLabel.Content = value;
                if (this.withFraction)
                    this.ShowWithFraction();
            }
        }

        public decimal Numerator
        {
            set 
            {
                this.numerator = value;
                this.umeratorLabel.Content = value;
                if (this.withFraction)
                    this.ShowWithFraction();
            }
        }

        public bool WithFraction
        {
            set
            {
                this.withFraction = value;
                if (value)
                {
                    this.ShowWithFraction();
                }
                else
                {
                }
            }
        }

        public FractionControl(double size, FontWeight weight, double seperatorHeight, Brush foreground)
        {
            InitializeComponent();

            this.Foreground = foreground;
            foreach (UIElement element in this.rightPanel.Children)
            {
                if (element is Control)
                {
                    Control ctrl = element as Control;
                    ctrl.FontSize = size;
                    ctrl.FontWeight = weight;
                    ctrl.Foreground = foreground;
                }
            }

            this.withLabel.FontSize = size;
            this.withLabel.FontWeight = weight;
            this.withLabel.Foreground = foreground;

            this.seperatorRect.Height = seperatorHeight;
            this.seperatorRect.Fill = foreground;
        }

        private void ShowWithFraction()
        {
            if (this.denominator == null ||
                this.numerator == null ||
                !this.withFraction)
                return;

            decimal withValue = this.numerator.Value / this.denominator.Value;
            decimal leftValue = this.numerator.Value % this.denominator.Value;
            if (withValue == 0 || leftValue == 0)
                return;

            this.withLabel.Content = withValue;
            this.umeratorLabel.Content = leftValue;
        }

        private void ShowFraction()
        {
            if (this.numerator != null)
                this.Numerator = this.numerator.Value;
            if (this.denominator != null)
                this.Denominator = this.denominator.Value;
        }
    }
}
