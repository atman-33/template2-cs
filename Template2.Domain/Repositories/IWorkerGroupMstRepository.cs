using Template2.Domain.Entities;
using System.Collections.Generic;

namespace Template2.Domain.Repositories
{
    public interface IWorkerGroupMstRepository
    {
        IReadOnlyList<WorkerGroupMstEntity> GetData();

        void Save(WorkerGroupMstEntity entity);

        void Delete(WorkerGroupMstEntity entity);
    }
}
