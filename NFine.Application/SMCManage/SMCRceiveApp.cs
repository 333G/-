
using NFine.Code;
using NFine.Domain.Entity.SMCManage;
using NFine.Domain.IRepository.SMCManage;
using NFine.Repository.SMCManage;
using System;
using System.Collections.Generic;
using System.Data;

namespace NFine.Application.SMCManage
{
    public class SMCRceiveApp
    {
        private ISMCRceiveRepository service = new SMCRceiveRepository();
        public List<Entity.Views.VSMCRceiveSms> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<SMCRceiveEntity>();
            var queryParam = queryJson.ToJObject();
            //查询条件 
            if (!queryParam["F_Mobile"].IsEmpty())
            {
                string F_Mobile = queryParam["F_Mobile"].ToString();
                expression = expression.And(t => t.mobile.Equals(F_Mobile));
            }

            if (!queryParam["F_SmsContent"].IsEmpty())
            {
                string F_SmsContent = queryParam["F_SmsContent"].ToString();
                expression = expression.And(t => t.receive_content.Contains(F_SmsContent));
            }
            /*
            if (!queryParam["GroupId"].IsEmpty())
            {
                string GroupId = queryParam["GroupId"].ToString();
                expression = expression.And(t => t.GroupId.Equals(GroupId));
            }
            */
            if (!queryParam["F_RceiveTime"].IsEmpty())
            {
                DateTime? F_RceiveTime = Convert.ToDateTime(queryParam["F_RceiveTime"]);
                expression = expression.And(t => t.receive_time.Value.Day == F_RceiveTime.Value.Day);

                //string F_RceiveTime = queryParam["F_RceiveTime"].ToString();
                //expression = expression.And(t => t.F_RceiveTime.Equals(F_RceiveTime));
            }
            /*
            if (!queryParam["F_TA"].IsEmpty())
            {
                string F_TA = queryParam["F_TA"].ToString();
                expression = expression.And(t => t.F_TA.Equals(F_TA));
            }
            */
            List<SMCRceiveEntity> SMCRceiveEntityList = service.FindList(expression, pagination);//take了pagination.rows个
            List<Entity.Views.VSMCRceiveSms> Vlist = new List<Entity.Views.VSMCRceiveSms>();
            for (int i = 0; i < SMCRceiveEntityList.Count; i++)
            {
                Entity.Views.VSMCRceiveSms VRceiveModel = new Entity.Views.VSMCRceiveSms();
                VRceiveModel.F_Id = SMCRceiveEntityList[i].F_Id;
                VRceiveModel.receive_content = SMCRceiveEntityList[i].receive_content;
                VRceiveModel.receive_time = SMCRceiveEntityList[i].receive_time;
                VRceiveModel.mobile = SMCRceiveEntityList[i].mobile;
                VRceiveModel.sp_number = SMCRceiveEntityList[i].sp_number;
                try
                {
                    Entity.Sev_SendDateDetail SendModel = DAL.Sev_SendDateDetailDAL.Instance.FindEntity(t => t.F_Id == SMCRceiveEntityList[i].MsgID);
                    Entity.Sys_Area AreaModel= DAL.Sys_AreaDAL.Instance.FindEntity(t => t.F_Id == SendModel.F_Province);
                    VRceiveModel.F_Province = DAL.Sys_AreaDAL.Instance.FindEntity(t => t.F_Id == AreaModel.F_ParentId).F_FullName+" "+ AreaModel.F_FullName;
                    VRceiveModel.F_SmsContent = SendModel.F_SmsContent;
                    VRceiveModel.F_UserId = SendModel.F_UserId;
                    VRceiveModel.F_Operator = SendModel.F_Operator.ToInt();
                    VRceiveModel.F_CreatorUserId = SendModel.F_CreatorUserId;
                }
                catch
                {
                    VRceiveModel.F_Province = "未知";
                    VRceiveModel.F_SmsContent = "未知";
                    VRceiveModel.F_UserId = 0;
                    VRceiveModel.F_Operator = 0;
                    VRceiveModel.F_CreatorUserId = "未知";
                }
                Vlist.Add(VRceiveModel);
            }
            return Vlist;
        }

        public SMCRceiveEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(SMCRceiveEntity SMCRceiveEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SMCRceiveEntity.Modify(keyValue);
            }
            else
            {
                SMCRceiveEntity.Create();
            }
            service.SubmitForm(SMCRceiveEntity, keyValue);
        }
        public void UpdateForm(SMCRceiveEntity SMCRceiveEntity)
        {
            service.Update(SMCRceiveEntity);
        }


    }
}
