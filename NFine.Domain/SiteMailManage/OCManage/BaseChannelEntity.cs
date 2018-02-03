using System;

namespace NFine.Domain.Entity.OCManage
{

    public class BaseChannelEntity : IEntity<BaseChannelEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public int Id { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_ChannelName { get; set; }
        public int F_Operator { get; set; }
        public int? F_ChannelState { get; set; }
        public Boolean? F_IsShared { get; set; }
        public string F_SharedSign { get; set; }
        public TimeSpan F_StartTime { get; set; }
        public TimeSpan F_EndTime { get; set; }
        public decimal? F_Price { get; set; }
        public decimal? F_ChaBalance { get; set; }
        public int? F_SurplusNum { get; set; }
        public int? F_SendedNum { get; set; }
        public int F_autograph { get; set; }
        public int F_unsubscribe { get; set; }
        public int? F_CharacterCount { get; set; }
        public string F_Description { get; set; }
        public Boolean? F_DeleteMark { get; set; }
        public Boolean? F_EnabledMark { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public Boolean? F_MonitorState { get; set; }
        public int? F_MonitorTime { get; set; }
        public string F_MonitorMobile { get; set; }
        public int? F_ChargeNumber { get; set; }
        public Boolean? F_LongSmsSign { get; set; }
        public int? F_LongSmsNumber { get; set; }
        public DateTime? F_LastSendTime { get; set; }
    }
}
