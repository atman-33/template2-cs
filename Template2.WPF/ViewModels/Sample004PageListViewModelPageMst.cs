using System.Collections.ObjectModel;
using Template2.Domain.Entities;

namespace Template2.WPF.ViewModels
{
    public class Sample004PageListViewModelPageMst
    {
        public Sample004PageListViewModelPageMst(PageMstEntity entity)
        {
            Entity = entity;
        }

        public int PageId => Entity.PageId.Value;
        public string PageName => Entity.PageName.Value;
        public string? MovieLink => Entity.MovieLink.Value;
        public string? ImageFolderLink => Entity.ImageFolderLink.Value;
        public int? ImagePageNo => Entity.ImagePageNo.Value;
        public float SlideWaitingTime => Entity.SlideWaitingTime.Value;
        public string? Note1 => Entity.Note1.Value;
        public string? Note2 => Entity.Note2.Value;
        public string? Note3 => Entity.Note3.Value;

        public PageMstEntity Entity { get; private set; }

        static public void MergeViewModelEntity(ref ObservableCollection<Sample004PageListViewModelPageMst> viewModelEntities, 
                                                PageMstEntity entity)
        {
            //// 既にKeyのエンティティが存在するなら差し替え
            foreach(var viewModelEntity in viewModelEntities)
            {
                if (viewModelEntity.Entity.PageId.Value == entity.PageId.Value)
                {
                    var index = viewModelEntities.IndexOf(viewModelEntity);
                    viewModelEntities[index] = new Sample004PageListViewModelPageMst(entity);
                    return;
                }

            }

            //// コレクションに存在しないエンティティなら追加
            viewModelEntities.Add(new Sample004PageListViewModelPageMst(entity));
        }

        static public void RemoveViewModelEntity(ref ObservableCollection<Sample004PageListViewModelPageMst> viewModelEntities,
                                                 PageMstEntity entity)
        {
            //// 既にKeyのエンティティが存在するなら除去
            foreach (var viewModelEntity in viewModelEntities)
            {
                if (viewModelEntity.Entity.PageId.Value == entity.PageId.Value)
                {
                    var index = viewModelEntities.IndexOf(viewModelEntity);
                    viewModelEntities.RemoveAt(index);
                    return;
                }
            }
        }
    }
}
