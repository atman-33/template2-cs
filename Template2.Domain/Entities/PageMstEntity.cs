using Template2.Domain.ValueObjects;

namespace Template2.Domain.Entities
{
    public sealed class PageMstEntity
    {
        public const string ImageExtension = "JPG";
        public const string ImageFileFixedName = "ƒXƒ‰ƒCƒh";

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


        static public string GetImageFilePath(string imageFolderLink, int? imagePageNo)
        {
            int pageNo;

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

        public PageMstEntity Clone(PageMstEntity entity)
        {
            return (PageMstEntity)MemberwiseClone();
        }
    }
}
