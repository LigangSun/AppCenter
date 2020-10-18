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
using SoonLearning.Assessment.Player.Data;

namespace SoonLearning.Assessment.Player.UserControls
{
    /// <summary>
    /// Interaction logic for ExerciseSettingUserControl.xaml
    /// </summary>
    public partial class SectionInfoSettingUserControl : UserControl
    {
        public SectionInfoSettingUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataMgr.Instance.DataCreator == null)
                    return;

                this.sectionInfoListBox.ItemsSource = DataMgr.Instance.DataCreator.SectionInfoCollection;
            }
            catch
            {
            }
        }
    }
}
