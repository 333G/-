using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class SMS_Test
    {
        
        /// <summary>
        /// Desc:主键，自增长 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public int ID {get;set;}

        /// <summary>
        /// Desc:测试名 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string TestName {get;set;}

        /// <summary>
        /// Desc:测试描述 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string TestDescription {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string TestCode {get;set;}

        /// <summary>
        /// Desc:测试状态（默认是1，0=禁用；1=启用；） 
        /// Default:((1)) 
        /// Nullable:True 
        /// </summary>
        public int? TestState {get;set;}

        /// <summary>
        /// Desc:创建时间（默认当前时间） 
        /// Default:(getdate()) 
        /// Nullable:True 
        /// </summary>
        public DateTime? CreateTime {get;set;}

        /// <summary>
        /// Desc:创建人ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? CreateUserId {get;set;}

        /// <summary>
        /// Desc:创建人账号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Creator {get;set;}

        /// <summary>
        /// Desc:最后编辑时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? LastEditTime {get;set;}

        /// <summary>
        /// Desc:最后编辑人账号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string LastEditor {get;set;}

        /// <summary>
        /// Desc:删除时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? DeleteTime {get;set;}

        /// <summary>
        /// Desc:删除人账号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Deletor {get;set;}

    }
}
