using Oracle.ManagedDataAccess.Client;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastructure.Oracle
{
    internal class WorkingTimePlanMstOracle : IWorkingTimePlanMstRepository
    {
        public IReadOnlyList<WorkingTimePlanMstEntity> GetData()
        {
            string sql = @"
SELECT
  worker_code,
  weekday,
  working_time
FROM
  tmp_working_time_plan_mst
";

            return OracleOdpHelper.Query(sql,
                reader =>
                {
                    return new WorkingTimePlanMstEntity(
                        Convert.ToString(reader["worker_code"]),
                        Convert.ToInt32(reader["weekday"]),
                        reader["working_time"] != DBNull.Value ? Convert.ToSingle(reader["working_time"]) : null
                        );
                });
        }

        public IReadOnlyList<WorkingTimePlanMstEntity> GetDataWithWorkerName()
        {
            throw new NotImplementedException();
        }


        public void Save(WorkingTimePlanMstEntity entity)
        {
            string insert = @"
INSERT INTO tmp_working_time_plan_mst
 (worker_code,
  weekday,
  working_time)
VALUES
 (:worker_code,
  :weekday,
  :working_time)
";
            string update = @"
UPDATE tmp_working_time_plan_mst
SET 
  weekday = :weekday,
  working_time = :working_time
WHERE
  worker_code = :worker_code
";
            var args = new List<OracleParameter>
            {
                new OracleParameter(":worker_code", entity.WorkerCode.Value),
                new OracleParameter(":weekday", entity.Weekday.Value),
                new OracleParameter(":working_time", entity.WorkingTime.Value)
            };

            OracleOdpHelper.Execute(insert, update, args.ToArray());
        }



        public void Delete(WorkingTimePlanMstEntity entity)
        {
            string delete = @"
DELETE FROM tmp_working_time_plan_mst WHERE worker_code = :worker_code
";

            var args = new List<OracleParameter>
            {
                new OracleParameter(":worker_code", entity.WorkerCode.Value)
            };

            OracleOdpHelper.Execute(delete, args.ToArray());
        }
    }
}
