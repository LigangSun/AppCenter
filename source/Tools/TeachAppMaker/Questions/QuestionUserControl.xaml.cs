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
using SoonLearning.Assessment.Data;

namespace SoonLearning.TeachAppMaker.Questions
{
    /// <summary>
    /// Interaction logic for QuestionUserControl.xaml
    /// </summary>
    public partial class QuestionUserControl : UserControl
    {
        private Question question;
        private QuestionContent solution;
        private bool solutionChanged;

        public QuestionUserControl()
        {
            InitializeComponent();
        }

        internal void Init(Question question)
        {
            this.initQuestionUserControl(question);
            this.question = question;

            this.difficultyLevelComboBox.SelectedIndex = question.DifficultyLevel - 1;
            this.solution = this.question.Solution.Clone() as QuestionContent;
        }

        internal bool Save()
        {
            if (this.controlPanel.Children.Count > 0)
            {
                if (this.controlPanel.Children[0] is IEditQuestion)
                {
                    if (!((IEditQuestion)this.controlPanel.Children[0]).Save())
                        return false;
                }
            }

            this.question.DifficultyLevel = System.Convert.ToInt32(((ComboBoxItem)this.difficultyLevelComboBox.SelectedItem).Content);
            if (this.solutionChanged)
                this.question.Solution = this.solution;

            return true;
        }

        private void initQuestionUserControl(Question question)
        {
            this.controlPanel.Children.Clear();

            UserControl ctrl = null;
            Type type = question.GetType();

            if (type.FullName == typeof(MCQuestion).FullName)
            {
                ctrl = new MCQuestionUserControl(question as MCQuestion);
            }
            else if (type.FullName == typeof(MRQuestion).FullName)
            {
                ctrl = new MRQuestionUserControl(question as MRQuestion);
            }
            else if (type.FullName == typeof(ESQuestion).FullName)
            {
                ctrl = new ESQuestionUserControl(question as ESQuestion);
            }
            else if (type.FullName == typeof(TFQuestion).FullName)
            {
                ctrl = new TFQuestionUserControl(question as TFQuestion);
            }
            else if (type.FullName == typeof(MAQuestion).FullName)
            {
                ctrl = new MAQuestionUserControl(question as MAQuestion);
            }
            else if (type.FullName == typeof(FIBQuestion).FullName)
            {
                ctrl = new FIBQuestionUserControl(question as FIBQuestion);
            }
            else if (type.FullName == typeof(CPQuestion).FullName)
            {
                ctrl = new CPQuestionUserControl(question as CPQuestion);
            }

            this.controlPanel.Children.Add(ctrl);
        }

        private void solutionButton_Click(object sender, RoutedEventArgs e)
        {
            SolutionEditWindow solutionEditWindow = new SolutionEditWindow(this.solution);
            if (solutionEditWindow.ShowDialog().Value)
            {
                this.solutionChanged = true;
            }
        }
    }
}
