using System.Collections.ObjectModel;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.WPF.ViewModelEntities;

namespace Template2.WPF.Collections
{
    public class WorkerMstCollection : ObservableCollection<WorkerMstViewModelEntity>
    {
        private IWorkerMstRepository _workerMstRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="workerMstRepository"></param>
        public WorkerMstCollection(IWorkerMstRepository workerMstRepository)
        {
            _workerMstRepository = workerMstRepository;
        }

        /// <summary>
        /// リポジトリから取得したデータをコレクションに格納
        /// </summary>
        public void LoadData()
        {
            Clear();
            var entities = _workerMstRepository.GetData();
            foreach (var entity in entities)
            {
                Add(new WorkerMstViewModelEntity(entity));
            }
        }

        /// <summary>
        /// コレクションのEntityを保存（Insert or Update）
        /// </summary>
        /// <param name="viewModelEntity"></param>
        public void UpsertItem(WorkerMstViewModelEntity viewModelEntity)
        {

            _workerMstRepository.Save(viewModelEntity.Entity);
            LoadData();
        }

        /// <summary>
        /// コレクションのEntityを保存（Insert or Update）
        /// </summary>
        /// <param name="entity"></param>
        public void UpsertItem(WorkerMstEntity entity)
        {

            _workerMstRepository.Save(entity);
            LoadData();
        }

        /// <summary>
        /// コレクションのEntityを削除
        /// </summary>
        /// <param name="viewModelEntity"></param>
        public void DeleteItem(WorkerMstViewModelEntity viewModelEntity)
        {

            _workerMstRepository.Delete(viewModelEntity.Entity);
            Remove(viewModelEntity);
        }

        public void AddNewItem()
        {
            var newItem = new WorkerMstEntity(string.Empty, string.Empty, string.Empty);
            Add(new WorkerMstViewModelEntity(newItem));
        }
    }
}