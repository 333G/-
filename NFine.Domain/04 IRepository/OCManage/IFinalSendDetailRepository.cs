using NFine.Data;
using NFine.Domain.Entity.OCManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.IRepository.OCManage
{
    public interface IFinalSendDetailRepository : IRepositoryBase<FinalSendDetailEntity>
    {
        void DeleteForm(string keyValue);
    }
}
