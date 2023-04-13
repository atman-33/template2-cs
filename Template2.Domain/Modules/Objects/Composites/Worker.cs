using Template2.Domain.Entities;

namespace Template2.Domain.Modules.Objects.Composites
{
    public sealed class Worker : OrganizationComponentBase
    {
        public Worker(WorkerMstEntity entity) : base(entity)
        {
        }

        protected override void Add(OrganizationComponentBase item)
        {
            throw new NotImplementedException("Addはできません！");
        }
    }
}
