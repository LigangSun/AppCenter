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
using Microsoft.Win32;
using System.Threading;
using SoonLearning.AppCenter.License;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    /// <summary>
    /// Interaction logic for AddMinusSettingControl.xaml
    /// </summary>
    public partial class AddMinusSettingControl
    {
        private bool reseting = true;

        public AddMinusSettingControl()
        {
            InitializeComponent();

            if (!MathSetting.Instance.EnableRange)
            {
                this.rangeLabel.Visibility = System.Windows.Visibility.Hidden;
                this.rangePanel.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MathSetting.Instance.MaxNumber - MathSetting.Instance.MinNumber < 10)
            {
                MessageWindow msgWnd = new MessageWindow();
                msgWnd.ShowMessage("取值范围必须不能小于10!", MessageBoxButton.OK, null);
                return;
            }

            if (!MathSetting.Instance.Valid())
                return;

            ControlMgr.Instance.MathStartupControl.ShowQuestionControl();
            ControlMgr.Instance.MathQuestionControl.NewQuiz();
        }

        private void maxQuestionCountHyperlink_Click(object sender, RoutedEventArgs e)
        {
            MathSetting.Instance.QuestionCount = MathSetting.Instance.MaxQuestionCount;
        }

        private void defineHyperlink_Click(object sender, RoutedEventArgs e)
        {
        //    this.definePanel.Visibility = System.Windows.Visibility.Visible;
        }

        private void defineBtn_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    int value = Convert.ToInt32(this.defineTextBox.Text);
            //    if (value < 10 || value > 10000)
            //        return;

            //    MathSetting.Instance.NumberCollection.Add(value);
            //    this.numberComboBox.SelectedItem = value;
            //}
            //catch
            //{
            //    return;
            //}

            //this.definePanel.Visibility = System.Windows.Visibility.Hidden;
        }

        private void defineTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int value;
            if (!int.TryParse(e.Text, out value))
                e.Handled = true;

            base.OnPreviewTextInput(e);
        }

        private void viewResultBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.MathStartupControl.ShowHistoryControl();
        }

        private void numberComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            return;
            //if (this.reseting)
            //    return;

            //bool trial = LicenseMgr.Instance.IsTrialVersion(ControlMgr.Instance.Entry.Id);
            //if (trial)
            //{
            //    this.reseting = true;

            //    UIHelper.ShowTrialMessage();
            ////    this.numberComboBox.SelectedIndex = 0;

            //    this.reseting = false;
            //}
        }

        private void questionCountUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            return;
            //if (this.reseting)
            //    return;

            //bool trial = LicenseMgr.Instance.IsTrialVersion(ControlMgr.Instance.Entry.Id);
            //if (trial)
            //{
            //    this.reseting = true;

            //    UIHelper.ShowTrialMessage();
            //    this.questionCountUpDown.Value = 20;

            //    this.reseting = false;
            //}
        }

        private void timeUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<decimal> e)
        {
            return;
            //if (this.reseting)
            //    return;

            //bool trial = LicenseMgr.Instance.IsTrialVersion(ControlMgr.Instance.Entry.Id);
            //if (trial)
            //{
            //    this.reseting = true;

            //    UIHelper.ShowTrialMessage();
            //    this.timeUpDown.Value = 5;

            //    this.reseting = false;
            //}
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.reseting = false;
        }
    }
}
