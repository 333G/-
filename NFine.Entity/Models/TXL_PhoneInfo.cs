using System;
using System.Linq;
using System.Text;

namespace NFine.Entity
{
    public class TXL_PhoneInfo
    {
        
        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:手机号码 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string Mobile {get;set;}

        /// <summary>
        /// Desc:姓名 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Name {get;set;}

        /// <summary>
        /// Desc:生日 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? Birthday {get;set;}

        /// <summary>
        /// Desc:性别 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Sex {get;set;}

        /// <summary>
        /// Desc:省 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Province {get;set;}

        /// <summary>
        /// Desc:市 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string City {get;set;}

        /// <summary>
        /// Desc:地址 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string Address {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string Operator {get;set;}

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? State {get;set;}

        /// <summary>
        /// Desc:所属组群组ＩＤ　 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string GroupId {get;set;}

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
