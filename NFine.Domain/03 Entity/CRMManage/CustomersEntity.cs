using System;

namespace NFine.Domain.Entity.CRMManage
{
    public class CustomersEntity : IEntity<CustomersEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_CustInfo { get; set; }
        public string F_MobilePhone { get; set; }
        public DateTime? F_CallTime { get; set; }
        public string F_StateId { get; set; }
        public int F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public long? F_CrmId { get; set;}
    }
}
