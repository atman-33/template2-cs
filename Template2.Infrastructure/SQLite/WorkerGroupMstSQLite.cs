using System.Data.SQLite;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastructure.SQLite
{
    internal class WorkerGroupMstSQLite : IWorkerGroupMstRepository
    {
        public IReadOnlyList<WorkerGroupMstEntity> GetData()
        {
            string sql = @"
SELECT
  worker_group_code,
  worker_group_name
FROM
  tmp_worker_group_mst
";

            return SQLiteHelper.Query(sql,
                reader =>
                {
                    return new WorkerGroupMstEntity(
                        Convert.ToString(reader["worker_group_code"]),
						Convert.ToString(reader["worker_group_name"])
                        );
                });
        }

        public void Save(WorkerGroupMstEntity entity)
        {
            string insert = @"
INSERT INTO tmp_worker_group_mst
 (worker_group_code,
  worker_group_name)
VALUES
 (@worker_group_code,
  @worker_group_name)
";
            string update = @"
UPDATE tmp_worker_group_mst
SET 
  worker_group_name = @worker_group_name
WHERE
  worker_group_code = @worker_group_code
";
            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@worker_group_code", entity.WorkerGroupCode.Value),
				new SQLiteParameter("@worker_group_name", entity.WorkerGroupName.Value)
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }



        public void Delete(WorkerGroupMstEntity entity)
        {
            string delete = @"
DELETE FROM tmp_worker_group_mst WHERE worker_group_code = @worker_group_code
";

            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@worker_group_code", entity.WorkerGroupCode.Value)
            };

            SQLiteHelper.Execute(delete, args.ToArray());
        }
    }
}
