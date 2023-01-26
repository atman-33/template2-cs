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
  working_time = @working_time
WHERE
  worker_code = @worker_code
  AND weekday = @weekday
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
  AND weekday = @weekday
";

            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@worker_code", entity.WorkerCode.Value),
				new SQLiteParameter("@weekday", entity.Weekday.Value)
            };

            SQLiteHelper.Execute(delete, args.ToArray());
        }
    }
}
