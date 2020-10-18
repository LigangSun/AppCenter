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
using SoonLearning.Math.Data;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    /// <summary>
    /// Interaction logic for Question_a_b_c_ResultControl.xaml
    /// </summary>
    public partial class Question_a_b_c_ResultControl : UserControl
    {
        public bool ShowAnswer
        {
            set
            {
                if (!value)
                    this.correctAnswerLabel.Visibility = System.Windows.Visibility.Hidden;
                else
                    this.correctAnswerLabel.Visibility = System.Windows.Visibility.Visible;
            }
        }

        public bool ShowResult
        {
            set
            {
                if (!value)
                {
                    this.feedbackImage.Visibility = System.Windows.Visibility.Hidden;
                    this.resultLabel.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    this.feedbackImage.Visibility = System.Windows.Visibility.Visible;
                    this.resultLabel.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }

        public Question_a_b_c_ResultControl(float fontSize, FontWeight fontWeight, double abWidth)
        {
            InitializeComponent();

            this.aColume.Width = new GridLength(abWidth, GridUnitType.Pixel);
            this.bColume.Width = new GridLength(abWidth, GridUnitType.Pixel);

            foreach (UIElement element in this.rootGrid.Children)
            {
                if (element is Control)
                {
                    Control ctrl = element as Control;
                    ctrl.FontSize = fontSize;
                    ctrl.FontWeight = FontWeight;
                }
            }

            if (MathSetting.Instance.SelectedMaxNumber < 100)
            {
                this.alabel.Width = 60;
                this.blabel.Width = 60;
            }
            else if (MathSetting.Instance.SelectedMaxNumber > 100 &&
                MathSetting.Instance.SelectedMaxNumber < 1000)
            {
                this.alabel.Width = 100;
                this.blabel.Width = 100;
            }
            else
            {
                this.alabel.Width = 160;
                this.blabel.Width = 160;
            }
        }

        public Question_a_b_c_ResultControl(float fontSize, FontWeight fontWeight, SolidColorBrush foreground)
        {
            InitializeComponent();

            foreach (UIElement element in this.rootGrid.Children)
            {
                if (element is Control)
                {
                    Control ctrl = element as Control;
                    ctrl.FontSize = fontSize;
                    ctrl.FontWeight = FontWeight;
                    ctrl.Foreground = foreground;
                }
            }

            if (MathSetting.Instance.SelectedMaxNumber < 100)
            {
                this.alabel.Width = 60;
                this.blabel.Width = 60;
            }
            else if (MathSetting.Instance.SelectedMaxNumber > 100 &&
                MathSetting.Instance.SelectedMaxNumber < 1000)
            {
                this.alabel.Width = 100;
                this.blabel.Width = 100;
            }
            else
            {
                this.alabel.Width = 160;
                this.blabel.Width = 160;
            }
        }
    }
}
