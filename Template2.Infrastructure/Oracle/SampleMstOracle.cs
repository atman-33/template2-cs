using Oracle.ManagedDataAccess.Client;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastructure.Oracle
{
    internal class SampleMstOracle : ISampleMstRepository
    {
        public IReadOnlyList<SampleMstEntity> GetData()
        {
            string sql = @"
SELECT
  sample_code,
  sample_name
FROM
  tmp_sample_mst
";

            return OracleOdpHelper.Query(sql,
                reader =>
                {
                    return new SampleMstEntity(
                        Convert.ToString(reader["sample_code"]),
                        Convert.ToString(reader["sample_name"])
                        );
                });
        }

        public void Save(SampleMstEntity entity)
        {
            string insert = @"
INSERT INTO tmp_sample_mst
 (sample_code,
  sample_name)
VALUES
 (:sample_code,
  :sample_name)
";
            string update = @"
UPDATE tmp_sample_mst
SET 
  sample_name = :sample_name
WHERE
  sample_code = :sample_code
";
            var args = new List<OracleParameter>
            {
                new OracleParameter(":sample_code", entity.SampleCode.Value),
                new OracleParameter(":sample_name", entity.SampleName.Value)
            };

            OracleOdpHelper.Execute(insert, update, args.ToArray());
        }



        public void Delete(SampleMstEntity entity)
        {
            string delete = @"
DELETE FROM tmp_sample_mst WHERE sample_code = :sample_code
";

            var args = new List<OracleParameter>
            {
                new OracleParameter(":sample_code", entity.SampleCode.Value)
            };

            OracleOdpHelper.Execute(delete, args.ToArray());
        }
    }
}
