
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NFine.Code;
using NFine.Data;
using NFine.Domain.Entity.OCManage;
using NFine.Entity;

namespace NFine.Domain.IRepository.OCManage
{
    public interface IUserInfoRepository : IRepositoryBase<UserInfoEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(UserInfoEntity userInfoEntity, string keyValue);
        List<UserInfoEntity> FindList(Expression<Func<UserInfoEntity, bool>> expression, Pagination pagination);
    }
}

