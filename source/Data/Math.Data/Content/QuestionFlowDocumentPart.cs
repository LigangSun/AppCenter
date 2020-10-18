using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class QuestionFlowDocumentPart : QuestionContentPart
    {
        private string flowDocument = @"";

        public string FlowDocument
        {
            get { return this.flowDocument; }
            set { this.flowDocument = value; }
        }

        protected override string Prefix
        {
            get { return "_$FLOWDOCUMENT_"; }
        }

        public QuestionFlowDocumentPart()
        {
        }

        public QuestionFlowDocumentPart(string doc)
        {
            this.flowDocument = doc;
        }

        public override int CompareTo(QuestionContentPart other)
        {
            if (!(other is QuestionFlowDocumentPart))
                return -1;

            QuestionFlowDocumentPart flowDocumentPart = other as QuestionFlowDocumentPart;
            return this.FlowDocument.CompareTo(flowDocumentPart.FlowDocument);
        }

        public override object Clone()
        {
            QuestionFlowDocumentPart newPart = new QuestionFlowDocumentPart();
            newPart.Id = this.Id;
            newPart.FlowDocument = this.FlowDocument;

            return newPart;
        }
    }
}
