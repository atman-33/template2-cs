using Prism.Commands;
using Prism.Events;
using Template2.Domain.Entities;
using Template2.Domain.Modules.Helpers;
using Template2.Domain.Repositories;
using Template2.Infrastructure;
using Template2.WPF.Collections;
using Template2.WPF.Services;
using Template2.WPF.ViewModelEntities;

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
            : this(eventAggregator, new MessageService(), Factories.CreateWorkerGroupMst())
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

            //// Repositoryからデータ取得
            _workerGroupMstCollection = new WorkerGroupMstCollection(workerGroupMstRepository);
            _workerGroupMstCollection.LoadData();
        }

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Screen transition
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        #endregion

        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Property Data Binding
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 

        private WorkerGroupMstCollection _workerGroupMstCollection;
        public WorkerGroupMstCollection WorkerGroupMstCollection
        {
            get { return _workerGroupMstCollection; }
            set { SetProperty(ref _workerGroupMstCollection, value); }
        }

        private WorkerGroupMstViewModelEntity _workerGroupMstCollectionSlectedItem;
        public WorkerGroupMstViewModelEntity WorkerGroupMstCollectionSlectedItem
        {
            get { return _workerGroupMstCollectionSlectedItem; }
            set { SetProperty(ref _workerGroupMstCollectionSlectedItem, value); }
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

        public DelegateCommand WorkerGroupMstCollectionSelectedCellsChanged =>
            new DelegateCommand(() =>
            {
                if (WorkerGroupMstCollectionSlectedItem == null)
                {
                    return;
                }

                WorkerGroupCodeText = WorkerGroupMstCollectionSlectedItem.WorkerGroupCode;
                WorkerGroupNameText = WorkerGroupMstCollectionSlectedItem.WorkerGroupName;

                WorkerGroupCodeIsEnabled = false;

            });

        public DelegateCommand NewButton =>
            new DelegateCommand(() =>
            {
                WorkerGroupCodeIsEnabled = true;
                WorkerGroupCodeText = string.Empty;
                WorkerGroupNameText = string.Empty;

                OnEdit();
            });

        public DelegateCommand SaveButton =>
            new DelegateCommand(() =>
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


                WorkerGroupMstCollection.UpsertItem(entity);
                OnEditCompleted();
            });


        public DelegateCommand DeleteButton =>
            new DelegateCommand(() =>
            {
                if (_messageService.Question("「" + WorkerGroupNameText + "」を削除しますか？") != System.Windows.MessageBoxResult.OK)
                {
                    return;
                }

                WorkerGroupMstCollection.DeleteItem(WorkerGroupMstCollectionSlectedItem);
            });

        #endregion


        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
        #region //// Others
        //// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 


        #endregion
    }
}