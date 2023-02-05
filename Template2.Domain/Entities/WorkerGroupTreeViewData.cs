
using System.Collections.ObjectModel;

namespace Template2.Domain.Entities
{
    public class WorkerGroupTreeViewData
    {
        public WorkerGroupTreeViewData(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public WorkerGroupTreeViewData(string id, string name, WorkerGroupTreeViewData upperData)
        {
            Id = id;
            Name = name;
            UpperTreeViewData = upperData;
        }


        public string Id { get; }
        public string Name { get; }

        public WorkerGroupTreeViewData? UpperTreeViewData { get; } = null;

        public List<WorkerGroupTreeViewData> Workers { get; set; }
            = new List<WorkerGroupTreeViewData>();

        /// <summary>
        /// TreeViewを生成
        /// </summary>
        /// <param name="workerGroupTreeView"></param>
        /// <param name="workerGroupMstEntities"></param>
        /// <param name="workerMstEntities"></param>
        public static void CreateTreeView(ref ObservableCollection<WorkerGroupTreeViewData> workerGroupTreeView, 
                                          IReadOnlyList<WorkerGroupMstEntity> workerGroupMstEntities, 
                                          IReadOnlyList<WorkerMstEntity> workerMstEntities)
        {
            if (workerGroupTreeView == null)
            {
                workerGroupTreeView = new ObservableCollection<WorkerGroupTreeViewData>();
            }

            workerGroupTreeView.Clear();

            foreach (var workerGroupMstEntity in workerGroupMstEntities)
            {
                var treeViewData = new WorkerGroupTreeViewData(workerGroupMstEntity.WorkerGroupCode.Value,
                                                               workerGroupMstEntity.WorkerGroupName.Value);

                List<WorkerGroupTreeViewData> workers = new List<WorkerGroupTreeViewData>();

                foreach (var workerMstEntity in workerMstEntities)
                {
                    if (workerGroupMstEntity.WorkerGroupCode.Value == workerMstEntity.WorkerGroupCode.Value)
                    {
                        workers.Add(new WorkerGroupTreeViewData(workerMstEntity.WorkerCode.Value,
                                                                workerMstEntity.WorkerName.Value,
                                                                treeViewData));

                    }
                }

                treeViewData.Workers = workers;

                workerGroupTreeView.Add(treeViewData);
            }
        }
    }
}
