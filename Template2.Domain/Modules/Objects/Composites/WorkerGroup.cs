using Template2.Domain.Entities;

namespace Template2.Domain.Modules.Objects.Composites
{
    public class WorkerGroup : OrganizationComponentBase
    {
        public WorkerGroup(WorkerGroupMstEntity entity) : base(entity)
        {
        }
        public List<OrganizationComponentBase> Workers { get; private set; } = new List<OrganizationComponentBase>();

        protected override void Add(OrganizationComponentBase item)
        {
            Workers.Add(item);
        }
    }
}
