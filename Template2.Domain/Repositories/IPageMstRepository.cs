using Template2.Domain.Entities;
using System.Collections.Generic;

namespace Template2.Domain.Repositories
{
    public interface IPageMstRepository
    {
        IReadOnlyList<PageMstEntity> GetData();

        void Save(PageMstEntity entity);

        void Delete(PageMstEntity entity);

        int GetNextId();
    }
}
