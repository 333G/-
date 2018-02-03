using NFine.Code;
using NFine.Domain.Entity.TXLManage;
using NFine.Domain.IRepository.TXLManage;
using NFine.Repository.TXLManage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NFine.Application.TXLManage
{
    public class PheGrupApp
    {
        private IPheGrupRepository service = new PheGrupRepository();

        public List<PheGrupEntity> GetList()
        {
            return service.IQueryable().OrderBy(t => t.F_CreatorTime).ToList();
        }

        //取得指定用户的下的数据
        public List<PheGrupEntity> GetList(string userId)
        {
            return service.IQueryable().Where(t => t.F_CreatorUserId.Equals(userId)).OrderBy(t => t.F_CreatorTime).ToList();
        }


        public List<PheGrupEntity> GetList(Pagination pagination, string keyword)
        {
            var expression = ExtLinq.True<PheGrupEntity>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.GroupName.Contains(keyword)); ;
            }
            //expression = expression.And(t => t.GroupName != "admin");
            return service.FindList(expression, pagination);
        }
        public PheGrupEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            if (NFine.DAL.Self.PheGrupDAL.Instance.IsHaveUser(keyValue))
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                service.Delete(t => t.F_Id == keyValue);
            }
        }
        public void SubmitForm(PheGrupEntity pheGrupEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                pheGrupEntity.Modify(keyValue);
                service.Update(pheGrupEntity);
            }
            else
            {
                pheGrupEntity.Create();
                service.Insert(pheGrupEntity);
            }
        }
    }
}
