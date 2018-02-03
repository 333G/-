using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class SMS_SubTest
    {
        
        /// <summary>
        /// Desc:主键，自增长 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:测试表（SMS_Test主键） 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? Test_Id {get;set;}

        /// <summary>
        /// Desc:子表测试名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string SubTestName {get;set;}

        /// <summary>
        /// Desc:子表测试描述 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string SubTestDescription {get;set;}

        /// <summary>
        /// Desc:创建时间（默认当前时间） 
        /// Default:(getdate()) 
        /// Nullable:True 
        /// </summary>
        public DateTime? CreateTime {get;set;}

        /// <summary>
        /// Desc:创建人 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Creator {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string TestSubTitle {get;set;}

    }
}
