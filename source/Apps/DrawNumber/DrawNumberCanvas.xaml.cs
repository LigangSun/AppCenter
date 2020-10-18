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
using System.IO;
using SoonLearning.AppCenter;
using SoonLearning.AppCenter.Controls;
using System.Threading;
using System.Windows.Threading;
using SoonLearning.AppCenter.Utility;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.ConnectNumber
{
    /// <summary>
    /// Interaction logic for DrawNumberCanvas.xaml
    /// </summary>
    public partial class DrawNumberCanvas : Canvas
    {
        private DrawNumberData drawNumberData;
        private Point? startPoint = null;
        private Point? pt1 = null;
        private Point? pt2 = null;

        private int currentIndex = -1;

        private double circleRadius = 10;

        private Image numberImage = null;

        private double bkScale = 1.0;

        public DrawNumberData Data
        {
            set
            {
                this.drawNumberData = value;
                this.Load();
            }
        }

        public DrawNumberCanvas()
        {
            InitializeComponent();

            try
            {
                this.circleRadius = (double)this.TryFindResource("circleDiameter") / 2;
            }
            catch
            {
            }
        }

        private void Load()
        {
            this.Children.Clear();
            this.currentIndex = -1;
            this.startPoint = null;
            this.pt1 = null;
            this.pt2 = null;

            if (this.drawNumberData == null)
                return;

            if (this.numberImage != null &&
                this.numberImage.IsVisible)
                this.numberImage.Visibility = System.Windows.Visibility.Hidden;

            if (this.drawNumberData.BackgroundImageData != null &&
                this.drawNumberData.BackgroundImageData.Length > 0)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(this.drawNumberData.BackgroundImageData);
                bi.EndInit();
                this.Background = new ImageBrush(bi);
                this.Background.Opacity = 0.5;

                this.bkScale = this.Width / bi.PixelWidth;
            }

        //    int deltaLeft = (int)(this.Width - this.drawNumberData.DrawNumberItem.CanvasWidth) / 2;
        //    int deltaTop = (int)(this.Height - this.drawNumberData.DrawNumberItem.CanvasHeight) / 2;

            for (int i = this.drawNumberData.DrawNumberItem.PointCollection.Count - 1; i >= 0; i--)
            {
                Point pt = this.drawNumberData.DrawNumberItem.PointCollection[i];

                Button circle = new Button();
                circle.Style = (Style)this.FindResource("numberElepse");
                circle.Content = (i + 1).ToString();
                circle.Tag = i;
                circle.Click += new RoutedEventHandler(circle_Click);
                Canvas.SetLeft(circle, pt.X);
                Canvas.SetTop(circle, pt.Y);
                this.Children.Add(circle);
            }
        }

        private void circle_Click(object sender, RoutedEventArgs e)
        {
            Button circle = sender as Button;
            if (circle == null)
                return;

            AudioHelper.PlayWaterDropSound();

            int index = (int)circle.Tag;
            if (index - 1 != this.currentIndex)
                return;

            Point pt = new Point(Canvas.GetLeft(circle), Canvas.GetTop(circle));

            if (startPoint == null)
                startPoint = pt;

            if (pt1 == null)
            {
                pt1 = pt;
            }
            else
            {
                pt2 = pt1;
                pt1 = pt;

                this.DrawLine(pt1.Value, pt2.Value);

                if (index == this.drawNumberData.DrawNumberItem.PointCollection.Count - 1)
                {
                    this.DrawLine(pt1.Value, startPoint.Value);
                    this.Done();
                }
            }

            this.currentIndex = index;

            this.Children.Remove(circle);
        }

        private void DrawLine(Point p1, Point p2)
        {
            Line line = new Line();
            line.Style = (Style)this.TryFindResource("lineStyle");
            line.X1 = p1.X + circleRadius;
            line.Y1 = p1.Y + circleRadius;
            line.X2 = p2.X + circleRadius;
            line.Y2 = p2.Y + circleRadius;
            this.Children.Add(line);
        }

        private void Done()
        {
            if (this.drawNumberData.DrawNumberImageData != null &&
                this.drawNumberData.DrawNumberImageData.Length > 0)
            {
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(this.drawNumberData.DrawNumberImageData);
                bi.EndInit();

                if (this.numberImage == null)
                {
                    this.numberImage = new Image();
                }

                this.Children.Add(this.numberImage);
                this.numberImage.Source = bi;
                this.numberImage.Stretch = Stretch.Uniform;
                this.numberImage.Width = this.drawNumberData.DrawNumberItem.ForegroundWidth;// bi.PixelWidth;// *bkScale;
                this.numberImage.Height = this.drawNumberData.DrawNumberItem.ForegroundHeight;// bi.PixelHeight;// *bkScale;
                Canvas.SetTop(this.numberImage, this.drawNumberData.DrawNumberItem.NumberImagePoint.Y);
                Canvas.SetLeft(this.numberImage, this.drawNumberData.DrawNumberItem.NumberImagePoint.X);
                this.numberImage.Visibility = System.Windows.Visibility.Visible;
                this.Background.Opacity = 1.0;
            }

            List<Line> lineList = new List<Line>();
            foreach (UIElement o in this.Children)
            {
                if (o is Line)
                    lineList.Add(o as Line);
            }

            foreach (Line line in lineList)
                this.Children.Remove(line);

         //   this.Children.Add(this.resultCtrl);

       //     this.resultCtrl.Show = true;
        //    Canvas.SetTop(this.resultCtrl, (this.Height - this.resultCtrl.ActualHeight) / 2);
        //    Canvas.SetLeft(this.resultCtrl, (this.Width - this.resultCtrl.ActualWidth) / 2);

            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    Thread.Sleep(2000);

                    ResultWindow resultWin = new ResultWindow();
                    resultWin.State = ResultState.Pass;
                    resultWin.ShowMessage(MessageWindowCallback);
                }),
                DispatcherPriority.Background,
                null);
        }

        private void MessageWindowCallback(bool ok)
        {
            if (ok)
            {
                DrawNumberControl.Instance.NextStage();
            }
            else
            {
                this.Load();
            }
        }
    }
}
