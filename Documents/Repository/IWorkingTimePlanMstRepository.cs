using Template2.Domain.Entities;
using System.Collections.Generic;

namespace Template2.Domain.Repositories
{
    public interface IWorkingTimePlanMstRepository
    {
        IReadOnlyList<WorkingTimePlanMstEntity> GetData();

        void Save(WorkingTimePlanMstEntity entity);

        void Delete(WorkingTimePlanMstEntity entity);
    }
}
