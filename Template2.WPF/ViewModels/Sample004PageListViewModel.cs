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
using Template2.WPF.Events;
using Template2.WPF.Services;
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

        /// <summary>
        /// PageMstEntitiesのオリジン（未フィルターのコレクション）
        /// </summary>
        private ObservableCollection<Sample004PageListViewModelPageMst> _pageMstEntitiesOrigin
            = new ObservableCollection<Sample004PageListViewModelPageMst>();

        public Sample004PageListViewModel(
            IRegionManager regionManager, 
            IDialogService dialogService, 
            IEventAggregator eventAggregator)
            : this(regionManager,dialogService,eventAggregator,new MessageService(), Factories.CreatePageMst())
        {
        }

        public Sample004PageListViewModel(
            IRegionManager regionManager,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            IMessageService messageService,
            IPageMstRepository pageMstRepository)
        {
            _regionManager = regionManager;
            _regionManager.RegisterViewWithRegion(_contentRegionName, nameof(Sample004PagePreviewView));

            //// 【補足】
            //// コンストラクタ内では、追加したRegion（今回の_contentRegionNameであり部分View）が
            //// _regionManagerに追加されていないため注意すること。つまり、この時点では部分Viewを操作できない。
            //// 部分Viewを操作する場合、ViewのLoadイベント等に実装すればよい。

            _dialogService = dialogService;

            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル004（画像を表示するDataGrid）");

            _messageService = messageService;

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _pageMstRepository = pageMstRepository;

            //// DelegateCommandメソッドを登録
            NewButton = new DelegateCommand(NewButtonExecute);
            ViewLoaded = new DelegateCommand(ViewLoadedExecute);
            SearchingPageNameTextChanged = new DelegateCommand(SearchingPageNameTextChangedExecute);
            PageMstEntitiesSelectedCellsChanged = new DelegateCommand(PageMstEntitiesSelectedCellsChangedExecute);
            EditButton = new DelegateCommand(EditButtonExecute);

            //// Repositoryからデータ取得
            UpdatePageMstEntities();
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
        private string _searchingPageNameText = String.Empty;
        public string SearchingPageNameText
        {
            get { return _searchingPageNameText; }
            set { SetProperty(ref _searchingPageNameText, value); }
        }

        /// <summary>
        /// 登録済みページ一覧を表示するDataGridのItemsSource
        /// </summary>
        private ObservableCollection<Sample004PageListViewModelPageMst> _pageMstEntities
            = new ObservableCollection<Sample004PageListViewModelPageMst>();
        public ObservableCollection<Sample004PageListViewModelPageMst> PageMstEntities
        {
            get { return _pageMstEntities; }
            set { SetProperty(ref _pageMstEntities, value); }
        }

        /// <summary>
        /// 登録済みページ一覧の選択アイテム
        /// </summary>
        private Sample004PageListViewModelPageMst _pageMstEntitiesSlectedItem;
        public Sample004PageListViewModelPageMst PageMstEntitiesSlectedItem
        {
            get { return _pageMstEntitiesSlectedItem; }
            set { SetProperty(ref _pageMstEntitiesSlectedItem, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand ViewLoaded { get; }

        private void ViewLoadedExecute()
        {
            //// 部分ViewのViewModelをプライベート変数に格納
            var view = _regionManager.Regions[_contentRegionName].Views.FirstOrDefault() as Sample004PagePreviewView;
            _pagePreviewViewModel = view.DataContext as Sample004PagePreviewViewModel;

            //// 【補足】
            //// 1つのContentRegionに1つのViewが対応しているため、Views.FirstOrDefaultでOK
        }

        /// <summary>
        /// ページ編集画面を表示（新規）
        /// </summary>
        public DelegateCommand NewButton { get; }

        private void NewButtonExecute()
        {
            var p = new DialogParameters();
            p.Add(nameof(Sample004PageEditingViewModel.IsNewPage), true);

            _dialogService.ShowDialog(nameof(Sample004PageEditingView), p, Sample004PageEditingViewClose);
        }

        /// <summary>
        /// ページ名称検索テキストが変化した際の処理
        /// </summary>
        public DelegateCommand SearchingPageNameTextChanged { get; }

        private void SearchingPageNameTextChangedExecute()
        {
            if (SearchingPageNameText == null)
            {
                return;
            }

            var enumerable = _pageMstEntitiesOrigin.Where(x => x.PageName.Contains(SearchingPageNameText));
            PageMstEntities = new ObservableCollection<Sample004PageListViewModelPageMst>(enumerable);
        }

        /// <summary>
        /// ページ一覧のDataGridの選択セルが変化した際の処理
        /// </summary>
        public DelegateCommand PageMstEntitiesSelectedCellsChanged { get; }

        private void PageMstEntitiesSelectedCellsChangedExecute()
        {
            if (PageMstEntitiesSlectedItem == null)
            {
                return;
            }

            //// プレビュー画面を更新
            PreviewPage();
        }

        public DelegateCommand EditButton { get; }

        private void EditButtonExecute()
        {
            Guard.IsNull(PageMstEntitiesSlectedItem, "編集するページを選択してください。");

            var p = new DialogParameters
            {
                { nameof(Sample004PageEditingViewModel.IsNewPage), false },
                { nameof(PageMstEntity), PageMstEntitiesSlectedItem.Entity }
            };

            _dialogService.ShowDialog(nameof(Sample004PageEditingView), p, Sample004PageEditingViewClose);
        }

        private void Sample004PageEditingViewClose(IDialogResult dialogResult)
        {
            //// OK => 保存 => 一覧に追加
            if (dialogResult.Result == ButtonResult.OK)
            {
                //// 編集後のデータを追加もしくは更新
                var handedEntity = dialogResult.Parameters.GetValue<PageMstEntity>(nameof(PageMstEntity));

                UpdatePageMstEntitiesOrigin();

                //// 更新したOriginから取得したエンティティで更新
                var entity = _pageMstEntitiesOrigin.FirstOrDefault(x => x.Entity.PageId.Value == handedEntity.PageId.Value);
                Sample004PageListViewModelPageMst.MergeViewModelEntity(ref _pageMstEntities, entity.Entity);
                PageMstEntitiesSlectedItem = entity;
                PreviewPage();
            }

            //// No => 削除 => 一覧から除去
            if (dialogResult.Result == ButtonResult.No)
            {
                //// 編集後のデータを追加もしくは更新
                var handedEntity = dialogResult.Parameters.GetValue<PageMstEntity>(nameof(PageMstEntity));

                UpdatePageMstEntitiesOrigin();

                Sample004PageListViewModelPageMst.RemoveViewModelEntity(ref _pageMstEntities, handedEntity);
                PageMstEntitiesSlectedItem = null;
                PreviewPage();
            }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        private void UpdatePageMstEntities()
        {
            _pageMstEntities.Clear();

            foreach (var entity in _pageMstRepository.GetData())
            {
                _pageMstEntities.Add(new Sample004PageListViewModelPageMst(entity));
            }

            //// Originに退避
            _pageMstEntitiesOrigin = _pageMstEntities;
        }

        private void UpdatePageMstEntitiesOrigin()
        {
            _pageMstEntitiesOrigin.Clear();

            foreach (var entity in _pageMstRepository.GetData())
            {
                _pageMstEntitiesOrigin.Add(new Sample004PageListViewModelPageMst(entity));
            }
        }

        private void PreviewPage()
        {
            PageMstEntity entity;

            if (PageMstEntitiesSlectedItem == null)
            {
                entity = null;
            }
            else
            {
                entity = PageMstEntitiesSlectedItem.Entity;
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
