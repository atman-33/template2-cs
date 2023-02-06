using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Template2.Domain;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Helpers;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Views;

namespace Template2.WPF.ViewModels
{
    public class Sample004PageEditingViewModel : ViewModelBase, IDialogAware
    {
        /// <summary>
        /// ページプレビューを表示するContentRegion
        /// </summary>
        const string PagePreviewContentRegion = "PageEditingPagePreviewContentRegion";

        //// 外部接触Repository
        private IPageMstRepository _pageMstRepository;

        private Sample004PagePreviewViewModel _pagePreviewViewModel;

        public Sample004PageEditingViewModel(IRegionManager regionManager)
            : this(Factories.CreatePageMst())
        {
            MainRegionManager = regionManager;

            //// 注意：コンストラクタ内でContentRegionをナビゲートしても反映されない
        }

        public Sample004PageEditingViewModel(IPageMstRepository pageMstRepository)
        {
            _pageMstRepository = pageMstRepository;

            //// DelegateCommandメソッドを登録
            CancelButton = new DelegateCommand(CancelButtonExecute);
            SaveButton = new DelegateCommand(SaveButtonExecute);
            DeleteButton = new DelegateCommand(DeleteButtonExecute);

            PreviewButton = new DelegateCommand(PreviewButtonExecute);
            OpenMovieFileButton = new DelegateCommand(OpenMovieFileButtonExecute);
            OpenImageFileButton = new DelegateCommand(OpenImageFileButtonExecute);
            ImagePageNoDownButton = new DelegateCommand(ImagePageNoDownButtonExecute);
            ImagePageNoUpButton = new DelegateCommand(ImagePageNoUpButtonExecute);

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _pageMstRepository = Factories.CreatePageMst();

            //// 新規の説明入力レコードを生成
            for (int i = 1; i <= 3; i++)
            {
                NoteEntities.Add(new NoteEntity(String.Empty));
            }
        }

        public string Title => "ページ編集";

        /// <summary>
        /// 新規ページ登録の時はTrue
        /// </summary>
        public bool IsNewPage { get; private set; }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private string _pageIdText;
        public string PageIdText
        {
            get { return _pageIdText; }
            set { SetProperty(ref _pageIdText, value); }
        }

        private string _pageNameText;
        public string PageNameText
        {
            get { return _pageNameText; }
            set { SetProperty(ref _pageNameText, value); }
        }

        private string _movieLinkText;
        public string MovieLinkText
        {
            get { return _movieLinkText; }
            set { SetProperty(ref _movieLinkText, value); }
        }

        private string _imageFolderLinkText;
        public string ImageFolderLinkText
        {
            get { return _imageFolderLinkText; }
            set { SetProperty(ref _imageFolderLinkText, value); }
        }

        private int? _imagePageNoText = null;
        public int? ImagePageNoText
        {
            get { return _imagePageNoText; }
            set { SetProperty(ref _imagePageNoText, value); }
        }

        private float? _slideWaitingTimeText = null;
        public float? SlideWaitingTimeText
        {
            get { return _slideWaitingTimeText; }
            set { SetProperty(ref _slideWaitingTimeText, value); }
        }

        private ObservableCollection<NoteEntity> _noteEntities = new ObservableCollection<NoteEntity>();
        public ObservableCollection<NoteEntity> NoteEntities
        {
            get { return _noteEntities; }
            set { SetProperty(ref _noteEntities, value); }
        }

        /// <summary>
        /// メイン画面のリージョンマネージャー
        /// ※ダイアログ画面（子Window）からContentRegionを画面遷移する際は必要
        /// </summary>
        private IRegionManager _mainRegionManager;
        public IRegionManager MainRegionManager
        {
            get { return _mainRegionManager; }
            set { SetProperty(ref _mainRegionManager, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand OpenMovieFileButton { get; }
        private void OpenMovieFileButtonExecute()
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = "動画ファイルを選択してください",
                IsFolderPicker = false,     //// フォルダ選択モードにしない
            })
            {
                //// ファイルの種類を設定
                cofd.Filters.Add(new CommonFileDialogFilter("mp4", "*.mp4"));
                cofd.Filters.Add(new CommonFileDialogFilter("avi", "*.avi"));
                cofd.Filters.Add(new CommonFileDialogFilter("mov", "*.mov"));

                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                MovieLinkText = cofd.FileName;
            }

            //// 動画プレビュー更新
            _pagePreviewViewModel.PreviewMovie(MovieLinkText);
        }

        public DelegateCommand OpenImageFileButton { get; }
        private void OpenImageFileButtonExecute()
        {
            using (var cofd = new CommonOpenFileDialog()
            {
                Title = "画像フォルダを選択してください",
                IsFolderPicker = true,      //// フォルダ選択モードにする
            })
            {
                if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
                {
                    return;
                }

                ImageFolderLinkText = cofd.FileName;
            }

            //// ページNoを設定
            ImagePageNoText = 1;

            _pagePreviewViewModel.PreviewImage(ImageFolderLinkText, Convert.ToInt32(ImagePageNoText));
        }
        public DelegateCommand ImagePageNoDownButton { get; }
        private void ImagePageNoDownButtonExecute()
        {
            if (ImagePageNoText <= 1)
            {
                return;
            }

            ImagePageNoText = ImagePageNoText - 1;

            //// 画像プレビュー更新
            _pagePreviewViewModel.PreviewImage(ImageFolderLinkText, Convert.ToInt32(ImagePageNoText));
        }
        public DelegateCommand ImagePageNoUpButton { get; }
        private void ImagePageNoUpButtonExecute()
        {
            //// ページNoをインクリしてファイルが無ければ何もしない
            string filePath = PageMstEntity.GetImageFilePath(ImageFolderLinkText, ImagePageNoText + 1);

            if (File.Exists(filePath) == false)
            {
                return;
            }

            ImagePageNoText = ImagePageNoText + 1;

            //// 画像プレビュー更新
            _pagePreviewViewModel.PreviewImage(ImageFolderLinkText, Convert.ToInt32(ImagePageNoText));
        }

