using System.Data.SQLite;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastructure.SQLite
{
    internal class WorkingTimePlanMstSQLite : IWorkingTimePlanMstRepository
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

            return SQLiteHelper.Query(sql,
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
            string sql = @"
SELECT
  wtpm.worker_code,
  wtpm.weekday,
  wtpm.working_time,
  wm.worker_name
FROM
  tmp_working_time_plan_mst wtpm
  LEFT OUTER JOIN tmp_worker_mst wm ON wtpm.worker_code = wm.worker_code
";

            return SQLiteHelper.Query(sql,
                reader =>
                {
                    return new WorkingTimePlanMstEntity(
                        Convert.ToString(reader["worker_code"]),
                        Convert.ToInt32(reader["weekday"]),
                        reader["working_time"] != DBNull.Value ? Convert.ToSingle(reader["working_time"]) : null,
                        reader["worker_name"] != DBNull.Value ? Convert.ToString(reader["worker_name"]) : String.Empty
                        );
                });
        }

        public void Save(WorkingTimePlanMstEntity entity)
        {
            string insert = @"
INSERT INTO tmp_working_time_plan_mst
 (worker_code,
  weekday,
  working_time)
VALUES
 (@worker_code,
  @weekday,
  @working_time)
";
            string update = @"
UPDATE tmp_working_time_plan_mst
SET 
  weekday = @weekday,
  working_time = @working_time
WHERE
  worker_code = @worker_code
";
            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@worker_code", entity.WorkerCode.Value),
                new SQLiteParameter("@weekday", entity.Weekday.Value),
                new SQLiteParameter("@working_time", entity.WorkingTime.Value)
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }



        public void Delete(WorkingTimePlanMstEntity entity)
        {
            string delete = @"
DELETE FROM tmp_working_time_plan_mst WHERE worker_code = @worker_code
";

            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@worker_code", entity.WorkerCode.Value)
            };

            SQLiteHelper.Execute(delete, args.ToArray());
        }
    }
}
