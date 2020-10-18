using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Threading;
using System.Windows.Media.Animation;
using System.IO;
using System.Windows.Markup;
using System.Reflection;
using System.Windows.Threading;
using SoonLearning.AppCenter.Controls;

namespace SoonLearning.Math.Presentation.Bridge
{
    /// <summary>
    /// Interaction logic for MainUserControl.xaml
    /// </summary>
    public partial class MainUserControl : UserControl
    {
        private static MainUserControl instance;

        public static MainUserControl Instance
        {
            get
            {
                Interlocked.CompareExchange<MainUserControl>(ref instance, new MainUserControl(), null);
                return instance;
            }
        }

        private Timer timer;
        private TimerCallback timerCallback;
        private int startTickcount;

        private DoubleAnimation doubleAnimation;
        private EventHandler doubleAnimationCompletedHandler;

        private double speedRatio = 60;

        public MainUserControl()
        {
            InitializeComponent();

            this.timerCallback = (obj) =>
            {
                int delta = Environment.TickCount - this.startTickcount;
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    int second = (int)(delta / 1000f);
                    double minute = second / this.speedRatio;
                    double left = second % this.speedRatio;
                    this.goingInfoTextBlock.Text = string.Format("火车用时{0}分钟{1}秒, 已经通过了{2}米。", minute.ToString("00"), left.ToString("00"),
                        System.Math.Round(delta / 1000f / (double)this.speedRatio * decimal.ToDouble(Data.Instance.TrainSpeed), 2));
                }),
                DispatcherPriority.Normal,
                null);
            };

            this.doubleAnimationCompletedHandler = (s, e) =>
            {
                this.blinkImage(this.bridgeImage, null);

                this.blinkImage(this.trainImage, (s1, e1) =>
                {
                    if (this.trainImage.ToolTip != null)
                        ((ToolTip)(this.trainImage.ToolTip)).IsOpen = false;

                    this.trainImage.ToolTip = null;
                });
                
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    string message = "火车已经通过大桥了。 火车走过的路程等于大桥的长度与火车的长度之和.";
                    showToolTip(this.trainImage, message);

                    this.goingInfoTextBlock.Text = message;
                }),
                DispatcherPriority.Background,
                null);
                if (this.timer == null)
                    return;

                this.timer.Dispose();
                this.timer = null;
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Data.Instance.IsValid)
            {
                MessageWindow messageWnd = new MessageWindow();
                messageWnd.ShowMessage("请在输入框中填入至少三项数据!", MessageBoxButton.OK, null);
                return;
            }

            ComboBoxItem selectedItem = this.speedComboBox.SelectedItem as ComboBoxItem;
            this.speedRatio = 60f / System.Convert.ToDouble(selectedItem.Content);

            if (this.doubleAnimation != null)
                this.doubleAnimation.Completed -= this.doubleAnimationCompletedHandler;

            double bridgeLeft = (this.bridgeCanvas.ActualWidth - this.bridgeImage.ActualWidth) / 2;
            Canvas.SetLeft(this.trainImage, bridgeLeft - this.trainImage.ActualWidth);
            if (this.trainImage.ToolTip != null)
                ((ToolTip)(this.trainImage.ToolTip)).IsOpen = false;

       //     this.blinkImage(this.trainImage, (s1, e1) =>
            {
             //   this.blinkImage(this.bridgeImage, (s2, e2) =>
                {
                    doubleAnimation = new DoubleAnimation();
                    doubleAnimation.Completed += this.doubleAnimationCompletedHandler;
                    doubleAnimation.From = bridgeLeft - this.trainImage.ActualWidth;
                    doubleAnimation.To = bridgeLeft + this.bridgeImage.ActualWidth;
                    doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(decimal.ToDouble(Data.Instance.calDuration()) * this.speedRatio));
                    this.trainImage.BeginAnimation(Canvas.LeftProperty, doubleAnimation);

                    this.goingInfoTextBlock.Text = string.Empty;
                    this.tipTextBlock.Text = string.Format("火车开始通过全长为{0}米的大桥，火车要走过的路程为{1}米，火车速度为{2}米/分钟，通过大桥需要{3}分钟。",
                        Data.Instance.BridgeLength, Data.Instance.BridgeLength + Data.Instance.TrainLength,
                        Data.Instance.TrainSpeed, Data.Instance.Duration);

                    if (this.timer != null)
                    {
                        this.timer.Dispose();
                    }

                    this.trainImage.ToolTip = null;
                    this.startTickcount = Environment.TickCount;
                    this.timer = new Timer(this.timerCallback, null, 100, 500);
                };
            };
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                double bridgeLeft = (this.bridgeCanvas.ActualWidth - this.bridgeImage.ActualWidth) / 2;
                double bridgeTop = this.bridgeCanvas.ActualHeight - this.bridgeImage.ActualHeight;

                Canvas.SetTop(this.roadPanel, bridgeTop);
                Canvas.SetLeft(this.trainImage, bridgeLeft - this.trainImage.ActualWidth);

                Canvas.SetTop(this.trainImage, this.bridgeCanvas.ActualHeight - 286 - this.trainImage.ActualHeight);

                this.leftRect.Width = bridgeLeft;
                this.rightRect.Width = bridgeLeft;

                this.bridgeCanvas.Visibility = System.Windows.Visibility.Visible;

                Assembly flowDocumentAssembly = Assembly.GetExecutingAssembly();
                Stream stream = flowDocumentAssembly.GetManifestResourceStream("SoonLearning.Math.Presentation.Bridge.AboutFlowDocument.xaml");
                FlowDocument doc = (FlowDocument)XamlReader.Load(stream);
                this.aboutDocumentReader.Document = doc;
            }),
            DispatcherPriority.Input,
            null);
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            this.solutionPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.Center;
            solutionPopup.IsOpen = true;
        }

        private void solutionPopup_Opened(object sender, EventArgs e)
        {
            
        }

        private void blinkImage(Image image, EventHandler completed)
        {
            DoubleAnimation blinkAnimation = new DoubleAnimation();
            blinkAnimation.From = 1.0;
            blinkAnimation.To = 0.0;
            blinkAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            blinkAnimation.AutoReverse = true;
            blinkAnimation.RepeatBehavior = new RepeatBehavior(6);
            if (completed != null)
                blinkAnimation.Completed += completed;

            image.BeginAnimation(Image.OpacityProperty, blinkAnimation);
        }

        private void showToolTip(Image image, string tip)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.Content = tip;
            toolTip.PlacementTarget = image;
            toolTip.Placement = System.Windows.Controls.Primitives.PlacementMode.Top;
            image.ToolTip = toolTip;
            toolTip.IsOpen = true;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            if (this.trainImage.ToolTip != null)
                ((ToolTip)(this.trainImage.ToolTip)).IsOpen = false;
        }
    }
}
