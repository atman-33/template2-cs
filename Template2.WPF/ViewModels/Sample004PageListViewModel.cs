using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Helpers;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Collections;
using Template2.WPF.Dto.Input;
using Template2.WPF.Services;
using Template2.WPF.ViewModelEntities;
using Template2.WPF.Views;

namespace Template2.WPF.ViewModels
{
    public class Sample004PageListViewModel : ViewModelBase
    {
        //// 外部接触Repository
        private IPageMstRepository _pageMstRepository;

        /// <summary>
        /// プレビューのViewModel
        /// </summary>
        private Sample004PagePreviewViewModel _pagePreviewViewModel;

        public Sample004PageListViewModel(
            IRegionManager regionManager,
            IDialogService dialogService,
            IEventAggregator eventAggregator)
            : this(regionManager, dialogService, eventAggregator, new MessageService(), Factories.CreatePageMst())
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager"></param>
        /// <param name="dialogService"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="messageService"></param>
        /// <param name="pageMstRepository"></param>
        /// <remarks>
        /// 【補足】
        /// コンストラクタ内では、追加したRegion（今回の_contentRegionNameであり部分View）が
        /// _regionManagerに追加されていないため注意。つまり、この時点では部分Viewを操作できない。
        /// 部分Viewを操作する場合、ViewのLoadイベント等に実装すればよい。
        /// </remarks>
        public Sample004PageListViewModel(
            IRegionManager regionManager,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IMessageService messageService,
            IPageMstRepository pageMstRepository)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion(_contentRegionName, nameof(Sample004PagePreviewView));

            _dialogService = dialogService;

            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル004（画像を表示するDataGrid）");

            _messageService = messageService;

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _pageMstRepository = pageMstRepository;

            //// Repositoryからデータ取得
            _pageMstCollection = new PageMstCollection(pageMstRepository);
            _pageMstCollection.LoadData();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private readonly string _contentRegionName = "PageListPagePreviewContentRegion";
        public string ContentRegionName
        {
            get { return _contentRegionName; }
        }

        /// <summary>
        /// ページ名称検索ボックスのテキスト
        /// </summary>
        private string _searchingPageNameText = string.Empty;
        public string SearchingPageNameText
        {
            get { return _searchingPageNameText; }
            set { SetProperty(ref _searchingPageNameText, value); }
        }

        /// <summary>
        /// 登録済みページ一覧を表示するDataGridのItemsSource
        /// </summary>
        private PageMstCollection _pageMstCollection;
        public PageMstCollection PageMstCollection
        {
            get { return _pageMstCollection; }
            set { SetProperty(ref _pageMstCollection, value); }
        }

        /// <summary>
        /// 登録済みページ一覧の選択アイテム
        /// </summary>
        private PageMstViewModelEntity _pageMstCollectionSlectedItem;
        public PageMstViewModelEntity PageMstCollectionSlectedItem
        {
            get { return _pageMstCollectionSlectedItem; }
            set { SetProperty(ref _pageMstCollectionSlectedItem, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// ContentRegionにViewを設定
        /// </summary>
        /// <remarks>
        /// 【補足】
        /// 1つのContentRegionに1つのViewが対応しているため、Views.FirstOrDefaultでOK
        /// </remarks>
        public DelegateCommand ViewLoaded =>
            new DelegateCommand(() =>
            {
                //// 部分ViewのViewModelをプライベート変数に格納
                var view = _regionManager.Regions[_contentRegionName].Views.FirstOrDefault() as Sample004PagePreviewView;
                _pagePreviewViewModel = view.DataContext as Sample004PagePreviewViewModel;

            });

        /// <summary>
        /// ページ編集画面を表示（新規）
        /// </summary>
        public DelegateCommand NewButton =>
            new DelegateCommand(() =>
            {
                var dto = new Sample004PageEditingViewModelInputDto(true, null);

                var p = new DialogParameters
                {
                    { nameof(Sample004PageEditingViewModelInputDto), dto }
                };

                _dialogService.ShowDialog(nameof(Sample004PageEditingView), p, Sample004PageEditingViewClose);

            });

        /// <summary>
        /// ページ名称検索テキストが変化した際の処理
        /// </summary>
        public DelegateCommand SearchingPageNameTextChanged =>
            new DelegateCommand(() =>
            {
                if (SearchingPageNameText == null)
                {
                    return;
                }

                PageMstCollection.FilterByPageName(SearchingPageNameText);
            });

        /// <summary>
        /// ページ一覧のDataGridの選択セルが変化した際の処理
        /// </summary>
        public DelegateCommand PageMstCollectionSelectedCellsChanged =>
            new DelegateCommand(() =>
            {
                if (PageMstCollectionSlectedItem == null)
                {
                    return;
                }

                //// プレビュー画面を更新
                PreviewPage();

            });

        public DelegateCommand EditButton =>
            new DelegateCommand(() =>
            {
                Guard.IsNull(PageMstCollectionSlectedItem, "編集するページを選択してください。");

                var dto = new Sample004PageEditingViewModelInputDto(
                    false,
                    PageMstCollectionSlectedItem.Entity);

                var p = new DialogParameters
                {
                    { nameof(Sample004PageEditingViewModelInputDto), dto },
                };

                _dialogService.ShowDialog(nameof(Sample004PageEditingView), p, Sample004PageEditingViewClose);

            });

        private void Sample004PageEditingViewClose(IDialogResult dialogResult)
        {
            //// OK => 保存 => 一覧に追加
            if (dialogResult.Result == ButtonResult.OK)
            {
                //// 編集後のデータを追加もしくは更新
                var dto = dialogResult.Parameters.GetValue<Sample004PageListViewModelInputDto>(nameof(Sample004PageListViewModelInputDto));
                var handedEntity = dto.PageMstEntityToSave;
                PageMstCollection.MergeWithoutSave(handedEntity);
                PageMstCollectionSlectedItem = PageMstCollection.FirstOrDefault(x => x.Entity.PageId.Value == handedEntity.PageId.Value);

                //// プレビュー更新
                PreviewPage();
            }

            //// No => 削除 => 一覧から除去
            if (dialogResult.Result == ButtonResult.No)
            {
                //// 編集後のデータを追加もしくは更新
                var dto = dialogResult.Parameters.GetValue<Sample004PageListViewModelInputDto>(nameof(Sample004PageListViewModelInputDto));
                var handedEntity = dto.PageMstEntityToDelete;
                PageMstCollection.RemoveWithoutSave(handedEntity);
                PageMstCollectionSlectedItem = null;

                //// プレビュー更新
                PreviewPage();
            }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private void PreviewPage()
        {
            PageMstEntity entity;

            if (PageMstCollectionSlectedItem == null)
            {
                entity = null;
            }
            else
            {
                entity = PageMstCollectionSlectedItem.Entity;
            }

            //// プレビュー用エンティティを格納
            _pagePreviewViewModel.PreviewPageMstEntity = entity;

            //// 動画プレビュー更新
            _pagePreviewViewModel.PreviewMovie();
            //// 画像プレビュー更新
            _pagePreviewViewModel.PreviewImage();
        }

        #endregion
    }
}
