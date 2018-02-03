using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class CRM_Task
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:任务信息 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_TaskInfo {get;set;}

        /// <summary>
        /// Desc:手机号码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_MobilePhone {get;set;}

        /// <summary>
        /// Desc:完成时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_TaskTime {get;set;}

        /// <summary>
        /// Desc:处理人 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_DoUser {get;set;}

        /// <summary>
        /// Desc:完成进度 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_StatId {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_SortCode {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_DeleteMark {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_EnabledMark {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Description {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_CreatorTime {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_CreatorUserId {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_LastModifyTime {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_LastModifyUserId {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_DeleteTime {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_DeleteUserId {get;set;}

    }
}
