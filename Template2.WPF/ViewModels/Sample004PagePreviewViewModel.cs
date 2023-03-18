using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Template2.Domain;
using Template2.Domain.Entities;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample004PagePreviewViewModel : ViewModelBase
    {
        /// <summary>
        /// 動画読み込み確認タイマー
        /// </summary>
        private DispatcherTimer _movieLodingCheckTimer;


        public Sample004PagePreviewViewModel()
        {
            Debug.WriteLine("★Sample004PagePreviewViewModel:コンストラクタ処理開始");

            //// DelegateCommandメソッドを登録
            MediaServiceLoaded = new DelegateCommand<IMediaService>(MediaServiceLoadedExecute);
            MovieMediaOpened = new DelegateCommand(MovieMediaOpenedExecute);
            MoviePlayButton = new DelegateCommand(MoviePlayButtonExecute);
            MovieStopButton = new DelegateCommand(MovieStopButtonExecute);
        }

        public IMediaService MediaService {get; private set;}

        public PageMstEntity PreviewPageMstEntity { get; set; }

        /// <summary>
        /// MediaService読み込み後にプレビューを表示させる時はTrue
        /// </summary>
        public bool ShowPreviewImmediately { get; set; } = false;

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //Debug.WriteLine("★Sample004PagePreviewViewModel:OnNavigatedTo開始");

            ////// プレビュー表示するエンティティを受け取り
            //PreviewPageMstEntity = navigationContext.Parameters.GetValue<PageMstEntity>(nameof(PreviewPageMstEntity));

            //Debug.WriteLine("★Sample004PagePreviewViewModel:エンティティ格納完了");

            ////// 自身をSharedに格納
            //Shared.Sample004PagePreviewViewModel = this;
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private Uri _movieSource;
        public Uri MovieSource
        {
            get { return _movieSource; }
            set { SetProperty(ref _movieSource, value); }
        }

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set { SetProperty(ref _imageSource, value); }
        }

        /// <summary>
        /// ローディング中表示のVisibility
        /// </summary>
        private Visibility _loadingBarVisibility = Visibility.Hidden;
        public Visibility LoadingBarVisibility
        {
            get { return _loadingBarVisibility; }
            set { SetProperty(ref _loadingBarVisibility, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand<IMediaService> MediaServiceLoaded { get; }
        private void MediaServiceLoadedExecute(IMediaService mediaService)
        {
            this.MediaService = mediaService;

            if (ShowPreviewImmediately)
            {
                PreviewMovie();
                PreviewImage();
            }
        }

        public DelegateCommand MovieMediaOpened { get; }
        private void MovieMediaOpenedExecute()
        {
            LoadingBarVisibility = Visibility.Collapsed;
        }

        public DelegateCommand MoviePlayButton { get; }
        private void MoviePlayButtonExecute()
        {
            if (MediaService == null)
            {
                LoadingBarVisibility = Visibility.Collapsed;
                return;
            }

            MediaService.Play();
        }

        public DelegateCommand MovieStopButton { get; }
        private void MovieStopButtonExecute()
        {
            if (MediaService == null)
            {
                LoadingBarVisibility = Visibility.Collapsed;
                return;
            }

            MediaService.Stop();
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public void PreviewMovie()
        {
            if (PreviewPageMstEntity == null)
            {
                MovieSource = null;
                return;
            }

            string moviePath = PreviewPageMstEntity.MovieLink.Value;
            PreviewMovie(moviePath);
        }

        public void PreviewMovie(string moviePath)
        {
            if (File.Exists(moviePath) == false)
            {
                MovieSource = null;
                LoadingBarVisibility = Visibility.Collapsed;
                return;
            }

            if (MediaService != null)
            {
                if (!MediaService.IsPlaying())
                {
                    LoadingBarVisibility = Visibility.Visible;
                }
            }

            MovieSource = new Uri(moviePath, UriKind.Relative);
            MoviePlayButtonExecute();
        }

        public void PreviewImage()
        {
            if (PreviewPageMstEntity == null)
            {
                ImageSource = null;
                return;
            }

            string filePath = PageMstEntity.GetImageFilePath(PreviewPageMstEntity.ImageFolderLink.Value,
                                     PreviewPageMstEntity.ImagePageNo.Value);

            PreviewImage(filePath);
        }

        public void PreviewImage(string folderPath, int pageNo)
        {
            string filePath = PageMstEntity.GetImageFilePath(folderPath, pageNo);
            PreviewImage(filePath);
        }

        public void PreviewImage(string filePath)
        {

            Console.WriteLine("画像ファイル：" + filePath);

            if (File.Exists(filePath) == false)
            {
                ImageSource = null;
                return;
            }

            BitmapImage bmpImage = new BitmapImage();
            using (FileStream stream = File.OpenRead(filePath))
            {
                bmpImage.BeginInit();
                bmpImage.StreamSource = stream;
                bmpImage.DecodePixelWidth = 500;
                bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                bmpImage.CreateOptions = BitmapCreateOptions.None;
                bmpImage.EndInit();
                bmpImage.Freeze();
            }

            ImageSource = bmpImage;
        }

        #endregion
    }
}
