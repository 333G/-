using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class Sys_Organize
    {
        /// <summary>
        /// Desc:组织主键 
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
        /// Desc:简称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ShortName {get;set;}

        /// <summary>
        /// Desc:分类 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_CategoryId {get;set;}

        /// <summary>
        /// Desc:负责人 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ManagerId {get;set;}

        /// <summary>
        /// Desc:电话 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_TelePhone {get;set;}

        /// <summary>
        /// Desc:手机 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_MobilePhone {get;set;}

        /// <summary>
        /// Desc:微信 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_WeChat {get;set;}

        /// <summary>
        /// Desc:传真 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Fax {get;set;}

        /// <summary>
        /// Desc:邮箱 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Email {get;set;}

        /// <summary>
        /// Desc:归属区域 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_AreaId {get;set;}

        /// <summary>
        /// Desc:联系地址 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Address {get;set;}

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
