using Template2.Domain.Entities;
using System.Collections.Generic;

namespace Template2.Domain.Repositories
{
    public interface IWorkerGroupMstRepositoryAsync
    {
        Task<IReadOnlyList<WorkerGroupMstEntity>> GetDataAsync();

        void SaveAsync(WorkerGroupMstEntity entity);

        void DeleteAsync(WorkerGroupMstEntity entity);
    }
}
