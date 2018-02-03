using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class Sys_ModuleFormInstance
    {
        
        /// <summary>
        /// Desc:表单实例主键 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:表单主键 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_FormId {get;set;}

        /// <summary>
        /// Desc:对象主键 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ObjectId {get;set;}

        /// <summary>
        /// Desc:表单实例Json 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_InstanceJson {get;set;}

        /// <summary>
        /// Desc:排序码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_SortCode {get;set;}

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
