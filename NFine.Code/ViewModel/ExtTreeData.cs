using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFine.Code.ViewModel
{
    /// <summary>
    /// 所有对EXT树数据绑定相关的json对象都要继承的抽象类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ExtTreeData<T>
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 显示文本
        /// </summary>
        public string text { get; set; }

        /// <summary>
        /// icon
        /// </summary>
        public string iconCls { get; set; }

        /// <summary>
        /// 叶子
        /// </summary>
        public bool leaf { get; set; }

        /// <summary>
        /// 儿子们
        /// </summary>
        public virtual IList<T> children { get; set; }

        /// <summary>
        /// 多少级
        /// </summary>
        public virtual int level { get; set; }
    }
}
