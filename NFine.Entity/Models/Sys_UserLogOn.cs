using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class Sys_UserLogOn
    {
        
        /// <summary>
        /// Desc:用户登录主键 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:用户主键 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_UserId {get;set;}

        /// <summary>
        /// Desc:用户密码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_UserPassword {get;set;}

        /// <summary>
        /// Desc:用户秘钥 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_UserSecretkey {get;set;}

        /// <summary>
        /// Desc:允许登录时间开始 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_AllowStartTime {get;set;}

        /// <summary>
        /// Desc:允许登录时间结束 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_AllowEndTime {get;set;}

        /// <summary>
        /// Desc:暂停用户开始日期 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_LockStartDate {get;set;}

        /// <summary>
        /// Desc:暂停用户结束日期 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_LockEndDate {get;set;}

        /// <summary>
        /// Desc:第一次访问时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_FirstVisitTime {get;set;}

        /// <summary>
        /// Desc:上一次访问时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_PreviousVisitTime {get;set;}

        /// <summary>
        /// Desc:最后访问时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_LastVisitTime {get;set;}

        /// <summary>
        /// Desc:最后修改密码日期 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_ChangePasswordDate {get;set;}

        /// <summary>
        /// Desc:允许同时有多用户登录 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_MultiUserLogin {get;set;}

        /// <summary>
        /// Desc:登录次数 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_LogOnCount {get;set;}

        /// <summary>
        /// Desc:在线状态 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_UserOnLine {get;set;}

        /// <summary>
        /// Desc:密码提示问题 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Question {get;set;}

        /// <summary>
        /// Desc:密码提示答案 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_AnswerQuestion {get;set;}

        /// <summary>
        /// Desc:是否访问限制 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_CheckIPAddress {get;set;}

        /// <summary>
        /// Desc:系统语言 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Language {get;set;}

        /// <summary>
        /// Desc:系统样式 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Theme {get;set;}

    }
}
