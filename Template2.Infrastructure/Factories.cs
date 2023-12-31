using Template2.Domain;
using Template2.Domain.Repositories;
using Template2.Infrastructure.Oracle;
using Template2.Infrastructure.SQLite;

namespace Template2.Infrastructure
{
    public abstract class AbstractFactory
    {
        /// <summary>
        /// Factoryを生成
        /// </summary>
        /// <returns></returns>
        public static AbstractFactory Create()
        {
#if DEBUG
            if (Shared.IsFake)
            {
                return new SQLiteFactory();
            }
#endif
            return new OracleFactory();
        }

        /// <summary>
        /// データ接続テスト
        /// </summary>
        public static void TestConnection()
        {
#if DEBUG
            if (Shared.IsFake)
            {
                SQLiteHelper.Open();
                return;
            }
#endif
            OracleOdpHelper.Open();
        }

        public abstract ISampleMstRepository CreateSampleMst();
        public abstract IWorkerGroupMstRepository CreateWorkerGroupMst();
        public abstract IWorkerMstRepository CreateWorkerMst();
        public abstract IWorkingTimePlanMstRepository CreateWorkingTimePlanMst();
        public abstract IPageMstRepository CreatePageMst();
        public abstract ITaskMstRepository CreateTaskMst();
    }

    internal class SQLiteFactory : AbstractFactory
    {
        public override IPageMstRepository CreatePageMst()
        {
            return new PageMstSQLite();
        }

        public override ISampleMstRepository CreateSampleMst()
        {
            return new SampleMstSQLite();
        }

        public override ITaskMstRepository CreateTaskMst()
        {
            return new TaskMstSQLite();
        }

        public override IWorkerGroupMstRepository CreateWorkerGroupMst()
        {
            return new WorkerGroupMstSQLite();
        }

        public override IWorkerMstRepository CreateWorkerMst()
        {
            return new WorkerMstSQLite();
        }

        public override IWorkingTimePlanMstRepository CreateWorkingTimePlanMst()
        {
            return new WorkingTimePlanMstSQLite();
        }
    }

    internal class OracleFactory : AbstractFactory
    {
        public override IPageMstRepository CreatePageMst()
        {
            return new PageMstOracle();
        }

        public override ISampleMstRepository CreateSampleMst()
        {
            return new SampleMstOracle();
        }

        public override ITaskMstRepository CreateTaskMst()
        {
            return new TaskMstOracle();
        }

        public override IWorkerGroupMstRepository CreateWorkerGroupMst()
        {
            return new WorkerGroupMstOracle();
        }

        public override IWorkerMstRepository CreateWorkerMst()
        {
            return new WorkerMstOracle();
        }

        public override IWorkingTimePlanMstRepository CreateWorkingTimePlanMst()
        {
            return new WorkingTimePlanMstOracle();
        }
    }
}