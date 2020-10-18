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
using SoonLearning.Assessment.Data;

namespace SoonLearning.Math.UI
{
    /// <summary>
    /// Interaction logic for FractionControl.xaml
    /// </summary>
    public partial class FractionControl : UserControl
    {
        private QuestionFractionPart fractionPart;

        public QuestionFractionPart FractionPart
        {
            set
            {
                this.fractionPart = value;
                this.ShowFractionPart();
            }
        }

        public FractionControl(double size, FontWeight weight, double seperatorHeight, Brush foreground)
        {
            InitializeComponent();

            this.FontSize = size;
            this.FontWeight = weight;
            this.Foreground = foreground;

            this.seperatorRect.Height = seperatorHeight;
            this.seperatorRect.Fill = foreground;
        }

        private void ShowFractionPart()
        {
            if (this.fractionPart == null)
                return;

            if (this.fractionPart.WithPart != null)
            {
                this.withPanel.Visibility = System.Windows.Visibility.Visible;
                CommonControlCreator.CreateContentControl(this.fractionPart.WithPart, this.withPanel, this.Foreground, null);
            }
            else
            {
                this.withPanel.Visibility = System.Windows.Visibility.Hidden;
            }

            CommonControlCreator.CreateContentControl(this.fractionPart.Numerator, this.umeratorPanel, this.Foreground, null);
            CommonControlCreator.CreateContentControl(this.fractionPart.Denominator, this.denominatorPanel, this.Foreground, null);
        }
    }
}
