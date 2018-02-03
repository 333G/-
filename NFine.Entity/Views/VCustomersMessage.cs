using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 

namespace NFine.Entity.Views
{
    public class VCustomersMessage
    {
        public long? CRMID { get; set; }
        public string customerId { get; set; }//客户ID
        public string customerInformation { get; set; }//客户信息
        public string customerStateId { get; set; }//客户等级
        public string Record { get; set; }//记录
        public string RecordId { get; set; }//记录ID

        public string Record1 { get; set; }//记录1
        public string Record1Id { get; set; }//记录1ID
        public string Record2 { get; set; }//记录2
        public string Record2Id { get; set; }//记录2ID
        public string Record3 { get; set; }//记录3
        public string Record3Id { get; set; }//记录3ID
        public string Record4 { get; set; }//记录4
        public string Record4Id { get; set; }//记录4ID

        public string F_CustInfo { get; set; }
        
        /// <summary>
        /// Desc:记录
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Record { get; set; }
        /// <summary>
        /// Desc:客户信息 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        

        /// <summary>
        /// Desc:客户手机号码 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_MobilePhone { get; set; }

        /// <summary>
        /// Desc:提醒时间 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_CallTime { get; set; }

        /// <summary>
        /// Desc:客户等级 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_StateId { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_SortCode { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_DeleteMark { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_EnabledMark { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Description { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_CreatorTime { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_CreatorUserId { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_LastModifyTime { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_LastModifyUserId { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public DateTime? F_DeleteTime { get; set; }

        /// <summary>
        /// Desc:- 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_DeleteUserId { get; set; }

    }
}
