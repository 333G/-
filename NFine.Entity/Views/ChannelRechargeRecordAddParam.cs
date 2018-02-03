using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Entity.Views
{
    /// <summary>
    /// 添加充值
    /// </summary>
    public class ChannelRechargeRecordAddParam
    {
        /// <summary>
        /// Desc:编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id { get; set; }

        /// <summary>
        /// Desc:通道ID 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_ChannleId { get; set; }

        /// <summary>
        /// Desc:充值金额 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Decimal F_recharge { get; set; }

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
