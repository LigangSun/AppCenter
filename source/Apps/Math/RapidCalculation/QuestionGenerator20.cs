using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    public class QuestionGenerator20 : BaseQuestionGenerator
    {
        public override void Generate(Grid rootGrid)
        {
            base.row = 10;
            base.col = 2;
            base.Generate(rootGrid);
        }

        protected override void AppendQuestionControl(System.Windows.Controls.Grid rootGrid)
        {
            for (int i = 0; i < 10; i++)
            {
                UIElement ctrl = CreateQuestionControl(24f, FontWeights.Medium);
                Grid.SetRow(ctrl, i);
                Grid.SetColumn(ctrl, 0);
                rootGrid.Children.Add(ctrl);

                UIElement ctrl1 = CreateQuestionControl(24f, FontWeights.Medium);
                Grid.SetRow(ctrl1, i);
                Grid.SetColumn(ctrl1, 1);
                rootGrid.Children.Add(ctrl1);
            }
        }

        protected override float GetFontSize()
        {
            return 24f;
        }
    }
}
