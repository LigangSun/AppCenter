using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    public class QuestionGenerator5 : BaseQuestionGenerator
    {
        public override void Generate(Grid rootGrid)
        {
            base.row = 5;
            base.col = 1;
            base.Generate(rootGrid);
        }

        protected override void AppendQuestionControl(System.Windows.Controls.Grid rootGrid)
        {
            for (int i = 0; i < 5; i++)
            {
                Control ctrl = CreateQuestionControl(32f, FontWeights.Medium);
                ctrl.Margin = new Thickness(8);
                Grid.SetRow(ctrl, i);
                Grid.SetColumn(ctrl, 0);
                rootGrid.Children.Add(ctrl);
            }
        }

        protected override float GetFontSize()
        {
            return 32f;
        }
    }
}
