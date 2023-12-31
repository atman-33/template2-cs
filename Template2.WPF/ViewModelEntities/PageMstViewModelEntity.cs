using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media.Imaging;
using Template2.Domain.Entities;

namespace Template2.WPF.ViewModelEntities
{
    public class PageMstViewModelEntity
    {
        public PageMstViewModelEntity(PageMstEntity entity)
        {
            Entity = entity;
        }
        public PageMstEntity Entity { get; private set; }

        public int PageId => Entity.PageId.Value;
        public string PageName => Entity.PageName.Value;
        public string MovieLink => Entity.MovieLink.Value;
        public string ImageFolderLink => Entity.ImageFolderLink.Value;
        public int? ImagePageNo => Entity.ImagePageNo.Value;
        public float SlideWaitingTime => Entity.SlideWaitingTime.Value;
        public string Note1 => Entity.Note1.Value;
        public string Note2 => Entity.Note2.Value;
        public string Note3 => Entity.Note3.Value;

        public BitmapImage Thumbnail 
        { 
            get
            {
                var imagePath = Entity.ImageFilePath;

                if (File.Exists(imagePath) == false)
                {
                    return null;
                }

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                bitmap.DecodePixelWidth = 100;
                //bitmap.DecodePixelHeight = 100;
                bitmap.UriSource = new Uri(imagePath);
                bitmap.EndInit();
                return bitmap;
            } 
        }
    }
}
