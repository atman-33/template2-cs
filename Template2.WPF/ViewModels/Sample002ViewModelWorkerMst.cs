using Template2.Domain.Entities;
using Template2.Domain.ValueObjects;

namespace Template2.WPF.ViewModels
{
    public class Sample002ViewModelWorkerMst
    {
        public Sample002ViewModelWorkerMst(WorkerMstEntity entity)
        {
            Entity = entity;
        }

        public WorkerMstEntity Entity { get; private set; }

        public string WorkerCode
        {
            get { return Entity.WorkerCode.Value; }
            set { Entity.WorkerCode = new WorkerCode(value); }
        }

        public string WorkerName
        {
            get { return Entity.WorkerName.Value; }
            set { Entity.WorkerName = new WorkerName(value); }
        }

        public string WorkerGroupCode
        {
            get { return Entity.WorkerGroupCode.Value; }
            set { Entity.WorkerGroupCode = new WorkerGroupCode(value); }
        }
    }
}
