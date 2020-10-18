using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    public class QuestionGenerator3 : BaseQuestionGenerator
    {
        public override void Generate(Grid rootGrid)
        {
            base.row = 3;
            base.col = 1;
            base.Generate(rootGrid);
        }

        protected override void AppendQuestionControl(System.Windows.Controls.Grid rootGrid)
        {
            for (int i = 0; i < 3; i++)
            {
                Control ctrl = CreateQuestionControl(36f, FontWeights.Medium);
                ctrl.Margin = new Thickness(5);
                Grid.SetRow(ctrl, i);
                Grid.SetColumn(ctrl, 0);
                rootGrid.Children.Add(ctrl);
            }
        }

        protected override float GetFontSize()
        {
            return 36f;
        }
    }
}
