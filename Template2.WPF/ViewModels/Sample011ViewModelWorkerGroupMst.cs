using Template2.Domain.Entities;
using Template2.Domain.ValueObjects;

namespace Template2.WPF.ViewModels
{
    public class Sample011ViewModelWorkerGroupMst
    {
        public Sample011ViewModelWorkerGroupMst(WorkerGroupMstEntity entity)
        {
            Entity = entity;
        }
        public WorkerGroupMstEntity Entity { get; private set; }

        public string WorkerGroupCode
        {
            get { return Entity.WorkerGroupCode.Value; }
            set { Entity.WorkerGroupCode = new WorkerGroupCode(value); }
        }
        public string WorkerGroupName
        {
            get { return Entity.WorkerGroupName.Value; }
            set { Entity.WorkerGroupName = new WorkerGroupName(value); }
        }
    }
}
