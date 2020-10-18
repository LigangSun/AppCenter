using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SoonLearning.TeachAppMaker.Commands
{
    public class ProjectCommands
    {
        public static readonly NewProjectCommand NewProjectCommand;
        public static readonly OpenProjectCommand OpenProjectCommand;
        public static readonly SaveProjectCommand SaveProjectCommand;
        public static readonly SettingCommand SettingCommand;
        public static readonly ExitCommand ExitCommand;

        public static readonly RoutedCommand PublishAppCommand;

        static ProjectCommands()
        {
            NewProjectCommand = new NewProjectCommand();
            OpenProjectCommand = new OpenProjectCommand();
            SaveProjectCommand = new SaveProjectCommand();
            SettingCommand = new SettingCommand();
            ExitCommand = new ExitCommand();

            PublishAppCommand = new RoutedCommand("PublishAppCommand", typeof(MainWindow));
            CommandManager.RegisterClassCommandBinding(typeof(MainWindow),
                new CommandBinding(PublishAppCommand, 
            (s, e) =>
            {
                LoginWindow loginWindow = new LoginWindow();
                if (loginWindow.ShowDialog().Value)
                {
                //    CreatePackageWindow createPackageWindow = new CreatePackageWindow();
                //    if (createPackageWindow.ShowDialog().Value)
                    {
                        PublishWindow publishWindow = new PublishWindow(loginWindow.UserId, loginWindow.Password);
                        publishWindow.ShowDialog();
                    }
                }
            },
            (s, e) =>
            {
                e.CanExecute = true;
            }));
        }

        public ProjectCommands()
        {

        }
    }
}
