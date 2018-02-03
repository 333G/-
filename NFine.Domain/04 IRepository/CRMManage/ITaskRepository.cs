
 

using NFine.Data;
using NFine.Domain.Entity.CRMManage;

namespace NFine.Domain.IRepository.CRMManage
{
    public interface ITaskRepository : IRepositoryBase<TaskEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(TaskEntity TaskEntity, string keyValue);
    }
}

