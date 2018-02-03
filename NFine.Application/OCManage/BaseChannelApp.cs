using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NFine.Application.SystemManage;
using NFine.Domain.Entity.SystemManage;

namespace NFine.Application.OCManage
{
    public class BaseChannelApp
    {
        private IBaseChannelRepository service = new BaseChannelRepository();
        public List<BaseChannelEntity> GetList()
        {
            return service.IQueryable().ToList();
        }
        public List<BaseChannelEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<BaseChannelEntity>();
            var queryParam = queryJson.ToJObject();
            //查询条件 
            if (!queryParam["F_ChannelName"].IsEmpty())
            {
                string F_ChannelName = queryParam["F_ChannelName"].ToString();
                expression = expression.And(t => t.F_ChannelName.Equals(F_ChannelName));
            }
            if (!queryParam["F_Operator"].IsEmpty())
            {
                int F_Operator = queryParam["F_Operator"].ToInt();
                expression = expression.And(t => t.F_Operator.Equals(F_Operator));
            }
            if (!queryParam["F_ChannelState"].IsEmpty())
            {
                int? F_ChannelState = queryParam["F_ChannelState"].ToInt();
                expression = expression.And(t => t.F_ChannelState.Equals(F_ChannelState));
            }
            //if (!queryParam["F_UrlType"].IsEmpty())
            //{
            //    string F_UrlType = queryParam["F_UrlType"].ToString();
            //    expression = expression.And(t => t.F_UrlType.Equals(F_UrlType));
            //}
            if (!queryParam[" F_ChaBalance_Up"].IsEmpty())
            {
                decimal F_ChaBalance_Up = Ext.ToList(queryParam["F_ChaBalance_Up"]);
                expression = expression.And(t => t.F_ChaBalance <= F_ChaBalance_Up);
            }
            if (!queryParam[" F_ChaBalance_Down"].IsEmpty())
            {
                decimal F_ChaBalance_Down = Ext.ToList(queryParam["F_ChaBalance_Down"]);
                expression = expression.And(t => t.F_ChaBalance >= F_ChaBalance_Down);
            }
            return service.FindList(expression, pagination);
        }
        public BaseChannelEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(BaseChannelEntity baseChannelEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                baseChannelEntity.Modify(keyValue);
            }
            else
            {
                baseChannelEntity.Create();
            }
            service.SubmitForm(baseChannelEntity, keyValue);
        }

        public void newSubmitForm(BaseChannelEntity basechannelEntity, ChannelProvinceEntity channelprovinceEntity, ChannelConfigEntity channelconfigEntity, string keyValue)
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (!string.IsNullOrEmpty(keyValue))//修改
            {
                basechannelEntity.Id = keyValue.ToInt();
                if (LoginInfo != null)
                {
                    basechannelEntity.F_LastModifyUserId = LoginInfo.UserId;
                    channelconfigEntity.F_LastModifyUserId = LoginInfo.UserId;
                }
                basechannelEntity.F_LastModifyTime = DateTime.Now;
                channelconfigEntity.F_LastModifyTime = DateTime.Now;
            }
            else//新建
            {
                basechannelEntity.F_Id = Common.GuId();
                if (LoginInfo != null)
                {
                    basechannelEntity.F_CreatorUserId = LoginInfo.UserId;
                    channelconfigEntity.F_CreatorUserId = LoginInfo.UserId;
                }
                basechannelEntity.F_CreatorTime = DateTime.Now;
                channelconfigEntity.F_CreatorTime = DateTime.Now;

                AreaApp areaApp = new AreaApp();
                string[] name = new string[34];
                string[] id = new string[34];
                var data = areaApp.GetList();
                var treelit = new List<TreeSelectModel>();
                int i = 0;
                foreach (AreaEntity item in data)
                {
                    if (item.F_Layers == 1)
                    {
                        name[i] = item.F_FullName;
                        id[i] = item.F_Id;
                        i++;
                    }
                }
                string Name = null;
                string Id = null;
                for (i = 0; i < name.Length; i++)
                {
                    Name += name[i] + ",";
                    Id += id[i] + ',';//遍历省份
                }

            }
            service.newSubmitForm(basechannelEntity, channelprovinceEntity, channelconfigEntity, keyValue);
        }


        public void UpdateForm(BaseChannelEntity BaseChannelEntity)
        {
            service.Update(BaseChannelEntity);
        }

        public DataTable GetDataTable(string queryJson)
        {
            var expression = ExtLinq.True<BaseChannelEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["F_ChannelName"].IsEmpty())
            {
                string F_ChannelName = queryParam["F_ChannelName"].ToString();
                expression = expression.And(t => t.F_ChannelName.Equals(F_ChannelName));
            }
            if (!queryParam["F_Operator"].IsEmpty())
            {
                int F_Operator = queryParam["F_Operator"].ToInt();
                expression = expression.And(t => t.F_Operator.Equals(F_Operator));
            }
            if (!queryParam["F_ChannelState"].IsEmpty())
            {
                int? F_ChannelState = queryParam["F_ChannelState"].ToInt();
                expression = expression.And(t => t.F_ChannelState.Equals(F_ChannelState));
            }
            if (!queryParam[" F_ChaBalance_Up"].IsEmpty())
            {
                decimal F_ChaBalance_Up = Ext.ToList(queryParam["F_ChaBalance_Up"]);
                expression = expression.And(t => t.F_ChaBalance <= F_ChaBalance_Up);
            }
            if (!queryParam[" F_ChaBalance_Down"].IsEmpty())
            {
                decimal F_ChaBalance_Down = Ext.ToList(queryParam["F_ChaBalance_Down"]);
                expression = expression.And(t => t.F_ChaBalance >= F_ChaBalance_Down);
            }

            DataTable getdatatable = NFine.Data.Extensions.DataTableExtensions.ToDataTable(service.FindList(expression, "F_CreatorTime desc"));
            return getdatatable;
        }
    }
}
