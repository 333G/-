using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class Sys_Log
    {
        
        /// <summary>
        /// Desc:日志主键 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:日期 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_Date {get;set;}

        /// <summary>
        /// Desc:用户名 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Account {get;set;}

        /// <summary>
        /// Desc:姓名 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_NickName {get;set;}

        /// <summary>
        /// Desc:类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Type {get;set;}

        /// <summary>
        /// Desc:IP地址 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_IPAddress {get;set;}

        /// <summary>
        /// Desc:IP所在城市 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_IPAddressName {get;set;}

        /// <summary>
        /// Desc:系统模块Id 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ModuleId {get;set;}

        /// <summary>
        /// Desc:系统模块 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ModuleName {get;set;}

        /// <summary>
        /// Desc:结果 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_Result {get;set;}

        /// <summary>
        /// Desc:描述 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Description {get;set;}

        /// <summary>
        /// Desc:创建时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_CreatorTime {get;set;}

        /// <summary>
        /// Desc:创建用户 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_CreatorUserId {get;set;}

    }
}
