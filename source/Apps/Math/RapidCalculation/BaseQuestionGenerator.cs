using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using SoonLearning.Math.Data;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    public abstract class BaseQuestionGenerator
    {
        protected int row = 1;
        protected int col = 1;
        protected double abColumeWidth = 260;

        public virtual void Generate(Grid rootGrid)
        {
            string maxNumber = MathSetting.Instance.CurrentQuestionData.MaxResultNumber.ToString();

            TextBlock testTextWidthLabel = new TextBlock();
            testTextWidthLabel.Text = maxNumber;
            testTextWidthLabel.FontSize = this.GetFontSize();
            testTextWidthLabel.FontWeight = FontWeights.Medium;
            testTextWidthLabel.HorizontalAlignment = HorizontalAlignment.Center;
            rootGrid.Children.Add(testTextWidthLabel);
            rootGrid.UpdateLayout();
            this.abColumeWidth = testTextWidthLabel.ActualWidth + 10;
            rootGrid.Children.Remove(testTextWidthLabel);

            for (int i = 0; i < row; i++)
            {
                RowDefinition rowDef = new RowDefinition();
                rowDef.Height = new System.Windows.GridLength(1.0f, System.Windows.GridUnitType.Star);
                rootGrid.RowDefinitions.Add(rowDef);
            }

            for (int i = 0; i < col; i++)
            {
                ColumnDefinition colDef = new ColumnDefinition();
                colDef.Width = new System.Windows.GridLength(1.0f, System.Windows.GridUnitType.Star);
                rootGrid.ColumnDefinitions.Add(colDef);
            }

            this.AppendQuestionControl(rootGrid);
        }

        protected abstract void AppendQuestionControl(Grid rootGrid);

        protected abstract float GetFontSize();

        protected virtual Control CreateQuestionControl(float fontSize, FontWeight weight)
        {
            Question_a_b_c_Control abcCtrl = new Question_a_b_c_Control(fontSize, weight, this.abColumeWidth);
            abcCtrl.VerticalAlignment = VerticalAlignment.Center;
            abcCtrl.HorizontalAlignment = HorizontalAlignment.Center;
            return abcCtrl;
        }
    }
}
