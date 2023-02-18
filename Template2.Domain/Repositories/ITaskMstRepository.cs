using Template2.Domain.Entities;
using System.Collections.Generic;

namespace Template2.Domain.Repositories
{
    public interface ITaskMstRepository
    {
        IReadOnlyList<TaskMstEntity> GetData();

        void Save(TaskMstEntity entity);

        void Delete(TaskMstEntity entity);
    }
}
