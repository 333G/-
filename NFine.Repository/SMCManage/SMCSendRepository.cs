
using System;
using System.Collections.Generic;
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.SMCManage;
using NFine.Domain.IRepository.SMCManage;
using NFine.Entity.Views;
using NFine.Repository.SMCManage;
using System.Linq;
using System.Linq.Expressions;

namespace NFine.Repository.SMCManage
{
    public class SMCSendRepository : RepositoryBase<SMCSendEntity>, ISMCSendRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<SMCSendEntity>(t => t.F_Id == i);
                db.Commit();
            }
            
        }

        public void SubmitForm(SMCSendEntity SMCSendEntity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(SMCSendEntity);
                }
                else
                {
                    db.Insert(SMCSendEntity);
                }
                db.Commit();
            }
        }
    }
}
