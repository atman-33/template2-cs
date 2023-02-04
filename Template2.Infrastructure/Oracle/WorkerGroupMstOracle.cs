using Oracle.ManagedDataAccess.Client;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastructure.Oracle
{
    internal class WorkerGroupMstOracle : IWorkerGroupMstRepository
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

            return OracleOdpHelper.Query(sql,
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
 (:worker_group_code,
  :worker_group_name)
";
            string update = @"
UPDATE tmp_worker_group_mst
SET 
  worker_group_name = :worker_group_name
WHERE
  worker_group_code = :worker_group_code
";
            var args = new List<OracleParameter>
            {
                new OracleParameter(":worker_group_code", entity.WorkerGroupCode.Value),
				new OracleParameter(":worker_group_name", entity.WorkerGroupName.Value)
            };

            OracleOdpHelper.Execute(insert, update, args.ToArray());
        }



        public void Delete(WorkerGroupMstEntity entity)
        {
            string delete = @"
DELETE FROM tmp_worker_group_mst WHERE worker_group_code = :worker_group_code
";

            var args = new List<OracleParameter>
            {
                new OracleParameter(":worker_group_code", entity.WorkerGroupCode.Value)
            };

            OracleOdpHelper.Execute(delete, args.ToArray());
        }
    }
}
