using System.Data.SQLite;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastructure.SQLite
{
    internal class TaskMstSQLite : ITaskMstRepository
    {
        public IReadOnlyList<TaskMstEntity> GetData()
        {
            string sql = @"
SELECT
  task_id,
  task,
  task_dead_line,
  process_code,
  worker_code
FROM
  tmp_task_mst
";

            return SQLiteHelper.Query(sql,
                reader =>
                {
                    return new TaskMstEntity(
                        Convert.ToInt32(reader["task_id"]),
						reader["task"] != DBNull.Value ? Convert.ToString(reader["task"]) : null,
						reader["task_dead_line"] != DBNull.Value ? Convert.ToDateTime(reader["task_dead_line"]) : null,
						reader["process_code"] != DBNull.Value ? Convert.ToString(reader["process_code"]) : null,
						reader["worker_code"] != DBNull.Value ? Convert.ToString(reader["worker_code"]) : null
                        );
                });
        }

        public void Save(TaskMstEntity entity)
        {
            string insert = @"
INSERT INTO tmp_task_mst
 (task_id,
  task,
  task_dead_line,
  process_code,
  worker_code)
VALUES
 (@task_id,
  @task,
  @task_dead_line,
  @process_code,
  @worker_code)
";
            string update = @"
UPDATE tmp_task_mst
SET 
  task = @task,
  task_dead_line = @task_dead_line,
  process_code = @process_code,
  worker_code = @worker_code
WHERE
  task_id = @task_id
";
            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@task_id", entity.TaskId.Value),
				new SQLiteParameter("@task", entity.Task.Value),
				new SQLiteParameter("@task_dead_line", entity.TaskDeadLine.Value),
				new SQLiteParameter("@process_code", entity.ProcessCode.Value),
				new SQLiteParameter("@worker_code", entity.WorkerCode.Value)
            };

            SQLiteHelper.Execute(insert, update, args.ToArray());
        }



        public void Delete(TaskMstEntity entity)
        {
            string delete = @"
DELETE FROM tmp_task_mst WHERE task_id = @task_id
";

            var args = new List<SQLiteParameter>
            {
                new SQLiteParameter("@task_id", entity.TaskId.Value)
            };

            SQLiteHelper.Execute(delete, args.ToArray());
        }
    }
}
