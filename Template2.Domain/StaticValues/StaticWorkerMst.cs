using System.Collections;
using System.Collections.ObjectModel;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Domain.StaticValues
{
    /// <summary>
    /// StaticなWorkerMstエンティティコレクション
    /// </summary>
    /// <remarks>
    /// アプリ内で共通して利用するデータ群
    /// </remarks>
    public static class StaticWorkerMst
    {
        public static ObservableCollection<WorkerMstEntity> Entities { get; private set; }
            = new ObservableCollection<WorkerMstEntity>();

        /// <summary>
        /// データ更新
        /// </summary>
        /// <param name="repository"></param>
        public static void Update(IWorkerMstRepository repository)
        {
            //// 処理途中に値が変わらないようにロックする
            lock (((ICollection)Entities).SyncRoot)
            {
                Entities.Clear();

                foreach (var entity in repository.GetData())
                {
                    Entities.Add(entity);
                }
            }
        }
    }
}
