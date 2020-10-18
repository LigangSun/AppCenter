using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SoonLearning.AppCenter.Controls
{
    public class ListBoxEx : ListBox
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ListBoxExItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ListBoxExItem;
        }
    }

    public class ListBoxExItem : ListBoxItem
    {
        private Selector ParentSelector
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as Selector; }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            ParentSelector.ReleaseMouseCapture();
        }
    } 

}
