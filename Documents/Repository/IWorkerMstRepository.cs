using Template2.Domain.Entities;
using System.Collections.Generic;

namespace Template2.Domain.Repositories
{
    public interface IWorkerMstRepository
    {
        IReadOnlyList<WorkerMstEntity> GetData();

        void Save(WorkerMstEntity entity);

        void Delete(WorkerMstEntity entity);
    }
}
