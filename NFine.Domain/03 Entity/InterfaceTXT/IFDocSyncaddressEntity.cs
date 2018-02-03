using System;

namespace NFine.Domain.Entity.InterfaceTXT
{
    public class IFDocSyncaddressEntity : IEntity<IFDocSyncaddressEntity>, ICreationAudited,  IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_ReAscRptUrl { get; set; }
        public string F_ReStatusRptUrl { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        
    }
}
