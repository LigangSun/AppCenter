using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows;

namespace SoonLearning.AppCenter.Controls
{
    public enum NavigationType
    {
        Opacity,
        MoveX,
        MoveY,
        MoveXY,
        MoveCross,
        MoveExpand
    }

    public class NavigationPanel : ContentControl
    {
        public EventHandler<EventArgs> MoveInCompletedEvent;
        public EventHandler<EventArgs> MoveOutCompletedEvent;
        public EventHandler<EventArgs> BackMoveInCompletedEvent;
        public EventHandler<EventArgs> BackMoveOutCompletedEvent;

        private object newObject;
        private NavigationType navigationType;

        private Stack<object> navigationStack = new Stack<object>();

        public new object Content
        {
            get { return base.Content; }
            set
            {
                navigationStack.Push(base.Content);
                base.Content = value;
                return;
                this.newObject = value;
                if (value == null)
                {
                    base.Content = null;
                    return;
                }

                if (base.Content == null)
                {
                    base.Content = value;
                    this.MoveIn();
                }
                else
                {
                    navigationStack.Push(base.Content);
                    this.MoveOut();
                }
            }
        }

        public NavigationType NavigationType
        {
            set
            {
                this.navigationType = value;
                switch (value)
                {
                    case Controls.NavigationType.MoveX:
                    case Controls.NavigationType.MoveY:
                    case Controls.NavigationType.MoveXY:
                        this.RenderTransform = new TranslateTransform();
                        break;
                }
            }
        }

        public bool CanGoback
        {
            get { return navigationStack.Count != 0; }
        }

        public NavigationPanel()
        {
            this.RenderTransform = new ScaleTransform();
            this.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        protected virtual void MoveIn()
        {
            switch (this.navigationType)
            {
                case Controls.NavigationType.Opacity:
                    {
                        ScaleTransform scaleTransform = this.RenderTransform as ScaleTransform;

                        DoubleAnimation doubleAnimationSX = new DoubleAnimation(0f, 1.0f, new Duration(TimeSpan.FromSeconds(0.3)));
                        DoubleAnimation doubleAnimationSY = new DoubleAnimation(0f, 1.0f, new Duration(TimeSpan.FromSeconds(0.3)));
                        doubleAnimationSX.Completed += new EventHandler(moveIn_Completed);
                        scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, doubleAnimationSX);
                        scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, doubleAnimationSY);

                        DoubleAnimation doubleAnimationOpacity = new DoubleAnimation(0.0f, 1.0f, new Duration(TimeSpan.FromSeconds(0.3)));
                        this.BeginAnimation(DockPanel.OpacityProperty, doubleAnimationOpacity);
                    }
                    break;
                case Controls.NavigationType.MoveX:
                    {

                    }
                    break;
            }   
        }

        private void moveIn_Completed(object sender, EventArgs e)
        {
            if (this.MoveInCompletedEvent != null)
                this.MoveInCompletedEvent(this, EventArgs.Empty);
        }

        protected virtual void MoveOut()
        {
            ScaleTransform scaleTransform = this.RenderTransform as ScaleTransform;

            DoubleAnimation doubleAnimationSX = new DoubleAnimation(1f, 3.0f, new Duration(TimeSpan.FromSeconds(0.3)));
            DoubleAnimation doubleAnimationSY = new DoubleAnimation(1f, 3.0f, new Duration(TimeSpan.FromSeconds(0.3)));
            doubleAnimationSX.Completed += new EventHandler(moveOut_Completed);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, doubleAnimationSX);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, doubleAnimationSY);

            DoubleAnimation doubleAnimationOpacity = new DoubleAnimation(1.0f, 0.0f, new Duration(TimeSpan.FromSeconds(0.3)));
            this.BeginAnimation(DockPanel.OpacityProperty, doubleAnimationOpacity);
        }

        private void moveOut_Completed(object sender, EventArgs e)
        {
            if (this.MoveOutCompletedEvent != null)
                this.MoveOutCompletedEvent(this, EventArgs.Empty);

            base.Content = this.newObject;
            this.MoveIn();
        }

        protected virtual void BackMoveIn()
        {
            ScaleTransform scaleTransform = this.RenderTransform as ScaleTransform;

            DoubleAnimation doubleAnimationSX = new DoubleAnimation(0f, 1.0f, new Duration(TimeSpan.FromSeconds(0.3)));
            DoubleAnimation doubleAnimationSY = new DoubleAnimation(0f, 1.0f, new Duration(TimeSpan.FromSeconds(0.3)));
            doubleAnimationSX.Completed += new EventHandler(backMoveIn_Completed);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, doubleAnimationSX);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, doubleAnimationSY);

            DoubleAnimation doubleAnimationOpacity = new DoubleAnimation(0.0f, 1.0f, new Duration(TimeSpan.FromSeconds(0.3)));
            this.BeginAnimation(DockPanel.OpacityProperty, doubleAnimationOpacity);
        }

        private void backMoveIn_Completed(object sender, EventArgs e)
        {
            if (this.BackMoveInCompletedEvent != null)
                this.BackMoveInCompletedEvent(this, EventArgs.Empty);
        }

        protected virtual void BackMoveOut()
        {
            ScaleTransform scaleTransform = this.RenderTransform as ScaleTransform;

            DoubleAnimation doubleAnimationSX = new DoubleAnimation(1f, 0.0f, new Duration(TimeSpan.FromSeconds(0.3)));
            DoubleAnimation doubleAnimationSY = new DoubleAnimation(1f, 0.0f, new Duration(TimeSpan.FromSeconds(0.3)));
            doubleAnimationSX.Completed += new EventHandler(backMoveOut_Completed);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, doubleAnimationSX);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, doubleAnimationSY);

            DoubleAnimation doubleAnimationOpacity = new DoubleAnimation(1.0f, 0.0f, new Duration(TimeSpan.FromSeconds(0.3)));
            this.BeginAnimation(DockPanel.OpacityProperty, doubleAnimationOpacity);
        }

        private void backMoveOut_Completed(object sender, EventArgs e)
        {
            if (this.BackMoveOutCompletedEvent != null)
                this.BackMoveOutCompletedEvent(this, EventArgs.Empty);

            base.Content = navigationStack.Pop();
            this.BackMoveIn();
        }

        public void GoBack()
        {
            if (!this.CanGoback)
                return;

            this.BackMoveOut();
        }

        public void RemoveEntry()
        {
            if (!this.CanGoback)
                return;

            this.navigationStack.Pop();
        }

        public void ClearEntry()
        {
            this.navigationStack.Clear();
        }
    }
}
