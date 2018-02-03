using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;
using System;
using System.Collections.Generic;
using System.Data;
using NFine.Entity;
using System.Text;

namespace NFine.Application.OCManage
{
    public class UserInfoApp
    {
        private IUserInfoRepository service = new UserInfoRepository();

        ////返回用户信息列表
        public List<UserInfoEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<UserInfoEntity>();
            var queryParam = queryJson.ToJObject();
            //查询条件 部分字段待定
            if (!queryParam["F_UserId"].IsEmpty())
            {
                int F_UserId = queryParam["F_UserId"].ToInt();
                expression = expression.And(t => t.F_UserId == F_UserId);
            }
            if (!queryParam["F_RootId"].IsEmpty())
            {
                int F_RootId = queryParam["F_RootId"].ToInt();
                expression = expression.And(t => t.F_RootId == F_RootId);
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(
                    expression.And(t => t.F_UserId.ToString().Contains(keyvalue))
                    .Or(t => t.F_Account.Contains(keyvalue))
                    .Or(t => t.F_RootId.ToString().Contains(keyvalue))
                    .Or(t => t.F_ManagerId.ToString().Contains(keyvalue))
                    .Or(t => t.F_SendedNum.ToString().Contains(keyvalue))
                    .Or(t => t.F_Balance.ToString().Contains(keyvalue))
                    .Or(t => t.F_Description.Contains(keyvalue))
                    );
            }
            if (!queryParam["F_BalanceMin"].IsEmpty())
            {
                int Min = queryParam["F_BalanceMin"].ToInt();
                expression = expression.And(t => (t.F_Balance >= Min));
            }
            if (!queryParam["F_BalanceMax"].IsEmpty())
            {
                int Max = queryParam["F_BalanceMax"].ToInt();
                expression = expression.And(t => t.F_Balance <= Max);
            }
            if (!queryParam["F_Reviewed"].IsEmpty())
            {
                int F_Reviewed = queryParam["F_Reviewed"].ToInt();
                expression = expression.And(t => t.F_Reviewed == (F_Reviewed));
            }
            if (!queryParam["F_State"].IsEmpty())
            {
                string F_State = queryParam["F_State"].ToString();
                expression = expression.And(t => t.F_State.Equals(F_State));
            }
            List<UserInfoEntity> list = service.FindList(expression, pagination);//获取UserInfo表中的条件查询得出的List。

            List<UserInfoEntity> newChannelIdlist = new List<UserInfoEntity>();//新建list，来存储条件查询结果得到的list
            List<UserInfoEntity> newSignaturelist = new List<UserInfoEntity>();//新建list，来存储条件查询结果得到的list
            List<UserInfoEntity> newlist = new List<UserInfoEntity>();//新建list，来存储条件查询结果得到的list

            //运营商、通道、签名字段查询*********************************其他表中*************************************************附加查询**********************************
            int flag = 0;//标记位，判断是否进入了附加查询
            if (!queryParam["carrieroperator"].IsEmpty())//运营商
            {
                string F_Operator = queryParam["carrieroperator"].ToString();
                 List<OC_GroupChannel> data = new List<OC_GroupChannel>();
                if (F_Operator == "1")
                    data = DAL.OC_GroupChannelDAL.Instance.FindList(t => t.F_MobileChannel != null);
                else if (F_Operator == "2")
                    data = DAL.OC_GroupChannelDAL.Instance.FindList(t => t.F_UnicomChannel != null);
                else if (F_Operator == "3")
                    data = DAL.OC_GroupChannelDAL.Instance.FindList(t => t.F_TelecomChannel != null);
                else if (F_Operator == "4")
                    data = DAL.OC_GroupChannelDAL.Instance.FindList(t => t.F_ChannelXLT != null);

                foreach (var item in data)
                {
                    foreach (var entity in list)//如果原来的list中用户通道包含运营商，则添加到新的列表中
                    {
                        if (item.F_UserId == entity.F_UserId && !newlist.Contains(entity))
                            newlist.Add(entity);
                    }
                }
                flag++;
            }

