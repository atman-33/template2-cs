using Oracle.ManagedDataAccess.Client;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastructure.Oracle
{
    internal class WorkerMstOracle : IWorkerMstRepository
    {
        public IReadOnlyList<WorkerMstEntity> GetData()
        {
            throw new NotImplementedException();
        }

        public void Save(WorkerMstEntity entity)
        {
            throw new NotImplementedException();

        }



        public void Delete(WorkerMstEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
