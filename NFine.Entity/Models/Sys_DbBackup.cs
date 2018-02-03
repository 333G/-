using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class Sys_DbBackup
    {
        
        /// <summary>
        /// Desc:备份主键 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:备份类型 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_BackupType {get;set;}

        /// <summary>
        /// Desc:数据库名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_DbName {get;set;}

        /// <summary>
        /// Desc:文件名称 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_FileName {get;set;}

        /// <summary>
        /// Desc:文件大小 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_FileSize {get;set;}

        /// <summary>
        /// Desc:文件路径 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_FilePath {get;set;}

        /// <summary>
        /// Desc:备份时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_BackupTime {get;set;}

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
