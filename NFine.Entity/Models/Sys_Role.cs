using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class Sys_Role
    {
        
        /// <summary>
        /// Desc:角色主键 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:组织主键 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_OrganizeId {get;set;}

        /// <summary>
        /// Desc:分类:1-角色2-岗位 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_Category {get;set;}

        /// <summary>
        /// Desc:编号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_EnCode {get;set;}

        /// <summary>
        /// Desc:名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_FullName {get;set;}

        /// <summary>
        /// Desc:类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Type {get;set;}

        /// <summary>
        /// Desc:允许编辑 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_AllowEdit {get;set;}

        /// <summary>
        /// Desc:允许删除 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_AllowDelete {get;set;}

        /// <summary>
        /// Desc:排序码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_SortCode {get;set;}

        /// <summary>
        /// Desc:删除标志 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_DeleteMark {get;set;}

        /// <summary>
        /// Desc:有效标志 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_EnabledMark {get;set;}

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

        /// <summary>
        /// Desc:最后修改时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_LastModifyTime {get;set;}

        /// <summary>
        /// Desc:最后修改用户 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_LastModifyUserId {get;set;}

        /// <summary>
        /// Desc:删除时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_DeleteTime {get;set;}

        /// <summary>
        /// Desc:删除用户 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_DeleteUserId {get;set;}

    }
}
