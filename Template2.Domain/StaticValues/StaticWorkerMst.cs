using System.Collections;
using System.Collections.ObjectModel;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Domain.StaticValues
{
    public static class StaticWorkerMst
    {
        private static ObservableCollection<WorkerMstEntity> _entities = new ObservableCollection<WorkerMstEntity>();

        public static ObservableCollection<WorkerMstEntity> Entities 
        { 
            get 
            {
                return _entities;
            } 
        }

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="repository"></param>
        public static void Update(IWorkerMstRepository repository)
        {
            //// 処理途中に値が変わらないようにロックする
            lock (((ICollection)_entities).SyncRoot)
            {
                _entities.Clear();

                foreach (var entity in repository.GetData())
                {
                    _entities.Add(entity);
                }
            }
        }
    }
}
