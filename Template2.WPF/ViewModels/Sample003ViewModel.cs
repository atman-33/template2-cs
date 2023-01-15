using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Objects;
using Template2.Domain.Repositories;
using Template2.Domain.ValueObjects;
using Template2.Infrastructure;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample003ViewModel : BindableBase, INavigationAware
    {
        /// <summary>
        /// MainWindow
        /// </summary>
        private MainWindowViewModel _mainWindowViewModel;

        /// <summary>
        /// メッセージボックス
        /// </summary>
        private IMessageService _messageService;

        //// 外部接触Repository
        private IWorkingTimePlanMstRepository _workingTimePlanMstRepository;

        /// <summary>
        /// WorkingTimePlanMstのViewModelEntity群（DataView変換を対応）
        /// </summary>
        private EntitiesDataTable<Weekday, string?> _workingTimePlanMstEntitiesDataTable;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Sample003ViewModel()
            : this(Factories.CreateWorkingTimePlanMst())
        {
        }

        public Sample003ViewModel(IWorkingTimePlanMstRepository workingTimePlanMstRepository)
        {
            //// メッセージボックス
            _messageService = new MessageService();

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _workingTimePlanMstRepository = workingTimePlanMstRepository;

            //// DelegateCommandメソッドを登録
            UnpivotTableButton = new DelegateCommand(UnpivotTableButtonExecute);
            SaveButton = new DelegateCommand(SaveButtonExecute);

            //// Repositoryからデータ取得
            UpdateWorkingTimePlanMstEntitiesDataView();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private DataView _workingTimePlanMstEntitiesDataView;
        public DataView WorkingTimePlanMstEntitiesDataView
        {
            get { return _workingTimePlanMstEntitiesDataView; }
            set { SetProperty(ref _workingTimePlanMstEntitiesDataView, value); }
        }


        private ObservableCollection<WorkingTimePlanMstEntity> _workingTimePlanMstEntities
            = new ObservableCollection<WorkingTimePlanMstEntity>();
        public ObservableCollection<WorkingTimePlanMstEntity> WorkingTimePlanMstEntities
        {
            get { return _workingTimePlanMstEntities; }
            set { SetProperty(ref _workingTimePlanMstEntities, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// テーブルをアンピボット変換
        /// </summary>
        public DelegateCommand UnpivotTableButton { get; }

        private void UnpivotTableButtonExecute()
        {
            _workingTimePlanMstEntitiesDataTable.CanConvertFloat("float数値の入力に誤りがあります。");

            WorkingTimePlanMstEntities = _workingTimePlanMstEntitiesDataTable.ToEntities(
                (id, keyValuePair, columnValueObject) =>
                {
                    return new WorkingTimePlanMstEntity(id, columnValueObject.Value , Convert.ToSingle(keyValuePair.Value));
                }
                );
        }

        /// <summary>
        /// テーブルのデータを保存
        /// </summary>
        public DelegateCommand SaveButton { get; }

        private void SaveButtonExecute()
        {
            if (_messageService.Question("保存しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            foreach (var entity in WorkingTimePlanMstEntities)
            {
                _workingTimePlanMstRepository.Save(entity);
            }

            _messageService.ShowDialog("保存しました。", "情報", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //// 遷移前の画面からパラメータ受け取り
            _mainWindowViewModel = navigationContext.Parameters.GetValue<MainWindowViewModel>("MainWindow");
            _mainWindowViewModel.ViewOutline = "> サンプル002（テーブルをピボット/アンピボット変換）";
        }

        /// <summary>
        /// 勤務予定テーブル（マトリックステーブル）を更新
        /// </summary>
        private void UpdateWorkingTimePlanMstEntitiesDataView()
        {
            //// マトリックス表を生成
            _workingTimePlanMstEntitiesDataTable = new EntitiesDataTable<Weekday, string?>("作業者コード");

            var dictionary = new Dictionary<string, Weekday>();
            dictionary.Add(Weekday.Sunday.DisplayValue, Weekday.Sunday);
            dictionary.Add(Weekday.Monday.DisplayValue, Weekday.Monday);
            dictionary.Add(Weekday.Tuesday.DisplayValue, Weekday.Tuesday);
            dictionary.Add(Weekday.Wednesday.DisplayValue, Weekday.Wednesday);
            dictionary.Add(Weekday.Thursday.DisplayValue, Weekday.Thursday);
            dictionary.Add(Weekday.Friday.DisplayValue, Weekday.Friday);
            dictionary.Add(Weekday.Saturday.DisplayValue, Weekday.Saturday);

            //// カラムを設定
            _workingTimePlanMstEntitiesDataTable.SetColumns(dictionary);

            //// DBのデータを取得
            var entities = new List<WorkingTimePlanMstEntity>();

            foreach (var entity in _workingTimePlanMstRepository.GetDataWithWorkerName())
            {
                entities.Add(entity);
            }

            //// DBのデータを格納
            _workingTimePlanMstEntitiesDataTable.SetData(entities,
                entities.ToLookup(x => x.WorkerCode.Value),
                getColumn => { return getColumn.Weekday.DisplayValue; },
                getValue => { return getValue.WorkingTime.Value.ToString(); }
                );

            //// DataGridに反映
            WorkingTimePlanMstEntitiesDataView = _workingTimePlanMstEntitiesDataTable.DataView;
        }

        #endregion
    }
}
