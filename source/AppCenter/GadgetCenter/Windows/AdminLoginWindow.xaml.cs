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
using System.Windows.Shapes;
using SoonLearning.AppCenter.Data;

namespace SoonLearning.AppCenter.Windows
{
    /// <summary>
    /// Interaction logic for AdminLoginWindow.xaml
    /// </summary>
    public partial class AdminLoginWindow : Window
    {
        public AdminLoginWindow()
        {
            InitializeComponent();

            this.idTextBox.Text = AdminInformation.adminUserName;
            this.passwordTextBox.Password = AdminInformation.adminPassword;
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (AdminInformation.adminPassword != this.passwordTextBox.Password)
                AdminInformation.adminPassword = LoginInfo.GetMD5Hash(this.passwordTextBox.Password);
            AdminInformation.adminUserName = this.idTextBox.Text;
            this.DialogResult = true;
            this.Close();
        }

        private void x_Name__cancelBtn__Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
