using NFine.Data;
using NFine.Domain.IRepository.OCManage;
using NFine.Domain.Entity.OCManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.OCManage
{
    public class SendDateDetailRepository : RepositoryBase<SendDateDetailEntity>, ISendDateDetailRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<SendDateDetailEntity>(t => t.F_Id.ToString() == keyValue);
                db.Commit();
            }

        }
    }
}
