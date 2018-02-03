using System;

namespace NFine.Domain.Entity.OCManage
{
    public class AutoReviewEntity : IEntity<AutoReviewEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public int F_UserID { get; set; }
        public int F_RootID { get; set; }
        public string F_ParentID { get; set; }
        public string F_SourceSms { get; set; }
        public string F_Analysis { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
    }
}
