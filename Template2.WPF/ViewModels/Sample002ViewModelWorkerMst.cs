using Template2.Domain.Entities;
using Template2.Domain.ValueObjects;

namespace Template2.WPF.ViewModels
{
    public class Sample002ViewModelWorkerMst
    {
        private WorkerMstEntity _entity;

        public Sample002ViewModelWorkerMst(WorkerMstEntity entity)
        {
            _entity = entity;
        }

        public string WorkerCode
        {
            get { return _entity.WorkerCode.Value; }
            set { _entity.WorkerCode = new WorkerCode(value); }
        }
        public string WorkerName
        {
            get { return _entity.WorkerName.Value; }
            set { _entity.WorkerName = new WorkerName(value); }
        }

        public WorkerMstEntity Entity { get { return _entity; } }
    }
}
