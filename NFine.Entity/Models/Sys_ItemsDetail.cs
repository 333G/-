using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class Sys_ItemsDetail
    {
        
        /// <summary>
        /// Desc:明细主键 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:主表主键 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ItemId {get;set;}

        /// <summary>
        /// Desc:父级 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ParentId {get;set;}

        /// <summary>
        /// Desc:编码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ItemCode {get;set;}

        /// <summary>
        /// Desc:名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ItemName {get;set;}

        /// <summary>
        /// Desc:简拼 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_SimpleSpelling {get;set;}

        /// <summary>
        /// Desc:默认 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_IsDefault {get;set;}

        /// <summary>
        /// Desc:层次 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_Layers {get;set;}

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
