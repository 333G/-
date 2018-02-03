/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/11/11 13:31:57
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.BLL.Self
{
    public class TestBLL
    {
        #region 单例模式
        private static TestBLL instance;
        private static object _lock = new object();

        private TestBLL()
        {
        }

        public static TestBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new TestBLL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式
        /// <summary>
        /// 多表联查例子
        /// </summary>
        /// <returns></returns>
        public List<Entity.Views.VTest> MultipleTable()
        {
            return DAL.Self.TestDAL.Instance.MultipleTable();
        }
    }
}
