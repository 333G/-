/*-------------------------------------------------------------------------
 * 作者：NeoLu
 * Email：1113865828@qq.com
 * github:https://github.com/Neo-Lu
 * 创建时间： 2016/11/10 9:43:54
 * 版本号：v1.0
 * 本类主要用途描述：
 *  -------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Entity.Views
{
    public class VTest
    {
        /// <summary>
        /// Desc:主键，自增长 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Desc:测试表（SMS_Test主键） 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? Test_Id { get; set; }

        /// <summary>
        /// Desc:子表测试名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string SubTestName { get; set; }

        /// <summary>
        /// Desc:子表测试描述 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string SubTestDescription { get; set; }

        /// <summary>
        /// Desc:创建时间（默认当前时间） 
        /// Default:(getdate()) 
        /// Nullable:True 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// Desc:创建人 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Creator { get; set; }
        /// <summary>
        /// 测试名称
        /// </summary>
        public string TestName { get; set; }
        /// <summary>
        /// Desc:测试描述 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string TestDescription { get; set; }
    }
}
