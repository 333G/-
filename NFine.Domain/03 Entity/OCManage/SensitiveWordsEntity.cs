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
        public bool F_IsChannelKeyWord { get; set; }
        public int? F_ChannelId { get; set; }

    }
}