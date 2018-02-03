using System;
using NFine.Code;
using NFine.Domain.Entity.OCManage;
using NFine.Domain.IRepository.OCManage;
using NFine.Repository.OCManage;
using System.Collections.Generic;
using System.Data;

namespace NFine.Application.OCManage
{
    public class SensitiveWordsApp
    {
        private ISensitiveWordsRepository service = new SensitiveWordsRepository();
        public void SubmitForm(SensitiveWordsEntity SensitiveWordsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                SensitiveWordsEntity.Modify(keyValue);
            }
            else
            {
                SensitiveWordsEntity.Create();
            }
            service.SubmitForm(SensitiveWordsEntity, keyValue);
        }
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }
    }
}
