using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.WPF.ViewModelEntities;

namespace Template2.WPF.Collections
{
    public class PageMstCollection : ObservableCollection<PageMstViewModelEntity>
    {
        private IPageMstRepository _pageMstRepository;

        /// <summary>
        /// コレクションの全データ（コレクションのフィルター処理で利用）
        /// </summary>
        private IEnumerable<PageMstViewModelEntity> _original;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pageMstRepository"></param>
        public PageMstCollection(IPageMstRepository pageMstRepository)
        {
            _pageMstRepository = pageMstRepository;
        }

        /// <summary>
        /// リポジトリから取得したデータをコレクションに格納
        /// </summary>
        public void LoadData()
        {
            Clear();
            var entities = _pageMstRepository.GetData();
            foreach (var entity in entities)
            {
                Add(new PageMstViewModelEntity(entity));
            }

            //// Originalデータを退避
            PageMstViewModelEntity[] allData;
            allData = new PageMstViewModelEntity[entities.Count];
            CopyTo(allData, 0);
            _original = new Collection<PageMstViewModelEntity>(allData);
        }

        /// <summary>
        /// コレクションのEntityを保存（Insert or Update）
        /// </summary>
        /// <param name="viewModelEntity"></param>
        public void UpsertItem(PageMstViewModelEntity viewModelEntity)
        {

            _pageMstRepository.Save(viewModelEntity.Entity);
            LoadData();
        }

        /// <summary>
        /// コレクションのEntityを保存（Insert or Update）
        /// </summary>
        /// <param name="entity"></param>
        public void UpsertItem(PageMstEntity entity)
        {

            _pageMstRepository.Save(entity);
            LoadData();
        }

        /// <summary>
        /// コレクションのEntityを削除
        /// </summary>
        /// <param name="viewModelEntity"></param>
        public void DeleteItem(PageMstViewModelEntity viewModelEntity)
        {

            _pageMstRepository.Delete(viewModelEntity.Entity);
            Remove(viewModelEntity);
        }

        /// <summary>
        /// コレクションをフィルター
        /// </summary>
        /// <param name="pageName"></param>
        public void FilterByPageName(string pageName)
        {
            var pages = _original.Where(x => x.PageName.Contains(pageName));

            Clear();
            this.AddRange(pages);
        }

        /// <summary>
        /// エンティティをコレクションに追加（DB保存無し）
        /// </summary>
        /// <param name="targetEntity"></param>
        public void MergeWithoutSave(PageMstEntity targetEntity)
        {
            UpdateOriginal();

            //// 既にKeyのエンティティが存在するなら差し替え
            foreach (var viewModelEntity in this)
            {
                if (viewModelEntity.Entity.PageId.Value == targetEntity.PageId.Value)
                {
                    var index = this.IndexOf(viewModelEntity);
                    this[index] = new PageMstViewModelEntity(targetEntity);
                    return;
                }
            }

            //// コレクションに存在しないエンティティなら追加
            this.Add(new PageMstViewModelEntity(targetEntity));
        }

        /// <summary>
        /// エンティティをコレクションから削除（DB保存無し）
        /// </summary>
        /// <param name="targetEntity"></param>
        public void RemoveWithoutSave(PageMstEntity targetEntity)
        {
            UpdateOriginal();

            //// 既にKeyのエンティティが存在するなら除去
            foreach (var viewModelEntity in this)
            {
                if (viewModelEntity.Entity.PageId.Value == targetEntity.PageId.Value)
                {
                    var index = this.IndexOf(viewModelEntity);
                    this.RemoveAt(index);
                    return;
                }
            }
        }

        /// <summary>
        /// フィルター用のOriginalデータを更新（本体のObservableCollectionは更新しない）
        /// </summary>
        private void UpdateOriginal()
        {
            var temp = new Collection<PageMstViewModelEntity>();
            var entities = _pageMstRepository.GetData();
            foreach (var entity in entities)
            {
                temp.Add(new PageMstViewModelEntity(entity));
            }

            //// Originalデータを退避
            PageMstViewModelEntity[] allData;
            allData = new PageMstViewModelEntity[entities.Count];
            temp.CopyTo(allData, 0);
            _original = new Collection<PageMstViewModelEntity>(allData);
        }
    }
}