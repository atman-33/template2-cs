using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.Infrastructure;

namespace Template2.WPF.ViewModels
{
    public class Sample002ViewModel : ViewModelBase, IDialogAware
    {
        //// 外部接触Repository
        private IWorkerMstRepository _workerMstRepository;
        private IWorkerGroupMstRepository _workerGroupMstRepository;

        public Sample002ViewModel()
            : this(Factories.CreateWorkerMst(), Factories.CreateWorkerGroupMst())
        {
        }

        public Sample002ViewModel(IWorkerMstRepository workerMstRepository, IWorkerGroupMstRepository workerGroupMstRepository)
        {
            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _workerMstRepository = workerMstRepository;
            _workerGroupMstRepository = workerGroupMstRepository;

            //// DelegateCommandメソッドを登録
            AddRowButton = new DelegateCommand(AddRowButtonExecute);
            DeleteRowButton = new DelegateCommand(DeleteRowButtonExecute);
            SaveButton = new DelegateCommand(SaveButtonExecute);
            WorkerMstEntitiesSelectedCellsChanged = new DelegateCommand(WorkerMstEntitiesSelectedCellsChangedExecute);
            WorkerMstEntitiesCurrentCellChanged = new DelegateCommand(WorkerMstEntitiesCurrentCellChangedExecute);

            //// Repositoryからデータ取得
            UpdateWorkerMstEntities();
            UpdateWorkerGroupMstEntities();
        }

        public string Title => "Sample002View";

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

        private ObservableCollection<WorkerGroupMstEntity> _workerGroupMstEntities
            = new ObservableCollection<WorkerGroupMstEntity>();

        public ObservableCollection<WorkerGroupMstEntity> WorkerGroupMstEntities
        {
            get { return _workerGroupMstEntities; }
            set { SetProperty(ref _workerGroupMstEntities, value); }
        }


        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// 2. Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand AddRowButton { get; }
        private void AddRowButtonExecute()
        {
            WorkerMstEntities.Add(new Sample002ViewModelWorkerMst(
                    new WorkerMstEntity(String.Empty, String.Empty, String.Empty)));
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
                    viewModelEntity.Entity.WorkerName.Value,
                    viewModelEntity.Entity.WorkerGroupCode.Value);
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

        private void UpdateWorkerGroupMstEntities()
        {
            WorkerGroupMstEntities.Clear();

            foreach (var entity in _workerGroupMstRepository.GetData())
            {
                WorkerGroupMstEntities.Add(entity);
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

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        #endregion
    }
}
