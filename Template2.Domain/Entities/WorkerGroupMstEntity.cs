using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class WorkerGroupMstEntity
    {

        public WorkerGroupMstEntity(
            string workerGroupCode,
            string workerGroupName)
        {
            WorkerGroupCode = new WorkerGroupCode(workerGroupCode);
            WorkerGroupName = new WorkerGroupName(workerGroupName);
        }

        public WorkerGroupCode WorkerGroupCode { get; }
        public WorkerGroupName WorkerGroupName { get; }

    }
}
