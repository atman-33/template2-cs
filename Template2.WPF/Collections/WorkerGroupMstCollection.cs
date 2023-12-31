using System.Collections.ObjectModel;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.WPF.ViewModelEntities;

namespace Template2.WPF.Collections
{
    public class WorkerGroupMstCollection: ObservableCollection<WorkerGroupMstViewModelEntity>
    {
        private IWorkerGroupMstRepository _workerGroupMstRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="workerGroupMstRepository"></param>
        public WorkerGroupMstCollection(IWorkerGroupMstRepository workerGroupMstRepository) {
            _workerGroupMstRepository = workerGroupMstRepository;
        }

        /// <summary>
        /// リポジトリから取得したデータをコレクションに格納
        /// </summary>
        public void LoadData()
        {
            Clear();
            var entities = _workerGroupMstRepository.GetData();
            foreach (var entity in entities)
            {
                Add(new WorkerGroupMstViewModelEntity(entity));
            }
        }

        /// <summary>
        /// コレクションのEntityを保存（Insert or Update）
        /// </summary>
        /// <param name="viewModelEntity"></param>
        public void UpsertItem(WorkerGroupMstViewModelEntity viewModelEntity) {

            _workerGroupMstRepository.Save(viewModelEntity.Entity);
            LoadData();
        }

        /// <summary>
        /// コレクションのEntityを保存（Insert or Update）
        /// </summary>
        /// <param name="entity"></param>
        public void UpsertItem(WorkerGroupMstEntity entity) {

            _workerGroupMstRepository.Save(entity);
            LoadData();
        }

        /// <summary>
        /// コレクションのEntityを削除
        /// </summary>
        /// <param name="viewModelEntity"></param>
        public void DeleteItem(WorkerGroupMstViewModelEntity viewModelEntity) {
        
            _workerGroupMstRepository.Delete(viewModelEntity.Entity);
            Remove(viewModelEntity);
        }
    }
}