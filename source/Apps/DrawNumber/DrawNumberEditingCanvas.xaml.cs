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
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.ConnectNumber
{
    /// <summary>
    /// Interaction logic for DrawNumberEditingCanvas.xaml
    /// </summary>
    public partial class DrawNumberEditingCanvas : Canvas
    {
        private DrawNumberData drawNumberData;
        private Point circleMoveStartPoint;

        private string imageFile;
        private string bkImageFile;

        private double bkScale = 1.0;

        public string BkImageFile
        {
            set
            {
                BitmapImage bi = new BitmapImage(new Uri(value));
                ImageBrush ib = new ImageBrush(bi);
                ib.TileMode = TileMode.Tile;
                this.bkImageFile = value;
                this.Background = ib;

                this.bkScale = this.Width / bi.Width;
            }
        }

        public string ImageFile
        {
            set
            {
                BitmapImage bi = new BitmapImage(new Uri(value));
                this.imageFile = value;

                this.drawingImage.BeginInit();
                this.drawingImage.Source = bi;
                this.drawingImage.EndInit();

             //   Vector vec = UIHelper.MatchImageToWnd(new Size(bi.Width, bi.Height), new Size(this.Width, this.Height));
                this.drawingImage.Width = bi.PixelWidth * this.bkScale;
                this.drawingImage.Height = bi.PixelHeight * this.bkScale;
                
                Canvas.SetLeft(this.drawingImage, (this.Width - this.drawingImage.Width) / 2);
                Canvas.SetTop(this.drawingImage, (this.Height - this.drawingImage.Height) / 2);
            }
        }

        public double NumberImageScale
        {
            get { return this.bkScale; }
            set
            {
                if (string.IsNullOrEmpty(this.imageFile))
                    return;

                BitmapImage bi = new BitmapImage(new Uri(this.imageFile));

                this.bkScale = value;
                this.drawingImage.Width = bi.PixelWidth * this.bkScale;
                this.drawingImage.Height = bi.PixelHeight * this.bkScale;
            }
        }

        public DrawNumberData DrawNumberData
        {
            get { return this.drawNumberData; }
        }

        public DrawNumberEditingCanvas()
        {
            InitializeComponent();

            this.drawingImage.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(drawingImage_PreviewMouseLeftButtonDown);
            this.drawingImage.PreviewMouseMove += new MouseEventHandler(drawingImage_PreviewMouseMove);
            this.drawingImage.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(drawingImage_PreviewMouseLeftButtonUp);
        }

        internal void NewDrawNumber()
        {
            this.drawNumberData = new DrawNumberData();

            List<Button> uiElementList = new List<Button>();
            foreach (UIElement ctrl in this.Children)
            {
                if (ctrl is Button)
                {
                    uiElementList.Add(ctrl as Button);
                }
            }

            foreach (Button btn in uiElementList)
            {
                this.Children.Remove(btn);
            }

            this.drawingImage.Source = null;
        }

        public void AddCircle()
        {
            Button circle = new Button();
            circle.Style = (Style)this.FindResource("numberElepse");
            Canvas.SetLeft(circle, 10);
            Canvas.SetTop(circle, 10);
            circle.Content = (this.drawNumberData.DrawNumberItem.PointCollection.Count + 1).ToString();
            circle.Tag = this.drawNumberData.DrawNumberItem.PointCollection.Count;
            circle.RenderTransformOrigin = new Point(0.5, 0.5);                                                                                                           
            circle.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(circle_PreviewMouseLeftButtonDown);
            circle.PreviewMouseMove += new MouseEventHandler(circle_PreviewMouseMove);
            circle.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(circle_PreviewMouseLeftButtonUp);
            this.Children.Add(circle);

            this.drawNumberData.DrawNumberItem.PointCollection.Add(new Point(10, 10));
        }

        private void circle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button circle = e.Source as Button;
            if (circle != null)
            {
                this.circleMoveStartPoint = Mouse.GetPosition(this);
                circle.CaptureMouse();
            }
        }

        private void circle_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;

            Button circle = e.Source as Button;
            if (circle != null)
            {
                Point curPt = Mouse.GetPosition(this);
                Vector deltaVc = curPt - this.circleMoveStartPoint;

                circle.RenderTransform = new TranslateTransform(deltaVc.X, deltaVc.Y);
            }
        }

        private void circle_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button circle = e.Source as Button;
            if (circle != null)
            {
                Point curPt = Mouse.GetPosition(this);
                Vector deltaVc = curPt - this.circleMoveStartPoint;
                
                circle.RenderTransform = null;

                double oldX = Canvas.GetLeft(circle);
                double oldY = Canvas.GetTop(circle);

                Canvas.SetLeft(circle, oldX + deltaVc.X);
                Canvas.SetTop(circle, oldY + deltaVc.Y);

                int index = (int)circle.Tag;

                this.drawNumberData.DrawNumberItem.PointCollection[index] = new Point(oldX + deltaVc.X, oldY + deltaVc.Y);

                circle.ReleaseMouseCapture();
            }
        }

        private void drawingImage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = e.Source as Image;
            if (img != null)
            {
                this.circleMoveStartPoint = Mouse.GetPosition(this);
                img.CaptureMouse();
            }
        }

        private void drawingImage_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
                return;

            Image img = e.Source as Image;
            if (img != null)
            {
                Point curPt = Mouse.GetPosition(this);
                Vector deltaVc = curPt - this.circleMoveStartPoint;

                img.RenderTransform = new TranslateTransform(deltaVc.X, deltaVc.Y);
            }
        }

        private void drawingImage_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image img = e.Source as Image;
            if (img != null)
            {
                Point curPt = Mouse.GetPosition(this);
                Vector deltaVc = curPt - this.circleMoveStartPoint;

                img.RenderTransform = null;

                double oldX = Canvas.GetLeft(img);
                double oldY = Canvas.GetTop(img);

                Canvas.SetLeft(img, oldX + deltaVc.X);
                Canvas.SetTop(img, oldY + deltaVc.Y);

                img.ReleaseMouseCapture();
            }
        }

        internal void Save()
        {
            byte[] bkImgData = new byte[0];
            byte[] numberImgData = new byte[0];
            if (!string.IsNullOrEmpty(this.bkImageFile))
            {
                BitmapImage bi = new BitmapImage(new Uri(this.bkImageFile));
                bkImgData = GC_UIHelper.GetBitmapData(this.bkImageFile, (int)bi.Width, (int)bi.Height);
            //    bkImgData = GC_UIHelper.GetBitmapData(this.bkImageFile, (int)this.Width, (int)this.Height);
            }            

            if (!string.IsNullOrEmpty(this.imageFile))
            {
                numberImgData = GC_UIHelper.GetBitmapData(this.imageFile, (int)this.drawingImage.Width, (int)this.drawingImage.Height);
            }

        //    RenderTargetBitmap bmp = new RenderTargetBitmap((int)this.Width, (int)this.Height, 96, 96, PixelFormats.Pbgra32); 
        //    bmp.Render(this);

            this.drawNumberData.DrawNumberItem.ThumbnailData = GC_UIHelper.GetBitmapData(this.bkImageFile, 128, 128);
            
            //BitmapEncoder encoder = new PngBitmapEncoder();
            
            //encoder.Frames.Add(BitmapFrame.Create(bmp));
            //using (MemoryStream ms = new MemoryStream()) 
            //{ 
            //    encoder.Save(ms);

            //    System.Drawing.Bitmap visualBmp = new System.Drawing.Bitmap(ms);

            //    Vector vec = GC_UIHelper.MatchImageToWnd(new Size(this.Width, this.Height), new Size(128, 128));
            //    System.Drawing.Image thumbnail = visualBmp.GetThumbnailImage((int)vec.X, (int)vec.Y, null, IntPtr.Zero);

            //    MemoryStream thumbnailMS = new MemoryStream();
            //    thumbnail.Save(thumbnailMS, System.Drawing.Imaging.ImageFormat.Png);
            //    this.drawNumberData.DrawNumberItem.ThumbnailData = thumbnailMS.ToArray();
            //}

            this.drawNumberData.DrawNumberItem.NumberImagePoint = new Point(Canvas.GetLeft(this.drawingImage), Canvas.GetTop(this.drawingImage));
            this.drawNumberData.DrawNumberItem.CanvasWidth = (int)this.Width;
            this.drawNumberData.DrawNumberItem.CanvasHeight = (int)this.Height;
            this.drawNumberData.DrawNumberItem.ForegroundWidth = this.drawingImage.ActualWidth;
            this.drawNumberData.DrawNumberItem.ForegroundHeight = this.drawingImage.ActualHeight;

            this.drawNumberData.BackgroundImageData = bkImgData;
            this.drawNumberData.DrawNumberImageData = numberImgData;
        }
    }
}
