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
    /// Interaction logic for MathEditorControl.xaml
    /// </summary>
    public partial class MathEditorControl : UserControl
    {
        #region Members

        private Control activeControl;

        #endregion

        #region Constructors

        public MathEditorControl()
        {
            InitializeComponent();

            Control currentControl = this.GetActiveControl();
            if (currentControl == null)
            {
                currentControl = this.CreateTextBox(string.Empty);
                this.rootPanel.Children.Add(currentControl);
                this.activeControl = currentControl;
            }
        }

        #endregion

        #region Events

        private void WrapPanel_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void rootPanel_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Control currentControl = this.GetActiveControl();
            if (currentControl == null)
            {
                currentControl = this.CreateTextBox(e.Key.ToString());
                this.rootPanel.Children.Add(currentControl);
                this.activeControl = currentControl;
            }
        }

        #endregion

        #region Private Methods

        private Control GetActiveControl()
        {
            return this.activeControl;
        }

        private TextBox CreateTextBox(string text)
        {
            TextBox textBox = new TextBox();
            textBox.Focus();
            textBox.SelectAll();
            textBox.Text = text;
            return textBox;
        }

        #endregion

        #region Public Methods

        public void InsertPowerExponent()
        {
            PowerExponentEditControl powerExponentEditCtrl = new PowerExponentEditControl();
            powerExponentEditCtrl.Width = 100;

            Control activeControl = this.GetActiveControl();
            if (activeControl == null)
                this.rootPanel.Children.Add(powerExponentEditCtrl);
            else
            {
                int index = this.rootPanel.Children.IndexOf(activeControl);
                if (activeControl is TextBox)
                {
                    this.rootPanel.Children.Insert(index, powerExponentEditCtrl);
                    TextBox textBox = activeControl as TextBox;
                    if (string.IsNullOrEmpty(textBox.Text))
                        this.rootPanel.Children.Remove(textBox);
                }
                else
                {
                    if (index == this.rootPanel.Children.Count - 1)
                        this.rootPanel.Children.Add(powerExponentEditCtrl);
                    else
                        this.rootPanel.Children.Insert(index, powerExponentEditCtrl);
                }
            }

            this.activeControl = powerExponentEditCtrl;
        }

        #endregion
    }
}
