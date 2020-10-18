using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.Assessment.Data;

namespace SoonLearning.TeachAppMaker.Data
{
    public class QuestionBankTreeNode : ProjectTreeNode
    {
        private ObservableCollection<ProjectTreeNode> questionTypeTreeNodeCollection = new ObservableCollection<ProjectTreeNode>();

        public ObservableCollection<ProjectTreeNode> Items
        {
            get { return this.questionTypeTreeNodeCollection; }
        }

        public QuestionBankTreeNode()
        {
            this.questionTypeTreeNodeCollection.Add(this.createQuestionTypeTreeNode("单选题", "pack://application:,,,/Resources/Images/MCImage.png", QuestionType.MultiChoice));
            this.questionTypeTreeNodeCollection.Add(this.createQuestionTypeTreeNode("多选题", "pack://application:,,,/Resources/Images/MRImage.png", QuestionType.MultiResponse));
            this.questionTypeTreeNodeCollection.Add(this.createQuestionTypeTreeNode("判断题", "pack://application:,,,/Resources/Images/TFImage.png", QuestionType.TrueFalse));
            this.questionTypeTreeNodeCollection.Add(this.createQuestionTypeTreeNode("填空题", "pack://application:,,,/Resources/Images/FIBImage.png", QuestionType.FillInBlank));
        //    this.questionTypeTreeNodeCollection.Add(this.createQuestionTypeTreeNode("配对题", "pack://application:,,,/Resources/Images/MMImage.png", QuestionType.Match));
            this.questionTypeTreeNodeCollection.Add(this.createQuestionTypeTreeNode("简答题", "pack://application:,,,/Resources/Images/ESImage.png", QuestionType.Essay));
        //    this.questionTypeTreeNodeCollection.Add(this.createQuestionTypeTreeNode("复合题", "pack://application:,,,/Resources/Images/CPImage.png", QuestionType.Composite));
        }

        private QuestionTypeTreeNode createQuestionTypeTreeNode(string name, string image, QuestionType type)
        {
            return new QuestionTypeTreeNode() { Name = name, Image = image, Type = type };
        }
    }
}
