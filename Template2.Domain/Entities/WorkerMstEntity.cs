using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class WorkerMstEntity
    {

        public WorkerMstEntity(
            string workerCode,
            string workerName,
            string workerGroupCode)
        {
            WorkerCode = new WorkerCode(workerCode);
            WorkerName = new WorkerName(workerName);
            WorkerGroupCode = new WorkerGroupCode(workerGroupCode);
        }

        public WorkerCode WorkerCode { get; set; }
        public WorkerName WorkerName { get; set; }
        public WorkerGroupCode WorkerGroupCode { get; set; }

    }
}
