using Template2.Domain;
using Template2.Domain.Repositories;
using Template2.Infrastructure.Oracle;
using Template2.Infrastructure.SQLite;

namespace Template2.Infrastructure
{
    /// <summary>
    /// Factories
    /// </summary>
    public static class Factories
    {
        public static ISampleMstRepository CreateSampleMst()
        {
#if DEBUG
            if (Shared.IsFake)
            {
                return new SampleMstSQLite();
            }
#endif
            return new SampleMstOracle();
        }
        public static IWorkerMstRepository CreateWorkerMst()
        {
#if DEBUG
            if (Shared.IsFake)
            {
                return new WorkerMstSQLite();
            }
#endif
            return new WorkerMstOracle();
        }
        public static IWorkingTimePlanMstRepository CreateWorkingTimePlanMst()
        {
#if DEBUG
            if (Shared.IsFake)
            {
                return new WorkingTimePlanMstSQLite();
            }
#endif
            return new WorkingTimePlanMstOracle();
        }

    }
}
