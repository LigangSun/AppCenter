using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Diagnostics;
using System.Windows.Input;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.Controls
{
    public class GadgetControlPanel : Panel, IScrollInfo
    {
        #region Members

        private static Size InfiniteSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
        private const double LineSize = 132;
        private const double WheelSize = 3 * LineSize;

        private bool _CanHorizontallyScroll;
        private bool _CanVerticallyScroll;
        private ScrollViewer _ScrollOwner;
        private Vector _Offset;
        private Size _Extent;
        private Size _Viewport;

        private UIElement preSelectedItem = null;

        private Dictionary<UIElement, Timeline> elementTimelineDictionary = new Dictionary<UIElement, Timeline>();

        private MouseClickManager mouseClickMgr = new MouseClickManager(200);

        private bool selectedEventRaised;

        #endregion

        #region Properties

        public double SelectedItemLeftMargin
        {
            get;
            set;
        }

        public double ItemTopMargin
        {
            get;
            set;
        }

        public static readonly RoutedEvent SelectionChangedEvent = EventManager.RegisterRoutedEvent(
            "SelectionChanged", RoutingStrategy.Bubble,
            typeof(SelectionChangedEventHandler), typeof(GadgetControlPanel));

        public event SelectionChangedEventHandler SelectionChanged
        {
            add { AddHandler(SelectionChangedEvent, value); }
            remove { RemoveHandler(SelectionChangedEvent, value); }
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.RegisterAttached(
                            "Orientation",
                            typeof(Orientation),
                            typeof(GadgetControlPanel),
                            new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public static readonly DependencyProperty ItemAnimationDurationProperty = DependencyProperty.RegisterAttached(
                            "AnimationDuration",
                            typeof(double),
                            typeof(GadgetControlPanel),
                            new PropertyMetadata(200.0));

        public double AnimationDuration
        {
            get { return (double)GetValue(ItemAnimationDurationProperty); }
            set { SetValue(ItemAnimationDurationProperty, value); }
        }

        public static readonly DependencyProperty ItemDefaultStyleProperty = DependencyProperty.RegisterAttached(
                            "ItemDefaultStyle",
                            typeof(Style),
                            typeof(GadgetControlPanel),
                            new PropertyMetadata(null));

        public Style ItemDefaultStyle
        {
            get { return (Style)GetValue(ItemDefaultStyleProperty); }
            set { SetValue(ItemDefaultStyleProperty, value); }
        }

        public static readonly DependencyProperty CircleItemsProperty = DependencyProperty.RegisterAttached(
                            "CircleItems",
                            typeof(bool),
                            typeof(GadgetControlPanel),
                            new PropertyMetadata(false));

        public bool CircleItems
        {
            get { return (bool)GetValue(CircleItemsProperty); }
            set { SetValue(CircleItemsProperty, value); }
        }

        public static readonly DependencyProperty SpaceAfterLastItemProperty = DependencyProperty.RegisterAttached(
                            "SpaceAfterLastItem",
                            typeof(double),
                            typeof(GadgetControlPanel),
                            new PropertyMetadata(0.0));

        public double SpaceAfterLastItem
        {
            get { return (double)GetValue(SpaceAfterLastItemProperty); }
            set { SetValue(SpaceAfterLastItemProperty, value); }
        }

        public static readonly DependencyProperty SpaceBetweenItemsProperty = DependencyProperty.RegisterAttached(
                            "SpaceBetweenItems",
                            typeof(double),
                            typeof(GadgetControlPanel),
                            new PropertyMetadata(0.0));

        public double SpaceBetweenItems
        {
            get { return (double)GetValue(SpaceBetweenItemsProperty); }
            set { SetValue(SpaceBetweenItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.RegisterAttached(
                            "SelectedIndex",
                            typeof(int),
                            typeof(GadgetControlPanel),
                            new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedIndexChanged)));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set
            {
                SetValue(SelectedIndexProperty, value);
                this.InvalidateMeasure();
            }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached(
                           "SelectedItem",
                           typeof(UIElement),
                           typeof(GadgetControlPanel),
                           new PropertyMetadata(null, new PropertyChangedCallback(OnSelectedItemChanged)));

        public UIElement SelectedItem
        {
            get
            {
                return (UIElement)GetValue(SelectedItemProperty); 
            }
            set
            { 
                SetValue(SelectedItemProperty, value);
                this.InvalidateMeasure();
            }
        }

        #endregion

        #region Constructor

        public GadgetControlPanel()
        {
            this.mouseClickMgr.Click += new MouseButtonEventHandler(mouseClickMgr_Click);
            this.mouseClickMgr.DoubleClick += new MouseButtonEventHandler(mouseClickMgr_DoubleClick);

            this.Loaded += new RoutedEventHandler(NavigationPanel_Loaded);

            this.Focusable = true;
            this.FocusVisualStyle = null;
        }

        #endregion

        private void NavigationPanel_Loaded(object sender, RoutedEventArgs e)
        {
            this.selectedEventRaised = false;
        }

        private void mouseClickMgr_Click(object sender, MouseButtonEventArgs e)
        {
            this.SelectItem(e.Source as UIElement);
        }

        private void mouseClickMgr_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.SelectItem(e.Source as UIElement))
                this.RaiseSelectionChangedEvent();
        }

        protected override void OnMouseLeftButtonUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.Source is UIElement)
            {
                this.mouseClickMgr.HandleClick(this, e);
            }

            base.OnMouseLeftButtonUp(e);
        }

        private bool SelectItem(UIElement element)
        {
            int currentIndex = -1;
            int index = -1;
            foreach (UIElement child in this.Children)
            {
                index++;
                if (UIHelper.FindChild<UIElement>(child, element) != null)
                {
                    currentIndex = index;
                    break;
                }
            }
            int deltaIndex = currentIndex - this.SelectedIndex;
            if (deltaIndex == 0 && currentIndex != -1)
            {
                this.RaiseSelectionChangedEvent();
                return false;
            }

            this.preSelectedItem = this.SelectedItem;
            this.SelectedIndex = currentIndex;

            return true;
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Delta > 0) // Move right
            {
                this.OnMoveRight(this, null);
                e.Handled = true;
            }
            else // Move left
            {
                this.OnMoveLeft(this, null);
                e.Handled = true;
            }
            base.OnMouseWheel(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    OnMoveLeft(this, null);
                    e.Handled = true;
                    break;
                case Key.Right:
                    OnMoveRight(this, null);
                    e.Handled = true;
                    break;
                case Key.Enter:
                    OnSelected(this, null);
                    e.Handled = true;
                    break;
            }

            base.OnPreviewKeyDown(e);
        }

        private void OnMoveLeft(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.SelectedIndex > 0)
            {
                --this.SelectedIndex;
            }
        }

        private void OnMoveRight(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.SelectedIndex < this.Children.Count - 1)
            {
                ++this.SelectedIndex;
            }
        }

        private void OnSelected(object sender, ExecutedRoutedEventArgs e)
        {
            this.RaiseSelectionChangedEvent();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            double curX = SelectedItemLeftMargin, curY = ItemTopMargin, curLineHeight = 0, maxLineWidth = 0;
            foreach (UIElement child in this.Children)
            {
                if (child is FrameworkElement)
                {
                    FrameworkElement frameworkElement = child as FrameworkElement;
                    if (frameworkElement.Style == null)
                        frameworkElement.Style = this.ItemDefaultStyle;
                }

                child.Measure(InfiniteSize);

                //if (curX + child.DesiredSize.Width > availableSize.Width)
                //{ //Wrap to next line
                //    curY += curLineHeight;
                //    curX = 0;
                //    curLineHeight = 0;
                //}

                curX += child.DesiredSize.Width;
                if (child.DesiredSize.Height > curLineHeight)
                {
                    curLineHeight = child.DesiredSize.Height; 
                }

                if (curX > maxLineWidth)
                {
                    maxLineWidth = curX;
                }
            }

            curY += curLineHeight;

       //     VerifyScrollData(availableSize, new Size(maxLineWidth, curY));

            _Viewport = new Size(curX, curY);
            return _Viewport;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (this.Children == null || this.Children.Count == 0)
            {
                return finalSize;
            }

            TranslateTransform trans = null;

            double curX = 0;
            double curY = ItemTopMargin;
           
            double delta = SelectedItemLeftMargin;
            bool init = false;

            TimeSpan animationLength = TimeSpan.FromMilliseconds(this.AnimationDuration);

            int index = 0;
            foreach (ListBoxItem child in this.Children)
            {
                child.Arrange(new Rect(0, 0, child.DesiredSize.Width, child.DesiredSize.Height));

                trans = child.RenderTransform as TranslateTransform;

                if (trans == null)
                {
                    child.RenderTransformOrigin = new Point(0, 0);
                    trans = new TranslateTransform();
                    child.RenderTransform = trans;

                    double toX = curX + delta - (this.SelectedIndex * child.ActualWidth);
                    trans.X = toX;
                    trans.Y = curY - VerticalOffset;

                    trans.BeginAnimation(TranslateTransform.XProperty,
                          new DoubleAnimation(toX, animationLength),
                          HandoffBehavior.Compose);
                    trans.BeginAnimation(TranslateTransform.YProperty,
                          new DoubleAnimation(curY - VerticalOffset, animationLength),
                          HandoffBehavior.Compose);

                    init = true;
                }
                else
                {
                    //if (this.elementTimelineDictionary.ContainsKey(child))
                    //{
                    //    DoubleAnimationUsingKeyFrames frames = this.elementTimelineDictionary[child] as DoubleAnimationUsingKeyFrames;
                    //    trans.X = frames.KeyFrames[frames.KeyFrames.Count - 1].Value;
                    //    //     frames.SpeedRatio = 100;

                    //    //     Thread.Sleep(1);

                    //    //     trans.BeginAnimation(TranslateTransform.YProperty, null, HandoffBehavior.Compose);
                    //}
                }

                curX += child.DesiredSize.Width;
                finalSize.Height = child.DesiredSize.Height;
            }

            if (init)
                return finalSize;

            UIElement selectedItem = this.SelectedItem;
            if (selectedItem != null)
            {
                TranslateTransform selectedItemTrans = selectedItem.RenderTransform as TranslateTransform;
                delta = SelectedItemLeftMargin - selectedItemTrans.X;
                if (this.elementTimelineDictionary.ContainsKey(selectedItem))
                {
                    if (this.elementTimelineDictionary[selectedItem] is DoubleAnimationUsingKeyFrames)
                    {
                        DoubleAnimationUsingKeyFrames frames = this.elementTimelineDictionary[selectedItem] as DoubleAnimationUsingKeyFrames;
                        if (frames.KeyFrames.Count > 0)
                            delta = SelectedItemLeftMargin - frames.KeyFrames[frames.KeyFrames.Count - 1].Value;
                    }
                    else
                    {
                        DoubleAnimation doubleAnimation = this.elementTimelineDictionary[selectedItem] as DoubleAnimation;
                        if (doubleAnimation != null)
                            delta = SelectedItemLeftMargin - doubleAnimation.To.Value;
                    }
                }
            }

            if (delta == 0)
            {
                return finalSize;
            }

            index = 0;
            if (this.CircleItems)
            {
                double lastRight = this.SpaceAfterLastItem;
                TranslateTransform lastTrans = this.Children[this.Children.Count - 1].RenderTransform as TranslateTransform;
                lastRight = lastTrans.X + this.Children[this.Children.Count - 1].DesiredSize.Width;

                if (this.elementTimelineDictionary.ContainsKey(this.Children[this.Children.Count - 1]))
                {
                    DoubleAnimationUsingKeyFrames frames = this.elementTimelineDictionary[this.Children[this.Children.Count - 1]] as DoubleAnimationUsingKeyFrames;
                    if (frames.KeyFrames.Count > 0)
                        lastRight = frames.KeyFrames[frames.KeyFrames.Count - 1].Value + this.Children[this.Children.Count - 1].DesiredSize.Width;
                }

                double firstLeft = this.SelectedItemLeftMargin;
                TranslateTransform firstTrans = this.Children[0].RenderTransform as TranslateTransform;
                firstLeft = firstTrans.X;

                if (this.elementTimelineDictionary.ContainsKey(this.Children[0]))
                {
                    DoubleAnimationUsingKeyFrames frames = this.elementTimelineDictionary[this.Children[0]] as DoubleAnimationUsingKeyFrames;
                    if (frames.KeyFrames.Count > 0)
                        firstLeft = frames.KeyFrames[frames.KeyFrames.Count - 1].Value;
                }

                double startX = lastRight + this.SpaceAfterLastItem;
                double endX = firstLeft - this.SpaceAfterLastItem;

                if (delta < 0) // forward
                {
                    foreach (UIElement child in this.Children)
                    {
                        trans = child.RenderTransform as TranslateTransform;

                        double fromX = trans.X;
                        Debug.WriteLine(string.Format("Start fromX left {0}", fromX));
                        if (this.elementTimelineDictionary.ContainsKey(child))
                        {
                            DoubleAnimationUsingKeyFrames frames = this.elementTimelineDictionary[child] as DoubleAnimationUsingKeyFrames;
                            fromX = frames.KeyFrames[frames.KeyFrames.Count - 1].Value;

                            Debug.WriteLine(string.Format("End fromX left {0}", fromX));
                        }

                        double toX = fromX + delta;

                        DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
                        doubleAnimationUsingKeyFrames.Duration = new Duration(animationLength + TimeSpan.FromMilliseconds(index * 10));
                        doubleAnimationUsingKeyFrames.RepeatBehavior = new RepeatBehavior(1);
                        doubleAnimationUsingKeyFrames.AutoReverse = false;
                        doubleAnimationUsingKeyFrames.FillBehavior = FillBehavior.HoldEnd;

                        if (child == this.SelectedItem)
                            doubleAnimationUsingKeyFrames.Completed += new EventHandler(doubleAnimationUsingKeyFrames_Completed);

                        if (!this.elementTimelineDictionary.ContainsKey(child))
                        {
                            this.elementTimelineDictionary.Add(child, doubleAnimationUsingKeyFrames);
                        }
                        else
                        {
                            this.elementTimelineDictionary[child] = doubleAnimationUsingKeyFrames;
                        }

                        LinearDoubleKeyFrame frame1 = new LinearDoubleKeyFrame();
                        frame1.Value = toX;
                        doubleAnimationUsingKeyFrames.KeyFrames.Add(frame1);

                        if (fromX > lastRight)
                        {
                            startX += child.DesiredSize.Width;
                        }

                        if (toX + child.DesiredSize.Width <= 0) // Move item to end
                        {
                            double fromBK = fromX;
                            if (fromX > lastRight)
                                startX -= child.DesiredSize.Width;

                            fromX = startX;
                            toX = startX + delta;
                            startX += child.DesiredSize.Width;

                            DiscreteDoubleKeyFrame frame2 = new DiscreteDoubleKeyFrame();
                            frame2.Value = fromX;
                            doubleAnimationUsingKeyFrames.KeyFrames.Add(frame2);

                            LinearDoubleKeyFrame frame3 = new LinearDoubleKeyFrame();
                            frame3.Value = toX;
                            doubleAnimationUsingKeyFrames.KeyFrames.Add(frame3);
                        }

                        if (doubleAnimationUsingKeyFrames.KeyFrames.Count == 3)
                        {
                            doubleAnimationUsingKeyFrames.KeyFrames[0].KeyTime = KeyTime.FromPercent(0.5);
                            doubleAnimationUsingKeyFrames.KeyFrames[1].KeyTime = KeyTime.FromPercent(0.5);
                            doubleAnimationUsingKeyFrames.KeyFrames[2].KeyTime = KeyTime.FromPercent(1.0);
                        }
                        else
                        {
                            doubleAnimationUsingKeyFrames.KeyFrames[0].KeyTime = KeyTime.FromPercent(1.0);
                        }

                        trans.BeginAnimation(TranslateTransform.XProperty,
                              doubleAnimationUsingKeyFrames,
                              HandoffBehavior.Compose);
                        trans.BeginAnimation(TranslateTransform.YProperty,
                              new DoubleAnimation(curY - VerticalOffset, animationLength),
                              HandoffBehavior.Compose);

                        index++;
                    }
                }
                else
                {
                    index = this.Children.Count;
                    for (int i = this.Children.Count - 1; i >= 0; i--)
                    {
                        UIElement child = this.Children[i];

                        trans = child.RenderTransform as TranslateTransform;

                        double fromX = trans.X;
                        if (this.elementTimelineDictionary.ContainsKey(child))
                        {
                            DoubleAnimationUsingKeyFrames frames = this.elementTimelineDictionary[child] as DoubleAnimationUsingKeyFrames;
                            fromX = frames.KeyFrames[frames.KeyFrames.Count - 1].Value;
                        }

                        double toX = fromX + delta;

                        DoubleAnimationUsingKeyFrames doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames();
                        doubleAnimationUsingKeyFrames.Duration = new Duration(animationLength + TimeSpan.FromMilliseconds(index * 10));
                        doubleAnimationUsingKeyFrames.RepeatBehavior = new RepeatBehavior(1);
                        doubleAnimationUsingKeyFrames.AutoReverse = false;
                        doubleAnimationUsingKeyFrames.FillBehavior = FillBehavior.HoldEnd;

                        if (i == this.SelectedIndex)
                            doubleAnimationUsingKeyFrames.Completed += new EventHandler(doubleAnimationUsingKeyFrames_Completed);

                        if (!this.elementTimelineDictionary.ContainsKey(child))
                        {
                            this.elementTimelineDictionary.Add(child, doubleAnimationUsingKeyFrames);
                        }
                        else
                        {
                            this.elementTimelineDictionary[child] = doubleAnimationUsingKeyFrames;
                        }

                        LinearDoubleKeyFrame frame1 = new LinearDoubleKeyFrame();
                        frame1.Value = toX;
                        doubleAnimationUsingKeyFrames.KeyFrames.Add(frame1);

                        if (fromX < firstLeft)
                            endX -= child.DesiredSize.Width;

                        if (toX > finalSize.Width) // Move item to front
                        {
                            double fromBK = fromX;
                            if (fromX < firstLeft)
                                endX += child.DesiredSize.Width;

                            fromX = endX - delta;
                            toX = endX;
                            endX -= child.DesiredSize.Width;

                            DiscreteDoubleKeyFrame frame2 = new DiscreteDoubleKeyFrame();
                            frame2.Value = fromX;
                            doubleAnimationUsingKeyFrames.KeyFrames.Add(frame2);

                            LinearDoubleKeyFrame frame3 = new LinearDoubleKeyFrame();
                            frame3.Value = toX;
                            doubleAnimationUsingKeyFrames.KeyFrames.Add(frame3);
                        }

                        if (doubleAnimationUsingKeyFrames.KeyFrames.Count == 3)
                        {
                            doubleAnimationUsingKeyFrames.KeyFrames[0].KeyTime = KeyTime.FromPercent(0.5);
                            doubleAnimationUsingKeyFrames.KeyFrames[1].KeyTime = KeyTime.FromPercent(0.5);
                            doubleAnimationUsingKeyFrames.KeyFrames[2].KeyTime = KeyTime.FromPercent(1.0);
                        }
                        else
                        {
                            doubleAnimationUsingKeyFrames.KeyFrames[0].KeyTime = KeyTime.FromPercent(1.0);
                        }

                        trans.BeginAnimation(TranslateTransform.XProperty,
                              doubleAnimationUsingKeyFrames,
                              HandoffBehavior.Compose);
                        trans.BeginAnimation(TranslateTransform.YProperty,
                              new DoubleAnimation(curY - VerticalOffset, animationLength),
                              HandoffBehavior.Compose);

                        index--;
                    }
                }
            }
            else
            {
                foreach (UIElement child in this.Children)
                {
                    trans = child.RenderTransform as TranslateTransform;

                    double fromX = trans.X;
                    if (this.elementTimelineDictionary.ContainsKey(child))
                    {
                        DoubleAnimation old = this.elementTimelineDictionary[child] as DoubleAnimation;
                        fromX = old.To.Value;
                    }
                    double toX = fromX + delta;

                    DoubleAnimation doubleAnimation = new DoubleAnimation();
                    doubleAnimation.From = fromX;
                    doubleAnimation.To = toX;
                    doubleAnimation.Duration = animationLength;

                    trans.BeginAnimation(TranslateTransform.XProperty,
                          doubleAnimation,
                          HandoffBehavior.Compose);
                    trans.BeginAnimation(TranslateTransform.YProperty,
                          new DoubleAnimation(curY - VerticalOffset, animationLength),
                          HandoffBehavior.Compose);

                    if (!this.elementTimelineDictionary.ContainsKey(child))
                    {
                        this.elementTimelineDictionary.Add(child, doubleAnimation);
                    }
                    else
                    {
                        this.elementTimelineDictionary[child] = doubleAnimation;
                    }

                    index++;
                }
            }

            return finalSize;
        }

        private void doubleAnimationUsingKeyFrames_Completed(object sender, EventArgs e)
        {
        //    this.RaiseSelectionChangedEvent();
        }

        private void RaiseSelectionChangedEvent()
        {
        //    if (this.selectedEventRaised)
        //        return;

            this.selectedEventRaised = true;
            this.RaiseEvent(new SelectionChangedEventArgs(SelectionChangedEvent, new List<UIElement>(), new List<UIElement>()));
        }

        #region Movement Methods
        public void LineDown()
        { SetVerticalOffset(VerticalOffset + LineSize); }

        public void LineUp()
        { SetVerticalOffset(VerticalOffset - LineSize); }

        public void LineLeft()
        { SetHorizontalOffset(HorizontalOffset - LineSize); }

        public void LineRight()
        { SetHorizontalOffset(HorizontalOffset + LineSize); }

        public void MouseWheelDown()
        { SetVerticalOffset(VerticalOffset + WheelSize); }

        public void MouseWheelUp()
        { SetVerticalOffset(VerticalOffset - WheelSize); }

        public void MouseWheelLeft()
        { SetHorizontalOffset(HorizontalOffset - WheelSize); }

        public void MouseWheelRight()
        { SetHorizontalOffset(HorizontalOffset + WheelSize); }

        public void PageDown()
        { SetVerticalOffset(VerticalOffset + ViewportHeight); }

        public void PageUp()
        { SetVerticalOffset(VerticalOffset - ViewportHeight); }

        public void PageLeft()
        { SetHorizontalOffset(HorizontalOffset - ViewportWidth); }

        public void PageRight()
        { SetHorizontalOffset(HorizontalOffset + ViewportWidth); }
        #endregion

        public ScrollViewer ScrollOwner
        {
            get { return _ScrollOwner; }
            set { _ScrollOwner = value; }
        }

        public bool CanHorizontallyScroll
        {
            get { return _CanHorizontallyScroll; }
            set { _CanHorizontallyScroll = value; }
        }

        public bool CanVerticallyScroll
        {
            get { return _CanVerticallyScroll; }
            set { _CanVerticallyScroll = value; }
        }

        public double ExtentHeight
        { get { return _Extent.Height; } }

        public double ExtentWidth
        { get { return _Extent.Width; } }

        public double HorizontalOffset
        { get { return _Offset.X; } }

        public double VerticalOffset
        { get { return _Offset.Y; } }

        public double ViewportHeight
        { get { return _Viewport.Height; } }

        public double ViewportWidth
        { get { return _Viewport.Width; } }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            if (rectangle.IsEmpty || visual == null
              || visual == this || !base.IsAncestorOf(visual))
            { return Rect.Empty; }

            rectangle = visual.TransformToAncestor(this).TransformBounds(rectangle);

            Rect viewRect = new Rect(HorizontalOffset,
              VerticalOffset, ViewportWidth, ViewportHeight);
            rectangle.X += viewRect.X;
            rectangle.Y += viewRect.Y;
            viewRect.X = CalculateNewScrollOffset(viewRect.Left,
              viewRect.Right, rectangle.Left, rectangle.Right);
            viewRect.Y = CalculateNewScrollOffset(viewRect.Top,
              viewRect.Bottom, rectangle.Top, rectangle.Bottom);
            SetHorizontalOffset(viewRect.X);
            SetVerticalOffset(viewRect.Y);
            rectangle.Intersect(viewRect);
            rectangle.X -= viewRect.X;
            rectangle.Y -= viewRect.Y;

            return rectangle;
        }

        private static double CalculateNewScrollOffset(double topView,
          double bottomView, double topChild, double bottomChild)
        {
            bool offBottom = topChild < topView && bottomChild < bottomView;
            bool offTop = bottomChild > bottomView && topChild > topView;
            bool tooLarge = (bottomChild - topChild) > (bottomView - topView);

            if (!offBottom && !offTop)
            { return topView; } //Don't do anything, already in view

            if ((offBottom && !tooLarge) || (offTop && tooLarge))
            { return topChild; }

            return (bottomChild - (bottomView - topView));
        }

        protected void VerifyScrollData(Size viewport, Size extent)
        {
            if (double.IsInfinity(viewport.Width))
            { viewport.Width = extent.Width; }

            if (double.IsInfinity(viewport.Height))
            { viewport.Height = extent.Height; }

            _Extent = extent;
            _Viewport = viewport;

            _Offset.X = Math.Max(0,
              Math.Min(_Offset.X, ExtentWidth - ViewportWidth));
            _Offset.Y = Math.Max(0,
              Math.Min(_Offset.Y, ExtentHeight - ViewportHeight));

            if (ScrollOwner != null)
            { ScrollOwner.InvalidateScrollInfo(); }
        }

        public void SetHorizontalOffset(double offset)
        {
            offset = Math.Max(0,
              Math.Min(offset, ExtentWidth - ViewportWidth));
            if (offset != _Offset.X)
            {
                _Offset.X = offset;
                InvalidateArrange();
            }
        }

        public void SetVerticalOffset(double offset)
        {
            offset = Math.Max(0,
              Math.Min(offset, ExtentHeight - ViewportHeight));
            if (offset != _Offset.Y)
            {
                _Offset.Y = offset;
                InvalidateArrange();
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            GadgetControlPanel panel = obj as GadgetControlPanel;
            if (panel != null)
            {
                panel.SelectedIndexChanged((int)e.NewValue, (int)e.OldValue);
            }
        }

        private void SelectedIndexChanged(int newIndex, int oldIndex)
        {
            if (oldIndex == newIndex)
                return;

            if (newIndex < 0 || newIndex >= this.Children.Count)
                return;

            this.selectedEventRaised = false;

            UIElement element = this.Children[newIndex];
            this.SelectedItem = element;
        }

        private static void OnSelectedItemChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            GadgetControlPanel panel = obj as GadgetControlPanel;
            if (panel != null)
            {
                panel.SelectedItemChanged((UIElement)e.NewValue, (UIElement)e.OldValue);
            }
        }

        private void SelectedItemChanged(UIElement element, UIElement oldElement)
        {
            if (oldElement == element)
                return;

            int index = this.Children.IndexOf(element);
            this.SelectedIndex = index;

            this.selectedEventRaised = false;

            if (element is ListBoxItem)
            {
                ((ListBoxItem)element).IsSelected = true;
            }

            if (oldElement is ListBoxItem)
            {
                ((ListBoxItem)oldElement).IsSelected = false;
            }
        }

        private bool IsLastElement(UIElement element)
        {
            if (this.Children.IndexOf(element) == this.Children.Count - 1)
                return true;

            return false;
        }

        private bool IsFirstElement(UIElement element)
        {
            if (this.Children.IndexOf(element) == 0)
                return true;

            return false;
        }
    }
}