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
    /// Interaction logic for Question_a_b_c_PrintControl.xaml
    /// </summary>
    public partial class Question_a_b_c_PrintControl : UserControl
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

        public Question Question
        {
            set
            {
                this.DataContext = value;
                Question_a_b_c abc = value as Question_a_b_c;
                if (abc != null)
                    this.SetOPImage(abc.Op);
            }
        }

        public Question_a_b_c_PrintControl(float fontSize, FontWeight fontWeight)
        {
            InitializeComponent();

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

        public Question_a_b_c_PrintControl(float fontSize, FontWeight fontWeight, SolidColorBrush foreground)
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

        private void SetOPImage(Operator op)
        {
            switch (op)
            {
                case Operator.Plus:
                    this.opImage.Source = new BitmapImage(new Uri(@"pack:\\application:,,,/Math;Component/Resources/add_print.png"));
                    break;
                case Operator.Minus:
                    this.opImage.Source = new BitmapImage(new Uri(@"pack:\\application:,,,/Math;Component/Resources/minus_print.png"));
                    break;
                case Operator.Multiply:
                    this.opImage.Source = new BitmapImage(new Uri(@"pack:\\application:,,,/Math;Component/Resources/mu_printl.png"));
                    break;
                case Operator.Division:
                    this.opImage.Source = new BitmapImage(new Uri(@"pack:\\application:,,,/Math;Component/Resources/div_print.png"));
                    break;
            }
        }
    }
}
