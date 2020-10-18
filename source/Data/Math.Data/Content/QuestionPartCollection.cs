using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Xml.Serialization;

namespace SoonLearning.Assessment.Data
{
    public delegate void QuestionContentPartRemovedDelegate(QuestionContentPart part);
    public delegate void QuestionContentPartAddedDelegate(QuestionContentPart part);

    [XmlInclude(typeof(ArithmeticDecimalValuePart))]
    [XmlInclude(typeof(ArithmeticFractionValuePart))]
    [XmlInclude(typeof(ArithmeticSignPart))]
    [XmlInclude(typeof(QuestionBlank))]
    [XmlInclude(typeof(QuestionTextPart))]
    [XmlInclude(typeof(QuestionFlowDocumentPart))]
    [XmlInclude(typeof(QuestionPowerPart))]
    [XmlInclude(typeof(QuestionPowerExponentPart))]
    public class QuestionPartCollection : List<QuestionContentPart>
    {
        public event QuestionContentPartAddedDelegate QuestionContentPartAddedEvent;
        public event QuestionContentPartRemovedDelegate QuestionContentPartRemovedEvent;

        public void Add(QuestionContentPart part)
        {
            base.Add(part);
            this.invokeQuestionContentPartAddedEvent(part);
        }

        public void AddRange(IEnumerable<QuestionContentPart> parts)
        {
            base.AddRange(parts);
            foreach (var part in parts)
                this.invokeQuestionContentPartAddedEvent(part);
        }

        public void Remove(QuestionContentPart part)
        {
            base.Remove(part);
            this.invokeQuestionContentPartRemovedEvent(part);
        }

        public void RemoveRange(int index, int count)
        {
            for (int i = index; i < index + count; i++)
                this.invokeQuestionContentPartRemovedEvent(this[i]);
            base.RemoveRange(index, count);
        }

        public QuestionContentPart this[int index]
        {
            get { return base[index] as QuestionContentPart; }
            set { base[index] = value; }
        }

        private void invokeQuestionContentPartAddedEvent(QuestionContentPart newPart)
        {
            QuestionContentPartAddedDelegate temp = this.QuestionContentPartAddedEvent;
            if (temp != null)
                temp(newPart);
        }

        private void invokeQuestionContentPartRemovedEvent(QuestionContentPart removedPart)
        {
            QuestionContentPartRemovedDelegate temp = this.QuestionContentPartRemovedEvent;
            if (temp != null)
                temp(removedPart);
        }
    }
}