        public DelegateCommand PreviewButton { get; }
        private void PreviewButtonExecute()
        {
            var entity = new PageMstEntity(
                0,                              //// プレビュー画面でありページIDは0で設定
                PageNameText,
                MovieLinkText,
                ImageFolderLinkText,
                ImagePageNoText,
                (float)SlideWaitingTimeText,
                NoteEntities[0].Note,
                NoteEntities[1].Note,
                NoteEntities[2].Note
                );

            //// ページプレビューにエンティティをセット
            _pagePreviewViewModel.PreviewPageMstEntity = entity;

            //// 動画プレビュー更新
            _pagePreviewViewModel.PreviewMovie();
            //// 画像プレビュー更新
            _pagePreviewViewModel.PreviewImage();
        }

        public DelegateCommand CancelButton { get; }
        private void CancelButtonExecute()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }


        public DelegateCommand SaveButton { get; }
        private void SaveButtonExecute()
        {
            Guard.IsNullOrEmpty(PageNameText, "ページ名称を入力してください。");
            Guard.IsNullOrEmpty(SlideWaitingTimeText, "スライド停止時間を入力してください。");
            var slideWaitingTimeText = Guard.IsFloat(SlideWaitingTimeText.ToString(), "スライド停止時間の入力に誤りがあります。");

            if (_messageService.Question("保存しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            int pageId;
            if (IsNewPage == true)
            {
                pageId = _pageMstRepository.GetNextId();
            }
            else
            {
                pageId = Convert.ToInt32(PageIdText);
            }

            var entity = new PageMstEntity(
                pageId,
                PageNameText,
                MovieLinkText,
                ImageFolderLinkText,
                ImagePageNoText,
                (float)SlideWaitingTimeText,
                NoteEntities[0].Note,
                NoteEntities[1].Note,
                NoteEntities[2].Note
                );

            _pageMstRepository.Save(entity);

            //// 保存完了のメッセージは表示させない
            // _messageService.ShowDialog("保存しました", "情報", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);

            var p = new DialogParameters();
            p.Add(nameof(PageMstEntity), entity);

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        }

        public DelegateCommand DeleteButton { get; }
        private void DeleteButtonExecute()
        {
            if (IsNewPage)
            {
                return;
            }

            if (_messageService.Question("削除しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            var entity = new PageMstEntity(
                Convert.ToInt32(PageIdText),
                PageNameText,
                MovieLinkText,
                ImageFolderLinkText,
                ImagePageNoText,
                (float)SlideWaitingTimeText,
                NoteEntities[0].Note,
                NoteEntities[1].Note,
                NoteEntities[2].Note
                );

            _pageMstRepository.Delete(entity);
            
            var p = new DialogParameters();
            p.Add(nameof(PageMstEntity), entity);

            RequestClose?.Invoke(new DialogResult(ButtonResult.No, p));
        }


        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            //// 画面内のリージョンを除去しておかないとエラーが発生してしまう
            MainRegionManager.Regions.Remove(PagePreviewContentRegion);
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            //// ページプレビューのContentRegionを表示
            MainRegionManager.RequestNavigate(PagePreviewContentRegion, nameof(Sample004PagePreviewView));
            _pagePreviewViewModel = Shared.Sample004PagePreviewViewModel as Sample004PagePreviewViewModel;

            //// 新規ページかどうか設定
            IsNewPage = parameters.GetValue<bool>(nameof(IsNewPage));

            if (IsNewPage)
            {
                PageIdText = "（新規のため未採番）";
            }
            else
            {
                var vme = parameters.GetValue<Sample004PageListViewModelPageMst>
                    (nameof(Sample004PageListViewModel.PageMstEntitiesSlectedItem));
                var editingEntity = vme.Entity;

                //// 編集対象のエンティティ情報を初期値に設定
                PageIdText = editingEntity.PageId.Value.ToString();
                PageNameText = editingEntity.PageName.Value;
                MovieLinkText = editingEntity.MovieLink.Value;
                ImageFolderLinkText = editingEntity.ImageFolderLink.Value;
                ImagePageNoText = editingEntity.ImagePageNo.Value;
                SlideWaitingTimeText = editingEntity.SlideWaitingTime.Value;

                NoteEntities[0] = new NoteEntity(editingEntity.Note1.Value);
                NoteEntities[1] = new NoteEntity(editingEntity.Note2.Value);
                NoteEntities[2] = new NoteEntity(editingEntity.Note3.Value);

                //// 編集モードはプレビュー表示
                PreviewButtonExecute();
            }
        }

        #endregion
    }
}
