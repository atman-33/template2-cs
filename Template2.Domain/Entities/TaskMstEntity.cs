using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class TaskMstEntity
    {

        public TaskMstEntity(
            int taskId,
			string? task,
			DateTime? taskDeadLine,
			string? processCode,
			string? workerCode)
        {
            TaskId = new TaskId(taskId);
			Task = new Task(task);
			TaskDeadLine = new TaskDeadLine(taskDeadLine);
			ProcessCode = new ProcessCode(processCode);
			WorkerCode = new WorkerCode(workerCode);
        }

        public TaskId TaskId { get; }
public Task Task { get; }
public TaskDeadLine TaskDeadLine { get; }
public ProcessCode ProcessCode { get; }
public WorkerCode WorkerCode { get; }

    }
}
