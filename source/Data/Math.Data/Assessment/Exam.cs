using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    /// <summary>
    /// Exam类是创建考试的类。
    /// </summary>
    public class Exam : Exercise
    {
        /// <summary>
        /// 获取或设置该测验的时限。
        /// 单位为秒。
        /// </summary>
        public int Duration
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置该测验是否为闭卷。
        /// 默认为false
        /// </summary>
        public bool CloseBook
        {
            get;
            set;
        }
    }
}
