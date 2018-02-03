using System;

namespace NFine.Domain.Entity.OCManage
{
    public class RechargeRecordEntity : IEntity<RechargeRecordEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_UserId { get; set; }
        public string F_Account { get; set; }
        public decimal F_Price { get; set; }
        public int F_ManagerId { get; set; }
        public int F_OperatorId { get; set; }
        public decimal F_ShowCash { get; set; }
        public decimal F_TrueCash { get; set; }
        public int F_RechargeStar { get; set; }
        public int F_RechargeOver { get; set; }
        public string F_State { get; set; }
        public string F_ShowDescription { get; set; }
        public string F_TrueDescription { get; set; }
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
