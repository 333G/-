using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFine.Code;
using NFine.Application.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;
using System.Data;
using NFine.Entity;
using NFine.Domain.Entity.OCManage;

namespace NFine.Application.OCManage
{
    public class GroupChannelApp
    {
        private IChannelConfigRepository service = new ChannelConfigRepository();

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        //提交新增用户通道
        public bool SubmitForm( OC_GroupChannel model_GC, int Id)
        {
            bool result = false;
            model_GC.F_ID = Common.GuId();
            model_GC.F_UserId = Id;
            result = DAL.OC_GroupChannelDAL.Instance.Add(model_GC) > 0;

            if (result)
                return true;
            else
                return false;
        }
    }
}
