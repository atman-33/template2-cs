using Template2.Domain.Entities;

namespace Template2.WPF.ViewModelEntities
{
    public class WorkerGroupMstViewModelEntity
    {
        public WorkerGroupMstViewModelEntity(WorkerGroupMstEntity entity)
        {
            Entity = entity;
        }
        public WorkerGroupMstEntity Entity { get; private set; }

        public string WorkerGroupCode => Entity.WorkerGroupCode.Value;
        public string WorkerGroupName => Entity.WorkerGroupName.Value;
    }
}
