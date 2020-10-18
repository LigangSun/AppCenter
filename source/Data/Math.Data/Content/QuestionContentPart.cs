using System;
using System.Collections.Generic;
using System.Text;
using Gadget.Math.Data.Help;
using System.Xml.Serialization;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// QuestionContentPart类是用于存储试题中出现的特殊符号和特殊表达式的类的基类。
    /// </summary>
    [XmlInclude(typeof(QuestionFractionPart))]
    [XmlInclude(typeof(ArithmeticSignPart))]
    [XmlInclude(typeof(ArithmeticDecimalValuePart))]
    [XmlInclude(typeof(ArithmeticFractionValuePart))]
    [XmlInclude(typeof(QuestionFlowDocumentPart))]
    [XmlInclude(typeof(QuestionPowerExponentPart))]
    [XmlInclude(typeof(QuestionPowerPart))]
    [XmlInclude(typeof(QuestionTextPart))]
    public abstract class QuestionContentPart : BaseObject, IComparable<QuestionContentPart>, IComparable, ICloneable
    {
        protected abstract string Prefix
        {
            get;
        }

        /// <summary>
        /// 获取特殊符号的占位符。
        /// </summary>
        public string PlaceHolder
        {
            get { return Helper.CreateQuestionPartPlaceHolder(this.Prefix, this.Id); }
        }

        /// <summary>
        /// QuestionContentPart对象与当前对象进行比较。
        /// </summary>
        /// <param name="other">要比较的对象</param>
        /// <returns>如果当前对象小于要比较的对象返回小于0的值，如果当前对象等于要比较的对象返回0，否则返回大于0的值。</returns>
        public abstract int CompareTo(QuestionContentPart other);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            return this.CompareTo(obj as QuestionContentPart);
        }

        public override bool Equals(object obj)
        {
            return this.CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public abstract object Clone();
    }
}
