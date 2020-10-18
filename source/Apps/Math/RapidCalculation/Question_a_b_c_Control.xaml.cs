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
    /// Interaction logic for Question_a_b_c_Control.xaml
    /// </summary>
    public partial class Question_a_b_c_Control : UserControl
    {
        public EventHandler<EventArgs> SelectedChanged;
        public EventHandler<EventArgs> QuestionValidated;

        private bool isSelected;

        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                this.isSelected = value;

                if (!this.isSelected)
                {
                    this.ValidResult();
                }
                else
                {
                }

                if (SelectedChanged != null)
                    SelectedChanged(this, EventArgs.Empty);

                if (value)
                    this.Opacity = 1.0f;
                else
                    this.Opacity = 0.8f;
            }
        }

        public Question_a_b_c_Control(float fontSize, FontWeight fontWeight, double abColumeWidth)
        {
            InitializeComponent();

            this.aColume.Width = new GridLength(abColumeWidth, GridUnitType.Pixel);
            this.bColume.Width = new GridLength(abColumeWidth, GridUnitType.Pixel);

            foreach (UIElement element in this.rootGrid.Children)
            {
                if (element is Control)
                {
                    Control ctrl = element as Control;
                    ctrl.FontSize = fontSize;
                    ctrl.FontWeight = FontWeight;
                }
            }

            this.IsSelected = false;

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

        private void UserControl_Initialized(object sender, EventArgs e)
        {

        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
        }

        private void resultTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            this.IsSelected = true;
        }

        private void resultTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            this.IsSelected = false;
        }

        private void resultTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int value;
            if (!int.TryParse(e.Text, out value))
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }

        public void Select()
        {
            this.IsSelected = true;
            this.resultTextBox.Focus();
            this.resultTextBox.SelectAll();
        }

        internal void ValidResult()
        {
            Question_a_b_c question = this.DataContext as Question_a_b_c;
            if (question != null)
            {
                BindingExpression be = this.resultTextBox.GetBindingExpression(TextBox.TextProperty);
                be.UpdateSource();
            }
        }

        internal void numberKeyboard_NumberVirtualKeyboardInputEvent(Key key)
        {
            string text = string.Empty;

            switch (key)
            {
                case Key.NumPad0:
                    text += "0";
                    break;
                case Key.NumPad1:
                    text += "1";
                    break;
                case Key.NumPad2:
                    text += "2";
                    break;
                case Key.NumPad3:
                    text += "3";
                    break;
                case Key.NumPad4:
                    text += "4";
                    break;
                case Key.NumPad5:
                    text += "5";
                    break;
                case Key.NumPad6:
                    text += "6";
                    break;
                case Key.NumPad7:
                    text += "7";
                    break;
                case Key.NumPad8:
                    text += "8";
                    break;
                case Key.NumPad9:
                    text += "9";
                    break;
                case Key.Enter:
                    return;
                case Key.Back:
                    {
                        this.isSelected = true;
                        if (!string.IsNullOrEmpty(this.resultTextBox.Text))
                        {
                            if (this.resultTextBox.CaretIndex == 0)
                                this.resultTextBox.CaretIndex = this.resultTextBox.Text.Length;
                            this.resultTextBox.Text = this.resultTextBox.Text.Remove(this.resultTextBox.CaretIndex - 1, 1);
                        }
                    }
                    return;
            }

            Clipboard.SetText(text);
            this.resultTextBox.Paste();
            this.resultTextBox.CaretIndex = this.resultTextBox.Text.Length;

            this.Opacity = 1.0f;
        }

        private void feedbackImage_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (this.QuestionValidated != null)
                this.QuestionValidated(this, EventArgs.Empty);
        }

        private void feedbackImage_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (this.QuestionValidated != null)
                this.QuestionValidated(this, EventArgs.Empty);
        }
    }
}
