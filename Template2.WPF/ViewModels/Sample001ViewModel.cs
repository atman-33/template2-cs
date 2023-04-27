using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Helpers;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Events;
using Template2.WPF.Services;

namespace Template2.WPF.ViewModels
{
    public class Sample001ViewModel : ViewModelBase
    {
        //// 外部接触Repository
        private IWorkerGroupMstRepository _workerGroupMstRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Sample001ViewModel(IEventAggregator eventAggregator)
            : this(eventAggregator ,new MessageService() ,Factories.CreateWorkerGroupMst())
        {
        }

        public Sample001ViewModel(
            IEventAggregator eventAggregator,
            IMessageService messageService,
            IWorkerGroupMstRepository workerGroupMstRepository)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MainWindowSetSubTitleEvent>().Publish("> サンプル001（読み取り専用DataGrid）");

            _messageService = messageService;

            //// Factories経由で作成したRepositoryを、プライベート変数に格納
            _workerGroupMstRepository = workerGroupMstRepository;

            //// DelegateCommandメソッドを登録
            WorkerGroupMstEntitiesSelectedCellsChanged = new DelegateCommand(WorkerGroupMstEntitiesSelectedCellsChangedExecute);
            NewButton = new DelegateCommand(NewButtonExecute);
            SaveButton = new DelegateCommand(SaveButtonExecute);
            DeleteButton = new DelegateCommand(DeleteButtonExecute);

            //// Repositoryからデータ取得
            UpdateWorkerGroupMstEntities();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private ObservableCollection<Sample001ViewModelWorkerGroupMst> _workerGroupMstEntities
            = new ObservableCollection<Sample001ViewModelWorkerGroupMst>();
        public ObservableCollection<Sample001ViewModelWorkerGroupMst> WorkerGroupMstEntities
        {
            get { return _workerGroupMstEntities; }
            set { SetProperty(ref _workerGroupMstEntities, value); }
        }

        private Sample001ViewModelWorkerGroupMst _workerGroupMstEntitiesSlectedItem;
        public Sample001ViewModelWorkerGroupMst WorkerGroupMstEntitiesSlectedItem
        {
            get { return _workerGroupMstEntitiesSlectedItem; }
            set { SetProperty(ref _workerGroupMstEntitiesSlectedItem, value); }
        }

        private string _workerGroupCodeText;
        public string WorkerGroupCodeText
        {
            get { return _workerGroupCodeText; }
            set { SetProperty(ref _workerGroupCodeText, value); }
        }

        private bool _workerGroupCodeIsEnabled = false;
        public bool WorkerGroupCodeIsEnabled
        {
            get { return _workerGroupCodeIsEnabled; }
            set { SetProperty(ref _workerGroupCodeIsEnabled, value); }
        }

        private string _workerGroupNameText;
        public string WorkerGroupNameText
        {
            get { return _workerGroupNameText; }
            set { SetProperty(ref _workerGroupNameText, value); }
        }

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Event Binding (DelegateCommand)
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        public DelegateCommand WorkerGroupMstEntitiesSelectedCellsChanged { get; }
        private void WorkerGroupMstEntitiesSelectedCellsChangedExecute()
        {
            if (WorkerGroupMstEntitiesSlectedItem == null)
            {
                return;
            }

            WorkerGroupCodeText = WorkerGroupMstEntitiesSlectedItem.WorkerGroupCode;
            WorkerGroupNameText = WorkerGroupMstEntitiesSlectedItem.WorkerGroupName;

            WorkerGroupCodeIsEnabled = false;
        }

        public DelegateCommand NewButton { get; }
        private void NewButtonExecute()
        {
            WorkerGroupCodeIsEnabled = true;
            WorkerGroupCodeText = String.Empty;
            WorkerGroupNameText = String.Empty;

            OnEdit();
        }

        public DelegateCommand SaveButton { get; }
        private void SaveButtonExecute()
        {
            Guard.IsNullOrEmpty(WorkerGroupNameText, "サンプル名称を入力してください。");

            if (_messageService.Question("保存しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            var entity = new WorkerGroupMstEntity(
                WorkerGroupCodeText,
                WorkerGroupNameText
                );

            _workerGroupMstRepository.Save(entity);

            UpdateWorkerGroupMstEntities();

            OnEditCompleted();
        }

        public DelegateCommand DeleteButton { get; }
        private void DeleteButtonExecute()
        {
            if (_messageService.Question("「" + WorkerGroupNameText + "」を削除しますか？") != System.Windows.MessageBoxResult.OK)
            {
                return;
            }

            var entity = new WorkerGroupMstEntity(
                WorkerGroupCodeText,
                WorkerGroupNameText
                );

            _workerGroupMstRepository.Delete(entity);

            UpdateWorkerGroupMstEntities();
        }

        #endregion


        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private void UpdateWorkerGroupMstEntities()
        {
            WorkerGroupMstEntities.Clear();

            foreach (var entity in _workerGroupMstRepository.GetData())
            {
                WorkerGroupMstEntities.Add(new Sample001ViewModelWorkerGroupMst(entity));
            }
        }

        #endregion
    }
}