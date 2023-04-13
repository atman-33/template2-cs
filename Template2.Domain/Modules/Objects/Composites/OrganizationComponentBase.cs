
using System.Collections.ObjectModel;
using Template2.Domain.Entities;

namespace Template2.Domain.Modules.Objects.Composites
{
    public abstract class OrganizationComponentBase
    {
        public OrganizationComponentBase(WorkerGroupMstEntity entity)
        {
            Name = entity.WorkerGroupName.Value;
            Code = entity.WorkerGroupCode.Value;
            WorkerGroupCode = entity.WorkerGroupCode.Value;
            WorkerGroupName = entity.WorkerGroupName.Value;
        }

        public OrganizationComponentBase(WorkerMstEntity entity)
        {
            Name = entity.WorkerName.Value;
            Code = entity.WorkerCode.Value;
            WorkerGroupCode = entity.WorkerGroupCode.Value;
            WorkerGroupName = string.Empty;
        }

        public string Name { get; private set;  }
        public string Code { get; private set;  }

        public string WorkerGroupName { get; internal set; }

        public string WorkerGroupCode { get; private set; }


        protected abstract void Add(OrganizationComponentBase item);

        /// <summary>
        /// 作業者グループと作業者のCompositesを生成
        /// </summary>
        /// <param name="workerGroupMstEntities"></param>
        /// <param name="workerMstEntities"></param>
        /// <returns></returns>
        public static IReadOnlyList<OrganizationComponentBase> CreateOrganizationComponents(
            IReadOnlyList<WorkerGroupMstEntity> workerGroupMstEntities,
            IReadOnlyList<WorkerMstEntity> workerMstEntities)
        {
            var workerGroups = new List<WorkerGroup>();
            var workers = new List<Worker>();

            foreach (var workerGroupMstEntity in workerGroupMstEntities)
            {
                workerGroups.Add(new WorkerGroup(workerGroupMstEntity));
            }

            foreach (var workerMstEntity in workerMstEntities)
            {
                workers.Add(new Worker(workerMstEntity));
            }

            foreach(var worker in workers)
            {
                var workerGroup = workerGroups.Find(x => x.WorkerGroupCode == worker.WorkerGroupCode);

                if (workerGroup != null)
                {
                    workerGroup.Add(worker);
                    worker.WorkerGroupName = workerGroup.Name;
                }
            }

            return workerGroups;
        }
    }
}
