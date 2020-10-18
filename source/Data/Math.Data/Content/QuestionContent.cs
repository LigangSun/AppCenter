using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// QuestionContent类用于存储试题的内容。
    /// </summary>
    public class QuestionContent : BaseObject, IComparable, IComparable<QuestionContent>, ICloneable
    {
        private QuestionPartCollection quPartList = new QuestionPartCollection();
        private string content = string.Empty;

        /// <summary>
        /// 获取QuestionContentPart的列表
        /// </summary>
        public QuestionPartCollection QuestionPartCollection
        {
            get { return this.quPartList; }
        }

        /// <summary>
        /// 获取或设置QuestionContent的内容，该内容为字符串，字符串中包含特殊符号或表达式的替代符。
        /// </summary>
        public string Content
        {
            get { return this.content; }
            set
            {
                this.content = value;
                base.OnPropertyChanged("Content");
            }
        }

        /// <summary>
        /// 获取或设置试题内容的文本类型。
        /// Text: 普通文本。
        /// FlowDocument: WPF的FlowDocument格式文本。
        /// Html: Html格式文本。
        /// Rtf: Rich Text Format格式文本。
        /// </summary>
        public ContentType ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// 初始化QuestionContent的新实例
        /// </summary>
        public QuestionContent()
        {
            this.Content = string.Empty;
            this.ContentType = ContentType.Text;
        }

        /// <summary>
        /// 初始化QuestionContent的新实例
        /// </summary>
        /// <param name="content">试题内容</param>
        /// <param name="type">试题内容格式</param>
        public QuestionContent(string content, ContentType type)
        {
            this.Content = content;
            this.ContentType = type;
        }

        /// <summary>
        /// 根据特殊符号或者表达式的占位符获取QuestionContentPart对象。
        /// </summary>
        /// <param name="holder">占位符。</param>
        /// <returns>返回QuestionContentPart对象。</returns>
        public QuestionContentPart GetContentPart(string holder)
        {
            foreach (QuestionContentPart part in quPartList)
            {
                if (part.PlaceHolder == holder)
                    return part;
            }

            return null;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is QuestionContent))
                return -1;

            return this.CompareTo(obj as QuestionContent);
        }

        public int CompareTo(QuestionContent other)
        {
            if (this.Content != other.Content)
                return this.Content.Length - other.Content.Length;

            if (this.QuestionPartCollection.Count != other.QuestionPartCollection.Count)
                return this.QuestionPartCollection.Count - other.QuestionPartCollection.Count;

            for (int i = 0; i < this.QuestionPartCollection.Count; i++)
            {
                QuestionContentPart part = this.QuestionPartCollection[i];
                QuestionContentPart partOther = other.QuestionPartCollection[i];
                int ret = part.CompareTo(partOther);
                if (ret != 0)
                    return ret;
            }

            return 0;
        }

        public object Clone()
        {
            QuestionContent newContent = new QuestionContent();
            newContent.Id = this.Id;
            newContent.Content = this.Content;
            newContent.ContentType = this.ContentType;
            foreach (QuestionContentPart part in this.QuestionPartCollection)
            {
                QuestionContentPart newPart = part.Clone() as QuestionContentPart;
                newContent.QuestionPartCollection.Add(newPart);
            }

            return newContent;
        }
    }
}
