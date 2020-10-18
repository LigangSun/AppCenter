using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    public class QuestionGenerator10 : BaseQuestionGenerator
    {
        public override void Generate(Grid rootGrid)
        {
            base.row = 10;
            base.col = 1;
            base.Generate(rootGrid);
        }

        protected override void AppendQuestionControl(Grid rootGrid)
        {
            for (int i = 0; i < 10; i++)
            {
                Control ctrl = CreateQuestionControl(28f, FontWeights.Medium);
                ctrl.Margin = new Thickness(3);
                Grid.SetRow(ctrl, i);
                Grid.SetColumn(ctrl, 0);
                rootGrid.Children.Add(ctrl);
            }
        }

        protected override float GetFontSize()
        {
            return 28f;
        }
    }
}
