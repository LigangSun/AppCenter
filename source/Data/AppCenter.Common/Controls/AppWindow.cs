using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Win32;

namespace SoonLearning.AppCenter.Controls
{
    public class AppWindow : Window
    {
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register("BorderThickness",
            typeof(Thickness),
            typeof(AppWindow));

        public static readonly DependencyProperty ResizableProperty = DependencyProperty.Register("Resizable",
            typeof(bool),
            typeof(AppWindow));

        private WindowInteropHelper _interopHelper;

        private Rect oldRect = Rect.Empty;

        public int TitleHeight
        {
            get;
            set;
        }

        public bool Resizable
        {
            get { return (bool)GetValue(ResizableProperty); }
            set { SetValue(ResizableProperty, value); }
        }

        public int CornerRadius
        {
            get;
            set;
        }

        public new Thickness BorderThickness
        {
            get { return (Thickness)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public bool DragFullWindows
        {
            get;
            set;
        }

        public WindowState StartupState
        {
            get;
            set;
        }

        public AppWindow()
        {
            this.TitleHeight = System.Windows.Forms.SystemInformation.CaptionHeight;
            this.Resizable = true;
            this.CornerRadius = 6;
            this.DragFullWindows = false;

            this._interopHelper = new WindowInteropHelper(this);

            this.Loaded += new RoutedEventHandler(NetGUIWindow_Loaded);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

       //     this.MaxHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
       //     this.MaxWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
        }

        private void NetGUIWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - this.ActualWidth) / 2;
            this.Top = (System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - this.ActualHeight) / 2;
            this.oldRect = new Rect(this.Left, this.Top, this.ActualWidth, this.ActualWidth);

            this.WindowState = this.StartupState;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            PointInWindowPart ptPos = this.FindPointPossition(e.MouseDevice.GetPosition(this));
            if (ptPos == PointInWindowPart.InTitle ||
                this.DragFullWindows && (ptPos == PointInWindowPart.None))
            {
                this.DragMove();
            }
            else if (ptPos != PointInWindowPart.None && this.Resizable)
            {
                Win32API.SendMessage(this._interopHelper.Handle, Win32API.WM_SYSCOMMAND, (int)(Win32API.SC_SIZE + ptPos), 0);
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.FindPointPossition(e.MouseDevice.GetPosition(this));
            base.OnMouseMove(e);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (this.Resizable)
            {
                if (this.DragFullWindows)
                {
                    this.SwitchWindowState();
                }
                else
                {
                    Rect titleRect = new Rect(this.CornerRadius,
                        this.BorderThickness.Top,
                        this.ActualWidth - this.CornerRadius * 2,
                        this.TitleHeight - this.BorderThickness.Top);
                    if (PtInRect(e.MouseDevice.GetPosition(this), titleRect))
                    {
                        this.SwitchWindowState();
                    }
                }
            }

            base.OnMouseDoubleClick(e);
        }

        public virtual void SwitchWindowState()
        {
            if (this.WindowState == System.Windows.WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
        }

        private PointInWindowPart FindPointPossition(Point pt)
        {
            if (this.ResizeMode != ResizeMode.NoResize &&
                this.ResizeMode != ResizeMode.CanMinimize)
                return PointInWindowPart.None;

            if (this.WindowState == System.Windows.WindowState.Maximized)
                return PointInWindowPart.None;

            Rect titleRect = new Rect(this.CornerRadius,
                this.BorderThickness.Top,
                this.ActualWidth - this.CornerRadius * 2,
                this.TitleHeight - this.BorderThickness.Top);
            Rect leftBorderRect = new Rect(0,
                this.CornerRadius,
                this.BorderThickness.Left,
                this.ActualHeight - this.CornerRadius * 2);
            Rect rightBorderRect = new Rect(this.ActualWidth - this.BorderThickness.Right,
                this.CornerRadius,
                this.BorderThickness.Right,
                this.ActualHeight - this.CornerRadius * 2);
            Rect topBorderRect = new Rect(this.CornerRadius,
                0,
                this.ActualWidth - this.CornerRadius * 2,
                this.BorderThickness.Top);
            Rect botttomBorderRect = new Rect(this.CornerRadius,
                this.ActualHeight - this.BorderThickness.Bottom,
                this.ActualWidth - this.CornerRadius * 2,
                this.BorderThickness.Bottom);
            Rect topLeftBorderRect = new Rect(0,
                0,
                this.CornerRadius,
                this.CornerRadius);
            Rect topRightBorderRect = new Rect(this.ActualWidth - this.CornerRadius,
                0,
                this.CornerRadius,
                this.CornerRadius);
            Rect bottomLeftBorderRect = new Rect(0,
                this.ActualHeight - this.CornerRadius,
                this.CornerRadius,
                this.CornerRadius);
            Rect bottomRightBorderRect = new Rect(this.ActualWidth - this.CornerRadius,
                this.ActualHeight - this.CornerRadius,
                this.CornerRadius,
                this.CornerRadius);

            PointInWindowPart ptPos = PointInWindowPart.None;
            if (this.PtInRect(pt, titleRect))
            {
                ptPos = PointInWindowPart.InTitle;
                this.Cursor = Cursors.Arrow;
            }
            else if (this.PtInRect(pt, leftBorderRect))
            {
                ptPos = PointInWindowPart.Left;
                if (this.Resizable)
                    this.Cursor = Cursors.SizeWE;
            }
            else if (this.PtInRect(pt, rightBorderRect))
            {
                ptPos = PointInWindowPart.Right;
                if (this.Resizable)
                    this.Cursor = Cursors.SizeWE;
            }
            else if (this.PtInRect(pt, topBorderRect))
            {
                ptPos = PointInWindowPart.Top;
                if (this.Resizable)
                    this.Cursor = Cursors.SizeNS;
            }
            else if (this.PtInRect(pt, botttomBorderRect))
            {
                ptPos = PointInWindowPart.Bottom;
                if (this.Resizable)
                    this.Cursor = Cursors.SizeNS;
            }
            else if (this.PtInRect(pt, topLeftBorderRect))
            {
                ptPos = PointInWindowPart.TopLeft;
                if (this.Resizable)
                    this.Cursor = Cursors.SizeNWSE;
            }
            else if (this.PtInRect(pt, topRightBorderRect))
            {
                ptPos = PointInWindowPart.TopRight;
                if (this.Resizable)
                    this.Cursor = Cursors.SizeNESW;
            }
            else if (this.PtInRect(pt, bottomLeftBorderRect))
            {
                ptPos = PointInWindowPart.BottomLeft;
                if (this.Resizable)
                    this.Cursor = Cursors.SizeNESW;
            }
            else if (this.PtInRect(pt, bottomRightBorderRect))
            {
                ptPos = PointInWindowPart.BottomRight;
                if (this.Resizable)
                    this.Cursor = Cursors.SizeNWSE;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
            }

            Debug.WriteLine(ptPos);

            return ptPos;
        }

        private bool PtInRect(Point pt, Rect rc)
        {
            if (pt.X >= rc.Left && pt.X < rc.Right &&
                pt.Y >= rc.Top && pt.Y < rc.Bottom)
                return true;

            return false;
        }
    }
}
