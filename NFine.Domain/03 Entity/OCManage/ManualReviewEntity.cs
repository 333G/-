using System;

namespace NFine.Domain.Entity.OCManage
{
    public class ManualReviewEntity : IEntity<ManualReviewEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public int F_UserID { get; set; }
        public int F_RootID { get; set; }
        public string F_ParentID { get; set; }
        public string F_RegexContent { get; set; }
        public string F_ChannelId { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public bool? F_Action { get; set; }
    }
}
