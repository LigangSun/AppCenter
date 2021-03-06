﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoonLearning.AppCenter.Controls
{
    public partial class NumericUpDown : Control
    {
        /// <summary>
        /// Initializes a new instance of the NumericUpDownControl.
        /// </summary>
        public NumericUpDown()
        {
            InitializeCommands();
        }

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
        }

        /// <summary>
        /// Gets or sets the value assigned to the control.
        /// </summary>
        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                "Value", typeof(decimal), typeof(NumericUpDown),
                new FrameworkPropertyMetadata(new decimal(0), new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            NumericUpDown control = (NumericUpDown)obj;

            RoutedPropertyChangedEventArgs<decimal> e = new RoutedPropertyChangedEventArgs<decimal>(
                (decimal)args.OldValue, (decimal)args.NewValue, ValueChangedEvent);
            control.OnValueChanged(e);
        }

        public static readonly DependencyProperty StepProperty =
                DependencyProperty.Register(
               "Step", typeof(decimal), typeof(NumericUpDown),
               new FrameworkPropertyMetadata(new decimal(1)));

        public decimal Step
        {
            get { return (decimal)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public static readonly DependencyProperty MinProperty =
                DependencyProperty.Register(
               "MinValue", typeof(decimal), typeof(NumericUpDown),
               new FrameworkPropertyMetadata(new decimal(0)));

        public decimal MinValue
        {
            get { return (decimal)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        public static readonly DependencyProperty MaxProperty =
               DependencyProperty.Register(
              "MaxValue", typeof(decimal), typeof(NumericUpDown),
              new FrameworkPropertyMetadata(new decimal(100)));

        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxProperty); }
            set
            {
                if (value > MaxValue)
                    this.Value = value;
                SetValue(MaxProperty, value);
            }
        }


        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<decimal>), typeof(NumericUpDown));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<decimal> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }
        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args)
        {
            RaiseEvent(args);
        }

        #region Commands

        public static RoutedCommand IncreaseCommand
        {
            get
            {
                return _increaseCommand;
            }
        }
        public static RoutedCommand DecreaseCommand
        {
            get
            {
                return _decreaseCommand;
            }
        }

        private static void InitializeCommands()
        {
            _increaseCommand = new RoutedCommand("IncreaseCommand", typeof(NumericUpDown));
            CommandManager.RegisterClassCommandBinding(typeof(NumericUpDown), new CommandBinding(_increaseCommand, OnIncreaseCommand));
            CommandManager.RegisterClassInputBinding(typeof(NumericUpDown), new InputBinding(_increaseCommand, new KeyGesture(Key.Up)));

            _decreaseCommand = new RoutedCommand("DecreaseCommand", typeof(NumericUpDown));
            CommandManager.RegisterClassCommandBinding(typeof(NumericUpDown), new CommandBinding(_decreaseCommand, OnDecreaseCommand));
            CommandManager.RegisterClassInputBinding(typeof(NumericUpDown), new InputBinding(_decreaseCommand, new KeyGesture(Key.Down)));
        }

        private static void OnIncreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;
            if (control != null)
            {
                control.OnIncrease();
            }
        }
        private static void OnDecreaseCommand(object sender, ExecutedRoutedEventArgs e)
        {
            NumericUpDown control = sender as NumericUpDown;
            if (control != null)
            {
                control.OnDecrease();
            }
        }

        protected virtual void OnIncrease()
        {
            if (this.Value < MaxValue)
            {
                this.Value += Step;
            }
        }
        protected virtual void OnDecrease()
        {
            if (this.Value > MinValue)
            {
                this.Value -= Step;
            }
        }

        private static RoutedCommand _increaseCommand;
        private static RoutedCommand _decreaseCommand;
        #endregion
    }
}
