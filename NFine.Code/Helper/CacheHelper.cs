using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFine.Code
{
    /// <summary>
    /// 缓存操作
    /// </summary>
    public class CacheHelper
    {
        private static volatile CacheHelper instance = null;
        private static object lockHelper = new object();

        /// <summary>
        /// 实例
        /// </summary>
        public static CacheHelper GetInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockHelper)
                    {
                        if (instance == null)
                        {
                            instance = new CacheHelper();
                        }
                    }
                }

                return instance;
            }
        }


        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        public void RemoveCache(string key)
        {
            
        }
    }
}
