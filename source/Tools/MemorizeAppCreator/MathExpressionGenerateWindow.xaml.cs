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
    /// Interaction logic for MathExpressionGenerateWindow.xaml
    /// </summary>
    public partial class MathExpressionGenerateWindow : Window
    {
        private List<MemorizeItem> itemList = new List<MemorizeItem>();

        public List<MemorizeItem> Items
        {
            get { return this.itemList; }
        }

        public MathExpressionGenerateWindow()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            int minValue;
            int maxValue;
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

            if (maxValue - minValue <= 2)
            {
                MessageBox.Show("取值范围不合法!", "生成算式", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (this.addCheckBox.IsChecked.Value)
                this.generatePlusExpression(minValue, maxValue);

            if (this.minusCheckBox.IsChecked.Value)
                this.generateMinusExpression(minValue, maxValue);

            if (this.multiplyCheckBox.IsChecked.Value)
                this.generateMultiplyExpression(minValue, maxValue);

            if (this.divisionCheckBox.IsChecked.Value)
                this.generateDivisionExpression(minValue, maxValue);

            this.DialogResult = true;
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void generatePlusExpression(int minValue, int maxValue)
        {
            for (int i = minValue; i <= maxValue; i++)
            {
                int loop = i / 2 + 1;
                for (int j = 0; j < loop; j++)
                {
                    int a = j;
                    int b = i - j;
                    this.createMemorizeItem(string.Format("{0}+{1}", a, b), i.ToString());
                    if (a != b)
                    {
                        this.createMemorizeItem(string.Format("{0}+{1}", b, a), i.ToString());
                    }
                }
            }
        }

        private void generateMinusExpression(int minValue, int maxValue)
        {
            for (int i = minValue; i <= maxValue; i++)
            {
                int loop = i / 2 + 1;
                for (int j = 0; j < loop; j++)
                {
                    int a = j;
                    int b = i - j;
                    this.createMemorizeItem(string.Format("{0}-{1}", i, a), b.ToString());
                    if (a != b)
                    {
                        this.createMemorizeItem(string.Format("{0}-{1}", i, b), a.ToString());
                    }
                }
            }
        }

        private void generateMultiplyExpression(int minValue, int maxValue)
        {
            for (int i = minValue; i <= maxValue; i++)
            {
                for (int j = minValue; j <= maxValue; j++)
                {
                    int a = i;
                    int b = j;
                    this.createMemorizeItem(string.Format("{0}x{1}", a, b), (a*b).ToString());
                }
            }
        }

        private void generateDivisionExpression(int minValue, int maxValue)
        {
            for (int i = minValue; i <= maxValue; i++)
            {
                for (int j = minValue; j <= maxValue; j++)
                {
                    int a = i;
                    int b = j;
                    this.createMemorizeItem(string.Format("{0}/{1}", (a * b), b), a.ToString());
                }
            }
        }

        private void createMemorizeItem(string left, string right)
        {
            MemorizeItem item = new MemorizeItem();

            MemorizeText leftText = new MemorizeText(left);
            MemorizeText rightText = new MemorizeText(right);
            item.ObjectA = leftText;
            item.ObjectB = rightText;

            itemList.Add(item);
        }
    }
}
