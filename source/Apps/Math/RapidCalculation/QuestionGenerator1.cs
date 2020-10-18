using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using SoonLearning.Math.Data;

namespace SoonLearning.Math.Fast.RapidCalculation
{
    public class QuestionGenerator1 : BaseQuestionGenerator
    {
        protected override void AppendQuestionControl(Grid rootGrid)
        {
            Control ctrl = CreateQuestionControl(48f, FontWeights.Medium);
            Grid.SetRow(ctrl, 0);
            Grid.SetColumn(ctrl, 0);
            rootGrid.Children.Add(ctrl);
        }

        protected override float GetFontSize()
        {
            return 48f;
        }
    }
}
