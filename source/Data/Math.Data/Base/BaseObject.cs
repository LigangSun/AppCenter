using System;
using System.Collections.Generic;
using System.Text;
using Gadget.Math.Data.Help;
using System.ComponentModel;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// BaseObject类教学相关的数据结构中所有类的基类。
    /// </summary>
    public abstract class BaseObject : IEquatable<BaseObject>, INotifyPropertyChanged
    {
        private string id;

        /// <summary>
        /// 获取或设置对象的Id，该Id需唯一。
        /// </summary>
        public string Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        /// <summary>
        /// 初始化BaseObject类的新实例。
        /// </summary>
        public BaseObject()
        {
            this.id = Helper.NewId();
        }

        /// <summary>
        /// 确定两个BaseObject对象是否相等。
        /// </summary>
        /// <param name="other">与当前BaseObject对象进行比较的BaseObject对象。</param>
        /// <returns>如果指定的BaseObject对象等于当前的BaseObject对象，则为true，否则为false。</returns>
        public bool Equals(BaseObject other)
        {
            return Helper.IsEqual(Id, other.Id);
        }

        /// <summary>
        /// 重载了基类的Equals方法，确定两个object对象是否相等。
        /// </summary>
        /// <param name="obj">与当前object对象进行比较的object对象。</param>
        /// <returns>如果指定的object对象等于当前的object对象，则为true，否则为false。</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is BaseObject))
                return false;

            return this.Equals(obj as BaseObject);
        }

        /// <summary>
        /// 用作特定类型的哈希函数。
        /// </summary>
        /// <returns>当前BaseObject的哈希代码。</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
