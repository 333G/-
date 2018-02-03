using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class Sys_Module
    {
        
        /// <summary>
        /// Desc:模块主键 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:父级 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ParentId {get;set;}

        /// <summary>
        /// Desc:层次 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_Layers {get;set;}

        /// <summary>
        /// Desc:编码 
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
        /// Desc:图标 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Icon {get;set;}

        /// <summary>
        /// Desc:连接 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_UrlAddress {get;set;}

        /// <summary>
        /// Desc:目标 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Target {get;set;}

        /// <summary>
        /// Desc:菜单 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_IsMenu {get;set;}

        /// <summary>
        /// Desc:展开 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_IsExpand {get;set;}

        /// <summary>
        /// Desc:公共 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_IsPublic {get;set;}

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
        /// Desc:创建日期 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_CreatorTime {get;set;}

        /// <summary>
        /// Desc:创建用户主键 
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
