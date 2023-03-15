using MS.WindowsAPICodePack.Internal;
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

namespace Template2.WPF.ViewModels
{
    public class Sample004PagePreviewViewModel : ViewModelBase
    {
        private MediaElement _movieMediaElement;

        /// <summary>
        /// 動画読み込み確認タイマー
        /// </summary>
        private DispatcherTimer _movieLodingCheckTimer;


        public Sample004PagePreviewViewModel()
        {
            Debug.WriteLine("★Sample004PagePreviewViewModel:コンストラクタ処理開始");

            //// 動画エレメントを設定
            _movieMediaElement = new MediaElement();
            MovieItemsControl.Add(_movieMediaElement);

            //// DelegateCommandメソッドを登録
            MoviePlayButton = new DelegateCommand(MoviePlayButtonExecute);
            MovieStopButton = new DelegateCommand(MovieStopButtonExecute);
        }

        public PageMstEntity PreviewPageMstEntity { get; set; }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private ObservableCollection<UIElement> _movieItemsControl = new ObservableCollection<UIElement>();
        public ObservableCollection<UIElement> MovieItemsControl
        {
            get { return _movieItemsControl; }
            set { SetProperty(ref _movieItemsControl, value); }
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
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        public DelegateCommand MoviePlayButton { get; }
        private async void MoviePlayButtonExecute()
        {
            LoadingBarVisibility = Visibility.Visible;

            await Task.Delay(2000);

            _movieMediaElement.Position = TimeSpan.Zero;

            _movieMediaElement.Play();

            //// LoadingBarVisibilityはUIスレッド上で操作が必要なため、Invokeで実行
            Application.Current.Dispatcher.Invoke(
                (Action)delegate () { CheckLoading(); });
        }

        public DelegateCommand MovieStopButton { get; }
        private void MovieStopButtonExecute()
        {
            _movieMediaElement.Stop();
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// 動画のローディング中を監視
        /// </summary>
        private async void CheckLoading()
        {
            for (int i = 0; i < 10000; i++)
            {
                if (_movieMediaElement.IsLoaded)
                {
                    // Debug.WriteLine($"動画ローディング確認中...{_movieMediaElement.IsLoaded}");

                    LoadingBarVisibility = Visibility.Hidden;
                    return;
                }

                await Task.Delay(50);
            }
        }

        public void PreviewMovie()
        {
            if (PreviewPageMstEntity == null)
            {
                _movieMediaElement.Source = null;
                return;
            }

            string moviePath = PreviewPageMstEntity.MovieLink.Value;
            PreviewMovie(moviePath);
        }

        public void PreviewMovie(string moviePath)
        {
            if (File.Exists(moviePath) == false)
            {
                _movieMediaElement.Source = null;
                return;
            }

            _movieMediaElement.Source = new Uri(moviePath, UriKind.Relative);

            _movieMediaElement.Position = TimeSpan.Zero;
            _movieMediaElement.Visibility = System.Windows.Visibility.Visible;
            _movieMediaElement.LoadedBehavior = MediaState.Manual;

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

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            Debug.WriteLine("★Sample004PagePreviewViewModel:OnNavigatedTo開始");

            //// プレビュー表示するエンティティを受け取り
            PreviewPageMstEntity = navigationContext.Parameters.GetValue<PageMstEntity>(nameof(PreviewPageMstEntity));

            Debug.WriteLine("★Sample004PagePreviewViewModel:エンティティ格納完了");

            //// 自身をSharedに格納
            Shared.Sample004PagePreviewViewModel = this;
        }

        #endregion

    }
}
