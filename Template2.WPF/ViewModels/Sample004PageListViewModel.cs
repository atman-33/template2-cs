using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Helpers;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Views;

//// ページ削除機能追加

namespace Template2.WPF.ViewModels
{
    public class Sample004PageListViewModel : ViewModelBase
    {
        private IDialogService _dialogService;

        //// 外部接触Repository
        private IPageMstRepository _pageMstRepository;

        /// <summary>
        /// PageMstEntitiesのオリジン（未フィルターのコレクション）
        /// </summary>
        private ObservableCollection<Sample004PageListViewModelPageMst> _pageMstEntitiesOrigin
            = new ObservableCollection<Sample004PageListViewModelPageMst>();

        public Sample004PageListViewModel(IDialogService dialogService)
            : this(Factories.CreatePageMst())
        {
            _dialogService = dialogService;
        }

        public Sample004PageListViewModel(IPageMstRepository pageMstRepository)
        {
            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _pageMstRepository = pageMstRepository;

            //// DelegateCommandメソッドを登録
            NewButton = new DelegateCommand(NewButtonExecute);
            SearchingPageNameTextChanged = new DelegateCommand(SearchingPageNameTextChangedExecute);
            PageMstEntitiesSelectedCellsChanged = new DelegateCommand(PageMstEntitiesSelectedCellsChangedExecute);
            EditButton = new DelegateCommand(EditButtonExecute);

            //// Repositoryからデータ取得
            UpdatePageMstEntitiesOrigin();

            //// Originに格納
            PageMstEntities = _pageMstEntitiesOrigin;
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

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
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// ページ編集画面を表示
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

            var p = new DialogParameters();
            p.Add(nameof(Sample004PageEditingViewModel.IsNewPage), false);
            p.Add(nameof(this.PageMstEntitiesSlectedItem), PageMstEntitiesSlectedItem);

            _dialogService.ShowDialog(nameof(Sample004PageEditingView), p, Sample004PageEditingViewClose);
        }

        private void Sample004PageEditingViewClose(IDialogResult dialogResult)
        {
            //// OKが返された時のみ
            if (dialogResult.Result == ButtonResult.OK)
            {
                //// 編集後のデータを追加もしくは更新
                var entity = dialogResult.Parameters.GetValue<PageMstEntity>(nameof(PageMstEntity));
                Sample004PageListViewModelPageMst.MergeViewModelEntity(ref _pageMstEntities, entity);

                //// Originを更新
                UpdatePageMstEntitiesOrigin();
            }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
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
            // ToDo: 未実装
            // throw new NotImplementedException();
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            _mainWindowViewModel.ViewOutline = "> サンプル004（画像を表示するDataGrid）";
        }

        #endregion
    }
}
