using System;


namespace NFine.Entity.Views
{
    public class VOC_UserInfo
    {
        
        /// <summary>
        /// Desc:编号 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public string F_Id {get;set;}

        /// <summary>
        /// Desc:账号GUID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_UserFid {get;set;}

        /// <summary>
        /// Desc:账号ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_UserId {get;set;}

        /// <summary>
        /// Desc:祖宗账号ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_RootId {get;set;}

        /// <summary>
        /// Desc:业务员账号ID 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_ManagerId {get;set;}

        /// <summary>
        /// Desc:账号 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Account {get;set;}

        /// <summary>
        /// Desc:发送数量 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_SendedNum {get;set;}

        /// <summary>
        /// Desc:余额 
        /// Default:- 
        /// Nullable:False 
        /// </summary>
        public Decimal F_Balance {get;set;}

        /// <summary>
        /// Desc:审核 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public int? F_Reviewed {get;set;}

        /// <summary>
        /// Desc:状态 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_State {get;set;}

        /// <summary>
        /// Desc:删除标记 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_DeleteMark {get;set;}

        /// <summary>
        /// Desc:可用标记 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public Boolean? F_EnabledMark {get;set;}

        /// <summary>
        /// Desc:备注 
        /// Default:- 
        /// Nullable:True 
        /// </summary>
        public string F_Description {get;set;}

        /// <summary>
        /// 通道Id,移动，联通，电信
        /// </summary>
        public int? F_MobileChannel { get; set; }
        public int? F_UnicomChannel { get; set; }
        public int? F_TelecomChannel { get; set; }
        public string F_ChannelName { get; set; }
        public string F_ChannelType { get; set; }

        public string Signature { get; set; }
        public Boolean? F_BalanceReminder { get; set; }     
        public int? F_MessageNum { get; set; }     
        public int? F_OneCode { get; set; }
        public int? F_TwentyFourCode { get; set; }

    }
}
