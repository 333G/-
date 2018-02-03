using NFine.Code;
using NFine.Domain.Entity.TXLManage;
using NFine.Domain.IRepository.TXLManage;
using NFine.Repository.TXLManage;
using System;
using System.Collections.Generic;
using System.Data;


using System.Linq;

namespace NFine.Application.TXLManage
{
    public class PheInfoApp
    {
        private IPheInfoRepository service = new PheInfoRepository();
        public List<PheInfoEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
            //return  service.IQueryable().ToList(< PheInfoEntity >);
        }
        public List<PheInfoEntity> GetList(Pagination pagination, string queryJson)
        {
            var expression = ExtLinq.True<PheInfoEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["GroupId"].IsEmpty())
            {
                string GroupId = queryParam["GroupId"].ToString();
                expression = expression.And(t => t.GroupId.Equals(GroupId));
            }
            if (!queryParam["Sex"].IsEmpty())
            {
                string Sex = queryParam["Sex"].ToString();
                expression = expression.And(t => t.Sex.Equals(Sex));
            }
            if (!queryParam["Province"].IsEmpty())
            {
                string Province = queryParam["Province"].ToString();
                expression = expression.And(t => t.Province.Equals(Province));
            }
            if (!queryParam["Operator"].IsEmpty())
            {
                string Operator = queryParam["Operator"].ToString();
                expression = expression.And(t => t.Operator.Equals(Operator));
            }
            if (!queryParam["State"].IsEmpty())
            {
                string State = queryParam["State"].ToString();
                expression = expression.And(t => t.State.ToString().Equals(State));
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(
                   expression.And(t => t.Name.Contains(keyvalue))
                   .Or(t => t.Mobile.Contains(keyvalue))
                   );
                //expression = expression.And(t => t.Name.Contains(keyvalue));
                //expression = expression.Or(t => t.Mobile.Contains(keyvalue));
            }

            //[Age]年龄段小于15值为0
            //年龄段 15-25值为1
            //年龄段 25-35值为2
            //年龄段 35-45值为3
            //年龄段 45-55值为4
            //年龄段大于55值为5
            if (!queryParam["Age"].IsEmpty())//取到的值是birthday，需要根据birthday来进行变换查询
            {
                DateTime MaxDate = new DateTime();
                DateTime MinDate = new DateTime();
                MaxDate =MinDate= DateTime.Now;
                //string age = (timenow - t.Birthday).ToString();
                int keyvalue = queryParam["Age"].ToInt();
                if (keyvalue == 0)//小于15
                {
                    MaxDate = DateTime.Now.AddYears(-15);
                }
                else if (keyvalue == 1)//15到25
                {
                    MaxDate = DateTime.Now.AddYears(-25);
                    MinDate = DateTime.Now.AddYears(-15);
                }
                else if (keyvalue == 2)//25到35
                {
                    MaxDate = DateTime.Now.AddYears(-35);
                    MinDate = DateTime.Now.AddYears(-25);
                }
                else if (keyvalue == 3)//35到45
                {
                    MaxDate = DateTime.Now.AddYears(-45);
                    MinDate = DateTime.Now.AddYears(-35);
                }
                else if (keyvalue == 4)//45到55
                {
                    MaxDate = DateTime.Now.AddYears(-55);
                    MinDate = DateTime.Now.AddYears(-45);
                }
                else if (keyvalue == 5)//大于55
                {
                    MaxDate = DateTime.Now.AddYears(-120);
                    MinDate = DateTime.Now.AddYears(-55);
                }
                expression = expression.And(t => t.Birthday > MaxDate);
                expression = expression.And(t => t.Birthday < MinDate);          
            }
            return service.FindList(expression, pagination);
        }
       
        public int ClearMember(Pagination pagination, string groupId)
        {
            var expression = ExtLinq.True<PheInfoEntity>();
            expression = expression.And(t => t.GroupId.ToString().Equals(groupId));
            int n = 0;
            foreach (PheInfoEntity p in service.FindList(expression, "F_CreatorTime desc"))
            {
                if (p.GroupId == groupId)
                {
                    service.DeleteForm(p.F_Id);
                    n++;
                }
            }
            return n;
        }
        public PheInfoEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public void SubmitForm(PheInfoEntity PheInfoEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                PheInfoEntity.Modify(keyValue);
            }
            else
            {
                PheInfoEntity.Create();
            }
            service.SubmitForm(PheInfoEntity, keyValue);
        }
        public void UpdateForm(PheInfoEntity PheInfoEntity)
        {
            service.Update(PheInfoEntity);
        }

        public DataTable GetDataTable(string queryJson)
        {
            var expression = ExtLinq.True<PheInfoEntity>();
            var queryParam = queryJson.ToJObject();
            if (!queryParam["Sex"].IsEmpty())
            {
                string Sex = queryParam["Sex"].ToString();
                expression = expression.And(t => t.Sex.Equals(Sex));
            }
            if (!queryParam["Operator"].IsEmpty())
            {
                string Operator = queryParam["Operator"].ToString();
                expression = expression.And(t => t.Operator.Equals(Operator));
            }
            if (!queryParam["State"].IsEmpty())
            {
                string State = queryParam["State"].ToString();
                expression = expression.And(t => t.State.ToString().Equals(State));
            }
            if (!queryParam["keyword"].IsEmpty())
            {
                string keyvalue = queryParam["keyword"].ToString();
                expression = expression.And(t => t.Name.Contains(keyvalue));
                expression = expression.Or(t => t.Mobile.Contains(keyvalue));
            }
            //return service.FindList(expression, pagination);
            DataTable getdatatable = NFine.Data.Extensions.DataTableExtensions.ToDataTable(service.FindList(expression, "F_CreatorTime desc"));
            return getdatatable;
        }
    }
}
