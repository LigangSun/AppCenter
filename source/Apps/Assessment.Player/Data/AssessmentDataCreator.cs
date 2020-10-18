using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoonLearning.Assessment.Data.Bank;
using HTMLConverter;
using SoonLearning.Assessment.Data;
using SoonLearning.Assessment.Player.CommonControl;
using System.Windows.Documents;

namespace SoonLearning.Assessment.Player.Data
{
    internal class AssessmentDataCreator : DataCreator
    {
        private AssessmentApp assessmentApp;

        public AssessmentApp App
        {
            set { this.assessmentApp = value; }
        }

        public AssessmentDataCreator(AssessmentApp app)
        {
            this.assessmentApp = app;
            base.examTitle = app.Name;
            base.exerciseTitle = app.Name;
            base.examDescription = app.Description;
            base.exerciseDescription = app.Description;
            this.PrepareSectionInfoCollection();
        }

        protected override void PrepareSectionInfoCollection()
        {
            if (this.assessmentApp == null)
                return;

            foreach (var question in assessmentApp.Items)
            {
                var query = from temp in base.sectionInfoCollection
                            where temp.QuestionType == question.Type
                            select temp;
                if (query.Count() > 0)
                {
                    query.First().QuestionCount++;
                    query.First().MaxQuestionCount++;
                }
                else
                {
                    SectionBaseInfo info = new SectionBaseInfo(question.Type, this.QuestionType2String(question.Type), string.Empty, 1);
                    info.MaxQuestionCount = 1;
                    base.sectionInfoCollection.Add(info);
                }
            }
        }

        private string QuestionType2String(QuestionType type)
        {
            switch (type)
            {
                case QuestionType.Composite:
                    return "复合题";
                case QuestionType.Essay:
                    return "简答题";
                case QuestionType.FillInBlank:
                    return "填空题";
                case QuestionType.Match:
                    return "配对题";
                case QuestionType.MultiChoice:
                    return "单选题";
                case QuestionType.MultiResponse:
                    return "多选题";
                case QuestionType.Table:
                    return "表格题";
                case QuestionType.TrueFalse:
                    return "判断题";
                case QuestionType.VerticalForm:
                    return "竖式题";
            }

            return string.Empty;
        }

        protected override void AppendQuestion(SectionBaseInfo info, Assessment.Data.Section section)
        {
            var query = from q in this.assessmentApp.Items
                        where q.Type == info.QuestionType
                        select q;

            List<Question> tempList = new List<Question>();
            foreach (var q in query)
            {
                bool found = false;
                foreach (var q1 in section.QuestionCollection)
                {
                    if (q == q1)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    tempList.Add(q);
            }

            if (tempList.Count == 0)
                throw new NoMoreQuestionException();

            Random rand = new Random(Environment.TickCount);
            section.QuestionCollection.Add(tempList[rand.Next(tempList.Count)]);
        }

        internal override System.Windows.Documents.FlowDocument GetKnowledgeDefinition()
        {
            if (this.assessmentApp.Knowledge.Content == null)
                return new System.Windows.Documents.FlowDocument();

            if (this.assessmentApp.Knowledge.Content.ContentType == Assessment.Data.ContentType.Html)
            {
                return HtmlToXamlConverter.ConvertHtmlToXaml(this.assessmentApp.Knowledge.Content.Content);
            }
            else if (this.assessmentApp.Knowledge.Content.ContentType == Assessment.Data.ContentType.FlowDocument)
            {
                FlowDocument doc =  HtmlToXamlConverter.DeserializeFlowDocument(this.assessmentApp.Knowledge.Content.Content);
                CommonControlCreator.replaceTextBoxWithRichTextBox(doc, this.assessmentApp.Knowledge.Content, null);
                return doc;
            }

            throw new NotSupportedException("Knowledge must be HTML or FlowDocument format!");
        }
    }
}
