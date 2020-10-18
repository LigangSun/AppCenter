using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// QuestionBlankCollection类是用于存储QuestionBlank的列表。
    /// </summary>
    public class QuestionBlankCollection : CollectionBase
    {
        /// <summary>
        /// 添加一个QuestionBlank实例到列表。
        /// </summary>
        /// <param name="item">QuestionBlank实例</param>
        public void Add(QuestionBlank item)
        {
            base.InnerList.Add(item);
        }

        /// <summary>
        /// 添加多个QuestionBlank实例到列表。
        /// </summary>
        /// <param name="items">多个QuestionBlank实例</param>
        public void AddRange(ICollection items)
        {
            base.InnerList.AddRange(items);
        }

        /// <summary>
        /// 插入一个QuestionBlank实例到列表。
        /// </summary>
        /// <param name="index">插入的位置</param>
        /// <param name="item">QuestionBlank实例</param>
        public void Insert(int index, QuestionBlank item)
        {
            base.InnerList.Insert(index, item);
        }

        /// <summary>
        /// 删除一个QuestionBlank实例。
        /// </summary>
        /// <param name="item">要删除的QuestionBlank实例</param>
        public void Remove(QuestionBlank item)
        {
            base.InnerList.Remove(item);
        }

        /// <summary>
        /// 删除某个位置上的QuestionBlank实例
        /// </summary>
        /// <param name="index">要删除的位置</param>
        public void RemoveAt(int index)
        {
            base.InnerList.RemoveAt(index);
        }

        /// <summary>
        /// 从指定位置开始删除多个QuestionBlank实例
        /// </summary>
        /// <param name="index">要删除的开始位置。</param>
        /// <param name="count">要删除的实例个数。</param>
        public void RemoveRange(int index, int count)
        {
            base.InnerList.RemoveRange(index, count);
        }

        /// <summary>
        /// 获取或设置QuestionBlank实例
        /// </summary>
        /// <param name="index">获取或设置的位置。</param>
        /// <returns>返回QuestionBlank实例。</returns>
        public QuestionBlank this[int index]
        {
            get { return base.InnerList[index] as QuestionBlank; }
            set { base.InnerList[index] = value; }
        }
    }
}
