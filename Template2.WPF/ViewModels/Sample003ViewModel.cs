using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Objects;
using Template2.Domain.Repositories;
using Template2.Domain.ValueObjects;
using Template2.Infrastructure;
using Template2.WPF.Events;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample003ViewModel : ViewModelBase
    {
        //// 外部接触Repository
        private IWorkingTimePlanMstRepository _workingTimePlanMstRepository;
        private IWorkerMstRepository _workerMstRepository;

        /// <summary>
        /// WorkingTimePlanMstのViewModelEntity群（DataView変換を対応）
        /// </summary>
        private EntityDataTable<Weekday, string> _workingTimePlanMstEntitiesDataTable;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Sample003ViewModel(IEventAggregator eventAggregator)
            : this(eventAggregator, 
                  new MessageService(),
                  Factories.CreateWorkingTimePlanMst(), 
                  Factories.CreateWorkerMst())
        {
        }

        public Sample003ViewModel(
            IEventAggregator eventAggregator,
            IMessageService messageService,
            IWorkingTimePlanMstRepository workingTimePlanMstRepository,
            IWorkerMstRepository workerMstRepository)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル003（テーブルをピボット / アンピボット変換）");

            _messageService = messageService;

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _workingTimePlanMstRepository = workingTimePlanMstRepository;
            _workerMstRepository = workerMstRepository;

            //// DelegateCommandメソッドを登録
            UnpivotTableButton = new DelegateCommand(UnpivotTableButtonExecute);
            SaveButton = new DelegateCommand(SaveButtonExecute);
            WorkingTimePlanMstEntitiesDataGridAutoGeneratingColumn 
                = new DelegateCommand<DataGridAutoGeneratingColumnEventArgs>(WorkingTimePlanMstEntitiesDataGridAutoGeneratingColumnExecute);

            //// Repositoryからデータ取得
            UpdateWorkingTimePlanMstEntitiesDataView();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
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
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// DataGridを生成する際の処理。IDカラムを非表示するために実装
        /// </summary>
        public DelegateCommand<DataGridAutoGeneratingColumnEventArgs> WorkingTimePlanMstEntitiesDataGridAutoGeneratingColumn { get; }

        private void WorkingTimePlanMstEntitiesDataGridAutoGeneratingColumnExecute(DataGridAutoGeneratingColumnEventArgs e)
        {
            Debug.WriteLine(e.PropertyName);

            if (e.PropertyName == _workingTimePlanMstEntitiesDataTable.IdHeader)
            {
                e.Column.Visibility = System.Windows.Visibility.Collapsed;
            }

            //// ヘッダーを定数値として扱えばswitch文でも対応可能
            //switch (e.PropertyName)
            //{
            //    case "SampleId":
            //        //e.Column.Visibility = Visibility.Collapsed;
            //        break;

            //    case "SampleText":
            //        e.Column.Header = "サンプルテキスト";
            //        break;

            //    case "SampleValue":
            //        e.Column.Header = "サンプル値";
            //        break;

            //    case "SampleDate":
            //        e.Column.Header = "サンプル日付";
            //        break;

            //    default:
            //        break;
            //}
        }

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
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// 勤務予定テーブル（マトリックステーブル）を更新
        /// </summary>
        private void UpdateWorkingTimePlanMstEntitiesDataView()
        {
            //// 1. マトリックス表を生成
            _workingTimePlanMstEntitiesDataTable = new EntityDataTable<Weekday, string>("作業者コード", true);

            //// 2. ID名称ヘッダーを設定
            _workingTimePlanMstEntitiesDataTable.SetIdNameHeader("作業者名称", true);

            //// 3. アイテム項目ヘッダーを設定
            var dictionary = new Dictionary<string, Weekday>();
            dictionary.Add(Weekday.Sunday.DisplayValue, Weekday.Sunday);
            dictionary.Add(Weekday.Monday.DisplayValue, Weekday.Monday);
            dictionary.Add(Weekday.Tuesday.DisplayValue, Weekday.Tuesday);
            dictionary.Add(Weekday.Wednesday.DisplayValue, Weekday.Wednesday);
            dictionary.Add(Weekday.Thursday.DisplayValue, Weekday.Thursday);
            dictionary.Add(Weekday.Friday.DisplayValue, Weekday.Friday);
            dictionary.Add(Weekday.Saturday.DisplayValue, Weekday.Saturday);

            _workingTimePlanMstEntitiesDataTable.SetItemHeaders(dictionary);

            //// 4. IDとID名称カラムにデータを設定
            foreach (var entity in _workerMstRepository.GetData())
            {
                _workingTimePlanMstEntitiesDataTable.SetIdData(entity.WorkerCode.Value, entity.WorkerName.Value);
            }

            //// 5. アイテムカラムにデータを格納
            var itemEntities = new List<WorkingTimePlanMstEntity>();
            foreach (var entity in _workingTimePlanMstRepository.GetDataWithWorkerName())
            {
                itemEntities.Add(entity);
            }

            _workingTimePlanMstEntitiesDataTable.SetItemData(
                itemEntities.ToLookup(x => x.WorkerCode.Value),
                getColumn => { return getColumn.Weekday.DisplayValue; },
                getValue => { return getValue.WorkingTime.Value.ToString(); }
                );

            //// 6. DataGridに反映
            WorkingTimePlanMstEntitiesDataView = _workingTimePlanMstEntitiesDataTable.DataView;
        }

        #endregion

    }
}
