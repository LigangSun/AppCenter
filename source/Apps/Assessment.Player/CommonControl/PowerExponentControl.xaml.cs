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

namespace SoonLearning.Assessment.Player.CommonControl
{
    /// <summary>
    /// Interaction logic for PowerExponentControl.xaml
    /// </summary>
    public partial class PowerExponentControl : UserControl
    {
        private QuestionPowerExponentPart powerExponentPart;

        public QuestionPowerExponentPart PowerExponentPart
        {
            set
            {
                this.powerExponentPart = value;
                this.ShowPowerExponentPart();
            }
        }

        public PowerExponentControl(double size, FontWeight weight, Brush foreground)
        {
            InitializeComponent();

            this.FontSize = size;
            this.FontWeight = weight;
            this.Foreground = foreground;
        }

        private void ShowPowerExponentPart()
        {
            if (this.powerExponentPart == null)
                return;

            this.baseNumberPanel.Children.Clear();
            this.powerPanel.Children.Clear();

            CommonControlCreator.CreateContentControl(this.powerExponentPart.BaseNumber, this.baseNumberPanel, this.Foreground, null);
            CommonControlCreator.CreateContentControl(this.powerExponentPart.Power.Content, this.powerPanel, this.Foreground, null);
        }
    }
}
