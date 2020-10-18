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
using SoonLearning.AppCenter.Utility;
using System.Windows.Threading;
using System.Reflection;
using SoonLearning.AppCenter.Controls;
using SoonLearning.AppCenter.Data;
using System.Threading;

namespace SoonLearning.ConnectNumber
{
    /// <summary>
    /// Interaction logic for DrawNumberPage.xaml
    /// </summary>
    public partial class DrawNumberPage : UserControl
    {
        private DrawNumberData drawNumberData;
        private bool update = false;
        
        public DrawNumberPage()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
        }

        internal void ShowDrawNumberData()
        {
            this.update = true;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            DrawNumberControl.Instance.ControlPanlVisible(true);
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
                {
                    this.Update();
                }),
                DispatcherPriority.Normal,
                null);
        }

        private void Update()
        {
            if (!this.update)
                return;

            DrawNumberItem item = ControlMgr.Instance.DataMgr.SelectedItem;
            if (item == null)
                return;

            DrawNumberControl.Instance.SelectedStage = ControlMgr.Instance.DataMgr.CurrentIndex;

            this.drawNumberData = DrawNumberData.Load(item.DataFile);

            Size imgSize = new Size();
            imgSize.Width = this.drawNumberData.DrawNumberItem.CanvasWidth;
            imgSize.Height = this.drawNumberData.DrawNumberItem.CanvasHeight;
            if (imgSize.Width < 1.0f)
                imgSize.Width = this.drawNumberData.DrawNumberItem.CanvasWidth;
            if (imgSize.Height < 1.0f)
                imgSize.Height = this.drawNumberData.DrawNumberItem.CanvasHeight;
            Vector vec = GC_UIHelper.MatchImageToWnd(imgSize, new Size(panelGrid.ActualWidth, panelGrid.ActualHeight));
            this.drawNumberCanvas.Width = vec.X;// this.drawNumberData.DrawNumberItem.CanvasWidth;
            this.drawNumberCanvas.Height = vec.Y;// this.drawNumberData.DrawNumberItem.CanvasHeight;

            this.drawNumberCanvas.Data = this.drawNumberData;
            this.update = false;
        }

        private void homeBtn_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.DrawNumberStartupPage.ShowListPage();
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Next();
        }

        private void preBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Previous();
        }

        internal void Next()
        {
            int next = ControlMgr.Instance.DataMgr.NextItem;
            this.update = true;
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.Update();
            }),
                DispatcherPriority.Normal,
                null);
        }

        internal void Previous()
        {
            int next = ControlMgr.Instance.DataMgr.PreItem;
            this.update = true;
            this.Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                this.Update();
            }),
                DispatcherPriority.Normal,
                null);
        }
    }
}
