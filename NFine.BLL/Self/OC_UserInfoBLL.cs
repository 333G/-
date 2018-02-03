using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NFine.Entity;
using System.Linq;

namespace NFine.BLL.Self
{
    public class OC_UserInfoBLL
    {
        #region 单例模式
        private static OC_UserInfoBLL instance;
        private static object _lock = new object();

        private OC_UserInfoBLL()
        {
        }

        public static OC_UserInfoBLL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new OC_UserInfoBLL();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion 单例模式

        public List<NFine.Entity.Views.VOC_UserInfo> GetReceiveList(Pagination pagination, string queryJson)
         {
            var expression_User = ExtLinq.True<OC_UserInfo>();
            var expression_Channel = ExtLinq.True<OC_GroupChannel>();
            var expression_SysUser = ExtLinq.True<Sys_User>();
            
            
            var queryParam = queryJson.ToJObject();
            
            if (!queryParam["F_UserId"].IsEmpty())
            {
                int F_UserId = queryParam["F_UserId"].ToInt();
                expression_User = expression_User.And(t => t.F_UserId == F_UserId);
            }
            if (!queryParam["F_RootId"].IsEmpty())
            {
                int F_RootId = queryParam["F_RootId"].ToInt();
                expression_User = expression_User.And(t => t.F_RootId == F_RootId);
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression_User = expression_User.And(t => t.F_UserId.ToString().Contains(keyvalue));
                expression_User = expression_User.Or(t => t.F_Account.Contains(keyvalue));
                expression_User = expression_User.Or(t => t.F_RootId.ToString().Contains(keyvalue));
                expression_User = expression_User.Or(t => t.F_ManagerId.ToString().Contains(keyvalue));
                //密码字段   expression = expression.Or(t => t.Passway.Contains(keyvalue));
                expression_User = expression_User.Or(t => t.F_SendedNum.ToString().Contains(keyvalue));
                expression_User = expression_User.Or(t => t.F_Balance.ToString().Contains(keyvalue));
                //expression = expression.Or(t => t.F_Reviewed.ToString().Contains((keyvalue));//审核不模糊查询
                // expression = expression.Or(t => t.F_State.Contains(keyvalue));//状态不需要模糊查询
                //计费字段   expression = expression.Or(t => t.Charging.Contains(keyvalue));
                //匹配字段   expression = expression.Or(t => t.Matching.Contains(keyvalue));
                expression_User = expression_User.Or(t => t.F_Description.Contains(keyvalue));

            }
            if (!queryParam["F_BalanceMin"].IsEmpty())
            {
                decimal Min =(decimal)queryParam["F_BalanceMin"];
                expression_User = expression_User.And(t => (t.F_Balance >= Min));
            }
            if (!queryParam["F_BalanceMax"].IsEmpty())
            {
                decimal Max =(decimal)queryParam["F_BalanceMax"];
                expression_User = expression_User.And(t => t.F_Balance <= Max);
            }
            //运营商、通道、签名字段查询待定*********************************其他表中***********************************************
            if (!queryParam["carrieroperator"].IsEmpty())//运营商
            {
                string F_Operator = queryParam["carrieroperator"].ToString();
                //expression=expression.And(t=>t.F_)//在别的表中
            }
            if (!queryParam["F_ChannelId"].IsEmpty())//通道
            {
                int F_ChannelId = queryParam["F_ChannelId"].ToInt();
                expression_Channel = expression_Channel.And(t => t.F_MobileChannel == F_ChannelId);
                expression_Channel = expression_Channel.Or(t => t.F_UnicomChannel == F_ChannelId);
                expression_Channel = expression_Channel.Or(t => t.F_TelecomChannel == F_ChannelId);
            }
            if (!queryParam["signature"].IsEmpty())
            {
                string F_signature = queryParam["signature"].ToString();
                expression_SysUser = expression_SysUser.And(t => t.F_Signature.Equals(F_signature));
            }
            //************************************************************************************************************************
            if (!queryParam["F_Reviewed"].IsEmpty())
            {
                int F_Reviewed = queryParam["F_Reviewed"].ToInt();
                expression_User = expression_User.And(t => t.F_Reviewed == (F_Reviewed));
            }
            if (!queryParam["F_State"].IsEmpty())
            {
                string F_State = queryParam["F_State"].ToString();
                expression_User = expression_User.And(t => t.F_State.Equals(F_State));
            }
            return OC_UserInfoManager.Instance.GetPageList(expression_User, expression_Channel, expression_SysUser, pagination);
        }

    }
}
