using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class PageMstEntity
    {
        public const string ImageExtension = "JPG";
        public const string ImageFileFixedName = "スライド";

        public PageMstEntity(
            int pageId,
            string pageName,
            string? movieLink,
            string? imageFolderLink,
            int? imagePageNo,
            float slideWaitingTime,
            string? note1,
            string? note2,
            string? note3)
        {
            PageId = new PageId(pageId);
            PageName = new PageName(pageName);
            MovieLink = new MovieLink(movieLink);
            ImageFolderLink = new ImageFolderLink(imageFolderLink);
            ImagePageNo = new ImagePageNo(imagePageNo);
            SlideWaitingTime = new SlideWaitingTime(slideWaitingTime);
            Note1 = new Note(note1);
            Note2 = new Note(note2);
            Note3 = new Note(note3);
        }

        public PageId PageId { get; }
        public PageName PageName { get; }
        public MovieLink MovieLink { get; }
        public ImageFolderLink ImageFolderLink { get; }
        public ImagePageNo ImagePageNo { get; }
        public SlideWaitingTime SlideWaitingTime { get; }
        public Note Note1 { get; }
        public Note Note2 { get; }
        public Note Note3 { get; }

        public string ImageFilePath
        {
            get
            {
                return PageMstEntity.GetImageFilePath(ImageFolderLink.Value, PageId.Value);
            }
        }

        static public string GetImageFilePath(string? imageFolderLink, int? imagePageNo)
        {
            int pageNo;

            if (imageFolderLink == null)
            {
                return string.Empty;
            }

            if (imagePageNo == null)
            {
                pageNo = 0;
            }
            else
            {
                pageNo = (int)imagePageNo;
            }

            return imageFolderLink + "\\" + ImageFileFixedName + pageNo.ToString() + "." + ImageExtension;
        }

        static public void MergeEntity(ref IList<PageMstEntity> entities, PageMstEntity targetEntity)
        {
            //// 既にKeyのエンティティが存在するなら差し替え
            foreach (var entity in entities)
            {
                if (entity.PageId.Value == targetEntity.PageId.Value)
                {
                    var index = entities.IndexOf(entity);
                    entities[index] = targetEntity.Clone(targetEntity);
                    return;
                }
            }

            //// コレクションに存在しないエンティティなら追加
            entities.Add(targetEntity);
        }

        static public void RemoveViewModelEntity(ref IList<PageMstEntity> entities, PageMstEntity targetEntity)
        {
            //// 既にKeyのエンティティが存在するなら除去
            foreach (var entity in entities)
            {
                if (entity.PageId.Value == targetEntity.PageId.Value)
                {
                    var index = entities.IndexOf(entity);
                    entities.RemoveAt(index);
                    return;
                }
            }
        }

        public PageMstEntity Clone(PageMstEntity entity)
        {
            return (PageMstEntity)MemberwiseClone();
        }
    }
}
