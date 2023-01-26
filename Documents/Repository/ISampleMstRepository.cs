using Template2.Domain.Entities;
using System.Collections.Generic;

namespace Template2.Domain.Repositories
{
    public interface ISampleMstRepository
    {
        IReadOnlyList<SampleMstEntity> GetData();

        void Save(SampleMstEntity entity);

        void Delete(SampleMstEntity entity);
    }
}
