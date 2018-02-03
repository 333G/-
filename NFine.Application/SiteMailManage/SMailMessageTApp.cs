using NFine.Code;
using NFine.Domain.Entity.SiteMailManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.IRepository.SiteMailManage;
using NFine.Repository.SiteMailManage;
using SqlSugar;
using NFine.DAL;
using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;

namespace NFine.Application.SiteMailManage
{
    public class SMailMessageTApp
    {
        private ISMailMessageTRepository service = new SMailMessageTRepository(); 
         
        
             
        public List<SMailMessageTEntity> GetList(Pagination pagination, string queryJson)
        {
            //登录信息对应字段查看LoginController.cs文件 
            var id = OperatorProvider.Provider.GetCurrent().UserId;
            var account = OperatorProvider.Provider.GetCurrent().UserCode;//当前登录账户名称；
            var expression = ExtLinq.True<SMailMessageTEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["F_Message"].IsEmpty())
            {
                string F_Message = queryParam["F_Message"].ToString();
                expression = expression.And(t => t.F_Message.Equals(F_Message));
            }
            if (account != "admin")//非管理员登录，增加查询约束
            {
                string Account = account.ToString();
                expression = expression.And(t => t.F_Id.Equals(account));
            }
            return service.FindList(expression, pagination);

        }


        public SMailMessageTEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        //发消息
        public void SubmitForm(SMailMessageTEntity SMailMessageTEntity, string keyValue)
        {

            SMailMessageTEntity.Create();
            service.SubmitForm(SMailMessageTEntity, keyValue);
        }
  
        public void UpdateForm(SMailMessageTEntity SMailMessageTEntity)
        {
            service.Update(SMailMessageTEntity);
        }


    }
}
