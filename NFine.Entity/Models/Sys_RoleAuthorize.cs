using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class Sys_RoleAuthorize
    {
        
        /// <summary>
        /// Desc:角色授权主键 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:项目类型1-模块2-按钮3-列表 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_ItemType {get;set;}

        /// <summary>
        /// Desc:项目主键 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ItemId {get;set;}

        /// <summary>
        /// Desc:对象分类1-角色2-部门-3用户 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_ObjectType {get;set;}

        /// <summary>
        /// Desc:对象主键 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_ObjectId {get;set;}

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
