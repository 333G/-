using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Repository.OCManage
{
   public class FinalSendDetailRepository : RepositoryBase<FinalSendDetailEntity>, IFinalSendDetailRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                string[] keys = keyValue.Split(',');
                foreach (string i in keys)
                    db.Delete<FinalSendDetailEntity>(t => t.F_Id == i);
                db.Commit();
            }
        }
    }
}
