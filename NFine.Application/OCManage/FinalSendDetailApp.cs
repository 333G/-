using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Application.OCManage
{
   public class FinalSendDetailApp
    {
        private IFinalSendDetailRepository service = new FinalSendDetailRepository();
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
        public FinalSendDetailEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void UpdateForm(FinalSendDetailEntity FinalSendDetailEntity)
        {
            service.Update(FinalSendDetailEntity);
        }
    }
}
