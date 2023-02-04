using System.Data.SQLite;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastructure.SQLite
{
    internal class WorkerMstSQLite : IWorkerMstRepository
    {
        public IReadOnlyList<WorkerMstEntity> GetData()
        {
            string sql = @"
SELECT
  worker_code,
  worker_name,
  worker_group_code
FROM
  tmp_worker_mst
";

            return SQLiteHelper.Query(sql,
                reader =>
                {
                    return new WorkerMstEntity(
                        Convert.ToString(reader["worker_code"]),
                        Convert.ToString(reader["worker_name"]),
                        reader["worker_group_code"] != DBNull.Value ? Convert.ToString(reader["worker_group_code"]) : null
                        );
                });
        }

        public void Save(WorkerMstEntity entity)
        {
            string insert = @"
INSERT INTO tmp_worker_mst
 (worker_code,
  worker_name,
  worker_group_code)
VALUES
 (@worker_code,
  @worker_name,
  @worker_group_code)
";
            string update = @"
UPDATE tmp_worker_mst
SET 
  worker_name = @worker_name,
  worker_group_code = @worker_group_code
WHERE
  worker_code = @worker_code
";
            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@worker_code", entity.WorkerCode.Value),
                new SQLiteParameter("@worker_name", entity.WorkerName.Value),
                new SQLiteParameter("@worker_group_code", entity.WorkerGroupCode.Value)
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }



        public void Delete(WorkerMstEntity entity)
        {
            string delete = @"
DELETE FROM tmp_worker_mst WHERE worker_code = @worker_code
";

            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@worker_code", entity.WorkerCode.Value)
            };

            SQLiteHelper.Execute(delete, args.ToArray());
        }
    }
}
