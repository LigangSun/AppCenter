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
using SoonLearning.TeachAppMaker.Modules;
using SoonLearning.TeachAppMaker.Data;
using SoonLearning.TeachAppMaker.Commands;
using SoonLearning.TeachAppMaker.Questions;
using SoonLearning.Assessment.Data;
using System.Windows.Threading;

namespace SoonLearning.TeachAppMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProjectModule projectModule;
        private EditModule editModule;
        private HelpModule helpModule;

        private KnowledgeUserControl knowledgeUserControl;
        private QuestionListUserControl questionListuserControl;

        private Action NewProjectCreatedAction;
        private Action ProjectOpenedAction;
        private Action KnowledgeChangedAction;

        private KnowledgeUserControl KnowledgeUserControl
        {
            get
            {
                if (this.knowledgeUserControl == null)
                    this.knowledgeUserControl = new KnowledgeUserControl();

                return this.knowledgeUserControl;
            }
        }

        private QuestionListUserControl QuestionListUserControl
        {
            get
            {
                if (this.questionListuserControl == null)
                    this.questionListuserControl = new QuestionListUserControl();

                return this.questionListuserControl;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            this.projectModule = new ProjectModule(this);
            this.editModule = new EditModule(this);
            this.helpModule = new HelpModule(this);

            this.prepareEventHandler();
        }

        private void prepareEventHandler()
        {
            this.NewProjectCreatedAction = () =>
            {
                this.rootTreeViewItem.Header = ProjectMgr.Instance.App;
                this.contentTreeView.Visibility = System.Windows.Visibility.Visible;
                this.updateRightPanel();
            };

            this.KnowledgeChangedAction = () =>
            {
                if (this.isControlVisible(this.KnowledgeUserControl))
                    this.KnowledgeUserControl.ShowKnowledge();
            };

            this.ProjectOpenedAction = () =>
            {
                this.rootTreeViewItem.Header = ProjectMgr.Instance.App;
                this.contentTreeView.Visibility = System.Windows.Visibility.Visible;
                this.updateRightPanel();
            };
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = ProjectMgr.Instance.CloseProject();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ProjectMgr.Instance.NewProjectCreated += this.NewProjectCreatedAction;
            ProjectMgr.Instance.ProjectOpened += this.ProjectOpenedAction;
            ProjectMgr.Instance.KnowledgeChanged += this.KnowledgeChangedAction;

            StartupWindow startupWnd = new StartupWindow();
            if (startupWnd.ShowDialog().Value)
            {
                if (startupWnd.Type == 1)
                {
                    ProjectCommands.NewProjectCommand.Execute(null);
                }
                else if (startupWnd.Type == 2)
                {
                    ProjectCommands.OpenProjectCommand.Execute(null);
                }
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            ProjectMgr.Instance.NewProjectCreated -= this.NewProjectCreatedAction;
            ProjectMgr.Instance.ProjectOpened -= this.ProjectOpenedAction;
            ProjectMgr.Instance.KnowledgeChanged -= this.KnowledgeChangedAction;
        }

        private void contentTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.updateRightPanel();
        }

        private void updateRightPanel()
        {
            if (this.contentTreeView.SelectedItem == null)
                return;

            if (this.contentTreeView.SelectedItem == this.rootTreeViewItem)
            {
                this.contentTreeView.ContextMenu = this.contentTreeView.Resources["eidtAppContextMenu"] as ContextMenu;
            }
            else if (this.contentTreeView.SelectedItem is KnowledgeTreeNode)
            {
                this.contentTreeView.ContextMenu = this.contentTreeView.Resources["knowledgeContextMenu"] as ContextMenu;

                this.rightContentDockPanel.Children.Clear();
                this.rightContentDockPanel.Children.Add(this.KnowledgeUserControl);
                this.KnowledgeUserControl.ShowKnowledge();
            }
            else if (this.contentTreeView.SelectedItem is QuestionBankTreeNode)
            {
                this.contentTreeView.ContextMenu = this.contentTreeView.Resources["questionBankContextMenu"] as ContextMenu;

                this.rightContentDockPanel.Children.Clear();
                this.rightContentDockPanel.Children.Add(this.QuestionListUserControl);
                this.QuestionListUserControl.ShowQuestion(QuestionType.None);
            }
            else if (this.contentTreeView.SelectedItem is QuestionTypeTreeNode)
            {
                this.contentTreeView.ContextMenu = this.contentTreeView.Resources["questionTypeContextMenu"] as ContextMenu;
                QuestionType type = ((QuestionTypeTreeNode)this.contentTreeView.SelectedItem).Type;
                ((MenuItem)(this.contentTreeView.ContextMenu.Items[0])).CommandParameter = type.ToString();

                this.rightContentDockPanel.Children.Clear();
                this.rightContentDockPanel.Children.Add(this.QuestionListUserControl);
                this.QuestionListUserControl.ShowQuestion(type);
            }
        }

        private bool isControlVisible(UIElement ctrl)
        {
            if (this.rightContentDockPanel.Children.Count > 0)
            {
                if (this.rightContentDockPanel.Children[0] == ctrl)
                    return true;
            }

            return false;
        }
    }
}
