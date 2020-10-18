using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SoonLearning.AppCenter.Controls
{
    [TemplatePart(Name = "PART_Title", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Index", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Spliter", Type = typeof(TextBlock))]
    [TemplatePart(Name = "PART_Total", Type = typeof(TextBlock))]
    public class GadgetUserControl : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(
                "Title", typeof(string), typeof(GadgetUserControl),
                new FrameworkPropertyMetadata(string.Empty));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, string.Empty); }
        }

        public static readonly DependencyProperty IndexProperty =
           DependencyProperty.Register(
               "Index", typeof(int), typeof(GadgetUserControl),
               new FrameworkPropertyMetadata(0));

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public static readonly DependencyProperty SpliterProperty =
           DependencyProperty.Register(
               "Spliter", typeof(string), typeof(GadgetUserControl),
               new FrameworkPropertyMetadata(string.Empty));

        public string Spliter
        {
            get { return (string)GetValue(SpliterProperty); }
            set { SetValue(SpliterProperty, value); }
        }

        public static readonly DependencyProperty TotalProperty =
           DependencyProperty.Register(
               "Total", typeof(int), typeof(GadgetUserControl),
               new FrameworkPropertyMetadata(0));

        public int Total
        {
            get { return (int)GetValue(TotalProperty); }
            set { SetValue(TotalProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
