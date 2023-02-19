using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class TaskMstEntity
    {
        public TaskMstEntity(
            int taskId,
            string? taskItem,
            DateTime? taskDeadLine)
        {
            TaskId = new TaskId(taskId);
            TaskItem = new TaskItem(taskItem);
            TaskDeadline = new TaskDeadline(taskDeadLine);
        }

        public TaskMstEntity(
            int taskId,
            string? taskItem,
            DateTime? taskDeadLine,
            string? processCode,
            string? workerCode)
        {
            TaskId = new TaskId(taskId);
            TaskItem = new TaskItem(taskItem);
            TaskDeadline = new TaskDeadline(taskDeadLine);
            ProcessCode = new ProcessCode(processCode);
            WorkerCode = new WorkerCode(workerCode);
        }

        public TaskId TaskId { get; }
        public TaskItem TaskItem { get; }
        public TaskDeadline TaskDeadline { get; }
        public ProcessCode ProcessCode { get; }
        public WorkerCode WorkerCode { get; }

    }
}
