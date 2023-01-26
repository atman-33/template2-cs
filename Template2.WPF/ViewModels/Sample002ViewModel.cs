using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample002ViewModel : ViewModelBase
    {
        //// 外部接触Repository
        private IWorkerMstRepository _workerMstRepository;

        public Sample002ViewModel()
            : this(Factories.CreateWorkerMst())
        {
        }

        public Sample002ViewModel(IWorkerMstRepository workerMstRepository)
        {
            //// メッセージボックス
            _messageService = new MessageService();

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _workerMstRepository = workerMstRepository;

            //// DelegateCommandメソッドを登録
            AddRowButton = new DelegateCommand(AddRowButtonExecute);
            DeleteRowButton = new DelegateCommand(DeleteRowButtonExecute);
            SaveButton = new DelegateCommand(SaveButtonExecute);
            WorkerMstEntitiesSelectedCellsChanged = new DelegateCommand(WorkerMstEntitiesSelectedCellsChangedExecute);
            WorkerMstEntitiesCurrentCellChanged = new DelegateCommand(WorkerMstEntitiesCurrentCellChangedExecute);

            //// Repositoryからデータ取得
            UpdateWorkerMstEntities();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 1. Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private ObservableCollection<Sample002ViewModelWorkerMst> _workerMstEntities
            = new ObservableCollection<Sample002ViewModelWorkerMst>();
        public ObservableCollection<Sample002ViewModelWorkerMst> WorkerMstEntities
        {
            get { return _workerMstEntities; }
            set { SetProperty(ref _workerMstEntities, value); }
        }

        private Sample002ViewModelWorkerMst _workerMstEntitiesSlectedItem;
        public Sample002ViewModelWorkerMst WorkerMstEntitiesSlectedItem
        {
            get { return _workerMstEntitiesSlectedItem; }
            set { SetProperty(ref _workerMstEntitiesSlectedItem, value); }
        }

        private string _workerCodeText;
        public string WorkerCodeText
        {
            get { return _workerCodeText; }
            set { SetProperty(ref _workerCodeText, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand AddRowButton { get; }
        private void AddRowButtonExecute()
        {
            WorkerMstEntities.Add(new Sample002ViewModelWorkerMst(
                    new WorkerMstEntity(String.Empty, String.Empty)));
        }

        public DelegateCommand DeleteRowButton { get; }
        private void DeleteRowButtonExecute()
        {
            if (WorkerMstEntitiesSlectedItem == null)
            {
                return;
            }

            WorkerMstEntities.Remove(WorkerMstEntitiesSlectedItem);
        }

        public DelegateCommand SaveButton { get; }
        private void SaveButtonExecute()
        {
            if (_messageService.Question("保存しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            //// DBテーブルの中身を全て削除
            foreach (var entity in _workerMstRepository.GetData())
            {
                _workerMstRepository.Delete(entity);
            }

            //// 画面に設定した内容をDBテーブルに保存
            foreach (var viewModelEntity in WorkerMstEntities)
            {
                var entity = new WorkerMstEntity(
                    viewModelEntity.Entity.WorkerCode.Value,
                    viewModelEntity.Entity.WorkerName.Value
                    );
                _workerMstRepository.Save(entity);
            }

            _messageService.ShowDialog("保存しました。", "情報", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
        }

        public DelegateCommand WorkerMstEntitiesSelectedCellsChanged { get; }
        private void WorkerMstEntitiesSelectedCellsChangedExecute()
        {
            if (WorkerMstEntitiesSlectedItem == null)
            {
                return;
            }

            WorkerCodeText = WorkerMstEntitiesSlectedItem.WorkerCode;
        }

        public DelegateCommand WorkerMstEntitiesCurrentCellChanged { get; }
        private void WorkerMstEntitiesCurrentCellChangedExecute()
        {
            WorkerMstEntitiesSelectedCellsChangedExecute();
        }


        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 3. Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private void UpdateWorkerMstEntities()
        {
            WorkerMstEntities.Clear();

            foreach (var entity in _workerMstRepository.GetData())
            {
                WorkerMstEntities.Add(new Sample002ViewModelWorkerMst(entity));
            }

            //// DBテーブルにレコードが存在しない場合は、空レコードを追加
            if (WorkerMstEntities.Count == 0)
            {
                AddRowButtonExecute();
            }
        }

        #endregion


        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);
            _mainWindowViewModel.ViewOutline = "> サンプル002（DataGridを直接編集）";
        }

        #endregion
    }
}
