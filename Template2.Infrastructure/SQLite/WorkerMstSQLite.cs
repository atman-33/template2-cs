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
  worker_name
FROM
  tmp_worker_mst
";

            return SQLiteHelper.Query(sql,
                reader =>
                {
                    return new WorkerMstEntity(
                        Convert.ToString(reader["worker_code"]),
                        Convert.ToString(reader["worker_name"])
                        );
                });
        }

        public void Save(WorkerMstEntity entity)
        {
            string insert = @"
INSERT INTO tmp_worker_mst
 (worker_code,
  worker_name)
VALUES
 (@worker_code,
  @worker_name)
";
            string update = @"
UPDATE tmp_worker_mst
SET 
  worker_name = @worker_name
WHERE
  worker_code = @worker_code
";
            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@worker_code", entity.WorkerCode.Value),
                new SQLiteParameter("@worker_name", entity.WorkerName.Value)
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
