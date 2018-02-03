using System;

namespace NFine.Domain.Entity.OCManage
{

    public class SensitiveWordsEntity : IEntity<SensitiveWordsEntity>, ICreationAudited, IModificationAudited
    {

        public string F_Id { get; set; }

        public string F_SensitiveWords { get; set; }

        public DateTime? F_CreatorTime { get; set; }

        public DateTime? F_LastModifyTime { get; set; }

        public DateTime? F_DeleteTime { get; set; }

        public string F_CreatorUserId { get; set; }

        public string F_LastModifyUserId { get; set; }

        public string F_DeleteUserId { get; set; }

        /// <summary>
        /// Desc:是否是通道关键字0否，1是。0表示系统关键字
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public bool F_IsChannelKeyWord { get; set; }

        /// <summary>
        /// Desc:通道关键字的通道Id
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int F_ChannelId { get; set; }

    }
}