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
                  AbstractFactory.Create())
        {
        }

        public Sample003ViewModel(
            IEventAggregator eventAggregator,
            IMessageService messageService,
            AbstractFactory factory)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル003（テーブルをピボット / アンピボット変換）");

            _messageService = messageService;

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _workingTimePlanMstRepository = factory.CreateWorkingTimePlanMst();
            _workerMstRepository = factory.CreateWorkerMst();

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

        private string _updatedRowTotalNumLabel = string.Empty;
        public string UpdatedRowTotalNumLabel
        {
            get { return _updatedRowTotalNumLabel; }
            set { SetProperty(ref _updatedRowTotalNumLabel, value); }
        }

        private DataView _workingTimePlanMstEntitiesDataView;
        public DataView WorkingTimePlanMstEntitiesDataView
        {
            get { return _workingTimePlanMstEntitiesDataView; }
            set { SetProperty(ref _workingTimePlanMstEntitiesDataView, value); }
        }

        private ObservableCollection<WorkingTimePlanMstEntity> _workingTimePlanMstCollection
            = new ObservableCollection<WorkingTimePlanMstEntity>();
        public ObservableCollection<WorkingTimePlanMstEntity> WorkingTimePlanMstCollection
        {
            get { return _workingTimePlanMstCollection; }
            set { SetProperty(ref _workingTimePlanMstCollection, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        /// <summary>
        /// DataGridを生成する際の処理。IDカラムを非表示するために実装
        /// </summary>
        public DelegateCommand<DataGridAutoGeneratingColumnEventArgs> WorkingTimePlanMstEntitiesDataViewAutoGeneratingColumn =>
            new DelegateCommand<DataGridAutoGeneratingColumnEventArgs>((e) =>
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
            });

        /// <summary>
        /// テーブル編集後にDataTableの値を更新して行の合計を更新。
        /// 【補足】
        /// 通常、行のフォーカスを変更しないとDataTableは更新されないため、TABでカラムのフォーカスを変更してもDataTableは変わらない。
        /// そのため、今回のようにCellEditEndingイベントで取得したEditingElementを、DataTableに格納すれば更新可能となる。
        /// </summary>
        public DelegateCommand<DataGridCellEditEndingEventArgs> WorkingTimePlanMstEntitiesDataViewCellEditEnding =>
            new DelegateCommand<DataGridCellEditEndingEventArgs>((e) =>
            {
                //// 編集されたセルの列と行を取得する
                int columnIndex = e.Column.DisplayIndex;
                int rowIndex = e.Row.GetIndex();

                //// 編集されたセルの値を取得する
                var editedCellValue = ((TextBox)e.EditingElement).Text;

                //// DataTableの該当するセルの値を更新する
                DataTable dataTable = _workingTimePlanMstEntitiesDataTable.DataTable;
                dataTable.Rows[rowIndex][columnIndex] = editedCellValue;

                UpdatedRowTotalNumLabel = Convert.ToString(_workingTimePlanMstEntitiesDataTable.SumRowData(rowIndex));

            });

        /// <summary>
        /// テーブルをアンピボット変換
        /// </summary>
        public DelegateCommand UnpivotTableButton =>
            new DelegateCommand(() =>
            {
                _workingTimePlanMstEntitiesDataTable.CanConvertFloat("float数値の入力に誤りがあります。");

                WorkingTimePlanMstCollection = _workingTimePlanMstEntitiesDataTable.ToEntities(
                    (id, keyValuePair, columnValueObject) =>
                    {
                        return new WorkingTimePlanMstEntity(id, columnValueObject.Value, Convert.ToSingle(keyValuePair.Value));
                    }
                    );
            });

        /// <summary>
        /// テーブルのデータを保存
        /// </summary>
        public DelegateCommand SaveButton =>
            new DelegateCommand(() =>
            {
                if (_messageService.Question("保存しますか？") != System.Windows.MessageBoxResult.OK)
                {
                    return;
                }

                foreach (var entity in WorkingTimePlanMstCollection)
                {
                    _workingTimePlanMstRepository.Save(entity);
                }

                _messageService.ShowDialog("保存しました。", "情報", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            });

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
            var dictionary = new Dictionary<string, Weekday>
            {
                { Weekday.Sunday.DisplayValue, Weekday.Sunday },
                { Weekday.Monday.DisplayValue, Weekday.Monday },
                { Weekday.Tuesday.DisplayValue, Weekday.Tuesday },
                { Weekday.Wednesday.DisplayValue, Weekday.Wednesday },
                { Weekday.Thursday.DisplayValue, Weekday.Thursday },
                { Weekday.Friday.DisplayValue, Weekday.Friday },
                { Weekday.Saturday.DisplayValue, Weekday.Saturday }
            };

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
