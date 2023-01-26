using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class WorkerMstEntity
    {

        public WorkerMstEntity(
            string workerCode,
			string workerName)
        {
            WorkerCode = new WorkerCode(workerCode);
			WorkerName = new WorkerName(workerName);
        }

        public WorkerCode WorkerCode { get; }
public WorkerName WorkerName { get; }

    }
}
