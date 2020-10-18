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
using System.Collections.ObjectModel;
using SoonLearning.Assessment.Data;
using System.Reflection;
using SoonLearning.AppCenter.Utility;
using SoonLearning.Assessment.Player.Data;
using SoonLearning.AppCenter.Controls;

namespace SoonLearning.Assessment.Player.UserControls
{
    /// <summary>
    /// Interaction logic for ExerciseHistoryUserControl.xaml
    /// </summary>
    public partial class ExerciseHistoryUserControl : UserControl
    {
        private ObservableCollection<ExerciseHistoryData> exerciseCollection = new ObservableCollection<ExerciseHistoryData>();

        public ExerciseHistoryUserControl()
        {
            InitializeComponent();

            this.exerciseListView.ItemsSource = this.exerciseCollection;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.exerciseCollection.Clear();

            string dataFolder = DataMgr.Instance.DataFolder;
            dataFolder = System.IO.Path.Combine(dataFolder, "Exercise");

            if (!System.IO.Directory.Exists(dataFolder))
            {
                System.IO.Directory.CreateDirectory(dataFolder);
            }
            
            string[] files = System.IO.Directory.GetFiles(dataFolder, "*.mxd");
            foreach (string file in files)
            {
                try
                {
                    Exercise exercise = SerializerHelper<Exercise>.XmlDeserialize(file);
                    exerciseCollection.Add(ExerciseHistoryData.FromExercise(exercise, file));
                }
                catch
                {
                }
            }
        }

        private void viewDetailButton_Click(object sender, RoutedEventArgs e)
        {
            ExerciseHistoryData data = this.exerciseListView.SelectedItem as ExerciseHistoryData;
            ControlMgr.Instance.StartupUserControl.ShowAllQuestionPage(data.Exercise, false);
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            MessageWindow msgWnd = new MessageWindow();
            msgWnd.ShowMessage("你确定要删除该练习记录吗？", MessageBoxButton.OKCancel, MessageWindowCallback);
        }

        private void MessageWindowCallback(bool ok)
        {
            if (!ok)
                return;

            ExerciseHistoryData data = this.exerciseListView.SelectedItem as ExerciseHistoryData;
            try
            {
                System.IO.File.Delete(data.File);
            }
            catch
            {
            }

            this.exerciseCollection.Remove(data);
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void exerciseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.viewDetailButton.IsEnabled =
                this.deleteButton.IsEnabled =
                (this.exerciseListView.SelectedItem != null);
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            ControlMgr.Instance.StartupUserControl.GoBack();
        }
    }
}
