
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.SMCManage;
using NFine.Entity.Views;
using NFine.Entity;

namespace NFine.Domain.IRepository.SMCManage
{
    public interface ISMCSendRepository : IRepositoryBase<SMCSendEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SMCSendEntity SMCSendEntity, string keyValue);

        //List<NFine.Entity.Views.VSMCSendSms> FindList(Expression<Func<NFine.Entity.Views.VSMCSendSms, bool>> expression_SendSms, Pagination pagination);
    }
}
