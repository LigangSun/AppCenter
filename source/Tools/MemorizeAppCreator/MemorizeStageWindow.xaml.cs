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
using System.Windows.Shapes;
using SoonLearning.Memorize.Data;

namespace MemorizeAppCreator
{
    /// <summary>
    /// Interaction logic for MemorizeStageWindow.xaml
    /// </summary>
    public partial class MemorizeStageWindow : Window
    {
        private MemorizeStage stage = null;

        public MemorizeStage Stage
        {
            get { return this.stage; }
        }

        public MemorizeStageWindow(MemorizeStage stage)
        {
            InitializeComponent();

            this.stage = stage;
            if (this.stage != null)
            {
                if (this.stage is MemorizeStableStage)
                {
                    this.stableRadioButton.IsChecked = true;
                }
                else if (this.stage is MemorizeRandomItemStage)
                {
                    this.randomRadioButton.IsChecked = true;
                    this.itemCountTextBox.Text = ((MemorizeRandomItemStage)this.stage).ItemCount.ToString();
                }
                else if (this.stage is MemorizeMathStage)
                {
                    this.mathStageRadioButton.IsChecked = true;
                    MemorizeMathStage mathStage = this.stage as MemorizeMathStage;
                    this.addCheckBox.IsChecked = mathStage.Plus;
                    this.minusCheckBox.IsChecked = mathStage.Minus;
                    this.multiplyCheckBox.IsChecked = mathStage.Multiplication;
                    this.divisionCheckBox.IsChecked = mathStage.Division;
                    this.minValueTextBox.Text = mathStage.MinValue.ToString();
                    this.maxValueTextBox.Text = mathStage.MaxValue.ToString();
                    this.mathCountTextBox.Text = mathStage.ItemCount.ToString();
                }
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.stableRadioButton.IsChecked.Value)
            {
                this.stage = new MemorizeStableStage();
            }
            else if (this.randomRadioButton.IsChecked.Value)
            {
                int count = 0;
                if (!int.TryParse(this.itemCountTextBox.Text, out count))
                {
                    MessageBox.Show("请输入合法的数字.", "关卡设置", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.itemCountTextBox.SelectAll();
                    this.itemCountTextBox.Focus();
                    return;
                }

                this.stage = new MemorizeRandomItemStage();
                ((MemorizeRandomItemStage)this.stage).ItemCount = count;
            }
            else if (this.mathStageRadioButton.IsChecked.Value)
            {
                int minValue;
                int maxValue;
                int count;
                if (!int.TryParse(this.minValueTextBox.Text, out minValue))
                {
                    MessageBox.Show("请为最小值输入合法的数字!", "生成算式", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.minValueTextBox.Focus();
                    this.minValueTextBox.SelectAll();
                    return;
                }

                if (!int.TryParse(this.maxValueTextBox.Text, out maxValue))
                {
                    MessageBox.Show("请为最大值输入合法的数字!", "生成算式", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.maxValueTextBox.Focus();
                    this.maxValueTextBox.SelectAll();
                    return;
                }

                if (!int.TryParse(this.mathCountTextBox.Text, out count))
                {
                    MessageBox.Show("请输入合法的数字.", "关卡设置", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.mathCountTextBox.SelectAll();
                    this.mathCountTextBox.Focus();
                    return;
                }

                if (maxValue - minValue <= 2)
                {
                    MessageBox.Show("取值范围不合法!", "生成算式", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                this.stage = new MemorizeMathStage();
                ((MemorizeMathStage)this.stage).MinValue = minValue;
                ((MemorizeMathStage)this.stage).MaxValue = maxValue;
                ((MemorizeMathStage)this.stage).Plus = this.addCheckBox.IsChecked.Value;
                ((MemorizeMathStage)this.stage).Minus = this.minusCheckBox.IsChecked.Value;
                ((MemorizeMathStage)this.stage).Multiplication = this.multiplyCheckBox.IsChecked.Value;
                ((MemorizeMathStage)this.stage).Division = this.divisionCheckBox.IsChecked.Value;
                ((MemorizeMathStage)this.stage).ItemCount = count;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