            if (!queryParam["F_ChannelId"].IsEmpty())//通道
            {
                int F_ChannelId = queryParam["F_ChannelId"].ToInt();
                List<OC_GroupChannel> data = new List<OC_GroupChannel>();
                data = DAL.OC_GroupChannelDAL.Instance.FindList(t => t.F_MobileChannel == F_ChannelId || t.F_TelecomChannel==F_ChannelId||t.F_UnicomChannel==F_ChannelId);
                if (flag == 0)//没有选择运营商查询条件
                {
                    foreach (var item in data)
                    {
                        foreach (var entity in list)
                        {
                            if (item.F_UserId == entity.F_UserId)
                            {
                                newlist.Add(entity);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in data)
                    {
                        foreach (var entity in newlist)//如果原来的list中用户通道包含运营商，则添加到新的列表中
                        {
                            if(item.F_UserId==entity.F_UserId && !newChannelIdlist.Contains(entity))
                            {
                                newChannelIdlist.Add(entity); 
                            }
                        }
                    }
                    newlist = newChannelIdlist;//重新引用list
                }                                        
                flag++;
            }

            if (!queryParam["signature"].IsEmpty())//签名
            {
                string F_signature = queryParam["signature"].ToString();
                List<Sys_User> data = new List<Sys_User>();
                if (F_signature == "有签名")
                    data = DAL.Sys_UserDAL.Instance.FindList(t => t.F_Signature != null);
                else if(F_signature=="无签名")
                    data = DAL.Sys_UserDAL.Instance.FindList(t => t.F_Signature == null);
                if (flag == 0)//没有前两个查询条件
                {
                    foreach (var item in data)
                    {
                        foreach (var entity in list)
                        {
                            if (item.Id == entity.F_UserId)
                            {
                                newlist.Add(entity);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in data)
                    {
                        foreach (var entity in newlist)//如果原来的list中用户通道包含运营商，则添加到新的列表中
                        {
                            if (item.Id == entity.F_UserId && !newSignaturelist.Contains(entity))
                            {
                                newSignaturelist.Add(entity);
                            }
                        }
                    }
                    newlist = newSignaturelist;//重新引用list
                }                            
                flag++;
            }
 //**************************************************************************************************************************************************************
            //返回新的list（flag>0代表进入了附加查询）
            if (flag >0)
                return newlist;
           else //返回旧list(flag=0代表没有进行附加查询）
            return list;
        }

        //获取表单信息
        public UserInfoEntity GetForm(string keyValue)//F_Id
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(UserInfoEntity UserInfoEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                UserInfoEntity.Modify(keyValue);
            }
            else
            {
                UserInfoEntity.Create();
            }
            service.SubmitForm(UserInfoEntity, keyValue);
        }

        //用于修改资料的更新表单方法。（特殊情况）
        public void UpdateForm(OC_UserInfo UserInfoEntity, int keyValue)
        {
            Sys_User model_SysUser = BLL.Sys_UserManager.Instance.GetModel(keyValue);
            List<OC_UserInfo> Old_model = DAL.OC_UserInfoDAL.Instance.FindList(t => t.F_UserId == keyValue);//找出原有的对象
            //给原对象赋新的值
            Old_model[0].F_ManagerId = UserInfoEntity.F_ManagerId;
            Old_model[0].F_State = UserInfoEntity.F_State;
            Old_model[0].F_Reviewed = UserInfoEntity.F_Reviewed;
            //修改两张表中的用户名
            Old_model[0].F_Account = UserInfoEntity.F_Account;
            model_SysUser.F_Account = UserInfoEntity.F_Account;
            //添加标志量
            model_SysUser.F_LastModifyTime = Old_model[0].F_LastModifyTime = DateTime.Now;
            model_SysUser.F_LastModifyUserId = Old_model[0].F_LastModifyUserId = OperatorProvider.Provider.GetCurrent().UserId;
            DAL.OC_UserInfoDAL.Instance.Update(Old_model[0]);
            DAL.Sys_UserDAL.Instance.Update(model_SysUser);
        }

        public DataTable GetDataTable(string queryJson)
        {
            var expression = ExtLinq.True<UserInfoEntity>();
            var queryParam = queryJson.ToJObject();
            /*//查询条件
             * if (!queryParam["F_StateId"].IsEmpty())
            {
                string F_StateId = queryParam["F_StateId"].ToString();
                expression = expression.And(t => t.F_StateId.Equals(F_StateId));
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(t => t.F_CustInfo.Contains(keyvalue));
            }
            */
            DataTable getdatatable = NFine.Data.Extensions.DataTableExtensions.ToDataTable(service.FindList(expression, "F_CallTime desc"));
            return getdatatable;
        }

    }
}
